using System.Collections.Generic;

namespace HTTAPI.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class DeclarationViewModel
    {
        /// <summary>
        /// Symptoms
        /// </summary>
        public List<OptionsViewModel> Symptoms { get; set; }

        /// <summary>
        /// Questions
        /// </summary>
        public List<OptionsViewModel> Questions { get; set; }

        /// <summary>
        /// Zones
        /// </summary>
        public List<OptionsViewModel> Zones { get; set; }

        /// <summary>
        /// Locations
        /// </summary>
        public List<OptionsViewModel> Locations { get; set; }
    }
}
