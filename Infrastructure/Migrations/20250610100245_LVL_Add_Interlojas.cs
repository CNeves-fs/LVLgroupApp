using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LVL_Add_Interlojas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InterOcorrenciaId",
                table: "Ocorrencias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "MasterOcorrencia",
                table: "Ocorrencias",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterOcorrenciaId",
                table: "Ocorrencias");

            migrationBuilder.DropColumn(
                name: "MasterOcorrencia",
                table: "Ocorrencias");
        }
    }
}
