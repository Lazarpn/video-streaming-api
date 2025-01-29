using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrandedGames.Data.Migrations
{
    /// <inheritdoc />
    public partial class FeatureDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GameFeatures",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "GameFeatures");
        }
    }
}
