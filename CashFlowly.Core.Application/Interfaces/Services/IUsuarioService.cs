namespace CashFlowly.Core.Application.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<string> IngresarSaldo(int id, decimal saldo);
    }
}