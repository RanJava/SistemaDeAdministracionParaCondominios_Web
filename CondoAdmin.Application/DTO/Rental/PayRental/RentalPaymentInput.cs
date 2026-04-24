namespace CondoAdmin.Application.DTO.Rental.PayRental;

/// <summary>
/// Pago que puede cubrir uno o varios meses pendientes a la vez,
/// y generar saldo a favor si el monto supera la deuda total.
/// </summary>
public class RentalPaymentInput
{
    public decimal Amount { get; set; }
    public required string PaymentMethod { get; set; }
    public string? Notes { get; set; }
}