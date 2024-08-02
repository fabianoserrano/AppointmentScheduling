using Application.User.Dtos;

namespace Application.User.Ports
{
    public interface ILoginManager
    {
        Task<object> FindByLogin(LoginDto user);
    }
}
