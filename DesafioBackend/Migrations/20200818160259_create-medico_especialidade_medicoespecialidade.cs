using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DesafioBackend.Migrations
{
    public partial class createmedico_especialidade_medicoespecialidade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Especialidade",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidade", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Medicos",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    nome = table.Column<string>(maxLength: 100, nullable: false),
                    cpf = table.Column<string>(nullable: false),
                    crm = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MedicosEspecialidades",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    especialidadeid = table.Column<Guid>(nullable: true),
                    medicoid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicosEspecialidades", x => x.id);
                    table.ForeignKey(
                        name: "FK_MedicosEspecialidades_Especialidade_especialidadeid",
                        column: x => x.especialidadeid,
                        principalTable: "Especialidade",
                        principalColumn: "id",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicosEspecialidades_Medicos_medicoid",
                        column: x => x.medicoid,
                        principalTable: "Medicos",
                        principalColumn: "id",
                        onUpdate: ReferentialAction.Cascade,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicosEspecialidades_especialidadeid",
                table: "MedicosEspecialidades",
                column: "especialidadeid");

            migrationBuilder.CreateIndex(
                name: "IX_MedicosEspecialidades_medicoid",
                table: "MedicosEspecialidades",
                column: "medicoid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicosEspecialidades");

            migrationBuilder.DropTable(
                name: "Especialidade");

            migrationBuilder.DropTable(
                name: "Medicos");
        }
    }
}
