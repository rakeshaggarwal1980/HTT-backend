using HTTAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HTTAPI.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailOptions
    {
        public List<MailUser> ToMailsList { get; set; } = new List<MailUser>();
        public List<MailUser> ToCcMailList { get; set; } = new List<MailUser>();
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string PlainBody { get; set; }
        public MailTemplate Template { get; set; }
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class MailUser
    {
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}
