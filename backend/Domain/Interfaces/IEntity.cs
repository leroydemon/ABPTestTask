namespace Domain.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; }
        DateTime Created { get; }
    }
}
