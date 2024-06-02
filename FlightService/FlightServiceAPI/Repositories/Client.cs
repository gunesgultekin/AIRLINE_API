using FlightServiceAPI.Context;
using FlightServiceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Linq;
using System.Text.RegularExpressions;

namespace FlightServiceAPI.Repositories
{
    public class Client : IClient
    {
        public DBContext _context;
        public Client(DBContext context)
        {
            _context = context;
        }

        public async Task deleteMyTicket(string clientEmail, string ticket_id)
        {
            await _context.AIRLINE_TICKETS.Where(t => t.ticket_id == ticket_id && t.client_mail == clientEmail)
                .ExecuteDeleteAsync();
        }

        public async Task<List<AIRLINE_TICKETS>> getMyTickets(string clientEmail)
        {
            return await _context.AIRLINE_TICKETS.Where(t => t.client_mail == clientEmail).ToListAsync();

        }

        public  async Task<string> loginMiles(MilesLoginModel credentials)
        {
            string hashed = SecurityService.ComputeSha512Hash(credentials.password);

            var member = await _context.AIRLINE_MILESMEMBERS.Where( m => m.email == credentials.email 
                && m.password == hashed   
            ).FirstAsync();

            if (member == null)
            {
                return "not exists";
            }
            else
            {
                return SecurityService.GenerateToken(member.email);
            }
        }

        public async Task<int> registerMiles(AIRLINE_MILESMEMBERS credentials)
        {
            var member = credentials;
            member.password = SecurityService.ComputeSha512Hash(credentials.password);
            await _context.AIRLINE_MILESMEMBERS.AddAsync(member);
            _context.SaveChanges();
            return credentials.miles_member_number;
        }

        public async Task<List<AIRLINE_FLIGHTS>> search(SearchModel search)
        {

            var list = await _context.AIRLINE_FLIGHTS
                .Where(f => (f.departure_city == search.from.ToString() ||
                            f.arrival_city == search.to.ToString()) &&
                            f.departure_date == DateOnly.Parse(search.departDate) &&
                            f.return_date == DateOnly.Parse(search.returnDate))
                .ToListAsync();

            return list;    
        }

        public async Task<List<AIRLINE_FLIGHTS>> flexSearch(SearchModel search)
        {
            var list = await _context.AIRLINE_FLIGHTS
              .Where(f => (f.departure_city == search.from.ToString() ||
                          f.arrival_city == search.to.ToString()) &&
                          f.departure_date <= DateOnly.Parse(search.departDate).AddMonths(3)
                          )
              .ToListAsync();

            return list;


        }


        public async Task<string> buyTicket(string flightCode, string clientEmail)
        {
            var check = await 
                _context.AIRLINE_TICKETS.Where(t => t.flight_code == flightCode && t.client_mail == clientEmail)
                .FirstOrDefaultAsync();
            if (check != null)
            {
                return "already exists";
            }

            var flight = await _context.AIRLINE_FLIGHTS.Where(f => f.flight_code == flightCode).FirstAsync();
            if (flight.capacity == 0)
            {
                return "capacity";
            }

            AIRLINE_TICKETS ticket = new AIRLINE_TICKETS();
            ticket.flight_code = flightCode;
            ticket.client_mail = clientEmail;

            Random rand = new Random();
            long ticket_id = rand.NextInt64(100000,10000000);

            ticket.ticket_id = ticket_id.ToString();

            _context.AIRLINE_TICKETS.Add(ticket);

            _context.AIRLINE_FLIGHTS.Where(f => f.flight_code == flightCode).First().capacity--;

            await _context.SaveChangesAsync();

            return $"success {ticket.ticket_id}";
        }

        public async Task<string> buyWithMiles(string flightCode, string clientEmail)
        {
           var check = await
                _context.AIRLINE_TICKETS.Where(t => t.flight_code == flightCode && t.client_mail == clientEmail)
                .FirstOrDefaultAsync();

            if (check != null)
            {
                return "already exists";
            }

            else
            {
                var member = await _context.AIRLINE_MILESMEMBERS.Where(m => m.email == clientEmail).FirstOrDefaultAsync(); 
                if (member != null)
                {
                    var flight = await _context.AIRLINE_FLIGHTS.Where(f => f.flight_code == flightCode).FirstAsync();
                    if (flight.capacity == 0)
                    {
                        return "capacity";
                    }

                    else
                    {
                        if (flight.miles_price <= member.miles_point)
                        {
                            AIRLINE_TICKETS ticket = new AIRLINE_TICKETS();
                            ticket.flight_code = flightCode;
                            ticket.client_mail = clientEmail;
                            Random rand = new Random();
                            long ticket_id = rand.NextInt64(200000, 20000000);
                            ticket.ticket_id = ticket_id.ToString();
                            _context.AIRLINE_TICKETS.Add(ticket);
                            _context.AIRLINE_FLIGHTS.Where(f => f.flight_code == flightCode).First().capacity--;
                            member.miles_point -= flight.miles_price;
                            member.miles_point += flight.miles_bonus;
                            await _context.SaveChangesAsync();


                            // SCHEDULE A TASK TO ADD MESSAGES TO MAILS QUEUE
                            MailModel mailModel = new MailModel
                            {
                                to = member.email,
                                bonus = flight.miles_bonus.ToString(),
                                flightCode = flight.flight_code
                            };

                            Scheduler.scheduleMail(mailModel);
   
                            return $"success {ticket.ticket_id}";
                        }
                        return "balance";        
                    }
                }
                else
                {
                    return "member not exist";
                }
                
            }
        }
    }
}
