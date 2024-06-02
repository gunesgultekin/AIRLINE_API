namespace FlightServiceAPI.Context
{
    public class AIRLINE_AIRPORTS
    {
        public int id {  get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string city { get; set; }
        public string country { get; set; }

        // Navigation properties
        public ICollection<AIRLINE_FLIGHTS> DepartureFlights { get; set; }
        public ICollection<AIRLINE_FLIGHTS> ArrivalFlights { get; set; }
    }
}
