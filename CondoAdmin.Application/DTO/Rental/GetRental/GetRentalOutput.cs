namespace CondoAdmin.Application.DTO.Rental.GetRental;

public class GetRentalOutput
{
    public int      Id            { get; set; }
    public required string   TenantName    { get; set; } 
    public required string   TenantDNI     { get; set; } 
    public required string   UnitNumber    { get; set; } 
    public required string   BuildingName  { get; set; } 
    public DateTime StartDate     { get; set; }
    public DateTime EndDate       { get; set; }
    public decimal  MonthlyRent   { get; set; }
    public decimal  DepositAmount { get; set; }
    public decimal  CreditBalance { get; set; }
    public required string   Status        { get; set; } 

    /// <summary>Meses con PaidAt == null.</summary>
    public int     PendingMonths { get; set; }

    /// <summary>PendingMonths × MonthlyRent (descontando CreditBalance).</summary>
    public decimal TotalDebt     { get; set; }
}