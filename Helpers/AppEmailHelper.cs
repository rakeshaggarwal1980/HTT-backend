using HTTAPI.Enums;
using HTTAPI.Manager.Contract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// view render service initialization
        /// </summary>
        private IViewRenderService _viewRenderService;

        /// <summary>
        /// email settings
        /// </summary>
        private AppEmailSetting _appEmailSetting;
        private readonly IHostingEnvironment _hostingEnvironment;
        /// <summary>
        /// Ctor
        /// </summary>
        public AppEmailHelper()
        {
            _appEmailSetting = AppHelper.Configuration.GetSection("EmailSetting").Get<AppEmailSetting>();

            // Service for View render service
            _viewRenderService = AppHelper.ServiceProvider.GetRequiredService<IViewRenderService>();

            // smtp client
            _client = new SmtpClient(_appEmailSetting.SmtpClient)
            {
                Port = _appEmailSetting.Port,
                Credentials = new NetworkCredential(_appEmailSetting.NetworkUserName, _appEmailSetting.NetworkPassword),
                EnableSsl = true
            };
        }

        /// <summary>
        /// Send Email
        /// </summary>
        public async void InitMailMessage()
        {
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
             //   message.Body = await PrepareMailBody();

                message.Body = AppEmailHelper.MailBody(_hostingEnvironment, emailOptions.Template);
                message.IsBodyHtml = true;

                {
                    await _client.SendMailAsync(message);
                }
            }
        }

        /// <summary>
        /// Prepare mail body
        /// </summary>
        private async Task<string> PrepareMailBody()
        {
            switch (MailType)
            {
                case MailType.RequestToHR:
                    var result = await _viewRenderService.RenderToStringAsync(EmailTemplatePath.RequestToHR, MailBodyViewModel);
                    return result.Body;
                case MailType.ResponseFromHR:
                    var result2 = await _viewRenderService.RenderToStringAsync(EmailTemplatePath.ResponseFromHR, MailBodyViewModel);
                    return result2.Body;
                default:
                    return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string MailBody(IHostingEnvironment hostingEnvironment, MailTemplate template)
        {
            var path = Path.Combine(hostingEnvironment.ContentRootPath, "MailTemplate");
            var msgBody = string.Empty;
            switch (template)
            {
                case MailTemplate.RequestToHR:
                    path += "/RequestToHR.html";
                    break;
                case MailTemplate.ResponseFromHR:
                    path += "/ResponseFromHR.html";
                    break;
            }
            if (File.Exists(path))
            {
                using (var reader = new StreamReader(path))
                {
                    msgBody = reader.ReadToEnd();
                }
            }
            return msgBody;
        }

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
