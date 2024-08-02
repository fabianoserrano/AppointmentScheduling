using Entities = Domain.Entities;

namespace Application.Email.Dtos
{
    public class EmailDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }

        public static Entities.Email MapToEntity(EmailDto emailDto)
        {
            return new Entities.Email
            {
                Id = emailDto.Id,
                Appointment = new Entities.Appointment { Id = emailDto.AppointmentId },
            };
        }
        public static EmailDto MapToDto(Entities.Email email) 
        {
            return new EmailDto
            {
                Id = email.Id,
                AppointmentId = email.Appointment.Id,
            };
        }
    }
}
