using Application.Email.Dtos;
using Application.Email.Ports;
using Application.Email.Requests;
using Application.Email.Responses;
using Domain.Email.Ports;
using Domain.Email.Exceptions;
using Domain.Appointment.Ports;

namespace Application.Patient
{
    public class EmailManager : IEmailManager
    {
        private IEmailRepository _emailRepository;
        private IEmailService _emailService;
        private IAppointmentRepository _appointmentRepository;

        public EmailManager(IEmailRepository patientRepository,
                            IEmailService emailService,
                            IAppointmentRepository appointmentRepository)
        {
            _emailRepository = patientRepository;
            _emailService = emailService;
            _appointmentRepository = appointmentRepository;
        }
        public async Task<EmailResponse> SendEmail(SendEmailRequest request)
        {
            try
            {
                var email = EmailDto.MapToEntity(request.Data);

                email.Appointment = await _appointmentRepository.Get(email.Appointment.Id);

                try
                {
                    await email.Send(_emailService);
                }
                catch (Exception ex)
                {
                    // Armazena email para envio posterior
                    await email.Save(_emailRepository);
                }

                request.Data.Id = email.Id;

                return new EmailResponse
                {
                    Data = request.Data,
                    Success = true,
                };
            }
            catch (AppointmentIsRequiredInformation)
            {
                return new EmailResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.EMAIL_MISSING_REQUIRED_INFORMATION,
                    Message = "The appointment id provided was not found"
                };
            }
            catch (Exception)
            {
                return new EmailResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.EMAIL_COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }
    }
}
