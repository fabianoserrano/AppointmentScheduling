using Application.Email.Requests;
using Application.Email.Responses;

namespace Application.Email.Ports
{
    public interface IEmailManager
    {
        Task<EmailResponse> SendEmail(SendEmailRequest request);
    }
}
