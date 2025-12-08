# ProjectDatabas
Databas1

# ProjectDatabases Store Console App

A simple C\#/.NET console application for managing a small store database.  
It supports customers, categories, products, orders, and reporting using Entity Framework Core with SQLite.

## Features

- Manage customers (list, add, edit, delete)
- Manage categories and products
- Create orders with one or more order rows
- View order summaries and detailed order information
- View product sales statistics via database views
- View customer order counts via database views
- Simple XOR-based helper for obfuscating customer emails in the database

## Tech Stack

- C\#
- .NET (console application)
- Entity Framework Core
- SQLite
- JetBrains Rider 2025.3 on Windows

## Data Model

Main entities:

- `Customer`  
  - Stores name, address and an *encrypted* email using `EncryptionHelper`.
- `Category`  
  - Groups products.
- `Product`  
  - Belongs to a category and has a price.
- `Order`  
  - Linked to a customer and contains one or more `OrderRow` items.
- `OrderRow`  
  - A single line in an order (product, quantity, unit price).

Database views (keyless models):

- `OrderSummary` \(`OrderSummaryView`\)  
- `CustomerOrderCountView`  
- `ProductSalesView`  
- `OrderDetailView`

## Getting Started

### Prerequisites

- .NET SDK installed
- SQLite available (no manual setup required, database file is created automatically)
- IDE such as `JetBrains Rider 2025.3`

### Setup

1. Clone the repository:
   ```bash
   git clone <https://github.com/Filipmonteza/ProjectDatabas.git>
   cd <ProjectDatabases>
