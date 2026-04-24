namespace CondoAdmin.Domain.Enums;

/// <summary>
/// Estado del ciclo de vida de un contrato de alquiler.
/// </summary>
public enum RentalContractStatus
{
    Active     = 0,  // Contrato vigente
    Terminated = 1,  // Terminado manualmente por el admin
    Cancelled  = 2   // Cancelado antes de iniciar
}