using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxGarage.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePrimary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "VehicleImages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "VehicleImages",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
