using Domain.Interfaces;

namespace Domain.Entities
{
    public abstract class Entity : IEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
