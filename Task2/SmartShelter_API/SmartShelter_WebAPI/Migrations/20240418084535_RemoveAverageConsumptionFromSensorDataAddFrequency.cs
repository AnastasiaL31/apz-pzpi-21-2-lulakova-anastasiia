using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShelter_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAverageConsumptionFromSensorDataAddFrequency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Staff_StaffId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AvgConsumption",
                table: "SensorsData");

            migrationBuilder.AddColumn<int>(
                name: "Frequency",
                table: "Sensors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "StaffId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Staff_StaffId",
                table: "Orders",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Staff_StaffId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "Sensors");

            migrationBuilder.AddColumn<float>(
                name: "AvgConsumption",
                table: "SensorsData",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<int>(
                name: "StaffId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Staff_StaffId",
                table: "Orders",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
