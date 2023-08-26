using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LincCut.Migrations
{
    /// <inheritdoc />
    public partial class SnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clicks_urls_UrlInfo_id",
                table: "clicks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_urls",
                table: "urls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_clicks",
                table: "clicks");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "users",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "users",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RegistrationDate",
                table: "users",
                newName: "registration_date");

            migrationBuilder.RenameColumn(
                name: "LastLogin",
                table: "users",
                newName: "last_login");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "urls",
                newName: "url");

            migrationBuilder.RenameColumn(
                name: "Expired_at",
                table: "urls",
                newName: "expired_at");

            migrationBuilder.RenameColumn(
                name: "Created_at",
                table: "urls",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Counter",
                table: "urls",
                newName: "counter");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "urls",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "NewUrl",
                table: "urls",
                newName: "new_url");

            migrationBuilder.RenameIndex(
                name: "IX_urls_NewUrl",
                table: "urls",
                newName: "IX_urls_new_url");

            migrationBuilder.RenameColumn(
                name: "Ip",
                table: "clicks",
                newName: "ip");

            migrationBuilder.RenameColumn(
                name: "Browser",
                table: "clicks",
                newName: "browser");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "clicks",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UrlInfo_id",
                table: "clicks",
                newName: "url_info_id");

            migrationBuilder.RenameIndex(
                name: "IX_clicks_UrlInfo_id",
                table: "clicks",
                newName: "IX_clicks_url_info_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_urls",
                table: "urls",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_clicks",
                table: "clicks",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_clicks_urls_url_info_id",
                table: "clicks",
                column: "url_info_id",
                principalTable: "urls",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_clicks_urls_url_info_id",
                table: "clicks");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_urls",
                table: "urls");

            migrationBuilder.DropPrimaryKey(
                name: "pk_clicks",
                table: "clicks");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "registration_date",
                table: "users",
                newName: "RegistrationDate");

            migrationBuilder.RenameColumn(
                name: "last_login",
                table: "users",
                newName: "LastLogin");

            migrationBuilder.RenameColumn(
                name: "url",
                table: "urls",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "expired_at",
                table: "urls",
                newName: "Expired_at");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "urls",
                newName: "Created_at");

            migrationBuilder.RenameColumn(
                name: "counter",
                table: "urls",
                newName: "Counter");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "urls",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "new_url",
                table: "urls",
                newName: "NewUrl");

            migrationBuilder.RenameIndex(
                name: "IX_urls_new_url",
                table: "urls",
                newName: "IX_urls_NewUrl");

            migrationBuilder.RenameColumn(
                name: "ip",
                table: "clicks",
                newName: "Ip");

            migrationBuilder.RenameColumn(
                name: "browser",
                table: "clicks",
                newName: "Browser");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "clicks",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "url_info_id",
                table: "clicks",
                newName: "UrlInfo_id");

            migrationBuilder.RenameIndex(
                name: "IX_clicks_url_info_id",
                table: "clicks",
                newName: "IX_clicks_UrlInfo_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_urls",
                table: "urls",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_clicks",
                table: "clicks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_clicks_urls_UrlInfo_id",
                table: "clicks",
                column: "UrlInfo_id",
                principalTable: "urls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
