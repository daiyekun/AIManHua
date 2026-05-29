using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIManHua.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Username = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComicTasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    Prompt = table.Column<string>(type: "longtext", nullable: false),
                    Style = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    Status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComicTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComicTasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GeneratedImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ComicTaskId = table.Column<long>(type: "bigint", nullable: false),
                    ImageUrl = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: false),
                    MinioObjectKey = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneratedImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneratedImages_ComicTasks_ComicTaskId",
                        column: x => x.ComicTaskId,
                        principalTable: "ComicTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Storyboards",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ComicTaskId = table.Column<long>(type: "bigint", nullable: false),
                    PanelIndex = table.Column<int>(type: "int", nullable: false),
                    SceneDescription = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: false),
                    Dialogue = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: false),
                    LayoutType = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storyboards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Storyboards_ComicTasks_ComicTaskId",
                        column: x => x.ComicTaskId,
                        principalTable: "ComicTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ComicTasks_Status",
                table: "ComicTasks",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ComicTasks_UserId",
                table: "ComicTasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedImages_ComicTaskId",
                table: "GeneratedImages",
                column: "ComicTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Storyboards_ComicTaskId",
                table: "Storyboards",
                column: "ComicTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneratedImages");

            migrationBuilder.DropTable(
                name: "Storyboards");

            migrationBuilder.DropTable(
                name: "ComicTasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
