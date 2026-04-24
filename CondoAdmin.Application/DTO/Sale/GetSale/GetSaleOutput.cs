namespace CondoAdmin.Application.DTO.Sale.GetSale;

public class GetSaleOutput
{
    public int      Id              { get; set; }
    public DateTime SaleDate        { get; set; }
    public decimal  SalePrice       { get; set; }
    public required string   MethodOfPayment { get; set; }
    public string?  Notes           { get; set; }
    public int      ResidentId      { get; set; }
    public required string   BuyerName       { get; set; }
    public required string   BuyerDNI        { get; set; } 
    public int      UnitId          { get; set; }
    public required string   UnitNumber      { get; set; } 
    public required string   BuildingName    { get; set; }
}