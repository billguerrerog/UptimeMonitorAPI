﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UptimeMonitor.API.Data;

#nullable disable

namespace UptimeMonitor.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250620165427_AddUptimeEvent")]
    partial class AddUptimeEvent
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UptimeMonitor.API.Entities.Component", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ServiceSystemId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ServiceSystemId");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("UptimeMonitor.API.Entities.ServiceSystem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("ServiceSystems");
                });

            modelBuilder.Entity("UptimeMonitor.API.Entities.UptimeCheck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AlertEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CheckInterval")
                        .HasColumnType("int");

                    b.Property<int>("CheckTimeout")
                        .HasColumnType("int");

                    b.Property<string>("CheckUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ComponentId")
                        .HasColumnType("int");

                    b.Property<int>("DownAlertDelay")
                        .HasColumnType("int");

                    b.Property<string>("DownAlertMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DownAlertResend")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestHeaders")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ServiceSystemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ComponentId");

                    b.HasIndex("ServiceSystemId");

                    b.ToTable("UptimeChecks");
                });

            modelBuilder.Entity("UptimeMonitor.API.Entities.UptimeEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsFalsePositive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUp")
                        .HasColumnType("bit");

                    b.Property<string>("JiraTicket")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaintenanceType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UptimeCheckId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UptimeCheckId");

                    b.ToTable("UptimeEvents");
                });

            modelBuilder.Entity("UptimeMonitor.API.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UptimeMonitor.API.Entities.Component", b =>
                {
                    b.HasOne("UptimeMonitor.API.Entities.ServiceSystem", "ServiceSystem")
                        .WithMany("Components")
                        .HasForeignKey("ServiceSystemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceSystem");
                });

            modelBuilder.Entity("UptimeMonitor.API.Entities.UptimeCheck", b =>
                {
                    b.HasOne("UptimeMonitor.API.Entities.Component", "Component")
                        .WithMany()
                        .HasForeignKey("ComponentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UptimeMonitor.API.Entities.ServiceSystem", "ServiceSystem")
                        .WithMany()
                        .HasForeignKey("ServiceSystemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Component");

                    b.Navigation("ServiceSystem");
                });

            modelBuilder.Entity("UptimeMonitor.API.Entities.UptimeEvent", b =>
                {
                    b.HasOne("UptimeMonitor.API.Entities.UptimeCheck", "UptimeCheck")
                        .WithMany()
                        .HasForeignKey("UptimeCheckId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UptimeCheck");
                });

            modelBuilder.Entity("UptimeMonitor.API.Entities.ServiceSystem", b =>
                {
                    b.Navigation("Components");
                });
#pragma warning restore 612, 618
        }
    }
}
