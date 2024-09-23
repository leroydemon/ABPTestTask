using BussinesLogic.EntitiesDto;
using BussinesLogic.Results;

namespace BussinesLogic.Interfaces
{
    public interface IAccountService
    {
        Task<ResultBase> Register(RegisterDto request);
        Task<string> Login(LoginDto input);
        Task<bool> LogOut(Guid userId);
    }
}
