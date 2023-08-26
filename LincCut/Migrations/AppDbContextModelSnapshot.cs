﻿// <auto-generated />
using System;
using LincCut.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LincCut.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LincCut.Models.Click", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("BROWSER")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("browser");

                    b.Property<string>("IP")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ip");

                    b.Property<int>("URL_ID")
                        .HasColumnType("integer")
                        .HasColumnName("url_id");

                    b.HasKey("ID")
                        .HasName("pk_clicks");

                    b.HasIndex("URL_ID");

                    b.ToTable("clicks");
                });

            modelBuilder.Entity("LincCut.Models.Url", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CREATED_AT")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("EXPIRED_AT")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expired_at");

                    b.Property<int>("MAX_CLICKS")
                        .HasColumnType("integer")
                        .HasColumnName("max_clicks");

                    b.Property<string>("ORIGINAL_URL")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("original_url");

                    b.Property<string>("SHORT_SLUG")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("short_slug");

                    b.Property<int>("USER_ID")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("ID")
                        .HasName("pk_urls");

                    b.HasIndex("SHORT_SLUG")
                        .IsUnique();

                    b.HasIndex("USER_ID");

                    b.ToTable("urls");
                });

            modelBuilder.Entity("LincCut.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("EMAIL")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<DateTime>("LAST_LOGIN_DATE")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_login_date");

                    b.Property<string>("PASSWORD")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<DateTime>("REGISTRATION_DATE")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("registration_date");

                    b.Property<int>("ROLE")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.HasKey("ID")
                        .HasName("pk_users");

                    b.ToTable("users");
                });

            modelBuilder.Entity("LincCut.Models.Click", b =>
                {
                    b.HasOne("LincCut.Models.Url", "URL")
                        .WithMany()
                        .HasForeignKey("URL_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_clicks_urls_url_id");

                    b.Navigation("URL");
                });

            modelBuilder.Entity("LincCut.Models.Url", b =>
                {
                    b.HasOne("LincCut.Models.User", "USER")
                        .WithMany()
                        .HasForeignKey("USER_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_urls_users_user_id");

                    b.Navigation("USER");
                });
#pragma warning restore 612, 618
        }
    }
}
