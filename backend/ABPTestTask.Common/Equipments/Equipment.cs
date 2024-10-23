using ABPTestTask.Common.Interfaces;

namespace ABPTestTask.Common.Equipments
{
    public class Equipment : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime Created { get; set; }
    }
}
