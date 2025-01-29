using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrandedGames.Data.Migrations
{
    /// <inheritdoc />
    public partial class PlatformTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameFormPlatformTypes_PlatformType_PlatformTypeId",
                table: "GameFormPlatformTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlatformType",
                table: "PlatformType");

            migrationBuilder.RenameTable(
                name: "PlatformType",
                newName: "PlatformTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlatformTypes",
                table: "PlatformTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameFormPlatformTypes_PlatformTypes_PlatformTypeId",
                table: "GameFormPlatformTypes",
                column: "PlatformTypeId",
                principalTable: "PlatformTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameFormPlatformTypes_PlatformTypes_PlatformTypeId",
                table: "GameFormPlatformTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlatformTypes",
                table: "PlatformTypes");

            migrationBuilder.RenameTable(
                name: "PlatformTypes",
                newName: "PlatformType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlatformType",
                table: "PlatformType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameFormPlatformTypes_PlatformType_PlatformTypeId",
                table: "GameFormPlatformTypes",
                column: "PlatformTypeId",
                principalTable: "PlatformType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
