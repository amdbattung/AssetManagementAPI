﻿// <auto-generated />
using System;
using System.Text.Json;
using AssetManagementAPI.Data;
using AssetManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AssetManagementAPI.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240619140628_MakeDateCreatedNullable")]
    partial class MakeDateCreatedNullable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "maintenance_action", new[] { "report", "service", "decommission" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "transaction_type", new[] { "delegate", "transfer", "return", "hand_over" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AssetManagementAPI.Models.Asset", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("CustodianId")
                        .HasColumnType("text")
                        .HasColumnName("custodian_id");

                    b.Property<Instant?>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("now()");

                    b.Property<JsonDocument>("Info")
                        .HasColumnType("jsonb")
                        .HasColumnName("info");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("ProprietorId")
                        .HasColumnType("text")
                        .HasColumnName("proprietor_id");

                    b.Property<string>("Type")
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_assets");

                    b.HasIndex("CustodianId")
                        .HasDatabaseName("ix_assets_custodian_id");

                    b.HasIndex("ProprietorId")
                        .HasDatabaseName("ix_assets_proprietor_id");

                    b.HasIndex("Type", "Name")
                        .HasDatabaseName("ix_assets_type_name")
                        .HasAnnotation("Npgsql:TsVectorConfig", "simple");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("Type", "Name"), "GIN");

                    b.ToTable("assets", (string)null);
                });

            modelBuilder.Entity("AssetManagementAPI.Models.Department", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<Instant?>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_departments");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_departments_name")
                        .HasAnnotation("Npgsql:TsVectorConfig", "simple");

                    b.ToTable("departments", (string)null);
                });

            modelBuilder.Entity("AssetManagementAPI.Models.Employee", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("AccountId")
                        .HasColumnType("text")
                        .HasColumnName("account_id");

                    b.Property<Instant?>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("DepartmentId")
                        .HasColumnType("text")
                        .HasColumnName("department_id");

                    b.Property<string>("FirstName")
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text")
                        .HasColumnName("middle_name");

                    b.HasKey("Id")
                        .HasName("pk_employees");

                    b.HasIndex("DepartmentId")
                        .HasDatabaseName("ix_employees_department_id");

                    b.HasIndex("LastName", "FirstName", "MiddleName")
                        .HasDatabaseName("ix_employees_last_name_first_name_middle_name")
                        .HasAnnotation("Npgsql:TsVectorConfig", "simple");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("LastName", "FirstName", "MiddleName"), "GIN");

                    b.ToTable("employees", (string)null);
                });

            modelBuilder.Entity("AssetManagementAPI.Models.MaintenanceRecord", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<MaintenanceRecord.MaintenanceAction>("Action")
                        .HasColumnType("maintenance_action")
                        .HasColumnName("action");

                    b.Property<string>("AssetId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("asset_id");

                    b.Property<string>("Comment")
                        .HasColumnType("text")
                        .HasColumnName("comment");

                    b.Property<Instant?>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("maintenance_date")
                        .HasDefaultValueSql("now()");

                    b.Property<Instant?>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("DocumentorId")
                        .HasColumnType("text")
                        .HasColumnName("documentor_id");

                    b.Property<string>("Reason")
                        .HasColumnType("text")
                        .HasColumnName("reason");

                    b.HasKey("Id")
                        .HasName("pk_maintenance_records");

                    b.HasIndex("AssetId")
                        .HasDatabaseName("ix_maintenance_records_asset_id");

                    b.HasIndex("DocumentorId")
                        .HasDatabaseName("ix_maintenance_records_documentor_id");

                    b.ToTable("maintenance_records", (string)null);
                });

            modelBuilder.Entity("AssetManagementAPI.Models.Transaction", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("ApproverId")
                        .HasColumnType("text")
                        .HasColumnName("approver_id");

                    b.Property<string>("AssetId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("asset_id");

                    b.Property<Instant?>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("transaction_date")
                        .HasDefaultValueSql("now()");

                    b.Property<Instant?>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Reason")
                        .HasColumnType("text")
                        .HasColumnName("reason");

                    b.Property<string>("Remark")
                        .HasColumnType("text")
                        .HasColumnName("remark");

                    b.Property<string>("TransacteeId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("transactee_id");

                    b.Property<string>("TransactorId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("transactor_id");

                    b.Property<Transaction.TransactionType>("Type")
                        .HasColumnType("transaction_type")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_transactions");

                    b.HasIndex("ApproverId")
                        .HasDatabaseName("ix_transactions_approver_id");

                    b.HasIndex("AssetId")
                        .HasDatabaseName("ix_transactions_asset_id");

                    b.HasIndex("TransacteeId")
                        .HasDatabaseName("ix_transactions_transactee_id");

                    b.HasIndex("TransactorId")
                        .HasDatabaseName("ix_transactions_transactor_id");

                    b.ToTable("transactions", (string)null);
                });

            modelBuilder.Entity("AssetManagementAPI.Models.Asset", b =>
                {
                    b.HasOne("AssetManagementAPI.Models.Employee", "Custodian")
                        .WithMany()
                        .HasForeignKey("CustodianId")
                        .HasConstraintName("fk_assets_employees_custodian_id");

                    b.HasOne("AssetManagementAPI.Models.Department", "Proprietor")
                        .WithMany()
                        .HasForeignKey("ProprietorId")
                        .HasConstraintName("fk_assets_departments_proprietor_id");

                    b.Navigation("Custodian");

                    b.Navigation("Proprietor");
                });

            modelBuilder.Entity("AssetManagementAPI.Models.Employee", b =>
                {
                    b.HasOne("AssetManagementAPI.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .HasConstraintName("fk_employees_departments_department_id");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("AssetManagementAPI.Models.MaintenanceRecord", b =>
                {
                    b.HasOne("AssetManagementAPI.Models.Asset", "Asset")
                        .WithMany("MaintenanceRecords")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_maintenance_records_assets_asset_id");

                    b.HasOne("AssetManagementAPI.Models.Employee", "Documentor")
                        .WithMany()
                        .HasForeignKey("DocumentorId")
                        .HasConstraintName("fk_maintenance_records_employees_documentor_id");

                    b.Navigation("Asset");

                    b.Navigation("Documentor");
                });

            modelBuilder.Entity("AssetManagementAPI.Models.Transaction", b =>
                {
                    b.HasOne("AssetManagementAPI.Models.Employee", "Approver")
                        .WithMany()
                        .HasForeignKey("ApproverId")
                        .HasConstraintName("fk_transactions_employees_approver_id");

                    b.HasOne("AssetManagementAPI.Models.Asset", "Asset")
                        .WithMany("Transactions")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_transactions_assets_asset_id");

                    b.HasOne("AssetManagementAPI.Models.Employee", "Transactee")
                        .WithMany()
                        .HasForeignKey("TransacteeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_transactions_employees_transactee_id");

                    b.HasOne("AssetManagementAPI.Models.Employee", "Transactor")
                        .WithMany()
                        .HasForeignKey("TransactorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_transactions_employees_transactor_id");

                    b.Navigation("Approver");

                    b.Navigation("Asset");

                    b.Navigation("Transactee");

                    b.Navigation("Transactor");
                });

            modelBuilder.Entity("AssetManagementAPI.Models.Asset", b =>
                {
                    b.Navigation("MaintenanceRecords");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
