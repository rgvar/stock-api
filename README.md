# Stock API

Stock API is a RESTful system for inventory management and control, developed in .NET. This project is designed to facilitate the tracking and administration of inventories efficiently and in a structured way.

## Features
- Built with .NET for robust performance.
- REST API for seamless integration with other systems.
- Ideal for projects requiring clear and scalable inventory control.

## Technologies used
- .NET
- C#
- Entity Framework (EF)
- SQL Server
- JWT

## Installation
`git clone https://github.com/rgvar/stock-api.git`

## Endpoints

### Authentication
- POST /api/auth/login - User login with credentials
- POST /api/auth/register - Register a new user

### Category Management
- GET /api/category - Retrieve all categories
- GET /api/category/{id}/products - Retrieve a category along with its products
- GET /api/category/{id} - Retrieve a specific category by ID
- POST /api/category - Create a new category (Admin only)
- DELETE /api/category/{id} - Delete a category by ID (Admin only)
- PUT /api/category/{id} - Update a category by ID (Admin only)

### Client Management
- GET /api/client - Retrieve all clients
- GET /api/client/{id} - Retrieve a specific client by ID
- POST /api/client - Create a new client (Admin only)
- PUT /api/client/{id} - Update a client by ID (Admin only)
- DELETE /api/client/{id} - Delete a client by ID (Admin only)

### InvoiceController
- GET /api/invoice - Retrieve all invoices
- GET /api/invoice/{id} - Retrieve a specific invoice by ID
- POST /api/invoice - Create a new invoice (Admin only)
- PUT /api/invoice/{id} - Update an invoice by ID (Admin only)
- DELETE /api/invoice/{id} - Delete an invoice by ID (Admin only)

### ProductController
- GET /api/product - Retrieve all products (with optional search)
- GET /api/product/{id} - Retrieve a specific product by ID
- POST /api/product - Create a new product (Admin only)
- PUT /api/product/{id} - Update a product by ID (Admin only)
- DELETE /api/product/{id} - Delete a product by ID (Admin only)

### PurchaseOrderController
- GET /api/purchaseorder - Retrieve all purchase orders
- GET /api/purchaseorder/{id} - Retrieve a specific purchase order by ID
- POST /api/purchaseorder - Create a new purchase order (Admin only)
- PUT /api/purchaseorder/{id} - Update a purchase order by ID (Admin only)
- DELETE /api/purchaseorder/{id} - Delete a purchase order by ID (Admin only)

### SalesOrderController
- GET /api/salesorder - Retrieve all sales orders
- GET /api/salesorder/{id} - Retrieve a specific sales order by ID
- POST /api/salesorder - Create a new sales order (Admin only)
- PUT /api/salesorder/{id} - Update a sales order by ID (Admin only)
- DELETE /api/salesorder/{id} - Delete a sales order by ID (Admin only)

### SupplierController
- GET /api/supplier - Retrieve all suppliers
- GET /api/supplier/{id} - Retrieve a specific supplier by ID
- POST /api/supplier - Create a new supplier (Admin only)
- PUT /api/supplier/{id} - Update a supplier by ID (Admin only)
- DELETE /api/supplier/{id} - Delete a supplier by ID (Admin only)



