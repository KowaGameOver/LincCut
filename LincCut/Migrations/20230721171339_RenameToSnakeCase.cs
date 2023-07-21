using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LincCut.Migrations
{
    /// <inheritdoc />
    public partial class RenameToSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clicks_UrlInfos_UrlInfo_id",
                table: "Clicks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clicks",
                table: "Clicks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UrlInfos",
                table: "UrlInfos");

            migrationBuilder.RenameTable(
                name: "Clicks",
                newName: "clicks");

            migrationBuilder.RenameTable(
                name: "UrlInfos",
                newName: "urls");

            migrationBuilder.RenameIndex(
                name: "IX_Clicks_UrlInfo_id",
                table: "clicks",
                newName: "IX_clicks_UrlInfo_id");

            migrationBuilder.RenameIndex(
                name: "IX_UrlInfos_NewUrl",
                table: "urls",
                newName: "IX_urls_NewUrl");

            migrationBuilder.AddPrimaryKey(
                name: "PK_clicks",
                table: "clicks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_urls",
                table: "urls",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_clicks_urls_UrlInfo_id",
                table: "clicks",
                column: "UrlInfo_id",
                principalTable: "urls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clicks_urls_UrlInfo_id",
                table: "clicks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_clicks",
                table: "clicks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_urls",
                table: "urls");

            migrationBuilder.RenameTable(
                name: "clicks",
                newName: "Clicks");

            migrationBuilder.RenameTable(
                name: "urls",
                newName: "UrlInfos");

            migrationBuilder.RenameIndex(
                name: "IX_clicks_UrlInfo_id",
                table: "Clicks",
                newName: "IX_Clicks_UrlInfo_id");

            migrationBuilder.RenameIndex(
                name: "IX_urls_NewUrl",
                table: "UrlInfos",
                newName: "IX_UrlInfos_NewUrl");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clicks",
                table: "Clicks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UrlInfos",
                table: "UrlInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clicks_UrlInfos_UrlInfo_id",
                table: "Clicks",
                column: "UrlInfo_id",
                principalTable: "UrlInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
