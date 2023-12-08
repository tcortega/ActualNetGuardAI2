﻿// <auto-generated />
using System;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetGuardAI.Core.Persistence;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetGuardAI.Core.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231117024451_ResultsWebhooks")]
    partial class ResultsWebhooks
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NetGuardAI.Core.Persistence.Entities.PortRange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FromPort")
                        .HasColumnType("integer");

                    b.Property<int?>("ToPort")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("PortRanges");
                });

            modelBuilder.Entity("NetGuardAI.Core.Persistence.Entities.ScanResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<IPAddress>("IpAddress")
                        .IsRequired()
                        .HasColumnType("inet");

                    b.Property<int>("Port")
                        .HasColumnType("integer");

                    b.Property<string>("ProcessedInfo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RawInfo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Instant>("ScanTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("ScanResults");
                });

            modelBuilder.Entity("NetGuardAI.Core.Persistence.Entities.ScanSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Duration>("IpCooldown")
                        .HasColumnType("interval");

                    b.Property<int>("MasscanRate")
                        .HasColumnType("integer");

                    b.Property<int>("NmapConcurrencyLimit")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ScanSettings");
                });

            modelBuilder.Entity("NetGuardAI.Core.Persistence.Entities.ScanTarget", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("IpRange")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ScanTargets");
                });

            modelBuilder.Entity("NetGuardAI.Core.Persistence.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NetGuardAI.Core.Persistence.Entities.UserWebhook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserWebhooks");
                });
#pragma warning restore 612, 618
        }
    }
}
