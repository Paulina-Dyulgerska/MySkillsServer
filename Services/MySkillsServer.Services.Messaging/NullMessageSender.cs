namespace MySkillsServer.Services.Messaging
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class NullMessageSender : IEmailSender
    {
        public Task SendEmailAsync(
            string from,
            string fromName,
            string to,
            string toName,
            string subject,
            string htmlContent,
            string userId = null,
            IEnumerable<EmailAttachment> attachments = null)
        {
            return Task.CompletedTask;
        }

        public Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            return Task.CompletedTask;
        }
    }
}
