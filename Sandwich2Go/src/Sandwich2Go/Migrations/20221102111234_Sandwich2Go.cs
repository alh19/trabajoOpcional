using Microsoft.EntityFrameworkCore.Migrations;

namespace Sandwich2Go.Migrations
{
    public partial class Sandwich2Go : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_Sandwich_SandwCreadoId",
                table: "Pedido");

            migrationBuilder.AlterColumn<int>(
                name: "SandwCreadoId",
                table: "Pedido",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_Sandwich_SandwCreadoId",
                table: "Pedido",
                column: "SandwCreadoId",
                principalTable: "Sandwich",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_Sandwich_SandwCreadoId",
                table: "Pedido");

            migrationBuilder.AlterColumn<int>(
                name: "SandwCreadoId",
                table: "Pedido",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_Sandwich_SandwCreadoId",
                table: "Pedido",
                column: "SandwCreadoId",
                principalTable: "Sandwich",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
