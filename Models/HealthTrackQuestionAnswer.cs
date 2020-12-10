using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTTAPI.Models
{
    /// <summary>
    /// HealthTrack
    /// </summary>
    public class HealthTrackQuestionAnswer
    {
        /// <summary>
        /// primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        /// <summary>
        /// HealthTrack ForeignKey
        /// </summary>
        [ForeignKey("HealthTrack")] public int HealthTrackId { get; set; }

        public HealthTrack HealthTrack { get; set; }


        /// <summary>
        /// Symptom ForeignKey
        /// </summary>
        [ForeignKey("Question")] public int QuestionId { get; set; }
        public Question Question { get; set; }


        /// <summary>
        /// value
        /// </summary>
        public string value { get; set; }

    }
}
