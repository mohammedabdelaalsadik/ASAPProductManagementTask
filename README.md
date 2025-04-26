# 📦 Auth and Product Management System

## 🛠️ Tech Stack
- Backend: ASP.NET Core 9
- Frontend: Angular 18 
- Database: Memory Data base
- Authentication: JWT Bearer Tokens
- UI Library: Kendo UI for Angular

---

## 🧩 Project Structure

### Backend
- **Controllers**
  - `AuthController.cs`: Login, Register, Refresh Token.
  - `ProductController.cs`: CRUD operations for products.
- **Services**
  - `AuthService.cs`: Handles authentication logic.
  - `ProductService.cs`: Business logic for product management.
- **Repositories**
  - `Repository.cs`: Generic Repository for  Database operations .

### Frontend
- **Pages**
  - `LoginComponent` 
  - `RegisterComponent` 
  - `ProductListComponent`
  - `ProductFormComponent`
- **Services**
  - `AuthService`: Manages login, logout, and token storage.
  - `ProductService`: Communicates with product APIs.

---

## 🔐 Authentication Flow

1. User submits login credentials via `LoginComponent`.
2. `AuthService` calls backend `api/auth/login`.
3. Backend verifies credentials and returns a JWT token.
4. Frontend stores the token in `localStorage`.
5. All secure APIs require Authorization header with the Bearer token.

---

## 🛒 Product Management Flow

1. Frontend calls `ProductService` to list, create, update, or delete products.
2. Backend `ProductController` handles API requests.
3. Products are stored and retrieved from the database.
4. Kendo Grid displays products with pagination and filters.

---

## 📦 Setup Instructions

### Backend

```bash
cd BackendProject
dotnet restore
dotnet run

cd UI
cd asap-task-ui
npm install
ng serve --open
