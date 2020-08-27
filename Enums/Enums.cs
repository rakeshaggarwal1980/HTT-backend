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
        Delete = 4
    }

    /// <summary>
    /// Type of mail to send
    /// </summary>
    public enum MailTemplate
    {
        /// <summary>
        /// TBD
        /// </summary>
        RequestToHR = 1,

        /// <summary>
        /// TBD
        /// </summary>
        ResponseFromHR = 2
    }
}
