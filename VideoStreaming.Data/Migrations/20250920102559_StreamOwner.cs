using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoStreaming.Data.Migrations
{
    /// <inheritdoc />
    public partial class StreamOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserOwnerId",
                table: "Meets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Meets_UserOwnerId",
                table: "Meets",
                column: "UserOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meets_AspNetUsers_UserOwnerId",
                table: "Meets",
                column: "UserOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meets_AspNetUsers_UserOwnerId",
                table: "Meets");

            migrationBuilder.DropIndex(
                name: "IX_Meets_UserOwnerId",
                table: "Meets");

            migrationBuilder.DropColumn(
                name: "UserOwnerId",
                table: "Meets");
        }
    }
}
