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

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfertaGerente",
                table: "OfertaGerente");

            migrationBuilder.DropIndex(
                name: "IX_OfertaGerente_OfertaId",
                table: "OfertaGerente");

            migrationBuilder.AlterColumn<string>(
                name: "GerenteId",
                table: "PedidoProv",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OfertaGerente",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfertaGerente",
                table: "OfertaGerente",
                columns: new[] { "OfertaId", "GerenteId" });

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
                name: "FK_PedidoProv_AspNetUsers_GerenteId",
                table: "PedidoProv");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfertaGerente",
                table: "OfertaGerente");

            migrationBuilder.AlterColumn<string>(
                name: "GerenteId",
                table: "PedidoProv",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OfertaGerente",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfertaGerente",
                table: "OfertaGerente",
                column: "Id");

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
