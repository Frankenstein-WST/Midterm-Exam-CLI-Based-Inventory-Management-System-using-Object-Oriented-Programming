# 💻 Nexus Hub — CLI Inventory Management System

**Midterm Practical Exam — IT Elective 2**

**👨‍💻 Submitted By:** Franco B. Marcelo
BSIT-3A | Bicol University Polangui

---

## 📑 Table of Contents
1. [Project Overview](#-project-overview)
2. [Core Features](#-core-features)
3. [6 Applied OOP Principles](#-6-applied-oop-principles)
4. [System Flow](#-system-flow)
5. [Menu Structure](#-menu-structure)
6. [Model Classes](#-model-classes)
7. [Installation & Execution](#-installation--execution)
8. [Authentication](#-authentication)
9. [Application Previews](#-application-previews)
10. [Important System Notes](#-important-system-notes)

---

## 🎯 Project Overview

The **Nexus Hub** project is a fully functional, Command-Line Interface (CLI) Inventory Management application built entirely in **C#**. It was designed specifically to meet the midterm practical exam requirements, utilizing strict Object-Oriented Programming architectures without the use of an external database.

**Key Technical Highlights:**
- ✅ **Pure C# Console Application**
- ✅ **In-Memory Storage** utilizing `List<T>` only (No Database)
- ✅ **100% OOP Compliance** (Classes, Encapsulation, Methods, etc.)
- ✅ **Secure Login System** with role-based access and hashed passwords
- ✅ **Automated Audit Trail** for all stock adjustments
- ✅ **Dynamic Low-Stock Warnings** based on custom per-product thresholds
- ✅ **Financial Valuation** computing total inventory worth in Philippine Peso (₱)

---

## ✨ Core Features

### 🏷️ Category & 🏭 Supplier Management
| Feature | Description |
|---------|-------------|
| Add Category | Create new product classifications |
| Add Supplier | Register vendor profiles with contact details |
| View Categories | Display formatted tables of all active categories |
| View Suppliers | Display formatted tables of all active suppliers |
| Update Category | Modify existing category name and description |
| Update Supplier | Modify supplier name, contact number, and email |
| Delete Category | Remove categories (protected if products are linked) |
| Delete Supplier | Remove suppliers (protected if products are linked) |

### 📦 Product Management
| Feature | Description |
|---------|-------------|
| Add Product | Create inventory items linked to Categories and Suppliers |
| View All Products | Display the master inventory list with real-time stock status |
| Search Product | Query the inventory via keyword or Product ID |
| Update Product | Adjust pricing, descriptions, or alert thresholds |
| Delete Product | Remove obsolete items permanently from the catalog |

### 📈 Transactions & Stock Reports
| Feature | Description |
|---------|-------------|
| Restock Product | Add inventory units (automatically logs a transaction) |
| Deduct Stock | Register sales/removals (validates to prevent negative stock) |
| Transaction History | View a chronological audit trail of all stock movements |
| Low-Stock Alert | Generate a report of items currently below their safe threshold |
| Compute Total Value | Calculate total capital tied up in inventory (Price × Quantity) |

---

## 🧠 6 Applied OOP Principles

This project demonstrates the following Object-Oriented Programming concepts to maintain clean, readable, and scalable code:

### 1. Classes and Objects
Objects are instantiated from structured class blueprints to represent real-world entities.
```csharp
// Example: Instantiating a new Product object for the inventory
Product item = new Product(101, "Mechanical Keyboard", 2500.50m, 15, 1, 1);
```

### 2. Encapsulation
Sensitive data fields are hidden from the outside and only accessible via validation properties to prevent corrupt data.
```csharp
private int _stockQuantity; // Hidden private field

public int StockQuantity    // Controlled public property
{
    get { return _stockQuantity; }
    set
    {
        if (value < 0) throw new ArgumentException("Stock cannot drop below zero.");
        _stockQuantity = value;
    }
}
```

### 3. Constructors
Ensures every object is in a valid state the moment it is created, such as automatically generating timestamps.
```csharp
public TransactionRecord(int productId, string type, int quantity, string user)
{
    ProductId   = productId;
    Type        = type;
    Quantity    = quantity;
    Date        = DateTime.Now; // Auto-assigned on creation
    ProcessedBy = user;
}
```

### 4. Properties (Getters and Setters)
All internal class fields are accessed through properties rather than direct variables, providing a layer of data protection and allowing for computed values.
```csharp
public decimal TotalValue => _price * _stockQuantity; // Read-only computed property
```

### 5. Access Modifiers
Strict control over what data can be seen or modified by different parts of the application.
- `private` — Used for internal fields (encapsulation)
- `public` — Used for accessible properties and methods
- `static` — Used for global helper methods and the lists in the main program

### 6. Exception Handling
Robust `try-catch` implementations prevent the console application from crashing due to improper user inputs.
```csharp
try
{
    Console.Write("Enter Price: ");
    decimal price = decimal.Parse(Console.ReadLine());
}
catch (FormatException)
{
    Console.WriteLine("[ERROR] Input must be a valid number format!");
}
```

---

## 🔄 System Flow

```
┌─────────────────────────────────────────────────────────────┐
│                     PROGRAM START                           │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│           Initialize Default Users & Sample Data            │
│           (categories, suppliers, products)                 │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                      LOGIN SCREEN                           │
│              ┌─────────────────────────┐                    │
│              │ Username: _____________ │                    │
│              │ Password: _____________ │                    │
│              └─────────────────────────┘                    │
│                    (3 attempts max)                         │
└─────────────────────────────────────────────────────────────┘
                            │
              ┌─────────────┴─────────────┐
              │                           │
         [Success]                  [3x Failed]
              │                           │
              ▼                           ▼
┌─────────────────────┐      ┌─────────────────────┐
│     MAIN MENU       │      │    EXIT PROGRAM     │
└─────────────────────┘      └─────────────────────┘
              │
              ▼
    ┌─────────────────────────────────────────┐
    │  [1] Category Management                │
    │  [2] Supplier Management                │
    │  [3] Product Management                 │
    │  [4] Transactions & Reports             │
    │  [0] Logout / Exit                      │
    └─────────────────────────────────────────┘
              │
              ▼
    ┌─────────────────────────────────────────┐
    │              SUB-MENUS                  │
    │    (Each with CRUD operations)          │
    │    - Add, View, Update, Delete          │
    │    - Go Back option (press 0)           │
    └─────────────────────────────────────────┘
```

---

## 📑 Menu Structure

### Main Menu
```
╔══════════════════════════════════════════════════════════════════════════════╗
║                         NEXUS HUB INVENTORY                                  ║
║                            MAIN MENU                                         ║
╠══════════════════════════════════════════════════════════════════════════════╣
║  👤 Logged in as: admin                 Role: Admin                          ║
╠══════════════════════════════════════════════════════════════════════════════╣
║     [1] 📁 Category Management       - Manage product categories             ║
║     [2] 🏭 Supplier Management       - Manage suppliers                      ║
║     [3] 📦 Product Management        - Add, view, update, delete products    ║
║     [4] 📊 Transactions & Reports    - Stock management and reports          ║
║     [0] 🚪 Logout / Exit                                                     ║
╚══════════════════════════════════════════════════════════════════════════════╝
```

### Sub-Menus

**Category Management:**
- [1] Add Category
- [2] View All Categories
- [3] Update Category
- [4] Delete Category
- [0] Back to Main Menu

**Supplier Management:**
- [1] Add Supplier
- [2] View All Suppliers
- [3] Update Supplier
- [4] Delete Supplier
- [0] Back to Main Menu

**Product Management:**
- [1] Add Product
- [2] View All Products
- [3] Search Product
- [4] Update Product
- [5] Delete Product
- [0] Back to Main Menu

**Transactions & Reports:**
- [1] Restock Product
- [2] Deduct Stock
- [3] View Transaction History
- [4] Show Low-Stock Items
- [5] Compute Total Inventory Value
- [0] Back to Main Menu

---

## 📊 Model Classes

### Category.cs
| Property | Type | Description |
|----------|------|-------------|
| Id | int | Unique identifier |
| Name | string | Category name |
| Description | string | Category description |

### Supplier.cs
| Property | Type | Description |
|----------|------|-------------|
| Id | int | Unique identifier |
| Name | string | Supplier company name |
| ContactNumber | string | Phone number |
| Email | string | Email address |

### Product.cs
| Property | Type | Description |
|----------|------|-------------|
| Id | int | Unique identifier |
| Name | string | Product name |
| Price | decimal | Product price (in ₱) |
| Quantity | int | Stock quantity |
| CategoryId | int | Foreign key to Category |
| SupplierId | int | Foreign key to Supplier |
| LowStockThreshold | int | Custom alert level per product |
| TotalValue | decimal | Computed: Price × Quantity |

### User.cs
| Property | Type | Description |
|----------|------|-------------|
| Id | int | Unique identifier |
| Username | string | Login username |
| Password | string | Login password (Hashed) |
| Role | string | User role (Admin, Staff) |

### TransactionRecord.cs
| Property | Type | Description |
|----------|------|-------------|
| Id | int | Unique identifier |
| ProductId | int | Related product ID |
| ProductName | string | Product name (snapshot) |
| TransactionType | string | "RESTOCK" or "DEDUCT" |
| Quantity | int | Amount restocked/deducted |
| Date | DateTime | Transaction timestamp |
| PerformedBy | string | Username who performed action |

---

## 🚀 Installation & Execution

**Prerequisites:** Visual Studio 2022 or .NET SDK 8.0+

**1. Clone the repository:**
```bash
git clone https://github.com/yourusername/your-repo-name.git
```

**2. Navigate to the directory:**
```bash
cd NexusHub
```

**3. Compile the build:**
```bash
dotnet build
```

**4. Launch the application:**
```bash
dotnet run
```

---

## 🔐 Authentication

Default credentials pre-loaded into the system memory for testing the application:

| Username | Password | Permission Level |
|----------|----------|-----------------|
| admin | admin123 | Administrator |
| staff1 | staff123 | Standard User |

---

## 📸 Application Previews

### Secure Login Gateway
```
╔═══════════════════════════════════════════════════════════════════════════════════╗
║                                                                                   ║
║    ███╗   ██╗███████╗██╗  ██╗██╗   ██╗███████╗    ██╗  ██╗██╗   ██╗██████╗       ║
║    ████╗  ██║██╔════╝╚██╗██╔╝██║   ██║██╔════╝    ██║  ██║██║   ██║██╔══██╗      ║
║    ██╔██╗ ██║█████╗   ╚███╔╝ ██║   ██║███████╗    ███████║██║   ██║██████╔╝      ║
║    ██║╚██╗██║██╔══╝   ██╔██╗ ██║   ██║╚════██║    ██╔══██║██║   ██║██╔══██╗      ║
║    ██║ ╚████║███████╗██╔╝ ██╗╚██████╔╝███████║    ██║  ██║╚██████╔╝██████╔╝      ║
║    ╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝ ╚═════╝ ╚══════╝    ╚═╝  ╚═╝ ╚═════╝ ╚═════╝       ║
║                                                                                   ║
║                        INVENTORY MANAGEMENT SYSTEM v1.0                           ║
║                                                                                   ║
║                          Midterm Exam in IT Elective 2                            ║
║                         C# Console Application with OOP                           ║
║                                                                                   ║
╠═══════════════════════════════════════════════════════════════════════════════════╣
║                                 SECURE LOGIN                                      ║
╚═══════════════════════════════════════════════════════════════════════════════════╝

Username: admin
Password: ********
```

### Formatted Data Output
```
┌──────┬──────────────────────┬───────────┬──────────┬──────────────────┬──────────────────┐
│  ID  │         Name         │   Price   │ Quantity │     Category     │     Supplier     │
├──────┼──────────────────────┼───────────┼──────────┼──────────────────┼──────────────────┤
│ 1    │ Wireless Mouse       │ ₱350.00   │ 30       │ Electronics      │ ABC Trading      │
│ 2    │ USB-C Cable 1m       │ ₱120.00   │ 50       │ Electronics      │ XYZ Supply Co.   │
│ 3    │ Office Chair         │ ₱3500.00  │ 10       │ Furniture        │ Global Imports   │
│ 4    │ Claw Hammer 16oz     │ ₱220.00   │ 15       │ Tools            │ Global Imports   │
└──────┴──────────────────────┴───────────┴──────────┴──────────────────┴──────────────────┘
```

---

## 📌 Important System Notes

- **Data Persistence:** This application relies exclusively on `List<T>` collections for in-memory storage. All data is cleared when the program exits. This is intentional to comply with the "no database" midterm requirement.
- **Dynamic Low-Stock Alerts:** Unlike hardcoded systems, each product has its own customizable `LowStockThreshold` (defaults to 5). Items falling below their specific limit are automatically flagged in the reports.
- **Input Validation:** All user inputs are protected by robust `try-catch` blocks to prevent system crashes from invalid data formats.
- **Audit Logging:** Every restock and deduction operation is automatically recorded with a timestamp, transaction type, quantity changed, and the active user's name.
- **Delete Protection:** Categories and Suppliers cannot be deleted while products are still linked to them.
- **Login Security:** Passwords are stored as hashed values — never in plain text.

---

## 🛠️ Technologies Used
- **Language:** C#
- **Framework:** .NET 8.0
- **Environment:** Visual Studio 2022 / VS Code
- **Version Control:** Git & GitHub

---

## 📄 License
This project was developed strictly for educational purposes as a practical submission for the **IT Elective 2 Midterm Exam**.

---

## 👤 Author
**Franco B. Marcelo**
BSIT-3A | Bicol University Polangui
IT Elective 2 — Midterm Exam

*Last Updated: April 2026*
