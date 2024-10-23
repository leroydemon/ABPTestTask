using ABPTestTask.Results;
using BussinesLogic.EntitiesDto;

namespace BussinesLogic.Interfaces
{
    public interface IAccountService
    {
        Task<ResultBase> Register(Register request);
        Task<string> Login(Login input);
        Task<bool> LogOut(Guid userId);
    }
}
