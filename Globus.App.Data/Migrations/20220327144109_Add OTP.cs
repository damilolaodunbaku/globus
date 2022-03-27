using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Globus.App.Data.Migrations
{
    public partial class AddOTP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OTPs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientEmailAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RecipientMobileNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    HashedOtp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsValidatedSuccessfully = table.Column<bool>(type: "bit", nullable: false),
                    ValidationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTPs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OTPs");
        }
    }
}
