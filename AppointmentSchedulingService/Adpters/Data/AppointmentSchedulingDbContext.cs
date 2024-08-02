using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppointmentSchedulingDbContext : DbContext
    {
        public AppointmentSchedulingDbContext(DbContextOptions<AppointmentSchedulingDbContext> options) : base(options) {
            if (Database.GetPendingMigrations().Any())
                Database.Migrate();
        }
        public virtual DbSet<Domain.Entities.User> User { get; set; }
        public virtual DbSet<Domain.Entities.Doctor> Doctor { get; set; }
        public virtual DbSet<Domain.Entities.Patient> Patient { get; set; }
        public virtual DbSet<Domain.Entities.Appointment> Appointment { get; set; }
        public virtual DbSet<Domain.Entities.Email> Email { get; set; }
    }
}
