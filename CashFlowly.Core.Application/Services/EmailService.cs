using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CashFlowly.Core.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace CashFlowly.Core.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpo)
        {
            var emailEmisor = _config["EmailSettings:Remitente"];
            var password = _config["EmailSettings:Password"];
            var host = _config["EmailSettings:SmtpHost"];
            var puerto = int.Parse(_config["EmailSettings:SmtpPort"]);

            var smtpCliente = new SmtpClient(host, puerto);
            smtpCliente.EnableSsl = true;
            smtpCliente.UseDefaultCredentials = false;

            smtpCliente.Credentials = new NetworkCredential(emailEmisor, password);
            var mensaje = new MailMessage(emailEmisor!, destinatario, asunto, cuerpo);
            await smtpCliente.SendMailAsync(mensaje);
        }
    }
}
