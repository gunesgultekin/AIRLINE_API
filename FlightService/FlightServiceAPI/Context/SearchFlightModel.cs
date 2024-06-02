namespace FlightServiceAPI.Context
{
    public class SearchFlightModel
    {
        public int departure_airport { get; set; }
        public int arrival_airport { get; set; }
        public string departure_city { get; set; }
        public string arrival_city { get; set; }
        public string departure_date { get; set; }
        public string return_date { get; set; }
        public string departure_time { get; set; }
        public string arrival_time { get; set; }
        public int capacity { get; set; }
        public int miles_point { get; set; }
        public string? plane_type { get; set; }
    }
}
