using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purse.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorCategoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "Categories");

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyBudget",
                table: "Categories",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthlyBudget",
                table: "Categories");

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "Categories",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
