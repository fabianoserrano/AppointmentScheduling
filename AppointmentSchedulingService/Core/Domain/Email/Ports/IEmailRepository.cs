namespace Domain.Email.Ports
{
    public interface IEmailRepository
    {
        Task<int> Create(Entities.Email emal);
    }
}
