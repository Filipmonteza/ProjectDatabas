using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectDatabases.Migrations
{
    /// <inheritdoc />
    public partial class CustomerOrderCountView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW IF NOT EXISTS CustomerOrderCountView AS
            SELECT
                c.CustomerId,
                c.CustomerName AS CustomerName,
                c.CustomerEmail AS CustomerEmail,
            COUNT(o.OrderId) AS NumberOfOrders
            FROM Customers c
            LEFT JOIN Orders o ON c.CustomerId = o.CustomerId
            GROUP BY c.CustomerId, c.CustomerName, c.CustomerEmail;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS CustomerOrderCountView");
        }
    }
}
