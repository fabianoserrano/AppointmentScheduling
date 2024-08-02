using Entities = Domain.Entities;

namespace Application.Appointment.Dtos
{
    public class AvailableAppointmentDto
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }

        public static Entities.Appointment MapToEntity(AvailableAppointmentDto appointmentDto)
        {
            return new Entities.Appointment
            {
                Id = appointmentDto.Id,
                Doctor = new Entities.Doctor { Id = appointmentDto.DoctorId },
                Date = appointmentDto.Date,
            };
        }

        public static AvailableAppointmentDto MapToDto(Entities.Appointment appointment)
        {
            return new AvailableAppointmentDto
            { 
                Id = appointment.Id,
                DoctorId = appointment.Doctor.Id,
                Date = appointment.Date,
            };
        }
    }
}
