using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LincCut.Migrations
{
    /// <inheritdoc />
    public partial class AddLifeTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created_at",
                table: "UrlInfos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Expired_at",
                table: "UrlInfos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created_at",
                table: "UrlInfos");

            migrationBuilder.DropColumn(
                name: "Expired_at",
                table: "UrlInfos");
        }
    }
}
