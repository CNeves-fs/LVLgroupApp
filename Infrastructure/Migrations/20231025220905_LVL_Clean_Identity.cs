using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LVL_Clean_Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gruposupervisores");

            migrationBuilder.DropTable(
                name: "Lojagerentes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gruposupervisores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrupolojaId = table.Column<int>(type: "int", nullable: false),
                    SupervisorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gruposupervisores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gruposupervisores_AspNetUsers_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gruposupervisores_Gruposlojas_GrupolojaId",
                        column: x => x.GrupolojaId,
                        principalTable: "Gruposlojas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lojagerentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GerentelojaId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LojaId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lojagerentes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lojagerentes_AspNetUsers_GerentelojaId",
                        column: x => x.GerentelojaId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lojagerentes_Lojas_LojaId",
                        column: x => x.LojaId,
                        principalTable: "Lojas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gruposupervisores_GrupolojaId",
                table: "Gruposupervisores",
                column: "GrupolojaId");

            migrationBuilder.CreateIndex(
                name: "IX_Gruposupervisores_SupervisorId",
                table: "Gruposupervisores",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Lojagerentes_GerentelojaId",
                table: "Lojagerentes",
                column: "GerentelojaId");

            migrationBuilder.CreateIndex(
                name: "IX_Lojagerentes_LojaId",
                table: "Lojagerentes",
                column: "LojaId");
        }
    }
}
