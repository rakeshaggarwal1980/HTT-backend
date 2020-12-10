using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTTAPI.Models
{
    /// <summary>
    /// HealthTrack
    /// </summary>
    public class HealthTrackSymptom
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
        [ForeignKey("Symptom")] public int SymptomId { get; set; }
        public Symptom Symptom { get; set; }


        /// <summary>
        /// value
        /// </summary>
        public bool value { get; set; }

    }
}
