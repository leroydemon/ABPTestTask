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
        public List<EquipmentDto>? Services { get; set; } = new List<EquipmentDto>();
        public bool IsConfirmed { get; set; }
    }
}
