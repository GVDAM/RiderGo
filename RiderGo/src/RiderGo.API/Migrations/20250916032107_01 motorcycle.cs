﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiderGo.API.Migrations
{
    /// <inheritdoc />
    public partial class _01motorcycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "motorcycles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    plate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_motorcycle", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_motorcycle_plate",
                table: "motorcycles",
                column: "plate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "motorcycles");
        }
    }
}
