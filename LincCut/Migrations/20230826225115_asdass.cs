using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LincCut.Migrations
{
    /// <inheritdoc />
    public partial class asdass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_urls_users_user_id",
                table: "urls");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "urls",
                newName: "userid");

            migrationBuilder.RenameIndex(
                name: "IX_urls_user_id",
                table: "urls",
                newName: "IX_urls_userid");

            migrationBuilder.AddForeignKey(
                name: "fk_urls_users_userid",
                table: "urls",
                column: "userid",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_urls_users_userid",
                table: "urls");

            migrationBuilder.RenameColumn(
                name: "userid",
                table: "urls",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_urls_userid",
                table: "urls",
                newName: "IX_urls_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_urls_users_user_id",
                table: "urls",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
