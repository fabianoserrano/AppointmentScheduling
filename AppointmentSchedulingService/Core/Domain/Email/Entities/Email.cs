using Domain.Email.Exceptions;
using Domain.Email.Ports;

namespace Domain.Entities
{
    public class Email
    {
        public int Id { get; set; }
        public Appointment Appointment { get; set; }

        private void ValidateState()
        {
            if (Appointment == null)
            {
                throw new AppointmentIsRequiredInformation();
            }
        }

        public async Task Save(IEmailRepository emailRepository)
        {
            this.ValidateState();

            if (Id == 0)
            {
                Id = await emailRepository.Create(this);
            }
        }

        public async Task Send(IEmailService emailService)
        {
            this.ValidateState();
            await emailService.Send(this);
        }
    }
}
