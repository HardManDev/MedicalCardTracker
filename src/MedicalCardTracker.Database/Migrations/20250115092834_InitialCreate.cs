using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalCardTracker.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerName = table.Column<string>(type: "text", nullable: false),
                    TargetAddress = table.Column<string>(type: "text", nullable: false),
                    PatientFullName = table.Column<string>(type: "text", nullable: false),
                    PatientBirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATE", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "DATE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardRequests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardRequests_CreatedAt",
                table: "CardRequests",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CardRequests_TargetAddress",
                table: "CardRequests",
                column: "TargetAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardRequests");
        }
    }
}
