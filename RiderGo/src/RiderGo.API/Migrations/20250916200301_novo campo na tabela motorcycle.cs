using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiderGo.API.Migrations
{
    /// <inheritdoc />
    public partial class novocamponatabelamotorcycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_from_2024",
                table: "motorcycles",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_from_2024",
                table: "motorcycles");
        }
    }
}
