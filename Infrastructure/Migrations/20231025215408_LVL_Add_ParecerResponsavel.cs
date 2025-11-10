using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LVL_Add_ParecerResponsavel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParecerResponsavelId",
                table: "Claims",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ParecerResponsavelId",
                table: "Claims",
                column: "ParecerResponsavelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Pareceres_ParecerResponsavelId",
                table: "Claims",
                column: "ParecerResponsavelId",
                principalTable: "Pareceres",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Pareceres_ParecerResponsavelId",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_ParecerResponsavelId",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ParecerResponsavelId",
                table: "Claims");
        }
    }
}
