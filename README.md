# 🛒 ECommerce API

A robust, production-ready RESTful API built with **ASP.NET Core 8** following **Clean Architecture** and **CQRS** patterns. It supports merchant product management with full variant and attribute flexibility, secured by **JWT authentication** with refresh token rotation.

---

## 📑 Table of Contents

- [Tech Stack](#-tech-stack)
- [Architecture Overview](#-architecture-overview)
- [Project Structure](#-project-structure)
- [Database Design](#-database-design)
- [Setup Instructions](#-setup-instructions)
- [Environment Configuration](#-environment-configuration)
- [API Documentation](#-api-documentation)
- [Design Patterns Used](#-design-patterns-used)
- [Security](#-security)

---

## 🧰 Tech Stack

| Layer          | Technology                          |
|----------------|--------------------------------------|
| Framework      | ASP.NET Core 8                       |
| ORM            | Entity Framework Core 8 (SQL Server) |
| Auth           | ASP.NET Core Identity + JWT Bearer   |
| Mediator       | MediatR 12                           |
| Mapping        | AutoMapper 13                        |
| Validation     | FluentValidation                     |
| Documentation  | Swagger / Swashbuckle                |
| Logging        | Serilog                              |

---

## 🏛️ Architecture Overview

This project follows **Clean Architecture**, which keeps the business logic completely independent of frameworks, databases, and external concerns. Dependencies only point **inward**.

```
┌──────────────────────────────────────────────────────┐
│                    ECommerce.API                     │  ← Presentation Layer
│         Controllers, Extensions, Program.cs          │
└───────────────────────┬──────────────────────────────┘
                        │ depends on
┌───────────────────────▼──────────────────────────────┐
│                ECommerce.Application                 │  ← Business Logic Layer
│   Commands, Queries, Handlers, DTOs, Validators,     │
│               Contracts (Interfaces)                 │
└───────────────────────┬──────────────────────────────┘
                        │ depends on
┌───────────────────────▼──────────────────────────────┐
│                  ECommerce.Domain                    │  ← Core Layer (no dependencies)
│         Models, Repository Interfaces, Common        │
└──────────────────────────────────────────────────────┘
                        ▲
                        │ implements
┌───────────────────────┴──────────────────────────────┐
│               ECommerce.Infrastructure               │  ← Data Access Layer
│    DbContext, Repositories, Migrations, Services,    │
│                 EF Configurations                    │
└──────────────────────────────────────────────────────┘
```

### 🔄 Request Flow (CQRS + Mediator)

```
HTTP Request
    │
    ▼
Controller
    │  sends Command / Query via IMediator
    ▼
MediatR Pipeline
    │  runs FluentValidation automatically
    ▼
Command / Query Handler
    │  calls Repository / Service
    ▼
Repository (Infrastructure)
    │  talks to SQL Server via EF Core
    ▼
Database
```

---

## 📁 Project Structure

```
ECommerce/
├── ECommerce.API/
│   ├── Controllers/
│   │   ├── IdentityController.cs          # Register, Login, Refresh Token
│   │   ├── ProductsController.cs          # Product CRUD
│   │   ├── ProductAttiributeController.cs # Product Attributes
│   │   ├── AttributeValuesController.cs   # Attribute Values
│   │   ├── VariantsController.cs          # Product Variants
│   │   └── VariantAttributeValuesController.cs
│   ├── Extensions/
│   │   └── WebApplicationBuilderExtensions.cs  # JWT + Swagger setup
│   └── Program.cs
│
├── ECommerce.Application/
│   ├── Authentication/
│   │   ├── Commands/         # Register, Login, RefreshToken
│   │   └── Dtos/
│   ├── Products/
│   │   ├── Command/          # Create, Update, Delete
│   │   ├── Queries/          # GetAll (paged), GetById
│   │   └── Dtos/
│   ├── ProductAttributes/
│   │   ├── Commands/
│   │   ├── Queries/
│   │   └── Dtos/
│   ├── AttributeValues/
│   ├── ProductVariants/
│   ├── VariantAttributeValues/
│   ├── Common/               # PagedResult<T>
│   ├── Contracts/            # Repository & Service Interfaces
│   └── Users/                # IUserContext, UserContext, CurrentUser
│
├── ECommerce.Domain/
│   ├── Models/               # All domain entities
│   ├── IRepositories/        # IMerchantRepository, IProductRepository
│   └── Common/               # ApiResponse<T>
│
└── ECommerce.Infrastructure/
    ├── Configurations/        # EF Fluent API configs per entity
    ├── Migrations/
    ├── Persistence/           # ApplicationDbContext
    ├── Repositories/          # Concrete repository implementations
    └── Services/              # TokenService
```

---

## 🗄️ Database Design

### Entity Relationship Overview

```
ApplicationUser (ASP.NET Identity)
    │ 1
    │
    ▼ N
Merchant ──────────────────────────────────────────┐
    │ 1                                             │
    │                                              UserId (string FK)
    ▼ N
Product
    │ 1                        │ 1
    │                          │
    ▼ N                        ▼ N
ProductAttribute          ProductVariant
    │ 1                        │ 1
    │                          │
    ▼ N                        ▼ N
AttributeValue ◄──── VariantAttributeValue
                  (junction / linking table)
```

### 📋 Tables & Key Design Decisions

#### `AspNetUsers` (Identity)
Standard ASP.NET Core Identity table. Extended via `ApplicationUser`.

#### `Merchants`
| Column     | Type     | Notes                          |
|------------|----------|--------------------------------|
| Id         | int PK   | Auto-increment                 |
| UserId     | nvarchar | FK → AspNetUsers (logical)     |
| Name       | nvarchar | Max 256 chars                  |
| CreatedAt  | datetime | Default: `GETUTCDATE()`        |
| UpdatedAt  | datetime | Default: `GETUTCDATE()`        |

Every registered user automatically becomes a Merchant. One-to-one relationship with the user.

#### `Products`
| Column      | Type          | Notes                           |
|-------------|---------------|---------------------------------|
| Id          | int PK        |                                 |
| MerchantId  | int FK        | → Merchants, CASCADE delete     |
| Name        | nvarchar(512) | Required                        |
| Description | nvarchar(2000)|                                 |
| Status      | nvarchar(50)  | Default: `"Active"`             |
| BasePrice   | decimal(18,2) | Required                        |
| CreatedAt   | datetime      | Default: `GETUTCDATE()`         |
| UpdatedAt   | datetime      | Default: `GETUTCDATE()`         |

#### `ProductAttributes`
Represents a named attribute category for a product (e.g., "Color", "Size").

| Column    | Type         | Notes                           |
|-----------|--------------|---------------------------------|
| Id        | int PK       |                                 |
| ProductId | int FK       | → Products, CASCADE delete      |
| Name      | nvarchar(256)| Required, unique per product    |

#### `AttributeValues`
Represents the possible values for a given attribute (e.g., "Red", "Blue" for "Color").

| Column             | Type         | Notes                               |
|--------------------|--------------|-------------------------------------|
| Id                 | int PK       |                                     |
| ProductAttributeId | int FK       | → ProductAttributes, CASCADE delete |
| Value              | nvarchar(512)| Required, unique per attribute      |

#### `ProductVariants`
A specific purchasable variation of a product (e.g., "Red + Large").

| Column        | Type          | Notes                          |
|---------------|---------------|--------------------------------|
| Id            | int PK        |                                |
| ProductId     | int FK        | → Products, CASCADE delete     |
| SKU           | nvarchar(256) | Required, **globally unique**  |
| Quantity      | int           | Default: 0                     |
| PriceOverride | decimal(18,2) | Optional, overrides BasePrice  |
| IsActive      | bit           | Default: true                  |
| CreatedAt     | datetime      |                                |
| UpdatedAt     | datetime      |                                |

#### `VariantAttributeValues` (Junction Table)
Links a ProductVariant to its chosen AttributeValues. Composite unique index on `(ProductVariantId, AttributeValueId)` prevents duplicate assignments.

| Column           | Type   | Notes                                            |
|------------------|--------|--------------------------------------------------|
| Id               | int PK |                                                  |
| ProductVariantId | int FK | → ProductVariants, CASCADE delete                |
| AttributeValueId | int FK | → AttributeValues, **NO ACTION** (avoids cycles) |

> ⚠️ `NO ACTION` on `AttributeValueId` is intentional to avoid EF Core's multiple cascade path error in SQL Server.

#### `RefreshTokens`
| Column            | Type     | Notes                          |
|-------------------|----------|--------------------------------|
| Id                | int PK   |                                |
| UserId            | nvarchar | FK → AspNetUsers               |
| JwtTokenId        | nvarchar | Links refresh token to a JWT   |
| RefreshTokenValue | nvarchar | Cryptographically random       |
| IsValid           | bit      | Revoked = false                |
| ExpiresAt         | datetime |                                |

---

## 🚀 Setup Instructions

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express / LocalDB)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Step 1 — Clone the Repository

```bash
git clone https://github.com/your-username/ECommerceAPI.git
cd ECommerceAPI
```

### Step 2 — Configure the Database Connection

Open `ECommerce.API/appsettings.json` and update the connection string:

```json
"ConnectionStrings": {
  "ECommerceDB": "Server=.;Database=ECommerceDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

> 💡 For a named SQL Server instance use: `Server=.\\SQLEXPRESS;...`

### Step 3 — Apply Database Migrations

In the terminal, navigate to the solution root and run:

```bash
dotnet ef database update --project ECommerce.Infrastructure --startup-project ECommerce.API
```

This will create the `ECommerceDB` database with all tables.

### Step 4 — Run the Application

```bash
cd ECommerce.API
dotnet run
```

Or press **F5** in Visual Studio.

The API will be available at:
- HTTP:  `http://localhost:5071`
- HTTPS: `https://localhost:7146`

Swagger UI: `https://localhost:7146/swagger`

---

## ⚙️ Environment Configuration

All configuration lives in `ECommerce.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "ECommerceDB": "Server=.;Database=ECommerceDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Issuer": "ECommerceTestAPI",
    "Audience": "ECommerceClients",
    "Key": "SuperSecretKey_1234567890_ThisMustBeAtLeast32Chars"
  }
}
```

| Key                        | Description                                          |
|----------------------------|------------------------------------------------------|
| `ConnectionStrings:ECommerceDB` | SQL Server connection string                   |
| `Jwt:Issuer`               | Token issuer name (must match on validation)         |
| `Jwt:Audience`             | Token audience name (must match on validation)       |
| `Jwt:Key`                  | HMAC-SHA256 signing key (**min 32 characters**)      |

> 🔐 **Production:** Never commit real secrets. Use environment variables or **Azure Key Vault** in production environments.

### Token Lifetimes

| Token         | Lifetime  | Configurable In         |
|---------------|-----------|-------------------------|
| Access Token  | 2 minutes | `LoginCommandHandler.cs`|
| Refresh Token | 5 minutes | `LoginCommandHandler.cs`|

---

## 📖 API Documentation

Swagger is enabled automatically in the **Development** environment and is accessible at `/swagger`.

### 🔑 Authentication

All endpoints except `Register`, `Login`, and `Refresh-Token` require a **Bearer token** in the `Authorization` header:

```
Authorization: Bearer <your_access_token>
```

---

### Identity Endpoints

#### `POST /api/Identity/Register`
Register a new user. A Merchant profile is automatically created.

**Request Body:**
```json
{
  "userName": "mahmoud123",
  "email": "mahmoud@example.com",
  "password": "Password1!"
}
```
**Password rules:** min 8 chars, at least one uppercase, one lowercase, one digit.

**Response:** `201 Created` with user info.

---

#### `POST /api/Identity/Login`
Authenticate and receive access + refresh tokens.

**Request Body:**
```json
{
  "email": "mahmoud@example.com",
  "password": "Password1!"
}
```

**Response:** `200 OK`
```json
{
  "accessToken": "eyJhbGci...",
  "refreshToken": "base64string...",
  "expiresAt": "2026-06-07T10:02:00Z"
}
```

---

#### `POST /api/Identity/Refresh-Token`
Exchange a valid refresh token for a new access token. The old refresh token is **revoked** (rotation strategy).

**Request Body:**
```json
{
  "refreshToken": "base64string..."
}
```

---

### 📦 Products Endpoints

> 🔒 All require authentication.

#### `GET /api/Products`
Get a paginated list of the authenticated merchant's products.

**Query Parameters:**
| Parameter     | Type   | Default | Description                   |
|---------------|--------|---------|-------------------------------|
| `searchPhrase`| string | null    | Filters by name or description|
| `pageNumber`  | int    | 1       | Page number                   |
| `pageSize`    | int    | 10      | Items per page (max 10)       |

**Response:** `200 OK` with `PagedResult<ProductDto>`

---

#### `GET /api/Products/{id}`
Get a single product by ID (must be owned by the authenticated merchant).

---

#### `POST /api/Products`
Create a new product.

**Request Body:**
```json
{
  "name": "Classic T-Shirt",
  "description": "A comfortable everyday t-shirt.",
  "basePrice": 19.99
}
```

---

#### `PATCH /api/Products/{id}`
Update an existing product.

**Request Body:**
```json
{
  "name": "Updated Name",
  "description": "Updated description here.",
  "basePrice": 24.99
}
```

---

#### `DELETE /api/Products/{id}`
Delete a product and all its attributes, variants, and related data (cascaded).

---

### 🏷️ Product Attributes Endpoints

Attributes define the dimensions of variation for a product (e.g., "Color", "Size").

#### `POST /api/ProductAttiribute/{productId}/attributes`
Create a new attribute for a product.

**Request Body:**
```json
{
  "name": "Color"
}
```

#### `GET /api/ProductAttiribute/attributes/{id}`
Get an attribute by its ID.

#### `GET /api/ProductAttiribute/product/{productId}`
Get all attributes for a specific product.

---

### 🎨 Attribute Values Endpoints

Values are the options within each attribute (e.g., "Red", "Blue" under "Color").

#### `POST /api/AttributeValues`
Add a value to an attribute.

**Request Body:**
```json
{
  "productAttributeId": 1,
  "value": "Red"
}
```

#### `GET /api/AttributeValues/{id}`
Get an attribute value by its ID.

#### `GET /api/AttributeValues/attribute/{productAttributeId}`
Get all values for a specific attribute.

---

### 🔀 Variants Endpoints

A variant is a specific purchasable combination (e.g., the "Red / Large" version of a t-shirt).

#### `POST /api/Variants`
Create a new variant.

**Request Body:**
```json
{
  "productId": 1,
  "sku": "TSHIRT-RED-L",
  "quantity": 100,
  "priceOverride": 21.99,
  "isActive": true
}
```
> `priceOverride` is optional. If omitted, the product's `BasePrice` applies.

#### `GET /api/Variants/{id}`
Get a variant by ID.

#### `GET /api/Variants/product/{productId}`
Get all variants for a product.

#### `PUT /api/Variants/{id}`
Fully update a variant (SKU, quantity, price, active status).

#### `DELETE /api/Variants/{id}`
Delete a variant and all its attribute value links.

---

### 🔗 Variant Attribute Values Endpoints

Links a variant to its specific attribute value choices.

**Example:** Variant "TSHIRT-RED-L" is linked to AttributeValue "Red" (Id=1) and AttributeValue "Large" (Id=5).

#### `POST /api/VariantAttributeValues`
Assign an attribute value to a variant.

**Request Body:**
```json
{
  "productVariantId": 1,
  "attributeValueId": 3
}
```

#### `GET /api/VariantAttributeValues/variant/{productVariantId}`
Get all attribute values assigned to a specific variant.

**Response:**
```json
[
  {
    "id": 1,
    "productVariantId": 1,
    "attributeValueId": 3,
    "attributeValueName": "Red"
  }
]
```

#### `DELETE /api/VariantAttributeValues/{id}`
Remove an attribute value assignment from a variant.

---

### 📐 Standard API Response Wrapper

Most endpoints return responses wrapped in `ApiResponse<T>`:

```json
{
  "success": true,
  "statusCode": 200,
  "message": "Products retrieved successfully!",
  "data": { ... },
  "errors": null,
  "timestamp": "2026-06-07T10:00:00Z"
}
```

---

## 🧩 Design Patterns Used

| Pattern              | Where Used                              | Why                                                       |
|----------------------|-----------------------------------------|-----------------------------------------------------------|
| **Clean Architecture**| Entire solution                        | Decouples business logic from infrastructure concerns     |
| **CQRS**             | All Application handlers               | Separates read and write responsibilities                 |
| **Mediator**         | MediatR in all Controllers & Handlers  | Decouples controllers from business logic                 |
| **Repository**       | All Infrastructure repositories        | Abstracts data access, enables unit testing               |
| **DTO / Mapping**    | AutoMapper profiles                    | Prevents domain model leakage to the API layer            |
| **Validation Pipeline**| FluentValidation + auto-validation   | Centralizes input validation, auto-fires before handler   |
| **Options Pattern**  | JWT config via `IConfiguration`        | Clean access to settings                                  |
| **Refresh Token Rotation** | TokenService                      | Security: each use issues a new token, old one is revoked |

---

## 🔐 Security

### Authentication Flow

```
1. POST /api/Identity/Login
        │
        ▼
   Validate credentials
        │
        ▼
   Issue Access Token (2 min) + Refresh Token (5 min)
        │
        ▼
   Client stores both tokens

2. When Access Token expires:
   POST /api/Identity/Refresh-Token  { refreshToken: "..." }
        │
        ▼
   Validate refresh token (not expired, not revoked)
        │
        ▼
   Revoke old refresh token
        │
        ▼
   Issue new Access Token + new Refresh Token
```

### Token Reuse Detection
If a **previously revoked** refresh token is used again, the system detects this as a potential token theft and **revokes all tokens** associated with that JWT family — forcing the user to log in again.

### Authorization
Every protected endpoint checks that:
1. The user is authenticated (valid JWT).
2. The requested resource **belongs to** the authenticated merchant. You cannot access, modify, or delete another merchant's products, attributes, or variants.

---

## 📝 Notes & Known Issues

- The `ProductAttiributeController` has a **typo** in the class/file name (`Attiribute` instead of `Attribute`) — this affects the route prefix in Swagger.
- Access and refresh token lifetimes are very short (2 min / 5 min) — suitable for development/demo. Increase for production.
- The `Merchant.UserId` column is `nvarchar(max)` — consider adding a proper FK constraint and indexing it for performance.

---

*Built with ❤️ using ASP.NET Core 8 and Clean Architecture principles.*
