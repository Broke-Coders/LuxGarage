using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxGarage.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleStatusLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "VehicleStatus",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VehicleStatusId",
                table: "Vehicles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleStatusId",
                table: "Vehicles",
                column: "VehicleStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleStatus_VehicleStatusId",
                table: "Vehicles",
                column: "VehicleStatusId",
                principalTable: "VehicleStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleStatus_VehicleStatusId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_VehicleStatusId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "VehicleStatus");

            migrationBuilder.DropColumn(
                name: "VehicleStatusId",
                table: "Vehicles");
        }
    }
}
