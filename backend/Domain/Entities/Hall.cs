namespace Domain.Entities
{
    public class Hall : Entity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set;}
        public List<Booking> Bookings { get; set; }
    }
}
