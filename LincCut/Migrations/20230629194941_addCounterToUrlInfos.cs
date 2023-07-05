using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LincCut.Migrations
{
    /// <inheritdoc />
    public partial class addCounterToUrlInfos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Counter",
                table: "UrlInfos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Counter",
                table: "UrlInfos");
        }
    }
}
