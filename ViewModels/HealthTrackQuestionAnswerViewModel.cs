namespace HTTAPI.Models
{
    /// <summary>
    /// HealthTrackQuestionAnswerViewModel
    /// </summary>
    public class HealthTrackQuestionAnswerViewModel
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// HealthTrack ForeignKey
        /// </summary>
        public int HealthTrackId { get; set; }


        /// <summary>
        /// Question ForeignKey
        public int QuestionId { get; set; }


        /// <summary>
        /// Question 
        public string Question { get; set; }


        /// <summary>
        /// value
        /// </summary>
        public string value { get; set; }

    }
}
