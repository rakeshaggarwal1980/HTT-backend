using HTTAPI.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace HTTAPI.Helpers
{
    /// <summary>
    /// Email Helper
    /// </summary>
    public static class AppEmailHelper
    {
        /// <summary>
        /// Smtp client
        /// </summary>
        private static SmtpClient _client;
        /// <summary>
        /// email settings
        /// </summary>
      //  private static AppEmailSetting _appEmailSetting;
        /// <summary>
        /// Configuration
        /// </summary>
        public static IConfiguration Configuration { get; set; }
        /// <summary>
        /// Ctor
        /// </summary>
        public static AppEmailSetting GetMailSettings(IConfiguration configuration)
        {
            var appEmailSetting = configuration.GetSection("EmailSetting").Get<AppEmailSetting>();
            // smtp client
            _client = new SmtpClient(appEmailSetting.SmtpClient)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(appEmailSetting.NetworkUserName, appEmailSetting.NetworkPassword),
                Host = "smtp.gmail.com",
                Port = appEmailSetting.Port
            };

            return appEmailSetting;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="emailOptions"></param>
        public static async void SendMailExtended(IConfiguration configuration, EmailOptions emailOptions)
        {
            var appEmailSetting = GetMailSettings(configuration);
            SendMail(appEmailSetting, emailOptions);
        }
        /// <summary>
        /// 
        /// </summary>
        public static async void SendMail(AppEmailSetting appEmailSetting, EmailOptions emailOptions)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(appEmailSetting.FromEmail, appEmailSetting.FromName);
                emailOptions.ToMailsList.ForEach(t => message.To.Add(t.Email));
                emailOptions.ToCcMailList.ForEach(t => message.CC.Add(t.Email));
                message.Subject = emailOptions.Subject;
                message.Body = emailOptions.HtmlBody;
                message.IsBodyHtml = true;
                await _client.SendMailAsync(message);
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string MailBody(IWebHostEnvironment hostingEnvironment, MailTemplate template)
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
