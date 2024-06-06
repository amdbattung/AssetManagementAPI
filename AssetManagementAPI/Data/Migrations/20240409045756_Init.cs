using System;
using System.Text.Json;
using AssetManagementAPI.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagementAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:maintenance_action", "report,service,decommission")
                .Annotation("Npgsql:Enum:transaction_type", "delegate,transfer,return,hand_over");

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_departments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    middle_name = table.Column<string>(type: "text", nullable: true),
                    department_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees", x => x.id);
                    table.ForeignKey(
                        name: "fk_employees_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "assets",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    info = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    proprietor_id = table.Column<string>(type: "text", nullable: true),
                    custodian_id = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assets", x => x.id);
                    table.ForeignKey(
                        name: "fk_assets_departments_proprietor_id",
                        column: x => x.proprietor_id,
                        principalTable: "departments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_assets_employees_custodian_id",
                        column: x => x.custodian_id,
                        principalTable: "employees",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "maintenance_records",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    asset_id = table.Column<string>(type: "text", nullable: false),
                    action = table.Column<MaintenanceRecord.MaintenanceAction>(type: "maintenance_action", nullable: false),
                    documentor_id = table.Column<string>(type: "text", nullable: true),
                    reason = table.Column<string>(type: "text", nullable: true),
                    comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_maintenance_records", x => x.id);
                    table.ForeignKey(
                        name: "fk_maintenance_records_assets_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_maintenance_records_employees_documentor_id",
                        column: x => x.documentor_id,
                        principalTable: "employees",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    asset_id = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<Transaction.TransactionType>(type: "transaction_type", nullable: false),
                    transactor_id = table.Column<string>(type: "text", nullable: false),
                    transactee_id = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reason = table.Column<string>(type: "text", nullable: true),
                    remark = table.Column<string>(type: "text", nullable: true),
                    approver_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_transactions_assets_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_transactions_employees_approver_id",
                        column: x => x.approver_id,
                        principalTable: "employees",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_transactions_employees_transactee_id",
                        column: x => x.transactee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_transactions_employees_transactor_id",
                        column: x => x.transactor_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_assets_custodian_id",
                table: "assets",
                column: "custodian_id");

            migrationBuilder.CreateIndex(
                name: "ix_assets_proprietor_id",
                table: "assets",
                column: "proprietor_id");

            migrationBuilder.CreateIndex(
                name: "ix_assets_type_name",
                table: "assets",
                columns: new[] { "type", "name" })
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "english");

            migrationBuilder.CreateIndex(
                name: "ix_departments_name",
                table: "departments",
                column: "name",
                unique: true)
                .Annotation("Npgsql:TsVectorConfig", "english");

            migrationBuilder.CreateIndex(
                name: "ix_employees_department_id",
                table: "employees",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_employees_last_name_first_name_middle_name",
                table: "employees",
                columns: new[] { "last_name", "first_name", "middle_name" })
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "english");

            migrationBuilder.CreateIndex(
                name: "ix_maintenance_records_asset_id",
                table: "maintenance_records",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_maintenance_records_documentor_id",
                table: "maintenance_records",
                column: "documentor_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_approver_id",
                table: "transactions",
                column: "approver_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_asset_id",
                table: "transactions",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_transactee_id",
                table: "transactions",
                column: "transactee_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_transactor_id",
                table: "transactions",
                column: "transactor_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "maintenance_records");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "assets");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "departments");
        }
    }
}
