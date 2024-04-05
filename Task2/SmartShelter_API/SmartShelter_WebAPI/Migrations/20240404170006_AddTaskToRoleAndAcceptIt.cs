using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShelter_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskToRoleAndAcceptIt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Staff_StaffId",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "StaffId",
                table: "Tasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "StaffRole",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "isAccepted",
                table: "Tasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Staff_StaffId",
                table: "Tasks",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Staff_StaffId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "StaffRole",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "isAccepted",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "StaffId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Staff_StaffId",
                table: "Tasks",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
