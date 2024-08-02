using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.User
{
    public class UserRepository : IUserRepository
    {
        private AppointmentSchedulingDbContext _appointmentSchedulingDbContext;
        public UserRepository(AppointmentSchedulingDbContext appointmentSchedulingDbContext)
        {
            _appointmentSchedulingDbContext = appointmentSchedulingDbContext;
        }
        public async Task<int> Create(Domain.Entities.User user)
        {
            _appointmentSchedulingDbContext.User.Add(user);
            await _appointmentSchedulingDbContext.SaveChangesAsync();
            return user.Id;
        }
        public Task<Domain.Entities.User> Get(int Id)
        {
            return _appointmentSchedulingDbContext.User.Where(u => u.Id == Id).FirstOrDefaultAsync();
        }
        public async Task<int> Update(Domain.Entities.User user)
        {
            _appointmentSchedulingDbContext.User.Update(user);
            await _appointmentSchedulingDbContext.SaveChangesAsync();
            return user.Id;
        }
        public async Task Delete(int Id)
        {
            var userId = await Get(Id);
            _appointmentSchedulingDbContext.User.Remove(userId);
            await _appointmentSchedulingDbContext.SaveChangesAsync();
        }
        public async Task<Domain.Entities.User> FindByLogin(string email)
        {
            return await _appointmentSchedulingDbContext.User.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }
    }
}
