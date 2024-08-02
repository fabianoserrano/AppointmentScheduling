using Entities = Domain.Entities;

namespace Application.Appointment.Dtos
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public int PatientId { get; set; }

        public static Entities.Appointment MapToEntity(AppointmentDto appointmentDto)
        {
            return new Entities.Appointment
            {
                Id = appointmentDto.Id,
                Doctor = new Entities.Doctor { Id = appointmentDto.DoctorId },
                Date = appointmentDto.Date,
                Patient = new Entities.Patient { Id = appointmentDto.PatientId },
            };
        }

        public static AppointmentDto MapToDto(Entities.Appointment appointment)
        {
            return new AppointmentDto
            { 
                Id = appointment.Id,
                DoctorId = appointment.Doctor.Id,
                Date = appointment.Date,
                PatientId = appointment.Patient == null ? 0 : appointment.Patient.Id,
            };
        }
    }
}
