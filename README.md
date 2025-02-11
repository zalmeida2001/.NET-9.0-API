# Todo List API

## 📌 Overview

This is a **.NET 9 Web API** for managing a Todo List. The API includes authentication via **JWT**, database operations using **SQL Server Stored Procedures**, and **Redis caching** for performance optimization.

## 🚀 Features

- **User Authentication** (JWT-based login)
- **CRUD Operations for Tasks**
- **Database Integration** (SQL Server with Stored Procedures)
- **Redis Caching** (For improved response times)

## 📦 Technologies Used

- **.NET 9**
- **Entity Framework Core**
- **SQL Server** (with Stored Procedures)
- **Redis** (Caching layer)
- **JWT Authentication**

---

## 🔧 Setup Instructions

### 1️⃣ Configure SQL Server

Ensure **SQL Server** is running and update `appsettings.json`:

```json
"ConnectionStrings": {
  "TodoConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TodoApi;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 2️⃣ Run Database Migrations

```sh
dotnet ef database update
```

### 3️⃣ Run Redis (For Caching)

#### Option 1: **Using Docker** (Recommended)

```sh
docker run --name redis -d -p 6379:6379 redis
```

#### Option 2: **Using WSL**

```sh
sudo apt install redis-server -y
sudo service redis-server start
```

### 4️⃣ Run the API

```sh
dotnet run
```

API will be available at `http://localhost:5000` (or `https://localhost:5001` for HTTPS).

---

## 🔐 Authentication (JWT)

### **Login Endpoint**

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "testuser",
  "password": "password123"
}
```

Response:

```json
{
  "token": "your-jwt-token-here"
}
```

### **Using the Token in Requests**

```http
POST /api/todo/1
Content-Type: application/json
Authorization: Bearer your-jwt-token-here

{
    "title": "Clean kitchen",
    "isCompleted": false
}
```

---

## 📜 API Endpoints

### **Tasks**

| Method | Endpoint         | Description             | Auth Required |
| ------ | ---------------- | ----------------------- | ------------- |
| GET    | `/api/todo`      | Get all tasks           | ❌ No          |
| GET    | `/api/todo/{id}` | Get task by ID          | ❌ No          |
| POST   | `/api/todo`      | Create new task         | ✅ Yes         |
| PUT    | `/api/todo/{id}` | Update an existing task | ✅ Yes         |
| DELETE | `/api/todo/{id}` | Delete a task           | ✅ Yes         |

---

## 🔥 Running Tests

To run unit tests, execute:

```sh
dotnet test
```

---
