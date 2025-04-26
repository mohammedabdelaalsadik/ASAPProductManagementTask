using ASAPTaskAPI.Application.Dto;

namespace ASAPTaskAPI.Application.Interfaces
{
    public interface IUserAppService
    {
        Task Register(LoginDto input);
        Task<string> Authenticate(LoginDto input);
    }
}
