using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadAlert.Migrations
{
    /// <inheritdoc />
    public partial class change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DangerousObjects_AspNetUsers_AppUserId",
                table: "DangerousObjects");

            migrationBuilder.DropIndex(
                name: "IX_DangerousObjects_AppUserId",
                table: "DangerousObjects");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "DangerousObjects");

            migrationBuilder.CreateTable(
                name: "AppUserDangerousObject",
                columns: table => new
                {
                    DangerousObjectsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserDangerousObject", x => new { x.DangerousObjectsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AppUserDangerousObject_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserDangerousObject_DangerousObjects_DangerousObjectsId",
                        column: x => x.DangerousObjectsId,
                        principalTable: "DangerousObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserDangerousObject_UsersId",
                table: "AppUserDangerousObject",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserDangerousObject");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "DangerousObjects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DangerousObjects_AppUserId",
                table: "DangerousObjects",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DangerousObjects_AspNetUsers_AppUserId",
                table: "DangerousObjects",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
