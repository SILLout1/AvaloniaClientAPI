using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYApi2.Migrations
{
    public partial class AddEmployeePhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "employees",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "employees");
        }
    }
}
