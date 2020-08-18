using HTTAPI.Enums;
using HTTAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HTTAPI.Repository.SeedData
{
    /// <summary>
    /// confirgure Zone data
    /// </summary>
    public class ZoneConfiguration : IEntityTypeConfiguration<Zone>
    {
        /// <summary>
        /// configure master data
        /// </summary>
        public void Configure(EntityTypeBuilder<Zone> builder)
        {
            builder.HasData
                 (
                    new Zone
                    {
                        Id = 1,
                        Name = "Containment Zone",
                        Order = 1,
                        Status = EntityStatus.Active

                    },
                     new Zone
                     {
                         Id = 2,
                         Name = "Red Zone",
                         Order = 2,
                         Status = EntityStatus.Active

                     },
                      new Zone
                      {
                          Id = 3,
                          Name = "Green Zone",
                          Order = 3,
                          Status = EntityStatus.Active

                      },
                       new Zone
                       {
                           Id = 4,
                           Name = "Orange Zone",
                           Order = 4,
                           Status = EntityStatus.Active

                       }

                 );
        }
    }
}
