using ABPTestTask.Common.Bookings;

namespace ABPTestTask.Common.Hall
{
    public class Hall
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public List<Booking> BookingsList { get; set; }
    }
}
