using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiderGo.API.Migrations
{
    /// <inheritdoc />
    public partial class Ridertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "is_from_2024",
                table: "motorcycles",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.CreateTable(
                name: "riders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cnpj = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    birth = table.Column<DateOnly>(type: "date", nullable: false),
                    cnh_number = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    cnh_type = table.Column<string>(type: "varchar", maxLength: 2, nullable: false),
                    cnh_image_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rider", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_rider_cnh_number",
                table: "riders",
                column: "cnh_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_rider_cnpj",
                table: "riders",
                column: "cnpj",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "riders");

            migrationBuilder.AlterColumn<bool>(
                name: "is_from_2024",
                table: "motorcycles",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);
        }
    }
}
