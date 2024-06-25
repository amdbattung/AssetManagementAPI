using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace AssetManagementAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDateCreatedOnTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date",
                table: "transactions",
                newName: "transaction_date");

            migrationBuilder.AlterColumn<Instant>(
                name: "transaction_date",
                table: "transactions",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<Instant>(
                name: "date_created",
                table: "transactions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "date_created",
                table: "maintenance_records",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "maintenance_date",
                table: "maintenance_records",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "date_created",
                table: "employees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "date_created",
                table: "departments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "date_created",
                table: "assets",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_created",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "date_created",
                table: "maintenance_records");

            migrationBuilder.DropColumn(
                name: "maintenance_date",
                table: "maintenance_records");

            migrationBuilder.DropColumn(
                name: "date_created",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "date_created",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "date_created",
                table: "assets");

            migrationBuilder.RenameColumn(
                name: "transaction_date",
                table: "transactions",
                newName: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date",
                table: "transactions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValueSql: "now()");
        }
    }
}
