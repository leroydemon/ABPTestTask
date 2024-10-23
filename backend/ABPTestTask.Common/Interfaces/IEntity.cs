namespace ABPTestTask.Common.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; }
        DateTime Created { get; }
    }
}
