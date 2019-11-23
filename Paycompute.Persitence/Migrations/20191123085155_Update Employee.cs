using Microsoft.EntityFrameworkCore.Migrations;

namespace Paycompute.Persitence.Migrations
{
    public partial class UpdateEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnionMember",
                table: "Employees",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UnionMember",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Employees",
                nullable: false,
                defaultValue: 0);
        }
    }
}
