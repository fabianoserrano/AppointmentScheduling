using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Appointment
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Domain.Entities.Appointment>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Appointment> builder)
        {
            /*// Configuração da chave estrangeira para User (se aplicável)
            builder.HasOne(p => p.User)
                .WithMany() // Portfolio pode ter apenas um User (depende do seu modelo)
                .HasForeignKey(p => p.User.Id)
                .IsRequired(); // Define que UserId é obrigatório*/
        }
    }
}
