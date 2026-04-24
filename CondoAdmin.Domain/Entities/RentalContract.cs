using CondoAdmin.Domain.Enums;

namespace CondoAdmin.Domain.Entities;

/// <summary>
/// Contrato de alquiler entre el condominio y un inquilino.
///
/// Un residente puede tener múltiples contratos activos (una por unidad).
/// Cuando se renueva, se extiende EndDate y se generan nuevos Payment
/// para los meses adicionales — no se crea un contrato nuevo.
///
/// Los pagos mensuales se pre-generan al crear el contrato para tener
/// visibilidad inmediata de qué meses están pendientes o pagados.
/// </summary>
public class RentalContract
{
    public int Id { get; set; }

    /// <summary>Primer día del primer mes del contrato.</summary>
    public DateTime StartDate { get; set; }

    /// <summary>Último día del último mes del contrato.</summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Monto mensual fijo acordado en el contrato.
    /// No cambia durante la vigencia. Para precio diferente = nuevo contrato.
    /// </summary>
    public decimal MonthlyRent { get; set; }

    /// <summary>
    /// Depósito de garantía pagado al inicio.
    /// Se registra como dato del contrato, no como Payment independiente.
    /// </summary>
    public decimal DepositAmount { get; set; }

    /// <summary>
    /// Saldo a favor del inquilino.
    /// Se acumula cuando paga más de lo adeudado y se aplica
    /// automáticamente al procesar el siguiente pago pendiente.
    /// </summary>
    public decimal CreditBalance { get; set; } = 0;

    public RentalContractStatus Status { get; set; } = RentalContractStatus.Active;

    public string? Notes { get; set; }

    // FK — unidad alquilada
    public int UnitId { get; set; }
    public Unit Unit { get; set; } = null!;

    // FK — inquilino
    public int ResidentId { get; set; }
    public Resident Resident { get; set; } = null!;

    // Pagos mensuales asociados a este contrato
    public ICollection<Payment> Payments { get; set; } = [];
}