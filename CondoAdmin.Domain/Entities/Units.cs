using CondoAdmin.Domain.Enums;

namespace CondoAdmin.Domain.Entities;

public class Unit
{
    public int Id { get; set; }
    public required string UnitNumber { get; set; }
    public int Floor { get; set; }
    public decimal AreaM2 { get; set; }
    public decimal MonthlyFee { get; set; }

    /// <summary>
    /// Estado actual de la unidad.
    /// Cambiado de bool a UnitStatus para soportar el estado "Alquilada".
    /// En BD: columna int (era bit). Migración convierte sin pérdida de datos.
    /// </summary>
    public UnitStatus Status { get; set; } = UnitStatus.Available;

    // FK
    public int BuildingId { get; set; }
    public Building Building { get; set; } = null!;

    // Navegación
    public ICollection<Resident>           Residents            { get; set; } = [];
    public ICollection<Visitor>            Visitors             { get; set; } = [];
    public ICollection<Sale>               Sales                { get; set; } = [];
    public ICollection<MaintenanceRequest> MaintenanceRequests  { get; set; } = [];
    public ICollection<RentalContract>     RentalContracts      { get; set; } = [];
}