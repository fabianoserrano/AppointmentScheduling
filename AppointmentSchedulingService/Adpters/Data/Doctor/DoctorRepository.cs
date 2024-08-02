using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Doctor
{
    public class DoctorRepository : IDoctorRepository
    {
        private AppointmentSchedulingDbContext _appointmentSchedulingDbContext;
        public DoctorRepository(AppointmentSchedulingDbContext appointmentSchedulingDbContext)
        {
            _appointmentSchedulingDbContext = appointmentSchedulingDbContext;
        }
        public async Task<int> Create(Domain.Entities.Doctor doctor)
        {
            _appointmentSchedulingDbContext.Doctor.Add(doctor);
            await _appointmentSchedulingDbContext.SaveChangesAsync();
            return doctor.Id;
        }
        public Task<Domain.Entities.Doctor> Get(int Id)
        {
            return _appointmentSchedulingDbContext.Doctor.Where(u => u.Id == Id).FirstOrDefaultAsync();
        }
        public Task<List<Domain.Entities.Doctor>> Get()
        {
            return _appointmentSchedulingDbContext.Doctor.ToListAsync();
        }
        public async Task<int> Update(Domain.Entities.Doctor doctor)
        {
            _appointmentSchedulingDbContext.Doctor.Update(doctor);
            await _appointmentSchedulingDbContext.SaveChangesAsync();
            return doctor.Id;
        }
        public async Task Delete(int Id)
        {
            var doctorId = await Get(Id);
            _appointmentSchedulingDbContext.Doctor.Remove(doctorId);
            await _appointmentSchedulingDbContext.SaveChangesAsync();
        }
        public async Task<Domain.Entities.Doctor> FindByLogin(string email, string password)
        {
            return await _appointmentSchedulingDbContext.Doctor.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Password.Equals(password));
        }
    }
}
