using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Noested.Migrations
{
    /// <inheritdoc />
    public partial class dan1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_ServiceOrder_OrderId",
                table: "Checklist");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Checklist",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_ServiceOrder_OrderId",
                table: "Checklist",
                column: "OrderId",
                principalTable: "ServiceOrder",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_ServiceOrder_OrderId",
                table: "Checklist");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Checklist",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_ServiceOrder_OrderId",
                table: "Checklist",
                column: "OrderId",
                principalTable: "ServiceOrder",
                principalColumn: "OrderId");
        }
    }
}
