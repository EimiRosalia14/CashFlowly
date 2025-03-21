namespace CashFlowly.Core.Application.DTOs.Cuentas
{
    public class UpdateCuentaDTO
    {
        public string Nombre { get; set; }
        public string NumeroDeCuenta { get; set; }
        public decimal SaldoDisponible { get; set; }
    }
}
