namespace CondoAdmin.Application.DTO.Rental.PayRental;

public class RentalPaymentOutput
{
    public int      ContractId       { get; set; }
    public required string   TenantName       { get; set; }
    public required string   UnitNumber       { get; set; } 
    public decimal  AmountReceived   { get; set; }
    public decimal  AmountApplied    { get; set; }
    public decimal  NewCreditBalance { get; set; }

    /// <summary>Lista de períodos (meses) marcados como pagados en esta operación.</summary>
    public List<string> PeriodsPaid { get; set; } = [];
    public required string Message { get; set; }
}