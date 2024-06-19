using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Awesome.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailVerifiedAt",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberVerificationToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PhoneNumberVerifiedAt",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerificationToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailVerifiedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumberVerificationToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumberVerifiedAt",
                table: "Users");
        }
    }
}
