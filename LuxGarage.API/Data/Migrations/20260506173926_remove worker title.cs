using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxGarage.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class removeworkertitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Workers_EmployeeId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Permissions_PermissionId",
                table: "Workers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Workplaces_WorkplaceId",
                table: "Workers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workers",
                table: "Workers");

            migrationBuilder.RenameTable(
                name: "Workers",
                newName: "Employees");

            migrationBuilder.RenameIndex(
                name: "IX_Workers_WorkplaceId",
                table: "Employees",
                newName: "IX_Employees_WorkplaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Workers_PermissionId",
                table: "Employees",
                newName: "IX_Employees_PermissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Permissions_PermissionId",
                table: "Employees",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Workplaces_WorkplaceId",
                table: "Employees",
                column: "WorkplaceId",
                principalTable: "Workplaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Employees_EmployeeId",
                table: "Rentals",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Permissions_PermissionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Workplaces_WorkplaceId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Employees_EmployeeId",
                table: "Rentals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Workers");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_WorkplaceId",
                table: "Workers",
                newName: "IX_Workers_WorkplaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_PermissionId",
                table: "Workers",
                newName: "IX_Workers_PermissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workers",
                table: "Workers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Workers_EmployeeId",
                table: "Rentals",
                column: "EmployeeId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Permissions_PermissionId",
                table: "Workers",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Workplaces_WorkplaceId",
                table: "Workers",
                column: "WorkplaceId",
                principalTable: "Workplaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
