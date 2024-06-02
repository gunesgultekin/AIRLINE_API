using FlightServiceAPI.Context;
using FlightServiceAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace FlightServiceAPI.Repositories
{
    public class FlightManager : IFlightManager
    {
        public DBContext _context;
        public FlightManager(DBContext context)
        {
            _context = context;
        }

        
        public async Task<List<AIRLINE_FLIGHTS>> getAll()
        {
            var flights = await _context.AIRLINE_FLIGHTS
                .Include(f => f.departPort)
                .Include(f => f.arrivePort)
                .ToListAsync();

            return flights;
        }
        
        public async Task<string> addFlight(AddFlightModel flightModel)
        {
            
            var flight = new AIRLINE_FLIGHTS();
            flight.flight_code = flightModel.flight_code;
            flight.departure_airport = flightModel.departure_airport;
            flight.arrival_airport = flightModel.arrival_airport;
            flight.departure_city = flightModel.departure_city;
            flight.arrival_city = flightModel.arrival_city;
            flight.departure_date = flightModel.departure_date;
            flight.return_date = flightModel.arrival_date;
            flight.departure_time = flightModel.departure_time;
            flight.arrival_time = flightModel.arrival_time;
            flight.capacity = flightModel.capacity;
            flight.miles_price = flightModel.miles_price;
            flight.miles_bonus = flightModel.miles_bonus;
            flight.plane_type = flightModel.plane_type;
            
            await _context.AIRLINE_FLIGHTS.AddAsync(flight);  
            _context.SaveChanges();

            return flightModel.flight_code;       
        }

        public async Task deleteFlight(string flightCode)
        {
            await _context.AIRLINE_FLIGHTS.Where(f => f.flight_code == flightCode)
                .ExecuteDeleteAsync();

            _context.SaveChanges();
        }

        public async Task<string> adminLogin(AdminLoginModel credentials) 
        {
            var hashed = SecurityService.ComputeSha512Hash(credentials.password);
            var admin = await _context.AIRLINE_ADMINS.Where(a => a.username == credentials.username && a.password == hashed).FirstOrDefaultAsync();
            if (admin == null)
            {
                return "not exists";
            }
            else
            {
                return SecurityService.GenerateToken(admin.username);
            }
        }

        public async Task<List<AIRLINE_AIRPORTS>> getAirports()
        {

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            List<AIRLINE_AIRPORTS> airports = new List<AIRLINE_AIRPORTS>();

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("REDIS_AZURE"));
            var db = redis.GetDatabase();

            long dbSize = db.HashLength("airports");

            // IF CACHE IS EMPTY THEN GET AIRPORTS FROM DB THEN SAVE TO CACHE
            if (dbSize == 0)
            {
                airports =  await _context.AIRLINE_AIRPORTS.ToListAsync();

                foreach(var entry in airports)
                {
                    db.HashSet("airports", new HashEntry[]
                    {
                         new HashEntry(entry.code, $"{entry.name}|{entry.city}|{entry.country}|{entry.id}")


                    });
                }
                return airports;
            }

            // IF CACHE IS NOT EMPTY THEN GET FROM CACHE
            else
            {
                var redisAirports = db.HashGetAll("airports");
                foreach (var redisAirport in redisAirports)
                {
                    string[] attributes = redisAirport.Value.ToString().Split('|');
                    airports.Add(new AIRLINE_AIRPORTS
                    {
                        code = redisAirport.Name,
                        name = attributes[0],
                        city = attributes[1],
                        country = attributes[2],
                        id = int.Parse(attributes[3]),

                    });
                }

                return airports;
            }       
        }

        public async Task<List<AIRLINE_CITIES>> getCities()
        {

            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();

            List<AIRLINE_CITIES> cities = new List<AIRLINE_CITIES>();

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("REDIS_AZURE"));

            var db = redis.GetDatabase();

            long dbSize = db.HashLength("cities");

            // IF CACHE IS EMPTY THEN GET AIRPORTS FROM DB THEN SAVE TO CACHE
            if (dbSize == 0)
            {
                cities = await _context.AIRLINE_CITIES.ToListAsync();

                foreach (var entry in cities)
                {
                    db.HashSet("cities", new HashEntry[]
                    {
                        new HashEntry(entry.id,entry.name)

                    });
                }
                return cities;
            }

            // IF CACHE IS NOT EMPTY THEN GET FROM CACHE
            else
            {
                var redisCities = db.HashGetAll("cities");

                foreach (var redisCity in redisCities)
                {
                    cities.Add(new AIRLINE_CITIES
                    {
                        id = int.Parse(redisCity.Name),
                        name = redisCity.Value

                    });

                }

                return cities;
            }
        }
    }
}
