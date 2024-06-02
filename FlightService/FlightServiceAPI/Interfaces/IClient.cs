using FlightServiceAPI.Context;

namespace FlightServiceAPI.Interfaces
{
    public interface IClient
    {
        public Task deleteMyTicket(string clientEmail, string ticket_id);
        public Task<List<AIRLINE_TICKETS>> getMyTickets(string clientEmail);
        public Task<string> loginMiles(MilesLoginModel credentials);
        public Task<int> registerMiles(AIRLINE_MILESMEMBERS credentials);
        public Task<List<AIRLINE_FLIGHTS>> search(SearchModel search);
        public Task<List<AIRLINE_FLIGHTS>> flexSearch(SearchModel search);
        public Task<string> buyTicket(string flightCode, string clientEmail);

        public Task<string> buyWithMiles(string flightCode, string clientEmail);

    }
}
