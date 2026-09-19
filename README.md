# 🍔 Online Food Ordering System

A complete full-stack food ordering web application built as a college/portfolio project.

- **Backend:** ASP.NET Core 8 Web API + Entity Framework Core (SQL Server) + JWT Authentication
- **Frontend:** HTML5, CSS3, Bootstrap 5, vanilla JavaScript (fetch API)

---

## 1. Project Structure

```
OnlineFoodOrderingSystem/
│
├── Backend/
│   └── FoodOrderingAPI/
│       ├── Controllers/          → API endpoints (Categories, FoodItems, Auth, Orders)
│       ├── Models/                → EF Core entity classes (database tables)
│       ├── DTOs/                  → Data Transfer Objects (what the API sends/receives)
│       ├── Data/                  → AppDbContext (EF Core) + seed data
│       ├── Services/               → TokenService (JWT creation)
│       ├── Program.cs             → App startup: DB, JWT, CORS, Swagger wiring
│       ├── appsettings.json       → Connection string & JWT settings
│       └── FoodOrderingAPI.csproj → NuGet package references
│
├── Frontend/
│   ├── index.html          → Home page: categories + menu grid + search
│   ├── cart.html           → Shopping cart page
│   ├── checkout.html       → Delivery details + place order
│   ├── login.html / register.html
│   ├── orders.html         → Order history ("My Orders")
│   ├── order-success.html  → Confirmation page after placing an order
│   ├── css/style.css       → Custom styling (on top of Bootstrap)
│   ├── js/
│   │   ├── api.js          → All API calls (fetch wrapper)
│   │   ├── auth.js         → Login / register / session / navbar
│   │   ├── cart.js         → Cart stored in localStorage
│   │   ├── app.js          → Home page logic (load menu, search, filter)
│   │   ├── checkout.js     → Cart + checkout page logic
│   │   └── orders.js       → Order history logic
│   └── images/             → Food images (placeholder included)
│
└── README.md  ← you are here
```

---

## 2. How the System Works (Architecture)

1. **Database (SQL Server via EF Core):** 5 tables — `Categories`, `FoodItems`, `Customers`, `Orders`, `OrderItems`. EF Core builds these automatically from the C# model classes using **Code-First Migrations**. Sample categories & food items are seeded automatically.

