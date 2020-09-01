using HTTAPI.Enums;
using HTTAPI.Manager.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HTTAPI.Helpers
{
    /// <summary>
    /// Email Helper
    /// </summary>
    public class AppEmailHelper
    {
        /// <summary>
        /// Smtp client
        /// </summary>
        private SmtpClient _client;
        private AppEmailSetting _appEmailSetting;
        /// <summary>
        /// 
        /// </summary>
        private IViewRenderService _viewRenderService;
        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// To mailing list
        /// </summary>
        public List<MailAddress> ToMailAddresses { get; set; } = new List<MailAddress>();

        /// <summary>
        /// Cc mailing list
        /// </summary>
        public List<MailAddress> CCMailAddresses { get; set; } = new List<MailAddress>();

        /// <summary>
        /// Bcc mailing list
        /// </summary>
        public List<MailAddress> BCCMailAddresses { get; set; } = new List<MailAddress>();

        /// <summary>
        /// From mail address
        /// </summary>
        public MailAddress FromMailAddress { get; set; }

        /// <summary>
        /// Mail Subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Type/Category of mail
        /// </summary>
        public MailTemplate MailTemplate { get; set; }

        /// <summary>
        /// Provide mail body viewmodel to prepare the email body as html template
        /// </summary>
        public object MailBodyViewModel { get; set; }
        /// <summary>
        /// Ctor
        /// </summary>
        /// 
        public AppEmailHelper()
        {
            _appEmailSetting = AppHelper.Configuration.GetSection("EmailSetting").Get<AppEmailSetting>();
            // Service for View render service
            _viewRenderService = AppHelper.ServiceProvider.GetRequiredService<IViewRenderService>();

            // smtp client
            _client = new SmtpClient(_appEmailSetting.SmtpClient)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_appEmailSetting.NetworkUserName, _appEmailSetting.NetworkPassword),
                Host = "smtp.gmail.com",
                Port = _appEmailSetting.Port
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<string> PrepareMailBody()
        {
            switch (MailTemplate)
            {
                case MailTemplate.RequestToHR:
                    var result = await _viewRenderService.RenderToStringAsync(EmailTemplatePath.RequestToHR, MailBodyViewModel);
                    return result.Body;
                case MailTemplate.ResponseFromHR:
                    var result2 = await _viewRenderService.RenderToStringAsync(EmailTemplatePath.ResponseFromHR, MailBodyViewModel);
                    return result2.Body;
                case MailTemplate.EmployeeDeclaration:
                    var result3 = await _viewRenderService.RenderToStringAsync(EmailTemplatePath.EmployeeDeclaration, MailBodyViewModel);
                    return result3.Body;
                case MailTemplate.UserConfirmation:
                    var result4 = await _viewRenderService.RenderToStringAsync(EmailTemplatePath.EmployeeConfirmation, MailBodyViewModel);
                    return result4.Body;
                case MailTemplate.UserRegisterationRequest:
                    var result5= await _viewRenderService.RenderToStringAsync(EmailTemplatePath.RegisterationRequest, MailBodyViewModel);
                    return result5.Body;
                default:
                    return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IResult> InitMailMessage()
        {
            var result = new Result
            {
                Operation = Operation.SendEmail,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };
            using (var message = new MailMessage())
            {
                if (FromMailAddress == null)
                    message.From = new MailAddress(_appEmailSetting.FromEmail, _appEmailSetting.FromName);
                else
                    message.From = FromMailAddress;
                ToMailAddresses.ForEach(t => message.To.Add(t));
                CCMailAddresses.ForEach(t => message.CC.Add(t));
                BCCMailAddresses.ForEach(t => message.Bcc.Add(t));

                message.Subject = Subject;
                message.Body = await PrepareMailBody();
                message.IsBodyHtml = true;
                try
                {
                    await _client.SendMailAsync(message);
                }
                catch (Exception e)
                {
                    result.Message = e.Message;
                    result.StatusCode = HttpStatusCode.InternalServerError;
                    result.Status = Status.Error;
                }
            }
            return result;
        }

        /// <summary>
        /// App email settings
        /// </summary>
        public class AppEmailSetting
        {
            /// <summary>
            /// smtp client
            /// </summary>
            [JsonProperty("smtpClient")]
            public string SmtpClient { get; set; }

            /// <summary>
            /// port
            /// </summary>
            [JsonProperty("port")]
            public int Port { get; set; }

            /// <summary>
            /// network user name
            /// </summary>
            [JsonProperty("networkUserName")]
            public string NetworkUserName { get; set; }

            /// <summary>
            /// network password
            /// </summary>
            [JsonProperty("networkPassword")]
            public string NetworkPassword { get; set; }

            /// <summary>
            /// mail sent from address
            /// </summary>
            [JsonProperty("fromEmail")]
            public string FromEmail { get; set; }

            /// <summary>
            /// Mail sent from address name
            /// </summary>
            [JsonProperty("fromName")]
            public string FromName { get; set; }
        }
    }
}
