using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectDatabases.Migrations
{
    /// <inheritdoc />
    public partial class OrderDetailView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW IF NOT EXISTS OrderDetailView AS
                SELECT
                    o.OrderId,
                    o.OrderDate,
                    c.CustomerName,
                    COUNT(orw.OrderRowId) AS TotalRows,
                    COALESCE(SUM(orw.OrderRowUnitPrice * orw.OrderRowQuantity), 0) AS TotalAmount
                FROM Orders AS o
                LEFT JOIN OrderRows AS orw ON o.OrderId = orw.OrderId
                LEFT JOIN Customers AS c ON c.CustomerId = o.CustomerId
                GROUP BY o.OrderId, c.CustomerName, o.OrderDate;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS OrderDetailView
            ");
        }
    }
}
