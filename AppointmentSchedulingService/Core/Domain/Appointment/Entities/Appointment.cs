using Domain.Appointment.Exceptions;
using Domain.Appointment.Ports;

namespace Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime Date { get; set; }
        public Patient? Patient { get; set; }

        private void ValidateState()
        {
            if (Doctor == null)
            {
                throw new DoctorIsRequiredInformation();
            }

            if (Date == DateTime.MinValue)
            {
                throw new MissingRequiredInformation();
            }
        }

        public async Task Save(IAppointmentRepository appointmentRepository)
        {
            this.ValidateState();

            if (Id == 0)
            {
                Id = await appointmentRepository.Create(this);
            }
            else
            {
                await appointmentRepository.Update(this);
            }
        }
    }
}
