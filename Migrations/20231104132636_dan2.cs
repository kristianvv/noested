using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Noested.Migrations
{
    /// <inheritdoc />
    public partial class dan2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrder_Checklist_ChecklistId",
                table: "ServiceOrder");

            migrationBuilder.AlterColumn<int>(
                name: "ChecklistId",
                table: "ServiceOrder",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrder_Checklist_ChecklistId",
                table: "ServiceOrder",
                column: "ChecklistId",
                principalTable: "Checklist",
                principalColumn: "ChecklistId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrder_Checklist_ChecklistId",
                table: "ServiceOrder");

            migrationBuilder.AlterColumn<int>(
                name: "ChecklistId",
                table: "ServiceOrder",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrder_Checklist_ChecklistId",
                table: "ServiceOrder",
                column: "ChecklistId",
                principalTable: "Checklist",
                principalColumn: "ChecklistId");
        }
    }
}
