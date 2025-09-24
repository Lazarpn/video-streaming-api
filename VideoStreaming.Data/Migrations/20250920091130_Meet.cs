using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoStreaming.Data.Migrations
{
    /// <inheritdoc />
    public partial class Meet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameFormFeatures");

            migrationBuilder.DropTable(
                name: "GameFormFiles");

            migrationBuilder.DropTable(
                name: "GameFormPlatformTypes");

            migrationBuilder.DropTable(
                name: "GameFeatures");

            migrationBuilder.DropTable(
                name: "GameForms");

            migrationBuilder.DropTable(
                name: "PlatformTypes");

            migrationBuilder.DropTable(
                name: "GameTypes");

            migrationBuilder.CreateTable(
                name: "Meets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StreamType = table.Column<int>(type: "integer", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meets", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meets");

            migrationBuilder.CreateTable(
                name: "GameFeatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IconName = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IconName = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IconName = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerType = table.Column<int>(type: "integer", nullable: false),
                    Items = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<int>(type: "integer", nullable: true),
                    RewardPlacements = table.Column<string>(type: "text", nullable: true),
                    RewardTopPlayers = table.Column<bool>(type: "boolean", nullable: false),
                    Rewards = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameForms_GameTypes_GameTypeId",
                        column: x => x.GameTypeId,
                        principalTable: "GameTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameFormFeatures",
                columns: table => new
                {
                    GameFormId = table.Column<Guid>(type: "uuid", nullable: false),
                    GameFeatureId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameFormFeatures", x => new { x.GameFormId, x.GameFeatureId });
                    table.ForeignKey(
                        name: "FK_GameFormFeatures_GameFeatures_GameFeatureId",
                        column: x => x.GameFeatureId,
                        principalTable: "GameFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameFormFeatures_GameForms_GameFormId",
                        column: x => x.GameFormId,
                        principalTable: "GameForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameFormFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameFormId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FileName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FileOriginalName = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FileType = table.Column<int>(type: "integer", nullable: false),
                    FileUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ThumbUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameFormFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameFormFiles_GameForms_GameFormId",
                        column: x => x.GameFormId,
                        principalTable: "GameForms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GameFormPlatformTypes",
                columns: table => new
                {
                    GameFormId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlatformTypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameFormPlatformTypes", x => new { x.GameFormId, x.PlatformTypeId });
                    table.ForeignKey(
                        name: "FK_GameFormPlatformTypes_GameForms_GameFormId",
                        column: x => x.GameFormId,
                        principalTable: "GameForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameFormPlatformTypes_PlatformTypes_PlatformTypeId",
                        column: x => x.PlatformTypeId,
                        principalTable: "PlatformTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameFormFeatures_GameFeatureId",
                table: "GameFormFeatures",
                column: "GameFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_GameFormFiles_GameFormId",
                table: "GameFormFiles",
                column: "GameFormId");

            migrationBuilder.CreateIndex(
                name: "IX_GameFormPlatformTypes_PlatformTypeId",
                table: "GameFormPlatformTypes",
                column: "PlatformTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GameForms_GameTypeId",
                table: "GameForms",
                column: "GameTypeId");
        }
    }
}
