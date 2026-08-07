using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HelpDesk.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RaisedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "CreatedDate", "Description", "Priority", "RaisedBy", "Status", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 8, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Requesting license key for Visual Studio Enterprise edition", "High", "John Doe", "Open", "Software License Request" },
                    { 2, new DateTime(2026, 8, 2, 11, 30, 0, 0, DateTimeKind.Unspecified), "Unable to connect to company VPN network from home", "Medium", "Jane Smith", "In Progress", "VPN Connectivity Issue" },
                    { 3, new DateTime(2026, 8, 3, 14, 15, 0, 0, DateTimeKind.Unspecified), "Flickering display on primary monitor", "Low", "Alice Johnson", "Closed", "Monitor Replacement" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}
