using HTTAPI.Enums;
using HTTAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HTTAPI.Repository.SeedData
{
    /// <summary>
    /// confirgure Symptom data
    /// </summary>
    public class SymptomConfiguration : IEntityTypeConfiguration<Symptom>
    {
        /// <summary>
        /// configure master data
        /// </summary>
        public void Configure(EntityTypeBuilder<Symptom> builder)
        {
            builder.HasData
                 (
                    new Symptom
                    {
                        Id = 1,
                        Name = "Fever",
                        Type = "Radio",
                        Order = 1,
                        Status = EntityStatus.Active

                    },
                     new Symptom
                     {
                         Id = 2,
                         Name = "Shortness Of Breath",
                         Type = "Radio",
                         Order = 2,
                         Status = EntityStatus.Active
                     },
                      new Symptom
                      {
                          Id = 3,
                          Name = "Dry Cough",
                          Type = "Radio",
                          Order = 3,
                          Status = EntityStatus.Active
                      },
                       new Symptom
                       {
                           Id = 4,
                           Name = "Running Nose",
                           Type = "Radio",
                           Order = 4,
                           Status = EntityStatus.Active
                       },
                        new Symptom
                        {
                            Id = 5,
                            Name = "Sore Throat",
                            Type = "Radio",
                            Order = 5,
                            Status = EntityStatus.Active
                        }

                 );
        }
    }
}
