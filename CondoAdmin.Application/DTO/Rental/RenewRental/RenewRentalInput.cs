namespace CondoAdmin.Application.DTO.Rental.RenewRental;

/// <summary>
/// Renovación del contrato existente.
/// Solo se extiende la fecha de fin — el precio no cambia.
/// Si el precio debe cambiar, crear un nuevo contrato.
/// </summary>
public class RenewRentalInput
{
    public DateTime NewEndDate { get; set; }
    public string?  Notes      { get; set; }
}