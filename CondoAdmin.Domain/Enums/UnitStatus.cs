namespace CondoAdmin.Domain.Enums;

/// <summary>
/// Estado actual de una unidad dentro del condominio.
///
/// Se usa enum respaldado en int (no bool) porque bool solo permite
/// dos estados. Con tres estados necesitamos un tipo que escale.
///
/// Mapeo desde la BD anterior:
///   bit false (0) → Available
///   bit true  (1) → Sold
/// La migración convierte bit → int sin pérdida de datos.
/// </summary>
public enum UnitStatus
{
    Available = 0,  // Libre para venta o alquiler
    Sold      = 1,  // Comprada — transferencia de propiedad definitiva
    Rented    = 2   // Bajo contrato de alquiler activo
}