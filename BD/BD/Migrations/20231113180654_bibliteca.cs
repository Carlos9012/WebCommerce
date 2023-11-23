using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BD.Migrations
{
    /// <inheritdoc />
    public partial class bibliteca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "genero",
                columns: table => new
                {
                    id_genero = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genero", x => x.id_genero);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id_user);
                });

            migrationBuilder.CreateTable(
                name: "livro",
                columns: table => new
                {
                    id_livro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    editora = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    n_pag = table.Column<int>(type: "int", nullable: false),
                    valor_multa = table.Column<double>(type: "float", nullable: false),
                    id_genero = table.Column<int>(type: "int", nullable: false),
                    id_user = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_livro", x => x.id_livro);
                    table.ForeignKey(
                        name: "FK_livro_genero_id_genero",
                        column: x => x.id_genero,
                        principalTable: "genero",
                        principalColumn: "id_genero",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_livro_user_id_user",
                        column: x => x.id_user,
                        principalTable: "user",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "emprestimo",
                columns: table => new
                {
                    id_emprestimo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    valor = table.Column<double>(type: "float", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_livro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emprestimo", x => x.id_emprestimo);
                    table.ForeignKey(
                        name: "FK_emprestimo_livro_id_livro",
                        column: x => x.id_livro,
                        principalTable: "livro",
                        principalColumn: "id_livro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_emprestimo_id_livro",
                table: "emprestimo",
                column: "id_livro");

            migrationBuilder.CreateIndex(
                name: "IX_livro_id_genero",
                table: "livro",
                column: "id_genero");

            migrationBuilder.CreateIndex(
                name: "IX_livro_id_user",
                table: "livro",
                column: "id_user");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "emprestimo");

            migrationBuilder.DropTable(
                name: "livro");

            migrationBuilder.DropTable(
                name: "genero");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
