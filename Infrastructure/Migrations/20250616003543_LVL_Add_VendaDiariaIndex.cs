using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LVL_Add_VendaDiariaIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "VendasDiarias",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "GrupolojaId",
                table: "VendasDiarias",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "MercadoId",
                table: "VendasDiarias",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_VendasDiarias_EmpresaId",
                table: "VendasDiarias",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_VendasDiarias_GrupolojaId",
                table: "VendasDiarias",
                column: "GrupolojaId");

            migrationBuilder.CreateIndex(
                name: "IX_VendasDiarias_MercadoId",
                table: "VendasDiarias",
                column: "MercadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendasDiarias_Empresas_EmpresaId",
                table: "VendasDiarias",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_VendasDiarias_Gruposlojas_GrupolojaId",
                table: "VendasDiarias",
                column: "GrupolojaId",
                principalTable: "Gruposlojas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_VendasDiarias_Mercados_MercadoId",
                table: "VendasDiarias",
                column: "MercadoId",
                principalTable: "Mercados",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendasDiarias_Empresas_EmpresaId",
                table: "VendasDiarias");

            migrationBuilder.DropForeignKey(
                name: "FK_VendasDiarias_Gruposlojas_GrupolojaId",
                table: "VendasDiarias");

            migrationBuilder.DropForeignKey(
                name: "FK_VendasDiarias_Mercados_MercadoId",
                table: "VendasDiarias");

            migrationBuilder.DropIndex(
                name: "IX_VendasDiarias_EmpresaId",
                table: "VendasDiarias");

            migrationBuilder.DropIndex(
                name: "IX_VendasDiarias_GrupolojaId",
                table: "VendasDiarias");

            migrationBuilder.DropIndex(
                name: "IX_VendasDiarias_MercadoId",
                table: "VendasDiarias");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "VendasDiarias");

            migrationBuilder.DropColumn(
                name: "GrupolojaId",
                table: "VendasDiarias");

            migrationBuilder.DropColumn(
                name: "MercadoId",
                table: "VendasDiarias");
        }
    }
}
