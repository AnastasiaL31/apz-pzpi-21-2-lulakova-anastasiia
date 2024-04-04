using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShelter_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFKIdentityToStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Staff",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_IdentityUserId",
                table: "Staff",
                column: "IdentityUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_AspNetUsers_IdentityUserId",
                table: "Staff",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_AspNetUsers_IdentityUserId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Staff_IdentityUserId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Staff");
        }
    }
}
