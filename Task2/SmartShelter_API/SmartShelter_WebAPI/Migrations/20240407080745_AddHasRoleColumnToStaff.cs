using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShelter_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddHasRoleColumnToStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Staff_ByStaffId",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "ByStaffId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasRole",
                table: "Staff",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Staff_ByStaffId",
                table: "Tasks",
                column: "ByStaffId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Staff_ByStaffId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "HasRole",
                table: "Staff");

            migrationBuilder.AlterColumn<int>(
                name: "ByStaffId",
                table: "Tasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Staff_ByStaffId",
                table: "Tasks",
                column: "ByStaffId",
                principalTable: "Staff",
                principalColumn: "Id");
        }
    }
}
