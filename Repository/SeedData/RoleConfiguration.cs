using HTTAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HTTAPI.Repository.SeedData
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        /// <summary>
        /// configure master data
        /// </summary>
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData
                 (
                    new Role
                    {
                        Id = 1,
                        Name = "HRManager"
                    },
                     new Role
                     {
                         Id = 2,
                         Name = "SecurityManager"
                     },
                     new Role
                     {
                         Id = 3,
                         Name = "Employee"
                     }
                 );
        }
    }
}
