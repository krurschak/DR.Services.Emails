using DR.Components.Emails.Commands;
using DR.Services.Emails.Data.Models;
using DR.Services.Emails.Data.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DR.Components.Emails.Enumeration;

namespace DR.Services.Emails.Consumers.Commands
{
    public class SendEmailConsumer : IConsumer<SendEmail>
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<SendEmailConsumer> logger;
        private readonly IOptions<EmailSettingsOptions> options;
        private readonly IRepository<Email> emailRepository;

        public SendEmailConsumer(
            IWebHostEnvironment env,
            ILogger<SendEmailConsumer> logger,
            IOptions<EmailSettingsOptions> options,
            IRepository<Email> emailRepository)
        {
            this.env = env;
            this.logger = logger;
            this.options = options;
            this.emailRepository = emailRepository;
        }

        public async Task Consume(ConsumeContext<SendEmail> context)
        {
            Email email = null;

            if (context.Message.EmailId.HasValue)
            {
                email = await emailRepository.GetAsync(context.Message.EmailId.Value);
            }

            if (email is null)
            {
                email = new Email()
                {
                    Id = NewId.NextGuid(),
                    AppId = context.Message.AppId,
                    Recipients = context.Message.Recipients,
                    Cc = context.Message.Cc,
                    Bcc = context.Message.Bcc,
                    Subject = context.Message.Subject,
                    Body = context.Message.Body,
                    Signature = context.Message.Signature,
                    CreateDateUtc = DateTime.UtcNow,
                    LastUpdateUtc = DateTime.UtcNow,
                    EmailStatus = EmailStatus.Pending
                };

                await emailRepository.InsertAsync(email);
                await emailRepository.SaveChangesAsync();
            }

            var errorMessage = await SendEmail(email, options.Value);

            var emailEntity = await emailRepository.GetAsync(email.Id);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                emailEntity.EmailStatus = EmailStatus.Failed;
                emailEntity.ErrorMessage = errorMessage;
                emailEntity.LastUpdateUtc = DateTime.UtcNow;
            }
            else
            {
                emailEntity.EmailStatus = EmailStatus.Sent;
                emailEntity.SentDateUtc = DateTime.UtcNow;
                emailEntity.LastUpdateUtc = DateTime.UtcNow;
            }

            await emailRepository.SaveChangesAsync();
        }

        private async Task<string> SendEmail(Email email, EmailSettingsOptions emailSettings)
        {
            var recipients = string.Empty;
            var cc = string.Empty;
            var bcc = string.Empty;

            if (env.IsProduction())
            {
                recipients = email.Recipients;
                cc = email.Cc;
                bcc = email.Bcc;
            }
            else
            {
                recipients = emailSettings.TestRecipients;
            }

            // preset body and subject
            var subject = string.Empty;
            var body = string.Empty;

            if (env.IsProduction())
            {
                subject = email.Subject;
            }
            else
            {
                subject = $"staging: {email.Subject}";
                body = $"CC: '{string.Join(",", email.Cc)}'<br />BCC: '{string.Join(",", email.Bcc)}'<br /><br />";
            }

            try
            {
                var message = new MimeMessage
                {
                    Subject = subject,
                    Sender = new MailboxAddress(emailSettings.FromName, emailSettings.EmailAddress)
                };

                message.From.Add(new MailboxAddress(emailSettings.FromName, emailSettings.EmailAddress));

                await SetRecipients(message, recipients, cc, bcc);

                body += email.Body + GetSignature(email, emailSettings);

                if (!body.Contains("<html"))
                {
                    body = "<html><body style=\"font-family: Calibri; font-size: 12pt;\">" + body + "</body></html>";
                }

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };
                message.Body = bodyBuilder.ToMessageBody();

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.MessageSent += (sender, args) =>
                {
                    logger.LogInformation("SMTP Response: " + args.Response);
                };
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                smtp.Connect(emailSettings.Host, emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(emailSettings.Username, emailSettings.Password);
                smtp.Send(message);
                smtp.Disconnect(true);

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private Task SetRecipients(MimeMessage message, string recipients, string cc, string bcc)
        {
            foreach (var recipient in recipients.Split(new char[] { ',' }))
            {
                message.To.Add(new MailboxAddress(System.Text.Encoding.UTF8, string.Empty, recipient));
            }

            if (!string.IsNullOrEmpty(cc))
            {
                foreach (var recipient in cc.Split(new char[] { ',' }))
                {
                    message.Cc.Add(new MailboxAddress(string.Empty, recipient));
                }
            }

            if (!string.IsNullOrEmpty(bcc))
            {
                foreach (var recipient in bcc.Split(new char[] { ',' }))
                {
                    message.Bcc.Add(new MailboxAddress(string.Empty, recipient));
                }
            }

            return Task.CompletedTask;
        }

        private string GetSignature(Email email, EmailSettingsOptions emailSettings)
        {
            if (!string.IsNullOrEmpty(email.Signature))
            {
                return email.Signature;
            }
            else
            {
                return emailSettings.Signature;
            }
        }
    }
}
