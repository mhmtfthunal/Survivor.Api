using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Survivor.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Competitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competitors_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedDate", "IsDeleted", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Ünlüler" },
                    { 2, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Gönüllüler" }
                });

            migrationBuilder.InsertData(
                table: "Competitors",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "FirstName", "IsDeleted", "LastName", "ModifiedDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Acun", false, "Ilıcalı", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Aleyna", false, "Avcı", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Hadise", false, "Açıkgöz", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Sertan", false, "Bozkuş", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 1, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Özge", false, "Açık", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 1, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Kıvanç", false, "Tatlıtuğ", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 2, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Ahmet", false, "Yılmaz", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 2, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Elif", false, "Demirtaş", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 2, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Cem", false, "Öztürk", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 2, new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Ayşe", false, "Karaca", new DateTime(2024, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CategoryId",
                table: "Competitors",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Competitors");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
