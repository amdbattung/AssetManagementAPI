using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagementAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeAccountId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_employees_last_name_first_name_middle_name",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "ix_departments_name",
                table: "departments");

            migrationBuilder.DropIndex(
                name: "ix_assets_type_name",
                table: "assets");

            migrationBuilder.AddColumn<string>(
                name: "account_id",
                table: "employees",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_employees_last_name_first_name_middle_name",
                table: "employees",
                columns: new[] { "last_name", "first_name", "middle_name" })
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "simple");

            migrationBuilder.CreateIndex(
                name: "ix_departments_name",
                table: "departments",
                column: "name",
                unique: true)
                .Annotation("Npgsql:TsVectorConfig", "simple");

            migrationBuilder.CreateIndex(
                name: "ix_assets_type_name",
                table: "assets",
                columns: new[] { "type", "name" })
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "simple");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_employees_last_name_first_name_middle_name",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "ix_departments_name",
                table: "departments");

            migrationBuilder.DropIndex(
                name: "ix_assets_type_name",
                table: "assets");

            migrationBuilder.DropColumn(
                name: "account_id",
                table: "employees");

            migrationBuilder.CreateIndex(
                name: "ix_employees_last_name_first_name_middle_name",
                table: "employees",
                columns: new[] { "last_name", "first_name", "middle_name" })
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "english");

            migrationBuilder.CreateIndex(
                name: "ix_departments_name",
                table: "departments",
                column: "name",
                unique: true)
                .Annotation("Npgsql:TsVectorConfig", "english");

            migrationBuilder.CreateIndex(
                name: "ix_assets_type_name",
                table: "assets",
                columns: new[] { "type", "name" })
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "english");
        }
    }
}
