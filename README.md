# ProjectDatabases  
*A Store Management Console Application (C#/.NET)*

ProjectDatabases is a C#/.NET console application that manages a simple store system using Entity Framework Core and SQLite.  
It provides full CRUD operations for customers, categories, products, OrderRow and orders, along with several reporting and analytics features implemented through database views.

## Features

### Customer Management
- List, add, edit, and delete customers  
- Emails stored as *obfuscated strings* using a simple XOR-based encryption helper  
- View total order count per customer (via database view)

### Category & Product Management
- Create, update, and remove categories  
- Add, list, edit, and delete products  
- View product sales totals (via database view)

### Order Management
- Create new orders  
- Add multiple order rows (products with quantities)  
- View order details  
- View combined order detail reports (via view)  
- Update order status via a dedicated status menu  

### Reporting & Analytics
- Order summaries  
- Product sales totals  
- Customer order count  
- Extended detail views (OrderDetailView)

## Tech Stack
- C# / .NET 7 (or later)
- Entity Framework Core
- SQLite || (file-based database `Shop.db`)
- JetBrains Rider 2025.3 (primary development environment)


## Data Model Overview

Main Entities
| Entity | Description |
 Customer | Name, address, *encrypted* email |
| Category | Product grouping |
| Product | Linked to a category; includes price |
| Order | Linked to a customer; contains multiple `OrderRow` records |
| OrderRow | Product, quantity, unit price |

 Database Views EF Core Keyless Models

| View | Purpose |
| OrderSummaryView | Displays order totals and aggregated info |
| CustomerOrderCountView | Shows number of orders per customer |
| ProductSalesView | Shows total sales per product |
| OrderDetailView | A combined order + line-item projection |

These views enable efficient reporting without manually writing complex queries inside the application.

## Getting Started

### Prerequisites
- .NET SDK installed (7.0+ recommended)
- No SQL server required — SQLite database file is created automatically
- Any IDE such as:
  - JetBrains Rider
  - Visual Studio
  - VS Code (with C# extension)
  

## Setup & Installation
- Microsoft.EntityFrameWorkCore.Design
- Microsoft.EntityFrameworkCore.Sqlite
- Microsoft.EntityFrameworkCore.Tools

### 1. Clone the repository
bash git  clone https://github.com/Filipmonteza/ProjectDatabas.git
cd ProjectDatabas
