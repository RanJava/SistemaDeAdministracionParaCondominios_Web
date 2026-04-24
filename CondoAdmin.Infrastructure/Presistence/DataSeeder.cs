using CondoAdmin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CondoAdmin.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Buildings.AnyAsync())
            return;

        var torreAltura = new Building
        {
            Name = "Torre Altura", Address = "Av. Arce 2345",
            City = "La Paz", TotalUnits = 4,
            CreatedAt = new DateTime(2023, 1, 15), IsActive = true
        };
        var residencialVerde = new Building
        {
            Name = "Residencial Verde", Address = "Calle 21 de Calacoto 890",
            City = "La Paz", TotalUnits = 2,
            CreatedAt = new DateTime(2024, 3, 10), IsActive = true
        };
        context.Buildings.AddRange(torreAltura, residencialVerde);
        await context.SaveChangesAsync();

        var unidades = new List<Unit>
        {
            new() { UnitNumber = "101", Floor = 1, AreaM2 = 65.0m,  MonthlyFee = 450.00m, Status = true,  BuildingId = torreAltura.Id      },
            new() { UnitNumber = "102", Floor = 1, AreaM2 = 70.5m,  MonthlyFee = 480.00m, Status = true,  BuildingId = torreAltura.Id      },
            new() { UnitNumber = "201", Floor = 2, AreaM2 = 80.0m,  MonthlyFee = 550.00m, Status = false, BuildingId = torreAltura.Id      },
            new() { UnitNumber = "202", Floor = 2, AreaM2 = 85.5m,  MonthlyFee = 600.00m, Status = false, BuildingId = torreAltura.Id      },
            new() { UnitNumber = "A1",  Floor = 1, AreaM2 = 90.0m,  MonthlyFee = 650.00m, Status = true,  BuildingId = residencialVerde.Id  },
            new() { UnitNumber = "A2",  Floor = 1, AreaM2 = 95.0m,  MonthlyFee = 700.00m, Status = false, BuildingId = residencialVerde.Id  },
        };
        context.Units.AddRange(unidades);
        await context.SaveChangesAsync();

        var u101 = unidades[0];
        var u102 = unidades[1];
        var uA1  = unidades[4];

        var carlos  = new Resident { FirstName = "Carlos",  LastName = "Mamani",    Email = "carlos.mamani@email.com", Phone = "77712345", DNI = "12345678", MoveInDate = new DateTime(2023, 6, 1),  IsActive = true, UnitId = u101.Id };
        var lucia   = new Resident { FirstName = "Lucía",   LastName = "Fernández", Email = "lucia.fdz@email.com",     Phone = "71198765", DNI = "87654321", MoveInDate = new DateTime(2024, 1, 15), IsActive = true, UnitId = u102.Id };
        var roberto = new Resident { FirstName = "Roberto", LastName = "Quispe",    Email = "rquispe@email.com",       Phone = "69933210", DNI = "55566677", MoveInDate = new DateTime(2024, 5, 20), IsActive = true, UnitId = uA1.Id  };
        context.Residents.AddRange(carlos, lucia, roberto);
        await context.SaveChangesAsync();

        context.Sales.AddRange(
            new Sale { SaleDate = new DateTime(2023, 5, 28), SalePrice = 85000.00m,  MethodOfPayment = "Financiamiento", Notes = "Crédito BCP a 15 años", UnitId = u101.Id, ResidentId = carlos.Id  },
            new Sale { SaleDate = new DateTime(2024, 1, 10), SalePrice = 92000.00m,  MethodOfPayment = "Transferencia",  Notes = "Pago al contado",        UnitId = u102.Id, ResidentId = lucia.Id   },
            new Sale { SaleDate = new DateTime(2024, 5, 15), SalePrice = 120000.00m, MethodOfPayment = "Efectivo",       Notes = null,                     UnitId = uA1.Id,  ResidentId = roberto.Id }
        );
        await context.SaveChangesAsync();

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

        context.Visitors.AddRange(
            new Visitor { FullName = "Ana Torres",   DNI = "11223344", LicensePlate = "3456-ABC", EntryTime = new DateTime(2025, 3, 1, 10, 0, 0), ExitTime = new DateTime(2025, 3, 1, 11, 30, 0), UnitId = u101.Id },
            new Visitor { FullName = "Pedro Vargas", DNI = "99887766", LicensePlate = null,        EntryTime = new DateTime(2025, 3, 5, 14, 0, 0), ExitTime = null,                                  UnitId = u102.Id }
        );
        await context.SaveChangesAsync();

        context.MaintenanceRequests.AddRange(
            new MaintenanceRequest { Title = "Fuga de agua en baño",          Description = "Goteo constante en la llave del baño principal.",          CreatedAt = new DateTime(2025, 2, 10, 9,  0, 0), ResolvedAt = new DateTime(2025, 2, 12, 15, 0, 0), UnitId = u101.Id },
            new MaintenanceRequest { Title = "Puerta de balcón no cierra",    Description = "El seguro de la puerta corrediza del balcón está dañado.", CreatedAt = new DateTime(2025, 3, 8,  11, 0, 0), ResolvedAt = null,                                UnitId = u102.Id },
            new MaintenanceRequest { Title = "Interruptor con chispa",        Description = "El interruptor de la sala hace chispa al accionarlo.",     CreatedAt = new DateTime(2025, 3, 15, 8,  30,0), ResolvedAt = null,                                UnitId = uA1.Id  }
        );
        await context.SaveChangesAsync();
    }
}