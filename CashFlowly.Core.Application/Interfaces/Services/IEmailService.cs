
namespace CashFlowly.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpo);
    }

}
