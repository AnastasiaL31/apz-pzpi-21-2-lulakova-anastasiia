using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShelter_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddByStaffFieldToTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Staff_StaffId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "isAccepted",
                table: "Tasks",
                newName: "IsAccepted");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "Tasks",
                newName: "ByStaffId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_StaffId",
                table: "Tasks",
                newName: "IX_Tasks_ByStaffId");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AimStaffId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AimStaffId",
                table: "Tasks",
                column: "AimStaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Staff_AimStaffId",
                table: "Tasks",
                column: "AimStaffId",
                principalTable: "Staff",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Staff_ByStaffId",
                table: "Tasks",
                column: "ByStaffId",
                principalTable: "Staff",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Staff_AimStaffId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Staff_ByStaffId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AimStaffId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AimStaffId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "IsAccepted",
                table: "Tasks",
                newName: "isAccepted");

            migrationBuilder.RenameColumn(
                name: "ByStaffId",
                table: "Tasks",
                newName: "StaffId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ByStaffId",
                table: "Tasks",
                newName: "IX_Tasks_StaffId");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Staff_StaffId",
                table: "Tasks",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id");
        }
    }
}
