using Domain.Entities;

namespace Authorization.Interfaces
{
    public interface ITokenGeneratorService
    {
        string GenerateJwtToken(User user);
    }
}
