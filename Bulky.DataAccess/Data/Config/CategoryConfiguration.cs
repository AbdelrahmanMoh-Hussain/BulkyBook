using Bulky.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulky.DataAccess.Data.Config
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x  => x.Name)
                .HasMaxLength(256)
                .IsRequired();

            builder.HasData(new List<Category>
            {
                new Category { Id = 1, Name = "Action", DisplayOrder = 3},
                new Category { Id = 2, Name = "Sci-Fic", DisplayOrder = 2},
            });
        }
    }
}
