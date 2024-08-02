using Domain.Email.Ports;
using Domain.Security;
using System.Net.Mail;

namespace Infra.Email
{
    public class EmailService : IEmailService
    {
        private EmailConfigurations _emailConfigurations;

        public EmailService(EmailConfigurations emailConfigurations)
        {
            _emailConfigurations = emailConfigurations;
        }

        public async Task Send(Domain.Entities.Email email)
        {
            try
            {
                MailAddress from = new MailAddress(_emailConfigurations.FromAddress, "Health&Med");
                MailAddress to = new MailAddress(email.Appointment.Doctor.Email, email.Appointment.Doctor.Name);
                MailMessage mail = new MailMessage(from, to);

                mail.Subject = "Health&Med - Nova consulta agendada";
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                mail.Body = @$"
Olá, Dr. <b>{email.Appointment.Doctor.Name}</b>!<br>
<br>
Você tem uma nova consulta marcada!<br>
Paciente: <b>{email.Appointment.Patient.Name}</b>.<br>
Data e horário: <b>{email.Appointment.Date:dd/MM/yyyy}</b> às <b>{email.Appointment.Date:HH:mm}</b>.<br>
                ";

                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient(_emailConfigurations.Server, _emailConfigurations.Port);
                smtpClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential basicAuthenticationInfo = new
                   System.Net.NetworkCredential(_emailConfigurations.Username, _emailConfigurations.Password);
                smtpClient.Credentials = basicAuthenticationInfo;
                smtpClient.Send(mail);
            }
            catch (SmtpException ex)
            {
                throw new ApplicationException
                  ("SmtpException has occured: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
