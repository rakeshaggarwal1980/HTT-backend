using HTTAPI.Enums;
using HTTAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HTTAPI.Repository.SeedData
{
    /// <summary>
    /// confirgure Question data
    /// </summary>
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        /// <summary>
        /// configure master data
        /// </summary>
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasData
                 (
                    new Question
                    {
                        Id = 1,
                        Name = "Please give a count of family members in your house?",
                        Type = "Input",
                        Order = 1,
                        Status = EntityStatus.Active

                    },
                     new Question
                     {
                         Id = 2,
                         Name = "Is any Of them under 5 years or over 65 years in age?",
                         Type = "CheckBox",
                         Order = 2,
                         Status = EntityStatus.Active

                     },
                      new Question
                      {
                          Id = 3,
                          Name = "Has any Of them presented Covid-19 related symptoms recently?",
                          Type = "CheckBox",
                          Order = 3,
                          Status = EntityStatus.Active

                      },
                       new Question
                       {
                           Id = 4,
                           Name = "Has any Of them had any recent travel — abroad, inter-state or inter or district ? ",
                           Type = "CheckBox",
                           Order = 4,
                           Status = EntityStatus.Active

                       }

                 );
        }
    }
}
