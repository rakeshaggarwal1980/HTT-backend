using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using HTTAPI.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace HTTAPI.Helpers
{
    public static class SmtpMailHelper
    {
        /// <summary>
        /// AppEmailSetting  and other settings
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static AppEmailSetting MailSettings(IConfiguration configuration)
        {
           var _appEmailSetting = AppHelper.Configuration.GetSection("EmailSetting").Get<AppEmailSetting>();
            return _appEmailSetting;
        }

           /// <summary>
        /// This method is to send email with pre-initialised settings
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task<Response> SendEmail(SendGridSetting settings, EmailOptions options)
        {
            return await SendEmailExtented(settings, options);
        }

        public static async Task<Response> SendEmail(IConfiguration configuration, EmailOptions options)
        {
            var settings = MailSettings(configuration);
            return await SendEmailExtented(settings, options);
        }

        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private static async Task<Response> SendEmailExtented(SendGridSetting settings, EmailOptions options)
        {
            var client = new SendGridClient(settings.ApiKey);
            var from = new EmailAddress(settings.FromEmail, settings.FromName);
            var to = new EmailAddress(options.ToMailsList[0].Email, options.ToMailsList[0].Name);
            var msg = MailHelper.CreateSingleEmail(from, to, options.Subject, options.PlainBody, options.HtmlBody);

            foreach (var ccMail in options.ToCcMailList.FindAll(x => x.Email != ""))
            {
                msg.AddCc(ccMail.Email, ccMail.Name);
            }
            if (options.Attachments != null && options.Attachments.Any())
            {
                msg.Attachments = options.Attachments.Select(a => new SendGrid.Helpers.Mail.Attachment
                {
                    Content = Convert.ToBase64String(a.Content),
                    Filename = a.Name,
                    Type = a.ContentType
                }).ToList();
            }


            var response = await client.SendEmailAsync(msg);
            return response;
        }

        public static async Task<Response> SendSingleEmailToMultipleRecipients(IConfiguration configuration, EmailOptions options)
        {
            var settings = MailSettings(configuration);
            var client = new SendGridClient(settings.ApiKey);
            var from = new EmailAddress(settings.FromEmail, settings.FromName);
            List<EmailAddress> tos = options.ToMailsList.Select(t => new EmailAddress(t.Email, t.Name)).ToList();

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, options.Subject, options.PlainBody, options.HtmlBody, false);
            foreach (var ccMail in options.ToCcMailList.FindAll(x => x.Email != ""))
            {
                msg.AddCc(ccMail.Email, ccMail.Name);
            }
            if (options.Attachments != null && options.Attachments.Any())
            {
                msg.Attachments = options.Attachments.Select(a => new SendGrid.Helpers.Mail.Attachment
                {
                    Content = Convert.ToBase64String(a.Content),
                    Filename = a.Name,
                    Type = a.ContentType
                }).ToList();
            }


            var response = await client.SendEmailAsync(msg);
            return response;
        }

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
    }


}


