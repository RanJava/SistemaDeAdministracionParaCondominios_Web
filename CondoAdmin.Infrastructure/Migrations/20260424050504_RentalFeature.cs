using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CondoAdmin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RentalFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Units",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "RentalContractId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RentalContracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonthlyRent = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    CreditBalance = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    ResidentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalContracts_Residents_ResidentId",
                        column: x => x.ResidentId,
                        principalTable: "Residents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentalContracts_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RentalContractId",
                table: "Payments",
                column: "RentalContractId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalContracts_ResidentId",
                table: "RentalContracts",
                column: "ResidentId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalContracts_UnitId",
                table: "RentalContracts",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_RentalContracts_RentalContractId",
                table: "Payments",
                column: "RentalContractId",
                principalTable: "RentalContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_RentalContracts_RentalContractId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "RentalContracts");

            migrationBuilder.DropIndex(
                name: "IX_Payments_RentalContractId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RentalContractId",
                table: "Payments");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Units",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
