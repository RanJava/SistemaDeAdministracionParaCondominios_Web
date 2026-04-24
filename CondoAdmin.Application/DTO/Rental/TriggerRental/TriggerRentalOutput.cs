namespace CondoAdmin.Application.DTO.Rental.TriggerRental;

public class TriggerRentalOutput
{
    public string Tenant    { get; set; } = string.Empty;
    public string DNI       { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<RentalContractOutput> Contracts { get; set; } = [];
}

public class RentalContractOutput
{
    public int      ContractId         { get; set; }
    public string   UnitNumber         { get; set; } = string.Empty;
    public string   BuildingName       { get; set; } = string.Empty;
    public DateTime StartDate          { get; set; }
    public DateTime EndDate            { get; set; }
    public decimal  MonthlyRent        { get; set; }
    public decimal  DepositAmount      { get; set; }
    public int      TotalMonths        { get; set; }
    public decimal  TotalContractValue { get; set; }
}