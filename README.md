# 🛒 ASP.NET 8 E-Commerce API

An advanced and scalable **E-Commerce Web API** built with **ASP.NET 8**, following clean architecture principles with **MediatR**, **JWT authentication with refresh token support**, **Rate Limiting**, and **Entity Framework Core** with **SQL Server**.

---

## 🔧 Tech Stack

- **ASP.NET Core 8 Web API**
- **MediatR** – For CQRS and request/response separation
- **JWT Authentication** – With refresh token and token revocation
- **RateLimiter** – To throttle requests and protect from abuse
- **Entity Framework Core** + **SQL Server**
- **LINQ** – For powerful and readable data querying

---

## 📑 API Endpoints

### 🔐 Admin
- `GET /api/Admin/Users` — Get all users  
- `DELETE /api/Admin/DeleteUser` — Delete a user  
- `GET /api/Admin/Roles` — Get all roles  
- `POST /api/Admin/Seed_Role` — Seed a new role  
- `DELETE /api/Admin/DeleteRole` — Delete a role  

### 📂 Category
- `GET /api/Category/Categories` — Get all categories  
- `POST /api/Category/AddCategory` — Add a new category  
- `PUT /api/Category/UpdateCategory` — Update a category  
- `DELETE /api/Category/DeleteCategory` — Delete a category  

### 📦 Product
- `GET /api/Product/Products` — Get all products  
- `GET /api/Product/ProductName` — Get product by name  
- `POST /api/Product/AddProduct` — Add a new product  
- `PUT /api/Product/UpdateProduct` — Update a product  
- `DELETE /api/Product/DeleteProduct` — Delete a product  

### 📑 Order
- `GET /api/Order/GetOrders` — Get all orders  
- `GET /api/Order/UserOrders` — Get orders by user  
- `POST /api/Order/CreateOrder` — Create a new order  
- `DELETE /api/Order/DeleteOrder` — Delete an order  

### 📦 Stock
- `GET /api/Stock/Stocks` — Get all stock entries  
- `POST /api/Stock/AddStock` — Add new stock  
- `PUT /api/Stock/UpdateStock` — Update stock  
- `DELETE /api/Stock/DeleteStock` — Delete stock  

### 🏭 Supplier
- `GET /api/Supplier/GetSuppliers` — Get all suppliers  
- `POST /api/Supplier/AddSupplier` — Add a new supplier  
- `POST /api/Supplier/GetSupplier` — Get supplier by criteria (ID, name)  
- `POST /api/Supplier/AddSupplierProduct` — Link supplier to a product  

### 👤 User Authentication
- `POST /api/User/Register` — Register a new user  
- `POST /api/User/Login` — Login  
- `GET /api/User/Refresh` — Refresh JWT token  
- `GET /api/User/Revoke` — Revoke JWT token  
- `POST /api/User/VerifyEmail` — Verify user's email  
- `POST /api/User/ForgotPassword` — Send password reset link  
- `POST /api/User/ResetPassword` — Reset user password  
- `GET /api/User/GetUserByUserName/{username}` — Get user by username  

---

## 📌 Features

- ✅ **JWT Authentication** with refresh token and revocation  
- ✅ **Role-Based Access Control (RBAC)**  
- ✅ **Request Rate Limiting**  
- ✅ **Entity Relationships & Validation**  
- ✅ **Comprehensive API** for products, categories, users, orders, suppliers, and stock  

---

