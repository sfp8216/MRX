using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MRX.SER.DTO;
using MRX.SER.Interfaces;

namespace MRX.SER
{
    public class MailJet : IEmail
    {
        public async Task Send(string emailAddress, string body, EmailOptionsDTO options)
        {
            var client = new SmtpClient();
            client.Host = options.Host;
            client.Credentials = new NetworkCredential(options.ApiKey, options.ApiKeySecret);
            client.Port = options.Port;

            var message = new MailMessage(options.SenderEmail, emailAddress);
            message.Body = body;
            message.IsBodyHtml = true;

            await client.SendMailAsync(message);
        }
    }
}