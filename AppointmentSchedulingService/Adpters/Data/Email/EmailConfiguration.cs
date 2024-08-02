using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Email
{
    public class EmailConfiguration : IEntityTypeConfiguration<Domain.Entities.Email>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Email> builder)
        {
        }
    }
}
