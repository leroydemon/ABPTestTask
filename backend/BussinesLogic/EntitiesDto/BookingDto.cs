using Domain.Entities;

namespace BussinesLogic.EntitiesDto
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public List<Service>? Services { get; set; } = new List<Service>();
        public bool IsConfirmed { get; set; }
    }
}
