using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiderGo.API.Migrations
{
    /// <inheritdoc />
    public partial class novascolunasparadevolucaodamoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "return_date",
                table: "rentals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_amount",
                table: "rentals",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "return_date",
                table: "rentals");

            migrationBuilder.DropColumn(
                name: "total_amount",
                table: "rentals");
        }
    }
}
