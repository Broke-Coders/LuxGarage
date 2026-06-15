using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxGarage.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserEmployeeRequestAndVehicleEngineName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "maxSpeed",
                table: "Vehicles");

            migrationBuilder.AddColumn<string>(
                name: "EngineName",
                table: "Vehicles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "SpeedToHundred",
                table: "Vehicles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "EmployeeRequested",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EngineName",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "SpeedToHundred",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "EmployeeRequested",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "maxSpeed",
                table: "Vehicles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
