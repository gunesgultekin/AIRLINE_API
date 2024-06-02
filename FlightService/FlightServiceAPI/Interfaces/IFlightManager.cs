using FlightServiceAPI.Context;

namespace FlightServiceAPI.Interfaces
{
    public interface IFlightManager
    {
        public Task<string> adminLogin(AdminLoginModel credentials);
        public Task<List<AIRLINE_FLIGHTS>> getAll();
        public Task deleteFlight(string flightCode);
        public Task<string> addFlight(AddFlightModel flight);
        public Task<List<AIRLINE_AIRPORTS>> getAirports();
        public Task<List<AIRLINE_CITIES>> getCities();
    }
}