2. **Backend (ASP.NET Core Web API):** Exposes REST endpoints under `/api/...`. Handles:
   - Listing categories & food items (public)
   - Registration/login with **JWT tokens** (passwords hashed with BCrypt — never stored in plain text)
   - Placing orders (**requires login** — the API reads the customer's ID from the JWT token, so a user can only see/create their own orders)
   - Swagger UI at `/swagger` for testing every endpoint directly

3. **Frontend (HTML/CSS/JS/Bootstrap):** A static site (no build step needed) that calls the API using `fetch()`. The shopping cart lives in the browser's `localStorage` until checkout, at which point it's sent to the API as a single order.

### Request Flow Example (placing an order)
```
Browser (checkout.html)
   │  cart items + delivery address
   ▼
POST /api/orders   (Authorization: Bearer <JWT>)
   │
OrdersController.CreateOrder()
   │  reads CustomerId from the JWT claim
   │  looks up each FoodItem's live price
   │  calculates total, saves Order + OrderItems
   ▼
EF Core → SQL Server database
   │
Response: { id, status, totalAmount, items[...] }
   ▼
Browser redirects to order-success.html
```

---

## 3. Setup Instructions

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (comes with Visual Studio) **or** any SQL Server instance
- A code editor: Visual Studio 2022 / VS Code
- Any modern browser

### A. Run the Backend (API)

```bash
cd Backend/FoodOrderingAPI

# Restore NuGet packages
dotnet restore

# Install EF Core CLI tool (one-time, if you don't have it)
dotnet tool install --global dotnet-ef

# Create the initial migration (generates the SQL to build your tables)
dotnet ef migrations add InitialCreate

# Apply it to the database (creates FoodOrderingDB + seeds sample data)
dotnet ef database update

# Run the API
dotnet run
```

The API will start at **http://localhost:5000** (see `Properties/launchSettings.json`).
Open **http://localhost:5000/swagger** to explore and test every endpoint interactively.

> If you don't have SQL Server LocalDB, edit the `DefaultConnection` string in
> `appsettings.json` to point to your own SQL Server / Azure SQL instance.

### B. Run the Frontend

The frontend is plain static HTML/CSS/JS — no npm install needed. Just serve the `Frontend` folder with any static server (opening `index.html` directly with `file://` also mostly works, but a local server avoids CORS quirks):

**Option 1 — VS Code:** Right-click `index.html` → "Open with Live Server"

**Option 2 — Python (built into most systems):**
```bash
cd Frontend
python -m http.server 5500
```
Then open **http://localhost:5500**

> Make sure the backend is running first. If your API runs on a different port,
> update `API_BASE_URL` at the top of `Frontend/js/api.js`.

---

## 4. API Endpoints Reference

| Method | Endpoint                    | Auth?  | Description                          |
|--------|------------------------------|--------|---------------------------------------|
| GET    | /api/categories              | No     | List all categories                   |
| GET    | /api/categories/{id}         | No     | Get one category                      |
| POST   | /api/categories              | No*    | Create a category (admin)             |
| PUT    | /api/categories/{id}         | No*    | Update a category (admin)             |
| DELETE | /api/categories/{id}         | No*    | Delete a category (admin)             |
| GET    | /api/fooditems                | No     | List food items (`?categoryId=&search=`) |
| GET    | /api/fooditems/{id}           | No     | Get one food item                     |
| POST   | /api/fooditems                | No*    | Create a food item (admin)            |
| PUT    | /api/fooditems/{id}           | No*    | Update a food item (admin)            |
| DELETE | /api/fooditems/{id}           | No*    | Delete a food item (admin)            |
| POST   | /api/auth/register            | No     | Create account → returns JWT          |
| POST   | /api/auth/login               | No     | Login → returns JWT                   |
| POST   | /api/orders                   | **Yes**| Place an order                        |
| GET    | /api/orders/my                | **Yes**| Get logged-in customer's orders       |
| GET    | /api/orders/{id}               | **Yes**| Get one order (own orders only)       |
| PUT    | /api/orders/{id}/status         | No*    | Update order status (admin/staff)     |

`*` These admin endpoints are left open (no `[Authorize]`) for simplicity in this student project.
In a production app, you would add an `Admin` role and protect them with `[Authorize(Roles = "Admin")]`.

### Sample: Register
```http
POST /api/auth/register
Content-Type: application/json

{
  "fullName": "Nikhil Sharma",
  "email": "nikhil@example.com",
  "password": "Test@123",
  "phone": "9876543210",
  "address": "Nashik, Maharashtra"
}
```
Response includes a `token` — save it and send it as `Authorization: Bearer <token>` on order requests.

### Sample: Place Order
```http
POST /api/orders
Authorization: Bearer eyJhbGciOi...
Content-Type: application/json

{
  "deliveryAddress": "123 MG Road, Nashik",
  "contactPhone": "9876543210",
  "paymentMethod": "Cash on Delivery",
  "items": [
    { "foodItemId": 1, "quantity": 2 },
    { "foodItemId": 7, "quantity": 1 }
  ]
}
```

---

## 5. Key Concepts Used (good for viva / interview explanation)

- **Code-First EF Core migrations** — database schema generated from C# classes.
- **DTOs** — API never exposes EF entities directly, avoiding circular JSON references and over-posting.
- **JWT Authentication** — stateless login; the API doesn't store sessions, it just validates the signed token on each request.
- **Password hashing (BCrypt)** — plain text passwords are never stored.
- **RESTful API design** — resources (`categories`, `fooditems`, `orders`) with standard HTTP verbs.
- **CORS** — enabled so the static frontend (served from a different port/origin) can call the API.
- **localStorage cart** — client-side cart persists across page reloads without needing a backend "cart" table.
- **Separation of concerns** — Controllers (API logic) / Models (data) / DTOs (contracts) / Data (persistence) / Services (JWT).

---

## 6. Possible Extensions (if you want to go further)

- Add an **Admin Dashboard** page to manage food items/categories/orders visually.
- Add **role-based authorization** (`Admin` vs `Customer`).
- Add **order status tracking with SignalR** for live updates.
- Add **payment gateway integration** (Razorpay/Stripe) instead of "Cash on Delivery".
- Deploy backend to **Azure App Service** and frontend to **Netlify/Vercel**.

---

Built as a demonstration full-stack project: ASP.NET Core Web API + HTML/CSS/JS/Bootstrap.
