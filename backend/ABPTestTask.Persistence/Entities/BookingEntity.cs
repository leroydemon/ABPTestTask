using ABPTestTask.Common.Equipments;
using Domain.Entities;

namespace ABPTestTask.DAL.Entities
{
    public class BookingEntity : Entity
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string? EquipmentsListJson { get; set; }
        public List<Equipment>? Equipments { get; set; }
        public bool IsConfirmed { get; set; }
    }
}