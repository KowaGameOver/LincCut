using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LincCut.Migrations
{
    /// <inheritdoc />
    public partial class AddIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UrlInfos_NewUrl",
                table: "UrlInfos",
                column: "NewUrl",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UrlInfos_NewUrl",
                table: "UrlInfos");
        }
    }
}
