namespace HTTAPI.Models
{
    /// <summary>
    /// HealthTrack
    /// </summary>
    public class HealthTrackSymptomViewModel
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// HealthTrackId
        /// </summary>
        public int HealthTrackId { get; set; }

        /// <summary>
        /// SymptomId
        /// </summary>
        public int SymptomId { get; set; }

        /// <summary>
        /// Symptom Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// value
        /// </summary>
        public bool value { get; set; }

    }
}
