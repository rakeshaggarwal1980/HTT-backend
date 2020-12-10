namespace HTTAPI.Enums
{
    /// <summary>
    /// Use in database to set status of records
    /// like general status, proposal, content, user invitation .
    /// </summary>
    public enum EntityStatus
    {
        /// <summary>
        /// records status
        /// </summary>
        Active,

        /// <summary>
        /// Delete status
        /// </summary>
        Deleted,

        /// <summary>
        /// proposal and content status
        /// </summary>
        InProgress,

        /// <summary>
        /// Finish
        /// </summary>
        Finished,

        /// <summary>
        /// User Invitation status
        /// </summary>
        Accept,

        /// <summary>
        /// Deny
        /// </summary>
        Deny,
    }


    /// <summary>
    /// Operation\Request status
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// fail
        /// </summary>
        Fail,
        /// <summary>
        /// Sucess
        /// </summary>
        Success,
        /// <summary>
        /// Error
        /// </summary>
        Error
    }

    /// <summary>
    /// Operation\Request type
    /// </summary>
    public enum Operation
    {
        /// <summary>
        /// Create
        /// </summary>
        Create = 1,
        /// <summary>
        /// Read
        /// </summary>
        Read = 2,
        /// <summary>
        /// Update
        /// </summary>
        Update = 3,
        /// <summary>
        /// Delete
        /// </summary>
        Delete = 4,
        /// <summary>
        /// 
        /// </summary>
        SendEmail = 5
    }

    /// <summary>
    /// Type of mail to send
    /// </summary>
    public enum MailTemplate
    {
        /// <summary>
        ///  Come to office request template
        /// </summary>
        RequestToHR = 1,

        /// <summary>
        ///  HR response to request
        /// </summary>
        ResponseFromHR = 2,

        /// <summary>
        /// Employee self declaration
        /// </summary>
        EmployeeDeclaration = 3,

        /// <summary>
        ///  Registeration request to HR
        /// </summary>
        UserRegisterationRequest = 4,

        /// <summary>
        /// Approval sent to employee
        /// </summary>
        UserConfirmation = 5,

        /// <summary>
        /// 
        /// </summary>
        PasswordReset = 6,

        /// <summary>
        /// 
        /// </summary>
        EmployeeCovidDeclaration = 7
    }

    /// <summary>
    /// Employee Roles
    /// </summary>
    public enum EmployeeRolesEnum
    {
        /// <summary>
        /// 
        /// </summary>
        HRManager,
        /// <summary>
        /// Security
        /// </summary>
        SecurityManager,

        /// <summary>
        /// Employee
        /// </summary>
        Employee,
        /// <summary>
        /// 
        /// </summary>
        SuperAdmin
    }

    public enum SortDirection
    {
        Asc, Desc
    }

    public enum ExpressionOperation
    {
        Equals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        StartsWith,
        EndsWith
    }
}
