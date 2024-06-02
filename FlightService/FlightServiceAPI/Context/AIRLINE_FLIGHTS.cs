using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace FlightServiceAPI.Context
{
    public class AIRLINE_FLIGHTS
    {
        public int id { get; set; }
        public string flight_code { get; set; }
        public int departure_airport { get; set; }
        public int arrival_airport { get; set; }
        public string departure_city { get; set; }
        public string arrival_city { get; set; }
        public DateOnly departure_date { get; set; }
        public DateOnly return_date { get; set; }
        public string departure_time { get; set; }
        public string arrival_time { get; set; }
        public int capacity { get; set; }
        public int miles_price { get; set; }
        public int miles_bonus { get; set; }
        public string? plane_type { get; set; }
        public virtual AIRLINE_AIRPORTS? departPort { get; set; }
        public virtual AIRLINE_AIRPORTS? arrivePort { get; set; }
    }
}
