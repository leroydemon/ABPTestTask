using ABPTestTask.Common.User;

namespace Authorization.Interfaces
{
    public interface ITokenGeneratorService
    {
        string GenerateJwtToken(User user);
    }
}
