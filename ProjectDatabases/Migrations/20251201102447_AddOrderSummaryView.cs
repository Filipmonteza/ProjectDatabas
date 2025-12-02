using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectDatabases.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderSummaryView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW IF NOT EXISTS OrderSummaryView AS
            SELECT
                o.OrderDate,
                o.OrderId,
                c.CustomerName AS CustomerName,
                c.CustomerEmail AS CustomerEmail,
                IFNULL(SUM(orw.OrderRowQuantity * orw.OrderRowUnitPrice), 0) AS OrderTotalPrice
            FROM Orders o
            JOIN Customers c ON c.CustomerId = o.CustomerId
            LEFT JOIN OrderRows Orw ON orw.OrderId = o.OrderId
            GROUP BY o.OrderDate, o.OrderId, c.CustomerName, c.CustomerEmail;   
            ");
            
            // AFTER INSERT
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT EXISTS trg_OrderRow_Insert
            AFTER INSERT ON OrderRows
            BEGIN
                UPDATE Orders
                SET OrderTotalPrice = (
                    SELECT IFNULL (SUM(OrderRowQuantity * OrderRowUnitPrice), 0)
                    FROM OrderRows 
                    WHERE OrderId = NEW.OrderId
                )
                WHERE OrderId = NEW.OrderId;
            END;
            ");
            
            // AFTER UPDATE
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT  EXISTS trg_OrderRow_Update
            AFTER UPDATE ON OrderRows
            BEGIN
                UPDATE Orders
                SET OrderTotalPrice = (
                    SELECT IFNULL (SUM(OrderRowQuantity * OrderRowUnitPrice), 0)
                    FROM OrderRows
                    WHERE OrderId = NEW.OrderId
                )
                WHERE OrderId = NEW.OrderId;
            END;
            ");
            
            // AFTER DELETE
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT EXISTS trg_OrderRow_Delete
            AFTER DELETE ON OrderRows
            BEGIN
                   UPDATE Orders
                   SET OrderTotalPrice = (
                   SELECT IFNULL (SUM(OrderRowQuantity * OrderRowUnitPrice), 0)
                   FROM OrderRows
                   WHERE OrderId = OLD.OrderId;
                )
                 WHERE OrderId = OLD.OrderId;
            END;
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS OrderSummaryView");
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trg_OrderRow_Delete;");
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trg_OrderRow_Insert;");
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trg_OrderRow_Update;");
        }
    }
}
