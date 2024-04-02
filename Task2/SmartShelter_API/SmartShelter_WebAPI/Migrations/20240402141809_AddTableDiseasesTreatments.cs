using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShelter_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTableDiseasesTreatments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aviaries_Animals_AnimalId",
                table: "Aviaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Diseases_DiseaseId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_DiseaseId",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "DiseaseId",
                table: "Treatments");

            migrationBuilder.AddColumn<int>(
                name: "AnimalId",
                table: "Diseases",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AnimalId",
                table: "Aviaries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "DiseasesTreatments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiseaseId = table.Column<int>(type: "int", nullable: false),
                    TreatmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiseasesTreatments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiseasesTreatments_Diseases_DiseaseId",
                        column: x => x.DiseaseId,
                        principalTable: "Diseases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiseasesTreatments_Treatments_TreatmentId",
                        column: x => x.TreatmentId,
                        principalTable: "Treatments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Diseases_AnimalId",
                table: "Diseases",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_DiseasesTreatments_DiseaseId",
                table: "DiseasesTreatments",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DiseasesTreatments_TreatmentId",
                table: "DiseasesTreatments",
                column: "TreatmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Aviaries_Animals_AnimalId",
                table: "Aviaries",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Diseases_Animals_AnimalId",
                table: "Diseases",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aviaries_Animals_AnimalId",
                table: "Aviaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Diseases_Animals_AnimalId",
                table: "Diseases");

            migrationBuilder.DropTable(
                name: "DiseasesTreatments");

            migrationBuilder.DropIndex(
                name: "IX_Diseases_AnimalId",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "Diseases");

            migrationBuilder.AddColumn<int>(
                name: "DiseaseId",
                table: "Treatments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AnimalId",
                table: "Aviaries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_DiseaseId",
                table: "Treatments",
                column: "DiseaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Aviaries_Animals_AnimalId",
                table: "Aviaries",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_Diseases_DiseaseId",
                table: "Treatments",
                column: "DiseaseId",
                principalTable: "Diseases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
