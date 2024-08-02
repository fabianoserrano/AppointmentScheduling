using Entities = Domain.Entities;

namespace Application.Appointment.Dtos
{
    public class CreateAppointmentDto
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }

        public static Entities.Appointment MapToEntity(CreateAppointmentDto appointmentDto)
        {
            return new Entities.Appointment
            {
                Id = appointmentDto.Id,
                Doctor = new Entities.Doctor { Id = appointmentDto.DoctorId },
                Date = appointmentDto.Date,
            };
        }

        public static CreateAppointmentDto MapToDto(Entities.Appointment appointment)
        {
            return new CreateAppointmentDto
            { 
                Id = appointment.Id,
                DoctorId = appointment.Doctor.Id,
                Date = appointment.Date,
            };
        }
    }
}
