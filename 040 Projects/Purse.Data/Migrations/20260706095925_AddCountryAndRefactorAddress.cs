using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purse.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryAndRefactorAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Vendors",
                newName: "CountryId");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "IncomeSources",
                newName: "CountryId");

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: true),
                    Version = table.Column<byte[]>(type: "BLOB", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Vendors",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "IncomeSources",
                newName: "Country");
        }
    }
}
