using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiderGo.API.Migrations
{
    /// <inheritdoc />
    public partial class createtablerentals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rentals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expected_end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    plan = table.Column<int>(type: "integer", nullable: false),
                    daily_rate = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    rider_id = table.Column<string>(type: "text", nullable: false),
                    motorcycle_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rental", x => x.Id);
                    table.ForeignKey(
                        name: "fk_rental_motorcycle",
                        column: x => x.motorcycle_id,
                        principalTable: "motorcycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_rental_rider",
                        column: x => x.rider_id,
                        principalTable: "riders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rentals_motorcycle_id",
                table: "rentals",
                column: "motorcycle_id");

            migrationBuilder.CreateIndex(
                name: "IX_rentals_rider_id",
                table: "rentals",
                column: "rider_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rentals");
        }
    }
}
