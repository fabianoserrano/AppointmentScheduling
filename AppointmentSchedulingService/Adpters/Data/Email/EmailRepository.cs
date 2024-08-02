using Domain.Email.Ports;

namespace Data.Appointment
{
    public class EmailRepository : IEmailRepository
    {
        private AppointmentSchedulingDbContext _appointmentSchedulingDbContext;
        public EmailRepository(AppointmentSchedulingDbContext appointmentSchedulingDbContext)
        {
            _appointmentSchedulingDbContext = appointmentSchedulingDbContext;
        }
        public async Task<int> Create(Domain.Entities.Email email)
        {
            _appointmentSchedulingDbContext.Email.Add(email);
            await _appointmentSchedulingDbContext.SaveChangesAsync();
            return email.Id;
        }
    }
}
