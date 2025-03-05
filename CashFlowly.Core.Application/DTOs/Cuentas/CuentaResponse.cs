namespace CashFlowly.Core.Application.DTOs.Cuentas
{
    public class CuentaResponse
    {
        public string Nombre { get; set; }
        public string NumeroDeCuenta { get; set; }
        public decimal SaldoDisponible { get; set; }
        public int UsuarioId { get; set; }
    }
}
