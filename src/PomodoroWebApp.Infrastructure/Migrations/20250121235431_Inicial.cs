using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PomodoroWebApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Baja = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadisticasUsuario",
                columns: table => new
                {
                    EstadisticasUsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProyectosCompletados = table.Column<int>(type: "int", nullable: false),
                    TareasCompletadas = table.Column<int>(type: "int", nullable: false),
                    CiclosCompletados = table.Column<int>(type: "int", nullable: false),
                    MinutosTrabajo = table.Column<int>(type: "int", nullable: false),
                    MinutosDescanso = table.Column<int>(type: "int", nullable: false),
                    MaximoTrabajoSinDescanso = table.Column<int>(type: "int", nullable: false),
                    PromedioDescanso = table.Column<int>(type: "int", nullable: false),
                    MinutosTrabajoPorTarea = table.Column<int>(type: "int", nullable: false),
                    MinutosTrabajoPorProyecto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadisticasUsuario", x => x.EstadisticasUsuarioId);
                    table.ForeignKey(
                        name: "FK_EstadisticasUsuario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proyectos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tareas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Prioridad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProyectoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tareas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tareas_Proyectos_ProyectoId",
                        column: x => x.ProyectoId,
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sesiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ciclos = table.Column<int>(type: "int", nullable: false),
                    DescansoCortoId = table.Column<int>(type: "int", nullable: false),
                    TareaId = table.Column<int>(type: "int", nullable: true),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sesiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sesiones_Tareas_TareaId",
                        column: x => x.TareaId,
                        principalTable: "Tareas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sesiones_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SesionesDescanso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    HoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EsDescansoLargo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SesionesDescanso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SesionesDescanso_Sesiones_Id",
                        column: x => x.Id,
                        principalTable: "Sesiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SesionesTrabajo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    HoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraFin = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SesionesTrabajo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SesionesTrabajo_Sesiones_Id",
                        column: x => x.Id,
                        principalTable: "Sesiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstadisticasUsuario_UsuarioId",
                table: "EstadisticasUsuario",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_UsuarioId",
                table: "Proyectos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Sesiones_DescansoCortoId",
                table: "Sesiones",
                column: "DescansoCortoId");

            migrationBuilder.CreateIndex(
                name: "IX_Sesiones_TareaId",
                table: "Sesiones",
                column: "TareaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sesiones_UsuarioId",
                table: "Sesiones",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_ProyectoId",
                table: "Tareas",
                column: "ProyectoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sesiones_SesionesDescanso_DescansoCortoId",
                table: "Sesiones",
                column: "DescansoCortoId",
                principalTable: "SesionesDescanso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proyectos_Usuarios_UsuarioId",
                table: "Proyectos");

            migrationBuilder.DropForeignKey(
                name: "FK_Sesiones_Usuarios_UsuarioId",
                table: "Sesiones");

            migrationBuilder.DropForeignKey(
                name: "FK_Sesiones_SesionesDescanso_DescansoCortoId",
                table: "Sesiones");

            migrationBuilder.DropTable(
                name: "EstadisticasUsuario");

            migrationBuilder.DropTable(
                name: "SesionesTrabajo");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "SesionesDescanso");

            migrationBuilder.DropTable(
                name: "Sesiones");

            migrationBuilder.DropTable(
                name: "Tareas");

            migrationBuilder.DropTable(
                name: "Proyectos");
        }
    }
}
