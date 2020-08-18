using HTTAPI.Enums;
using HTTAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HTTAPI.Repository.SeedData
{
    /// <summary>
    /// confirgure location data
    /// </summary>
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        /// <summary>
        /// configure master data
        /// </summary>
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasData
                 (
                    new Location
                    {
                        Id = 1,
                        Name = "Tricity (CHD/PKL/Mohali)",
                        Order = 1,
                        Status = EntityStatus.Active

                    },
                     new Location
                     {
                         Id = 2,
                         Name = "Outside Tricity",
                         Order = 2,
                         Status = EntityStatus.Active
                     }

                 );
        }
    }
}
