using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagementAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFullTextSearchToTransactionAndMaintenanceRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_transactions_reason_remark",
                table: "transactions",
                columns: new[] { "reason", "remark" })
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "english");

            migrationBuilder.CreateIndex(
                name: "ix_maintenance_records_reason_comment",
                table: "maintenance_records",
                columns: new[] { "reason", "comment" })
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "english");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_transactions_reason_remark",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "ix_maintenance_records_reason_comment",
                table: "maintenance_records");
        }
    }
}
