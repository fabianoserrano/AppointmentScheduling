using Domain.Appointment.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Appointment
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private AppointmentSchedulingDbContext _appointmentSchedulingDbContext;
        public AppointmentRepository(AppointmentSchedulingDbContext appointmentSchedulingDbContext)
        {
            _appointmentSchedulingDbContext = appointmentSchedulingDbContext;
        }
        public async Task<int> Create(Domain.Entities.Appointment appointment)
        {
            _appointmentSchedulingDbContext.Appointment.Add(appointment);
            await _appointmentSchedulingDbContext.SaveChangesAsync();
            return appointment.Id;
        }
        public async Task<Domain.Entities.Appointment> Get(int id)
        {
            return await _appointmentSchedulingDbContext.Appointment
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<List<Domain.Entities.Appointment>> GetAppointments(int doctorId, DateTime date)
        {
            return await _appointmentSchedulingDbContext.Appointment
                .Where(a => a.Doctor.Id == doctorId && a.Date.Date == date.Date)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToListAsync();
        }
        public async Task<List<Domain.Entities.Appointment>> GetAvailableAppointments(int doctorId)
        {
            return await _appointmentSchedulingDbContext.Appointment
                .Where(a => a.Doctor.Id == doctorId && a.Patient == null)
                .Include(a => a.Doctor)
                .ToListAsync();
        }
        public async Task<int> Update(Domain.Entities.Appointment appointment)
        {
            _appointmentSchedulingDbContext.Appointment.Update(appointment);
            await _appointmentSchedulingDbContext.SaveChangesAsync();
            return appointment.Id;
        }
        public async Task Delete(int id)
        {
            var appointment = await Get(id);
            _appointmentSchedulingDbContext.Appointment.Remove(appointment);
            await _appointmentSchedulingDbContext.SaveChangesAsync();
        }
    }
}
