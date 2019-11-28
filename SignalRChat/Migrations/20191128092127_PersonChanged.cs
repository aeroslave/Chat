using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalRChat.Migrations
{
    public partial class PersonChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Persons");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Persons",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Persons");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Persons",
                nullable: true);
        }
    }
}
