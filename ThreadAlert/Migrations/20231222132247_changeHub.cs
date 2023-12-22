using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadAlert.Migrations
{
    /// <inheritdoc />
    public partial class changeHub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HubConnections_DangerousObjects_DangerousObjectId",
                table: "HubConnections");

            migrationBuilder.DropIndex(
                name: "IX_HubConnections_DangerousObjectId",
                table: "HubConnections");

            migrationBuilder.DropColumn(
                name: "DangerousObjectId",
                table: "HubConnections");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "HubConnections",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_HubConnections_UserId",
                table: "HubConnections",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HubConnections_AspNetUsers_UserId",
                table: "HubConnections",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HubConnections_AspNetUsers_UserId",
                table: "HubConnections");

            migrationBuilder.DropIndex(
                name: "IX_HubConnections_UserId",
                table: "HubConnections");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "HubConnections");

            migrationBuilder.AddColumn<int>(
                name: "DangerousObjectId",
                table: "HubConnections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HubConnections_DangerousObjectId",
                table: "HubConnections",
                column: "DangerousObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_HubConnections_DangerousObjects_DangerousObjectId",
                table: "HubConnections",
                column: "DangerousObjectId",
                principalTable: "DangerousObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
