using Microsoft.EntityFrameworkCore.Migrations;

namespace Sandwich2Go.Migrations
{
    public partial class Sandwich2Go : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoProv_AspNetUsers_GerenteId",
                table: "PedidoProv");

            migrationBuilder.DropTable(
                name: "OfertaGerente");

            migrationBuilder.AlterColumn<string>(
                name: "GerenteId",
                table: "PedidoProv",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GerenteId",
                table: "Oferta",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Oferta_GerenteId",
                table: "Oferta",
                column: "GerenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Oferta_AspNetUsers_GerenteId",
                table: "Oferta",
                column: "GerenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoProv_AspNetUsers_GerenteId",
                table: "PedidoProv",
                column: "GerenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Oferta_AspNetUsers_GerenteId",
                table: "Oferta");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoProv_AspNetUsers_GerenteId",
                table: "PedidoProv");

            migrationBuilder.DropIndex(
                name: "IX_Oferta_GerenteId",
                table: "Oferta");

            migrationBuilder.DropColumn(
                name: "GerenteId",
                table: "Oferta");

            migrationBuilder.AlterColumn<string>(
                name: "GerenteId",
                table: "PedidoProv",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "OfertaGerente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GerenteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OfertaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfertaGerente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfertaGerente_AspNetUsers_GerenteId",
                        column: x => x.GerenteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfertaGerente_Oferta_OfertaId",
                        column: x => x.OfertaId,
                        principalTable: "Oferta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfertaGerente_GerenteId",
                table: "OfertaGerente",
                column: "GerenteId");

            migrationBuilder.CreateIndex(
                name: "IX_OfertaGerente_OfertaId",
                table: "OfertaGerente",
                column: "OfertaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoProv_AspNetUsers_GerenteId",
                table: "PedidoProv",
                column: "GerenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
