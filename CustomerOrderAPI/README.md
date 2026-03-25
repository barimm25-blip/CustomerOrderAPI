# 🛒 Customer Order Management API

A RESTful API for managing customer orders, built with ASP.NET Core Web API and SQL Server.

---

## 📌 Features

- **Customer Management** — Add, edit, soft-delete customers with search
- **Product Management** — Manage products with stock tracking and image support
- **Order Management** — Create orders with automatic stock deduction and total calculation
- **Order Status Tracking** — Status flow with change history log
- **Dashboard** — Summary KPIs, orders by status, monthly report, low stock alert
- **Swagger UI** — Interactive API documentation

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 9 Web API (C#) |
| Database | SQL Server Express 2025 |
| ORM | Entity Framework Core 9 |
| API Docs | Swagger / OpenAPI |
| Frontend | HTML, CSS, JavaScript (Vanilla) |

---

## 🗄️ Database Schema

| Table | Description |
|---|---|
| `Customers` | Customer information |
| `Products` | Product catalog with stock |
| `Orders` | Order header |
| `OrderItems` | Order line items |
| `OrderStatusLogs` | Status change history |

---

## 📁 Project Structure
```
CustomerOrderAPI/
├── Controllers/        — API endpoints
├── Data/               — DbContext
├── DTOs/               — Request / Response objects
├── Models/             — Entity models
├── Services/           — Business logic
│   └── Interfaces/
├── wwwroot/            — Frontend (HTML/CSS/JS)
│   ├── css/
│   ├── js/
│   ├── index.html      — Dashboard
│   ├── orders.html
│   ├── products.html
│   └── customers.html
├── appsettings.json
└── Program.cs
```

---

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- SQL Server Express
- Visual Studio 2022 or VS Code

### Setup

**1. Clone the repository**
```bash
git clone https://github.com/YOUR_USERNAME/CustomerOrderAPI.git
cd CustomerOrderAPI/CustomerOrderAPI
```

**2. Update connection string in `appsettings.json`**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS01;Database=CustomerOrderDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**3. Run database migration**
```bash
dotnet ef database update
```

**4. Run the application**
```bash
dotnet run
```

**5. Open in browser**
- Frontend: `http://localhost:5237/index.html`
- Swagger: `http://localhost:5237/swagger`

---

## 📡 API Endpoints

### Customers
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/customers` | Get all customers |
| GET | `/api/customers/{id}` | Get customer by ID |
| POST | `/api/customers` | Create customer |
| PUT | `/api/customers/{id}` | Update customer |
| DELETE | `/api/customers/{id}` | Soft delete customer |

### Products
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create product |
| PUT | `/api/products/{id}` | Update product |

### Orders
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/orders` | Get all orders (filter by status) |
| GET | `/api/orders/{id}` | Get order by ID |
| POST | `/api/orders` | Create order |
| PATCH | `/api/orders/{id}/status` | Update order status |
| DELETE | `/api/orders/{id}` | Cancel order |

### Dashboard
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/dashboard/summary` | KPI summary |
| GET | `/api/dashboard/orders-by-status` | Orders grouped by status |
| GET | `/api/dashboard/monthly-report` | Monthly report |
| GET | `/api/dashboard/low-stock` | Low stock products |

---

## ⚙️ Order Status Flow
```
Pending → Confirmed → Processing → Shipped → Completed
                                          ↘ Cancelled (Pending/Confirmed only)
```

---

## 👩‍💻 Author

Developed by **Arissa** as a portfolio project to demonstrate
RESTful API design with ASP.NET Core and SQL Server.