using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Noested.Migrations
{
    /// <inheritdoc />
    public partial class dan3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_ServiceOrder_OrderId",
                table: "Checklist");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrder_Checklist_ChecklistId",
                table: "ServiceOrder");

            migrationBuilder.DropIndex(
                name: "IX_ServiceOrder_ChecklistId",
                table: "ServiceOrder");

            migrationBuilder.DropIndex(
                name: "IX_Checklist_OrderId",
                table: "Checklist");

            migrationBuilder.DropColumn(
                name: "ChecklistId",
                table: "ServiceOrder");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Checklist");

            migrationBuilder.AlterColumn<int>(
                name: "ChecklistId",
                table: "Checklist",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_ServiceOrder_ChecklistId",
                table: "Checklist",
                column: "ChecklistId",
                principalTable: "ServiceOrder",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_ServiceOrder_ChecklistId",
                table: "Checklist");

            migrationBuilder.AddColumn<int>(
                name: "ChecklistId",
                table: "ServiceOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ChecklistId",
                table: "Checklist",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Checklist",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrder_ChecklistId",
                table: "ServiceOrder",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_Checklist_OrderId",
                table: "Checklist",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_ServiceOrder_OrderId",
                table: "Checklist",
                column: "OrderId",
                principalTable: "ServiceOrder",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrder_Checklist_ChecklistId",
                table: "ServiceOrder",
                column: "ChecklistId",
                principalTable: "Checklist",
                principalColumn: "ChecklistId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
