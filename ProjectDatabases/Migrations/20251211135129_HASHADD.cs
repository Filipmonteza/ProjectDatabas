using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectDatabases.Migrations
{
    /// <inheritdoc />
    public partial class HASHADD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SsnHash",
                table: "Customers",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SsnSalt",
                table: "Customers",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SsnHash",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SsnSalt",
                table: "Customers");
        }
    }
}
