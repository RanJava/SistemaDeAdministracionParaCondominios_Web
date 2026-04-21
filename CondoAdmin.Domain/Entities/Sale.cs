namespace CondoAdmin.Domain.Entities;

public class Sale
{
    public int Id { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal SalePrice { get; set; }
    public string Notes { get; set; } = string.Empty;

    // FK
    public int UnitId { get; set; }
    public Unit Unit { get; set; } = null!;

    public int ResidentId { get; set; }
    public Resident Resident { get; set; } = null!;
}