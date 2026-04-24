namespace CondoAdmin.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? Notes { get; set; }

    /// <summary>Período que cubre este pago. Ej: "Enero 2025".</summary>
    public required string Month { get; set; }

    // FK — residente que debe el pago
    public int ResidentId { get; set; }
    public Resident Resident { get; set; } = null!;

    /// <summary>
    /// FK opcional al contrato de alquiler.
    /// null → pago de expensas ordinario.
    /// not null → pago mensual de alquiler generado por un contrato.
    /// </summary>
    public int? RentalContractId { get; set; }
    public RentalContract? RentalContract { get; set; }
}