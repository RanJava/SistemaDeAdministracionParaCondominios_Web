namespace CondoAdmin.Application.DTO.Rental.TriggerRental;

/// <summary>
/// Input para crear uno o varios contratos de alquiler en una sola operación.
/// Si el residente no existe por DNI, se crea automáticamente.
/// </summary>
public class TriggerRentalInput
{
    public string? FirstName { get; set; }
    public string? LastName  { get; set; }
    public required string DNI   { get; set; }
    public string? Email    { get; set; }
    public string? Phone    { get; set; }

    /// <summary>Una entrada por cada unidad a alquilar.</summary>
    public List<RentalUnitInput> Units { get; set; } = [];
}

public class RentalUnitInput
{
    public required string UnitNumber    { get; set; }
    public DateTime        StartDate     { get; set; }
    public DateTime        EndDate       { get; set; }
    public decimal         MonthlyRent   { get; set; }
    public decimal         DepositAmount { get; set; }
    public string?         Notes         { get; set; }
}