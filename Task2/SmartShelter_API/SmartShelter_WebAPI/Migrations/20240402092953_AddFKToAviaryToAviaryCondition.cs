using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShelter_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFKToAviaryToAviaryCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AviariesConditions_Aviaries_AviaryId",
                table: "AviariesConditions");

            migrationBuilder.DropIndex(
                name: "IX_AviariesConditions_AviaryId",
                table: "AviariesConditions");

            migrationBuilder.DropColumn(
                name: "AviaryId",
                table: "AviariesConditions");

            migrationBuilder.AddColumn<int>(
                name: "AviaryConditionId",
                table: "Aviaries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Aviaries_AviaryConditionId",
                table: "Aviaries",
                column: "AviaryConditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Aviaries_AviariesConditions_AviaryConditionId",
                table: "Aviaries",
                column: "AviaryConditionId",
                principalTable: "AviariesConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aviaries_AviariesConditions_AviaryConditionId",
                table: "Aviaries");

            migrationBuilder.DropIndex(
                name: "IX_Aviaries_AviaryConditionId",
                table: "Aviaries");

            migrationBuilder.DropColumn(
                name: "AviaryConditionId",
                table: "Aviaries");

            migrationBuilder.AddColumn<int>(
                name: "AviaryId",
                table: "AviariesConditions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AviariesConditions_AviaryId",
                table: "AviariesConditions",
                column: "AviaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AviariesConditions_Aviaries_AviaryId",
                table: "AviariesConditions",
                column: "AviaryId",
                principalTable: "Aviaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
