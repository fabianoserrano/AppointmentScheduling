namespace Domain.Email.Ports
{
    public interface IEmailService
    {
        Task Send(Entities.Email email);
    }
}
