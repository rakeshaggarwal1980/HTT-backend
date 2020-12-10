namespace HTTAPI.Helpers
{
    /// <summary>
    /// Application constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Added By Column
        /// </summary>
        public const string CreatedBy = "CreatedBy";

        /// <summary>
        /// Added Date Column
        /// </summary>
        public const string CreatedDate = "CreatedDate";

        /// <summary>
        /// The modified by column
        /// </summary>
        public const string ModifiedBy = "ModifiedBy";

        /// <summary>
        /// The modified date column
        /// </summary>
        public const string ModifiedDate = "ModifiedDate";

        /// <summary>
        /// limited collaborator to be shown
        /// </summary>
        public const int FilterCollaboratorCount = 4;

    }


    /// <summary>
    /// Email template path
    /// </summary>
    public static class EmailTemplatePath
    {
        /// <summary>
        /// Request mail template path
        /// </summary>
        public const string RequestToHR = "Views/RequestHR.cshtml";
        /// <summary>
        /// Response mail template path
        /// </summary>
        public const string ResponseFromHR = "Views/HRResponse.cshtml";

        /// <summary>
        /// 
        /// </summary>
        public const string EmployeeDeclaration = "Views/EmployeeDeclaration.cshtml";
        /// <summary>
        /// 
        /// </summary>
        public const string EmployeeConfirmation = "Views/EmployeeConfirmation.cshtml";
        /// <summary>
        /// 
        /// </summary>
        public const string RegisterationRequest = "Views/RegisterationRequestToHR.cshtml";
        /// <summary>
        /// 
        /// </summary>
        public const string PasswordResetEmail = "Views/PasswordReset.cshtml";

        /// <summary>
        /// 
        /// </summary>
        public const string EmployeeCovidDeclaration = "Views/EmployeeCovidDeclaration.cshtml";
    }

}
