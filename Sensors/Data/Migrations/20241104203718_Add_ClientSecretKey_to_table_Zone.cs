using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sensors.Data.Migrations
{
    public partial class Add_ClientSecretKey_to_table_Zone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientSecretKey",
                table: "Zones",
                type: "varchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientSecretKey",
                table: "Zones");
        }
    }
}
