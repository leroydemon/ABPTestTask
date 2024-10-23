using ABPTestTask.DAL.Entities;
using Domain.Entities;

namespace ABPTestTask.Common.Hall
{
    public class HallEntity : Entity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public List<BookingEntity> Bookings { get; set; }
    }
}
