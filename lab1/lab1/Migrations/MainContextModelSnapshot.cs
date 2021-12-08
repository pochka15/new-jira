﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using lab1.Models.Data;

namespace lab1.Migrations
{
    [DbContext(typeof(MainContext))]
    partial class MainContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.12");

            modelBuilder.Entity("lab1.Models.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Date")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("MonthReportId")
                        .HasColumnType("int");

                    b.Property<string>("ProjectCode")
                        .HasColumnType("longtext");

                    b.Property<string>("SubprojectCode")
                        .HasColumnType("longtext");

                    b.Property<int>("Time")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MonthReportId");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("lab1.Models.MonthReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsFrozen")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("MonthReport");
                });

            modelBuilder.Entity("lab1.Models.Project", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Budget")
                        .HasColumnType("int");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Manager")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("lab1.Models.ProjectCodeAndTime", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("MonthReportId")
                        .HasColumnType("int");

                    b.Property<int>("Time")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MonthReportId");

                    b.ToTable("ProjectTime");
                });

            modelBuilder.Entity("lab1.Models.Subproject", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProjectId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Subproject");
                });

            modelBuilder.Entity("lab1.Models.Activity", b =>
                {
                    b.HasOne("lab1.Models.MonthReport", null)
                        .WithMany("Activities")
                        .HasForeignKey("MonthReportId");
                });

            modelBuilder.Entity("lab1.Models.ProjectCodeAndTime", b =>
                {
                    b.HasOne("lab1.Models.MonthReport", null)
                        .WithMany("AcceptedWork")
                        .HasForeignKey("MonthReportId");
                });

            modelBuilder.Entity("lab1.Models.Subproject", b =>
                {
                    b.HasOne("lab1.Models.Project", null)
                        .WithMany("Subprojects")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("lab1.Models.MonthReport", b =>
                {
                    b.Navigation("AcceptedWork");

                    b.Navigation("Activities");
                });

            modelBuilder.Entity("lab1.Models.Project", b =>
                {
                    b.Navigation("Subprojects");
                });
#pragma warning restore 612, 618
        }
    }
}
