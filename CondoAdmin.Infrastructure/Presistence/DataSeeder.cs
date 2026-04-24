using System.Globalization;
using CondoAdmin.Domain.Entities;
using CondoAdmin.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CondoAdmin.Infrastructure.Persistence;

/// <summary>
/// Puebla la base de datos con datos iniciales de desarrollo.
///
/// Reglas de diseño:
/// - Solo corre en entorno Development (ver Program.cs).
/// - Idempotente: verifica AnyAsync() → seguro en cada reinicio.
/// - SaveChangesAsync() entre grupos para obtener IDs antes de usarlos como FK.
/// - Los pagos de alquiler se pre-generan al crear el contrato,
///   igual que lo hace el RentalController en producción.
/// </summary>
public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Buildings.AnyAsync())
            return;

        // ── 1. EDIFICIOS ───────────────────────────────────────────────
        var torreAltura = new Building
        {
            Name      = "Torre Altura",
            Address   = "Av. Arce 2345",
            City      = "La Paz",
            TotalUnits = 4,
            CreatedAt = new DateTime(2023, 1, 15),
            IsActive  = true
        };
        var residencialVerde = new Building
        {
            Name      = "Residencial Verde",
            Address   = "Calle 21 de Calacoto 890",
            City      = "La Paz",
            TotalUnits = 2,
            CreatedAt = new DateTime(2024, 3, 10),
            IsActive  = true
        };
        context.Buildings.AddRange(torreAltura, residencialVerde);
        await context.SaveChangesAsync();

        // ── 2. UNIDADES ────────────────────────────────────────────────
        // UnitStatus.Sold      → comprada, transferencia definitiva
        // UnitStatus.Available → libre para venta o alquiler
        // UnitStatus.Rented    → bajo contrato de alquiler activo
        var unidades = new List<Unit>
        {
            new() { UnitNumber = "101", Floor = 1, AreaM2 = 65.0m, MonthlyFee = 450.00m, Status = UnitStatus.Sold,      BuildingId = torreAltura.Id      },
            new() { UnitNumber = "102", Floor = 1, AreaM2 = 70.5m, MonthlyFee = 480.00m, Status = UnitStatus.Sold,      BuildingId = torreAltura.Id      },
            new() { UnitNumber = "201", Floor = 2, AreaM2 = 80.0m, MonthlyFee = 550.00m, Status = UnitStatus.Rented,    BuildingId = torreAltura.Id      },
            new() { UnitNumber = "202", Floor = 2, AreaM2 = 85.5m, MonthlyFee = 600.00m, Status = UnitStatus.Available, BuildingId = torreAltura.Id      },
            new() { UnitNumber = "A1",  Floor = 1, AreaM2 = 90.0m, MonthlyFee = 650.00m, Status = UnitStatus.Sold,      BuildingId = residencialVerde.Id },
            new() { UnitNumber = "A2",  Floor = 1, AreaM2 = 95.0m, MonthlyFee = 700.00m, Status = UnitStatus.Available, BuildingId = residencialVerde.Id },
        };
        context.Units.AddRange(unidades);
        await context.SaveChangesAsync();

        // Alias por índice — la lista es inmutable desde aquí
        var u101 = unidades[0]; // Sold
        var u102 = unidades[1]; // Sold
        var u201 = unidades[2]; // Rented  ← se usará para el contrato de alquiler
        var uA1  = unidades[4]; // Sold

        // ── 3. RESIDENTES PROPIETARIOS ─────────────────────────────────
        var carlos  = new Resident { FirstName = "Carlos",  LastName = "Mamani",    Email = "carlos.mamani@email.com", Phone = "77712345", DNI = "12345678", MoveInDate = new DateTime(2023, 6,  1),  IsActive = true, UnitId = u101.Id };
        var lucia   = new Resident { FirstName = "Lucía",   LastName = "Fernández", Email = "lucia.fdz@email.com",     Phone = "71198765", DNI = "87654321", MoveInDate = new DateTime(2024, 1, 15), IsActive = true, UnitId = u102.Id };
        var roberto = new Resident { FirstName = "Roberto", LastName = "Quispe",    Email = "rquispe@email.com",       Phone = "69933210", DNI = "55566677", MoveInDate = new DateTime(2024, 5, 20), IsActive = true, UnitId = uA1.Id  };
        context.Residents.AddRange(carlos, lucia, roberto);
        await context.SaveChangesAsync();

        // ── 4. VENTAS ──────────────────────────────────────────────────
        context.Sales.AddRange(
            new Sale { SaleDate = new DateTime(2023, 5, 28), SalePrice = 85000.00m,  MethodOfPayment = "Financiamiento", Notes = "Crédito BCP a 15 años", UnitId = u101.Id, ResidentId = carlos.Id  },
            new Sale { SaleDate = new DateTime(2024, 1, 10), SalePrice = 92000.00m,  MethodOfPayment = "Transferencia",  Notes = "Pago al contado",        UnitId = u102.Id, ResidentId = lucia.Id   },
            new Sale { SaleDate = new DateTime(2024, 5, 15), SalePrice = 120000.00m, MethodOfPayment = "Efectivo",       Notes = null,                     UnitId = uA1.Id,  ResidentId = roberto.Id }
        );
        await context.SaveChangesAsync();

        // ── 5. PAGOS DE EXPENSAS (propietarios) ───────────────────────
        // PaidAt = null → mes pendiente de pago
        context.Payments.AddRange(
            new Payment { Month = "Enero 2025",   Amount = 450.00m, DueDate = new DateTime(2025, 1, 10), PaidAt = new DateTime(2025, 1, 8),  ResidentId = carlos.Id  },
            new Payment { Month = "Febrero 2025", Amount = 450.00m, DueDate = new DateTime(2025, 2, 10), PaidAt = new DateTime(2025, 2, 12), ResidentId = carlos.Id  },
            new Payment { Month = "Marzo 2025",   Amount = 450.00m, DueDate = new DateTime(2025, 3, 10), PaidAt = null, Notes = "Pendiente",  ResidentId = carlos.Id  },
            new Payment { Month = "Enero 2025",   Amount = 480.00m, DueDate = new DateTime(2025, 1, 10), PaidAt = new DateTime(2025, 1, 9),  ResidentId = lucia.Id   },
            new Payment { Month = "Febrero 2025", Amount = 480.00m, DueDate = new DateTime(2025, 2, 10), PaidAt = new DateTime(2025, 2, 10), ResidentId = lucia.Id   },
            new Payment { Month = "Marzo 2025",   Amount = 480.00m, DueDate = new DateTime(2025, 3, 10), PaidAt = new DateTime(2025, 3, 11), ResidentId = lucia.Id   },
            new Payment { Month = "Enero 2025",   Amount = 650.00m, DueDate = new DateTime(2025, 1, 10), PaidAt = new DateTime(2025, 1, 7),  ResidentId = roberto.Id },
            new Payment { Month = "Febrero 2025", Amount = 650.00m, DueDate = new DateTime(2025, 2, 10), PaidAt = null, Notes = "En proceso", ResidentId = roberto.Id }
        );
        await context.SaveChangesAsync();

        // ── 6. VISITANTES ──────────────────────────────────────────────
        // ExitTime = null → visitante que aún no ha salido
        context.Visitors.AddRange(
            new Visitor { FullName = "Ana Torres",   DNI = "11223344", LicensePlate = "3456-ABC", EntryTime = new DateTime(2025, 3, 1, 10, 0, 0), ExitTime = new DateTime(2025, 3, 1, 11, 30, 0), UnitId = u101.Id },
            new Visitor { FullName = "Pedro Vargas", DNI = "99887766", LicensePlate = null,        EntryTime = new DateTime(2025, 3, 5, 14, 0, 0), ExitTime = null,                                  UnitId = u102.Id }
        );
        await context.SaveChangesAsync();

        // ── 7. SOLICITUDES DE MANTENIMIENTO ───────────────────────────
        // ResolvedAt = null → solicitud aún pendiente
        context.MaintenanceRequests.AddRange(
            new MaintenanceRequest { Title = "Fuga de agua en baño",       Description = "Goteo constante en la llave del baño principal.",          CreatedAt = new DateTime(2025, 2, 10, 9,   0, 0), ResolvedAt = new DateTime(2025, 2, 12, 15, 0, 0), UnitId = u101.Id },
            new MaintenanceRequest { Title = "Puerta de balcón no cierra", Description = "El seguro de la puerta corrediza del balcón está dañado.", CreatedAt = new DateTime(2025, 3,  8, 11,   0, 0), ResolvedAt = null,                                UnitId = u102.Id },
            new MaintenanceRequest { Title = "Interruptor con chispa",     Description = "El interruptor de la sala hace chispa al accionarlo.",     CreatedAt = new DateTime(2025, 3, 15,  8,  30, 0), ResolvedAt = null,                                UnitId = uA1.Id  }
        );
        await context.SaveChangesAsync();

        // ── 8. INQUILINO Y CONTRATO DE ALQUILER ───────────────────────
        // Creamos un residente inquilino (sin unidad propia — UnitId null)
        // y un contrato anual para la unidad 201 de Torre Altura.
        var maria = new Resident
        {
            FirstName  = "María",
            LastName   = "Condori",
            Email      = "mcondori@email.com",
            Phone      = "76543210",
            DNI        = "44455566",
            MoveInDate = new DateTime(2025, 1, 1),
            IsActive   = true,
            UnitId     = null  // Inquilino: no es propietario de ninguna unidad
        };
        context.Residents.Add(maria);
        await context.SaveChangesAsync();

        var contrato = new RentalContract
        {
            StartDate     = new DateTime(2025, 1, 1),
            EndDate       = new DateTime(2025, 12, 31),
            MonthlyRent   = 550.00m,
            DepositAmount = 1100.00m,   // 2 meses de garantía — práctica común
            CreditBalance = 0,
            Status        = RentalContractStatus.Active,
            Notes         = "Contrato anual 2025",
            UnitId        = u201.Id,
            ResidentId    = maria.Id
        };
        context.RentalContracts.Add(contrato);
        await context.SaveChangesAsync();

        // ── 9. PAGOS MENSUALES DEL CONTRATO ───────────────────────────
        // Se pre-generan todos los meses del contrato (Enero–Diciembre 2025).
        // Los primeros 3 meses están pagados, el resto pendiente.
        // Esto simula una situación realista de seguimiento de deuda.
        var culture = new CultureInfo("es-ES");
        var pagosAlquiler = new List<Payment>();

        for (int mes = 1; mes <= 12; mes++)
        {
            var fecha = new DateTime(2025, mes, 1);

            pagosAlquiler.Add(new Payment
            {
                // Nombre del mes en español con primera letra mayúscula
                Month            = culture.TextInfo.ToTitleCase(fecha.ToString("MMMM yyyy", culture)),
                Amount           = contrato.MonthlyRent,
                DueDate          = new DateTime(2025, mes, 5), // Vence el día 5 de cada mes
                PaidAt           = mes <= 3                    // Enero, Febrero, Marzo pagados
                                   ? new DateTime(2025, mes, 4)
                                   : null,
                ResidentId       = maria.Id,
                RentalContractId = contrato.Id  // Vinculado al contrato — no es expensa ordinaria
            });
        }

        context.Payments.AddRange(pagosAlquiler);
        await context.SaveChangesAsync();
    }
}