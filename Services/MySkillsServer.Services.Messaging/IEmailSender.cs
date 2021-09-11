namespace MySkillsServer.Services.Messaging
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEmailSender
    {
        Task SendEmailAsync(
            string from,
            string fromName,
            string to,
            string toName,
            string subject,
            string htmlContent,
            string userId = null,
            IEnumerable<EmailAttachment> attachments = null);

        Task SendEmailAsync(
            string to,
            string subject,
            string htmlContent);
    }
}
