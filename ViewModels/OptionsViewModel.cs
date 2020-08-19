namespace HTTAPI.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class OptionsViewModel
    {
        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Option type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Symptom position\order
        /// </summary>
        public int Order { get; set; }
    }
}
