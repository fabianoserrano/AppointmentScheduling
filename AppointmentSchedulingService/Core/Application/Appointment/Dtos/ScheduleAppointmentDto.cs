using Entities = Domain.Entities;

namespace Application.Appointment.Dtos
{
    public class ScheduleAppointmentDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }

        public static Entities.Appointment MapToEntity(ScheduleAppointmentDto appointmentDto)
        {
            return new Entities.Appointment
            {
                Id = appointmentDto.Id,
                Patient = new Entities.Patient { Id = appointmentDto.PatientId },
            };
        }

        public static ScheduleAppointmentDto MapToDto(Entities.Appointment appointment)
        {
            return new ScheduleAppointmentDto
            { 
                Id = appointment.Id,
                PatientId = appointment.Patient.Id,
            };
        }
    }
}
