﻿// <auto-generated />
using System;
using LincCut.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LincCut.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230813163657_Fix")]
    partial class Fix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LincCut.Models.Click", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Browser")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UrlInfo_id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UrlInfo_id");

                    b.ToTable("clicks");
                });

            modelBuilder.Entity("LincCut.Models.UrlInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Counter")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Expired_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NewUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("NewUrl")
                        .IsUnique();

                    b.ToTable("urls");
                });

            modelBuilder.Entity("LincCut.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("LincCut.Models.Click", b =>
                {
                    b.HasOne("LincCut.Models.UrlInfo", "UrlInfos")
                        .WithMany()
                        .HasForeignKey("UrlInfo_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UrlInfos");
                });
#pragma warning restore 612, 618
        }
    }
}
