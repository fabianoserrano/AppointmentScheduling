using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Patient
{
    public class PatientRepository : IPatientRepository
    {
        private AppointmentSchedulingDbContext _appointmentSchedulingDbContext;
        public PatientRepository(AppointmentSchedulingDbContext appointmentSchedulingDbContext)
        {
            _appointmentSchedulingDbContext = appointmentSchedulingDbContext;
        }
        public async Task<int> Create(Domain.Entities.Patient patient)
        {
            _appointmentSchedulingDbContext.Patient.Add(patient);
            await _appointmentSchedulingDbContext.SaveChangesAsync();
            return patient.Id;
        }
        public Task<Domain.Entities.Patient> Get(int Id)
        {
            return _appointmentSchedulingDbContext.Patient.Where(u => u.Id == Id).FirstOrDefaultAsync();
        }
        public async Task<int> Update(Domain.Entities.Patient patient)
        {
            _appointmentSchedulingDbContext.Patient.Update(patient);
            await _appointmentSchedulingDbContext.SaveChangesAsync();
            return patient.Id;
        }
        public async Task Delete(int Id)
        {
            var patientId = await Get(Id);
            _appointmentSchedulingDbContext.Patient.Remove(patientId);
            await _appointmentSchedulingDbContext.SaveChangesAsync();
        }
        public async Task<Domain.Entities.Patient> FindByLogin(string email, string password)
        {
            return await _appointmentSchedulingDbContext.Patient.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Password.Equals(password));
        }
    }
}
