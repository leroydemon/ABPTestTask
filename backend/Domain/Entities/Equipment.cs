using Domain.Interfaces;

namespace Domain.Entities
{
    public class Equipment : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime Created {  get; set; }
    }
}
