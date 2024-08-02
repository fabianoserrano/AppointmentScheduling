namespace Domain.Ports
{
    public interface IUserRepository
    {
        Task<int> Create(Domain.Entities.User user);
        Task<Domain.Entities.User> Get(int Id);
        Task<int> Update(Domain.Entities.User user);
        Task Delete(int Id);
        Task<Domain.Entities.User> FindByLogin(string email);
    }
}
