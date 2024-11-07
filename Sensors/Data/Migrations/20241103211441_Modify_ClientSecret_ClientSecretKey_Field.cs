using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sensors.Data.Migrations
{
    public partial class Modify_ClientSecret_ClientSecretKey_Field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ClientSecretKey",
                table: "ClientSecrets",
                type: "varchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ClientSecretKey",
                table: "ClientSecrets",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(6)",
                oldMaxLength: 6);
        }
    }
}
