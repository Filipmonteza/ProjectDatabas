using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectDatabases.Migrations
{
    /// <inheritdoc />
    public partial class ProductSalesView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW IF NOT EXISTS ProductSalesView AS
            SELECT
                p.ProductId,
                p.ProductName AS ProductName,
                IFNULL(SUM(orw.OrderRowQuantity),0) AS TotalQuantitySold
            FROM Products p
            LEFT JOIN OrderRows orw ON p.ProductId = orw.ProductId
            GROUP BY p.ProductId, p.ProductName;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS ProductSalesView");
        }
    }
}
