using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Noested.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitalModelsSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Checklist",
                columns: table => new
                {
                    ChecklistID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ServiceProcedure = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApprovedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PreparedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MechBrakes = table.Column<int>(type: "int", nullable: false),
                    MechDrumBearing = table.Column<int>(type: "int", nullable: false),
                    MechStoragePTO = table.Column<int>(type: "int", nullable: false),
                    MechWire = table.Column<int>(type: "int", nullable: false),
                    MechChainTensioner = table.Column<int>(type: "int", nullable: false),
                    MechPinionBearing = table.Column<int>(type: "int", nullable: false),
                    MechClutch = table.Column<int>(type: "int", nullable: false),
                    MechSprocketWedges = table.Column<int>(type: "int", nullable: false),
                    HydCylinder = table.Column<int>(type: "int", nullable: false),
                    HydHydraulicBlock = table.Column<int>(type: "int", nullable: false),
                    HydTankOil = table.Column<int>(type: "int", nullable: false),
                    HydGearboxOil = table.Column<int>(type: "int", nullable: false),
                    HydBrakeCylinder = table.Column<int>(type: "int", nullable: false),
                    ElCableNetwork = table.Column<int>(type: "int", nullable: false),
                    ElRadio = table.Column<int>(type: "int", nullable: false),
                    ElButtonBox = table.Column<int>(type: "int", nullable: false),
                    TensionCheckBar = table.Column<int>(type: "int", nullable: false),
                    TestWinch = table.Column<int>(type: "int", nullable: false),
                    TestTraction = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MechSignature = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateCompleted = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checklist", x => x.ChecklistID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Street = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PostalCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ServiceOrder",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    OrderRecieved = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    OrderCompleted = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    ModelYear = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SerialNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Warranty = table.Column<int>(type: "int", nullable: false),
                    CustomerAgreement = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderDescription = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiscardedParts = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReplacedPartsReturned = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Shipping = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceOrder", x => x.OrderID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checklist");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "ServiceOrder");
        }
    }
}
