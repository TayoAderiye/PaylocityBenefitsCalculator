using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class systemconfigtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BaseCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    DependentCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    AdditionalSalaryThreshold = table.Column<decimal>(type: "TEXT", nullable: false),
                    SalaryDeductionRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    AgeBasedDeduction = table.Column<decimal>(type: "TEXT", nullable: false),
                    PaycheckPerYear = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfig", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemConfig");
        }
    }
}
