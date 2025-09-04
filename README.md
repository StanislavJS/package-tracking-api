---

## 📄 README.md

````markdown
# 📦 Package Tracking API

A simple **ASP.NET Core Web API** for managing package tracking.  
Implements CRUD operations, status transitions with validation rules, and status history logging.  

---

## 🚀 Features

- Create new packages with sender/recipient information.
- Auto-generated unique tracking number.
- View all packages or details of a specific one.
- Change package status with validation according to business rules.
- Store full history of status changes with timestamps.
- Swagger UI for testing.

---

## 🛠️ Tech Stack

- **ASP.NET Core 7**
- **Entity Framework Core (InMemory DB)**
- **Swagger / Swashbuckle**

---

## 📌 Status Transition Rules

- `Created` → `Sent`, `Canceled`
- `Sent` → `Accepted`, `Returned`, `Canceled`
- `Returned` → `Sent`, `Canceled`
- `Accepted` → Final (cannot be changed)
- `Canceled` → Final (cannot be changed)

---

## ▶️ Getting Started

### 1. Clone the repo

```bash
git clone https://github.com/your-username/package-tracking-api.git
cd package-tracking-api
```

### 2. Install dependencies

```bash
dotnet restore
```

### 3. Run the API

```bash
dotnet run
```

By default, the app runs on:

- `http://localhost:5259`
- Swagger UI: [http://localhost:5259/swagger](http://localhost:5259/swagger)

---

## 🔗 API Endpoints

### Packages

- **GET** `/api/packages` → Get all packages
- **GET** `/api/packages/{id}` → Get package by ID
- **GET** `/api/packages/track/{trackingNumber}` → Get package by tracking number
- **POST** `/api/packages` → Create new package
- **GET** `/api/packages/{id}/allowed-statuses` → Get allowed status transitions
- **PUT** `/api/packages/{id}/status` → Update package status

---

## 📦 Example Requests

### Create Package

```json
{
  "senderName": "Alice",
  "senderAddress": "123 Main St",
  "senderPhone": "+123456789",
  "recipientName": "Bob",
  "recipientAddress": "456 Market St",
  "recipientPhone": "+987654321"
}
```

### Update Status

```json
{
  "newStatus": "Sent"
}
```

---

## ✅ Evaluation Criteria

- Functionality of the API
- Clean code
- Proper architecture and separation of concerns
- Use of EF Core InMemory
- Swagger for testing

```

---
```
