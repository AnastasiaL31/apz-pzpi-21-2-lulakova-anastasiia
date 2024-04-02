using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShelter_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAnimalInAviaryAndDietTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalsInAviary");

            migrationBuilder.DropTable(
                name: "Diets");


            migrationBuilder.AddColumn<float>(
                name: "Amount",
                table: "MealPlans",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MealPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AnimalId",
                table: "Aviaries",
                type: "int",
                nullable: false,
                defaultValue: 0);


            migrationBuilder.CreateIndex(
                name: "IX_Aviaries_AnimalId",
                table: "Aviaries",
                column: "AnimalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Aviaries_Animals_AnimalId",
                table: "Aviaries",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aviaries_Animals_AnimalId",
                table: "Aviaries");

            migrationBuilder.DropIndex(
                name: "IX_Aviaries_AnimalId",
                table: "Aviaries");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "Aviaries");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Animals");

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "Animals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "AnimalsInAviary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    AviaryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalsInAviary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalsInAviary_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalsInAviary_Aviaries_AviaryId",
                        column: x => x.AviaryId,
                        principalTable: "Aviaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Diets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diets_MealPlans_MealId",
                        column: x => x.MealId,
                        principalTable: "MealPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalsInAviary_AnimalId",
                table: "AnimalsInAviary",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalsInAviary_AviaryId",
                table: "AnimalsInAviary",
                column: "AviaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Diets_MealId",
                table: "Diets",
                column: "MealId");
        }
    }
}
