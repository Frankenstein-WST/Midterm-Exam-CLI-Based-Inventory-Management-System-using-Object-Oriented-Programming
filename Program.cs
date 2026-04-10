/*
 ══════════════════════════════════════════════════════════════════
   NEXUS HUB — CLI Inventory Management System
   Midterm Practical Exam — IT Elective 2
   C# Console Application with OOP
 ══════════════════════════════════════════════════════════════════
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace NexusHub
{
    // ══════════════════════════════════════════════════════════════
    // ENUM
    // ══════════════════════════════════════════════════════════════

    public enum TransactionType { Restock, Deduct }

    // ══════════════════════════════════════════════════════════════
    // MODEL 1: Category
    // ══════════════════════════════════════════════════════════════

    public class Category
    {
        // Private fields
        private int    _id;
        private string _name;
        private string _description;

        // Default constructor
        public Category() { }

        // Parameterized constructor
        public Category(int id, string name, string description)
        {
            _id          = id;
            _name        = name;
            _description = description;
        }

        // Properties
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Category name cannot be empty.");
                _name = value.Trim();
            }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value == null ? "" : value.Trim(); }
        }
    }

    // ══════════════════════════════════════════════════════════════
    // MODEL 2: Supplier
    // ══════════════════════════════════════════════════════════════

    public class Supplier
    {
        // Private fields
        private int    _id;
        private string _name;
        private string _contactNumber;
        private string _email;

        // Default constructor
        public Supplier() { }

        // Parameterized constructor
        public Supplier(int id, string name, string contactNumber, string email)
        {
            _id            = id;
            _name          = name;
            _contactNumber = contactNumber;
            _email         = email;
        }

        // Properties
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Supplier name cannot be empty.");
                _name = value.Trim();
            }
        }

        public string ContactNumber
        {
            get { return _contactNumber; }
            set { _contactNumber = value == null ? "N/A" : value.Trim(); }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !value.Contains("@"))
                    throw new ArgumentException("Invalid email format.");
                _email = value == null ? "" : value.Trim();
            }
        }
    }

    // ══════════════════════════════════════════════════════════════
    // MODEL 3: Product
    // ══════════════════════════════════════════════════════════════

    public class Product
    {
        // Private fields
        private int     _id;
        private string  _name;
        private decimal _price;
        private int     _stockQuantity;
        private int     _categoryId;
        private int     _supplierId;
        private int     _lowStockThreshold;

        // Default constructor
        public Product() { }

        // Parameterized constructor
        public Product(int id, string name, decimal price, int quantity,
                       int categoryId, int supplierId, int lowStockThreshold = 5)
        {
            _id                = id;
            _name              = name;
            _price             = price;
            _stockQuantity     = quantity;
            _categoryId        = categoryId;
            _supplierId        = supplierId;
            _lowStockThreshold = lowStockThreshold;
        }

        // Properties
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Product name cannot be empty.");
                _name = value.Trim();
            }
        }

        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative.");
                _price = value;
            }
        }

        public int StockQuantity
        {
            get { return _stockQuantity; }
            set
            {
                if (value < 0) throw new ArgumentException("Stock cannot drop below zero.");
                _stockQuantity = value;
            }
        }

        public int CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        public int SupplierId
        {
            get { return _supplierId; }
            set { _supplierId = value; }
        }

        public int LowStockThreshold
        {
            get { return _lowStockThreshold; }
            set
            {
                if (value < 0) throw new ArgumentException("Threshold cannot be negative.");
                _lowStockThreshold = value;
            }
        }

        // Computed / read-only properties
        public decimal TotalValue => _price * _stockQuantity;
        public bool    IsLowStock => _stockQuantity <= _lowStockThreshold;
        public bool    OutOfStock => _stockQuantity == 0;
    }

    // ══════════════════════════════════════════════════════════════
    // MODEL 4: User
    // ══════════════════════════════════════════════════════════════

    public class User
    {
        // Private fields
        private int    _id;
        private string _username;
        private string _hashedPassword;   // passwords are never stored in plain text
        private string _role;

        // Default constructor
        public User() { }

        // Parameterized constructor — password is hashed immediately
        public User(int id, string username, string plainPassword, string role)
        {
            _id             = id;
            _username       = username;
            _hashedPassword = HashPassword(plainPassword);
            _role           = role;
        }

        // Properties
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username cannot be empty.");
                _username = value.Trim().ToLower();
            }
        }

        // Password property — only exposes the hash (never the raw value)
        public string Password
        {
            get { return _hashedPassword; }
            set { _hashedPassword = value; }
        }

        public string Role
        {
            get { return _role; }
            set { _role = value; }
        }

        // Hash helper — simple polynomial hash for demo purposes
        private static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be empty.");
            int hash = 17;
            foreach (char c in password)
                hash = hash * 31 + c;
            return Math.Abs(hash).ToString("X8");
        }

        // Authenticate by comparing hashes
        public bool Authenticate(string plainPassword)
        {
            return _hashedPassword == HashPassword(plainPassword);
        }
    }

    // ══════════════════════════════════════════════════════════════
    // MODEL 5: TransactionRecord
    // ══════════════════════════════════════════════════════════════

    public class TransactionRecord
    {
        // Private fields
        private int             _id;
        private int             _productId;
        private string          _productName;
        private TransactionType _transactionType;
        private int             _quantity;
        private DateTime        _date;
        private string          _performedBy;

        // Default constructor
        public TransactionRecord() { }

        // Parameterized constructor — Date is auto-assigned on creation
        public TransactionRecord(int id, int productId, string productName,
                                 TransactionType transactionType, int quantity,
                                 string performedBy)
        {
            _id              = id;
            _productId       = productId;
            _productName     = productName;
            _transactionType = transactionType;
            _quantity        = quantity;
            _date            = DateTime.Now;  // Auto-assigned
            _performedBy     = performedBy;
        }

        // Properties
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int ProductId
        {
            get { return _productId; }
            set { _productId = value; }
        }

        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }

        public TransactionType TxnType
        {
            get { return _transactionType; }
            set { _transactionType = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public string PerformedBy
        {
            get { return _performedBy; }
            set { _performedBy = value; }
        }

        // Computed label
        public string TypeLabel =>
            _transactionType == TransactionType.Restock ? "RESTOCK" : "DEDUCT";
    }

    // ══════════════════════════════════════════════════════════════
    // MAIN PROGRAM
    // ══════════════════════════════════════════════════════════════

    class Program
    {
        // ── In-memory storage (List<T> only — no database) ───────
        static List<Category>          categories   = new List<Category>();
        static List<Supplier>          suppliers    = new List<Supplier>();
        static List<Product>           products     = new List<Product>();
        static List<User>              users        = new List<User>();
        static List<TransactionRecord> transactions = new List<TransactionRecord>();

        // ── Auto-increment counters ──────────────────────────────
        static int nextCategoryId    = 1;
        static int nextSupplierId    = 1;
        static int nextProductId     = 1;
        static int nextTransactionId = 1;

        // ── Active session user ──────────────────────────────────
        static User currentUser = null;

        // ════════════════════════════════════════════════════════
        // ENTRY POINT
        // ════════════════════════════════════════════════════════

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            InitializeData();
            ShowLogin();
        }

        // ════════════════════════════════════════════════════════
        // SEED / DEFAULT DATA
        // ════════════════════════════════════════════════════════

        static void InitializeData()
        {
            // Default users (passwords stored as hash)
            users.Add(new User(1, "admin",  "admin123", "Admin"));
            users.Add(new User(2, "staff1", "staff123", "Staff"));

            // Default categories
            categories.Add(new Category(nextCategoryId++, "Electronics",  "Electronic devices and gadgets"));
            categories.Add(new Category(nextCategoryId++, "Furniture",    "Home and office furniture"));
            categories.Add(new Category(nextCategoryId++, "Clothing",     "Apparel and accessories"));
            categories.Add(new Category(nextCategoryId++, "Food & Bev",   "Consumables and beverages"));
            categories.Add(new Category(nextCategoryId++, "Tools",        "Hardware and hand tools"));

            // Default suppliers
            suppliers.Add(new Supplier(nextSupplierId++, "ABC Trading",    "09171234567", "abc@mail.com"));
            suppliers.Add(new Supplier(nextSupplierId++, "XYZ Supply Co.", "09281234567", "xyz@mail.com"));
            suppliers.Add(new Supplier(nextSupplierId++, "Global Imports", "09391234567", "global@mail.com"));

            // Default products (each with its own LowStockThreshold)
            products.Add(new Product(nextProductId++, "Wireless Mouse",       350.00m,  30, 1, 1, 5));
            products.Add(new Product(nextProductId++, "USB-C Cable 1m",       120.00m,  50, 1, 2, 10));
            products.Add(new Product(nextProductId++, "Mechanical Keyboard", 2499.00m,   8, 1, 1, 5));
            products.Add(new Product(nextProductId++, "Office Chair",        3500.00m,  10, 2, 2, 3));
            products.Add(new Product(nextProductId++, "Claw Hammer 16oz",     220.00m,  15, 5, 3, 4));
            products.Add(new Product(nextProductId++, "Bottled Water 500ml",   25.00m,   4, 4, 3, 5));
        }

        // ════════════════════════════════════════════════════════
        // UI HELPERS
        // ════════════════════════════════════════════════════════

        static void Clear() => Console.Clear();

        static void BoxTop(int w = 80)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔" + new string('═', w - 2) + "╗");
            Console.ResetColor();
        }

        static void BoxMid(int w = 80)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╠" + new string('═', w - 2) + "╣");
            Console.ResetColor();
        }

        static void BoxBot(int w = 80)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╚" + new string('═', w - 2) + "╝");
            Console.ResetColor();
        }

        static void BoxRow(string text, int w = 80)
        {
            int inner = w - 2;
            string padded = text.Length >= inner
                ? text.Substring(0, inner)
                : text + new string(' ', inner - text.Length);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("║");
            Console.ResetColor();
            Console.Write(padded);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("║");
            Console.ResetColor();
        }

        static void OK(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  [OK] {msg}");
            Console.ResetColor();
        }

        static void Err(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  [ERROR] {msg}");
            Console.ResetColor();
        }

        static void Warn(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  [!] {msg}");
            Console.ResetColor();
        }

        static string Ask(string label)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"\n  {label}: ");
            Console.ResetColor();
            return (Console.ReadLine() ?? "").Trim();
        }

        static string AskPassword(string label)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"\n  {label}: ");
            Console.ResetColor();
            string val = "";
            ConsoleKeyInfo k;
            while ((k = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (k.Key == ConsoleKey.Backspace && val.Length > 0)
                { val = val[..^1]; Console.Write("\b \b"); }
                else if (k.Key != ConsoleKey.Backspace)
                { val += k.KeyChar; Console.Write("*"); }
            }
            Console.WriteLine();
            return val;
        }

        static void Pause()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\n  Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        static string Clip(string s, int max)
        {
            if (s == null) return "";
            return s.Length <= max ? s : s.Substring(0, max - 1) + "~";
        }

        // ════════════════════════════════════════════════════════
        // LOGIN  (max 3 attempts)
        // ════════════════════════════════════════════════════════

        static void ShowLogin()
        {
            int attempts = 0;
            while (attempts < 3)
            {
                Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"
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
");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  Attempt {attempts + 1} of 3\n");
                Console.ResetColor();

                string username = Ask("Username");
                string password = AskPassword("Password");

                try
                {
                    var user = users.FirstOrDefault(u =>
                        u.Username == username.Trim().ToLower());

                    if (user != null && user.Authenticate(password))
                    {
                        currentUser = user;
                        OK($"Login successful! Welcome, {user.Username}!");
                        System.Threading.Thread.Sleep(800);
                        ShowMainMenu();
                        return;
                    }
                    else
                    {
                        attempts++;
                        Err($"Invalid username or password. {3 - attempts} attempt(s) remaining.");
                        Pause();
                    }
                }
                catch (Exception ex)
                {
                    Err($"Login error: {ex.Message}");
                    Pause();
                }
            }

            // Exceeded max attempts
            Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n  Too many failed attempts. The program will now exit.");
            Console.ResetColor();
            System.Threading.Thread.Sleep(1500);
        }

        // ════════════════════════════════════════════════════════
        // MAIN MENU
        // ════════════════════════════════════════════════════════

        static void ShowMainMenu()
        {
            while (true)
            {
                Clear();
                BoxTop();
                BoxRow("                   NEXUS HUB INVENTORY");
                BoxRow("                      MAIN MENU");
                BoxMid();
                BoxRow($"  👤 Logged in as: {currentUser.Username}                   Role: {currentUser.Role}");
                BoxMid();
                BoxRow("     [1] 📁 Category Management       - Manage product categories");
                BoxRow("     [2] 🏭 Supplier Management       - Manage suppliers");
                BoxRow("     [3] 📦 Product Management        - Add, view, update, delete products");
                BoxRow("     [4] 📊 Transactions & Reports    - Stock management and reports");
                BoxRow("     [0] 🚪 Logout / Exit");
                BoxBot();

                string choice = Ask("Select an option");
                switch (choice)
                {
                    case "1": CategoryMenu();  break;
                    case "2": SupplierMenu();  break;
                    case "3": ProductMenu();   break;
                    case "4": ReportsMenu();   break;
                    case "0":
                        currentUser = null;
                        ShowLogin();
                        return;
                    default:
                        Err("Invalid input! Please enter a number from the menu.");
                        Pause();
                        break;
                }
            }
        }

        // ════════════════════════════════════════════════════════
        // CATEGORY MANAGEMENT
        // ════════════════════════════════════════════════════════

        static void CategoryMenu()
        {
            while (true)
            {
                Clear();
                BoxTop();
                BoxRow("                   NEXUS HUB INVENTORY");
                BoxRow("               📁 CATEGORY MANAGEMENT");
                BoxMid();
                BoxRow("     [1] Add Category");
                BoxRow("     [2] View All Categories");
                BoxRow("     [3] Update Category");
                BoxRow("     [4] Delete Category");
                BoxRow("     [0] Back to Main Menu");
                BoxBot();

                string choice = Ask("Select an option");
                switch (choice)
                {
                    case "1": AddCategory();    break;
                    case "2": ViewCategories(); break;
                    case "3": UpdateCategory(); break;
                    case "4": DeleteCategory(); break;
                    case "0": return;
                    default: Err("Invalid input!"); Pause(); break;
                }
            }
        }

        static void AddCategory()
        {
            Clear();
            BoxTop(); BoxRow("  ➕ ADD CATEGORY"); BoxBot();
            try
            {
                string name = Ask("Category Name");
                string desc = Ask("Description");

                if (categories.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidOperationException($"Category '{name}' already exists.");

                var cat = new Category(nextCategoryId++, name, desc);
                categories.Add(cat);
                OK($"Category '{cat.Name}' added successfully! (ID: {cat.Id})");
            }
            catch (Exception ex) { Err(ex.Message); }
            Pause();
        }

        static void ViewCategories()
        {
            Clear();
            BoxTop(); BoxRow("  📋 VIEW ALL CATEGORIES"); BoxBot();

            if (categories.Count == 0) { Warn("No categories found."); Pause(); return; }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n  {"ID",-5} {"Name",-24} {"Description",-42}");
            Console.WriteLine("  " + new string('─', 72));
            Console.ResetColor();

            foreach (var c in categories)
                Console.WriteLine($"  {c.Id,-5} {Clip(c.Name, 23),-24} {Clip(c.Description, 41),-42}");

            Console.WriteLine("  " + new string('─', 72));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"\n  Total: {categories.Count} categories");
            Console.ResetColor();
            Pause();
        }

        static void UpdateCategory()
        {
            Clear();
            BoxTop(); BoxRow("  ✏️  UPDATE CATEGORY"); BoxBot();
            try
            {
                ViewCategories();
                string idStr = Ask("Enter Category ID to update");
                if (!int.TryParse(idStr, out int id)) throw new FormatException("Invalid ID format!");

                var cat = categories.FirstOrDefault(c => c.Id == id);
                if (cat == null) throw new ArgumentException($"Category ID {id} not found.");

                Console.WriteLine($"\n  Editing: {cat.Name} — press Enter to keep current value");
                string name = Ask($"New Name [{cat.Name}]");
                if (!string.IsNullOrWhiteSpace(name)) cat.Name = name;

                string desc = Ask($"New Description [{cat.Description}]");
                if (!string.IsNullOrWhiteSpace(desc)) cat.Description = desc;

                OK($"Category '{cat.Name}' updated successfully!");
            }
            catch (Exception ex) { Err(ex.Message); }
            Pause();
        }

        static void DeleteCategory()
        {
            Clear();
            BoxTop(); BoxRow("  🗑️  DELETE CATEGORY"); BoxBot();
            try
            {
                ViewCategories();
                string idStr = Ask("Enter Category ID to delete");
                if (!int.TryParse(idStr, out int id)) throw new FormatException("Invalid ID format!");

                var cat = categories.FirstOrDefault(c => c.Id == id);
                if (cat == null) throw new ArgumentException($"Category ID {id} not found.");

                if (products.Any(p => p.CategoryId == id))
                    throw new InvalidOperationException(
                        $"Cannot delete '{cat.Name}' — products are still linked to it.");

                Warn($"You are about to delete: {cat.Name} (ID: {cat.Id})");
                string confirm = Ask("Type YES to confirm");
                if (confirm != "YES") { Console.WriteLine("\n  Deletion cancelled."); Pause(); return; }

                categories.Remove(cat);
                OK($"Category '{cat.Name}' deleted successfully!");
            }
            catch (Exception ex) { Err(ex.Message); }
            Pause();
        }

        // ════════════════════════════════════════════════════════
        // SUPPLIER MANAGEMENT
        // ════════════════════════════════════════════════════════

        static void SupplierMenu()
        {
            while (true)
            {
                Clear();
                BoxTop();
                BoxRow("                   NEXUS HUB INVENTORY");
                BoxRow("               🏭 SUPPLIER MANAGEMENT");
                BoxMid();
                BoxRow("     [1] Add Supplier");
                BoxRow("     [2] View All Suppliers");
                BoxRow("     [3] Update Supplier");
                BoxRow("     [4] Delete Supplier");
                BoxRow("     [0] Back to Main Menu");
                BoxBot();

                string choice = Ask("Select an option");
                switch (choice)
                {
                    case "1": AddSupplier();    break;
                    case "2": ViewSuppliers();  break;
                    case "3": UpdateSupplier(); break;
                    case "4": DeleteSupplier(); break;
                    case "0": return;
                    default: Err("Invalid input!"); Pause(); break;
                }
            }
        }

        static void AddSupplier()
        {
            Clear();
            BoxTop(); BoxRow("  ➕ ADD SUPPLIER"); BoxBot();
            try
            {
                string name    = Ask("Company Name");
                string contact = Ask("Contact Number");
                string email   = Ask("Email Address");

                if (suppliers.Any(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidOperationException($"Supplier '{name}' already exists.");

                var sup = new Supplier(nextSupplierId++, name, contact, email);
                suppliers.Add(sup);
                OK($"Supplier '{sup.Name}' added successfully! (ID: {sup.Id})");
            }
            catch (Exception ex) { Err(ex.Message); }
            Pause();
        }

        static void ViewSuppliers()
        {
            Clear();
            BoxTop(); BoxRow("  📋 VIEW ALL SUPPLIERS"); BoxBot();

            if (suppliers.Count == 0) { Warn("No suppliers found."); Pause(); return; }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n  {"ID",-5} {"Company Name",-24} {"Contact Number",-16} {"Email",-28}");
            Console.WriteLine("  " + new string('─', 74));
            Console.ResetColor();

            foreach (var s in suppliers)
                Console.WriteLine($"  {s.Id,-5} {Clip(s.Name, 23),-24} " +
                                  $"{Clip(s.ContactNumber, 15),-16} {Clip(s.Email, 27),-28}");

            Console.WriteLine("  " + new string('─', 74));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"\n  Total: {suppliers.Count} suppliers");
            Console.ResetColor();
            Pause();
        }

        static void UpdateSupplier()
        {
            Clear();
            BoxTop(); BoxRow("  ✏️  UPDATE SUPPLIER"); BoxBot();
            try
            {
                ViewSuppliers();
                string idStr = Ask("Enter Supplier ID to update");
                if (!int.TryParse(idStr, out int id)) throw new FormatException("Invalid ID format!");

                var sup = suppliers.FirstOrDefault(s => s.Id == id);
                if (sup == null) throw new ArgumentException($"Supplier ID {id} not found.");

                Console.WriteLine($"\n  Editing: {sup.Name} — press Enter to keep current value");
                string name = Ask($"New Name [{sup.Name}]");
                if (!string.IsNullOrWhiteSpace(name)) sup.Name = name;

                string contact = Ask($"New Contact Number [{sup.ContactNumber}]");
                if (!string.IsNullOrWhiteSpace(contact)) sup.ContactNumber = contact;

                string email = Ask($"New Email [{sup.Email}]");
                if (!string.IsNullOrWhiteSpace(email)) sup.Email = email;

                OK($"Supplier '{sup.Name}' updated successfully!");
            }
            catch (Exception ex) { Err(ex.Message); }
            Pause();
        }

        static void DeleteSupplier()
        {
            Clear();
            BoxTop(); BoxRow("  🗑️  DELETE SUPPLIER"); BoxBot();
            try
            {
                ViewSuppliers();
                string idStr = Ask("Enter Supplier ID to delete");
                if (!int.TryParse(idStr, out int id)) throw new FormatException("Invalid ID format!");

                var sup = suppliers.FirstOrDefault(s => s.Id == id);
                if (sup == null) throw new ArgumentException($"Supplier ID {id} not found.");

                if (products.Any(p => p.SupplierId == id))
                    throw new InvalidOperationException(
                        $"Cannot delete '{sup.Name}' — products are still linked to it.");

                Warn($"You are about to delete: {sup.Name} (ID: {sup.Id})");
                string confirm = Ask("Type YES to confirm");
                if (confirm != "YES") { Console.WriteLine("\n  Deletion cancelled."); Pause(); return; }

                suppliers.Remove(sup);
                OK($"Supplier '{sup.Name}' deleted successfully!");
            }
            catch (Exception ex) { Err(ex.Message); }
            Pause();
        }

        // ════════════════════════════════════════════════════════
        // PRODUCT MANAGEMENT
        // ════════════════════════════════════════════════════════

        static void ProductMenu()
        {
            while (true)
            {
                Clear();
                BoxTop();
                BoxRow("                   NEXUS HUB INVENTORY");
                BoxRow("               📦 PRODUCT MANAGEMENT");
                BoxMid();
                BoxRow("     [1] Add Product");
                BoxRow("     [2] View All Products");
                BoxRow("     [3] Search Product");
                BoxRow("     [4] Update Product");
                BoxRow("     [5] Delete Product");
                BoxRow("     [0] Back to Main Menu");
                BoxBot();

                string choice = Ask("Select an option");
                switch (choice)
                {
                    case "1": AddProduct();    break;
                    case "2": ViewProducts();  break;
                    case "3": SearchProduct(); break;
                    case "4": UpdateProduct(); break;
                    case "5": DeleteProduct(); break;
                    case "0": return;
                    default: Err("Invalid input!"); Pause(); break;
                }
            }
        }

        static void PrintProductTable(List<Product> list)
        {
            if (list.Count == 0) { Warn("No products to display."); return; }

            Console.WriteLine();
            Console.WriteLine("  ┌──────┬──────────────────────┬───────────┬──────────┬──────────────────┬──────────────────┐");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  │  ID  │         Name         │   Price   │ Quantity │     Category     │     Supplier     │");
            Console.ResetColor();
            Console.WriteLine("  ├──────┼──────────────────────┼───────────┼──────────┼──────────────────┼──────────────────┤");

            foreach (var p in list)
            {
                string catName = categories.FirstOrDefault(c => c.Id == p.CategoryId)?.Name ?? "—";
                string supName = suppliers.FirstOrDefault(s => s.Id == p.SupplierId)?.Name ?? "—";

                if      (p.OutOfStock) Console.ForegroundColor = ConsoleColor.Red;
                else if (p.IsLowStock) Console.ForegroundColor = ConsoleColor.Yellow;
                else                   Console.ForegroundColor = ConsoleColor.Gray;

                Console.WriteLine(
                    $"  │ {p.Id,-4} │ {Clip(p.Name, 20),-20} │ ₱{p.Price,-8:N2} │ {p.StockQuantity,-8} │ {Clip(catName, 16),-16} │ {Clip(supName, 16),-16} │");
            }

            Console.ResetColor();
            Console.WriteLine("  └──────┴──────────────────────┴───────────┴──────────┴──────────────────┴──────────────────┘");
        }

        static void AddProduct()
        {
            Clear();
            BoxTop(); BoxRow("  ➕ ADD PRODUCT"); BoxBot();
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("\n  Categories: ");
                foreach (var c in categories) Console.Write($"[{c.Id}] {c.Name}  ");
                Console.WriteLine();
                Console.ResetColor();

                string catStr = Ask("Category ID");
                if (!int.TryParse(catStr, out int catId))
                    throw new FormatException("Invalid Category ID format!");
                if (!categories.Any(c => c.Id == catId))
                    throw new ArgumentException($"Category ID {catId} not found.");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("\n  Suppliers:  ");
                foreach (var s in suppliers) Console.Write($"[{s.Id}] {s.Name}  ");
                Console.WriteLine();
                Console.ResetColor();

                string supStr = Ask("Supplier ID");
                if (!int.TryParse(supStr, out int supId))
                    throw new FormatException("Invalid Supplier ID format!");
                if (!suppliers.Any(s => s.Id == supId))
                    throw new ArgumentException($"Supplier ID {supId} not found.");

                string name = Ask("Product Name");

                string priceStr = Ask("Price (₱)");
                if (!decimal.TryParse(priceStr, out decimal price) || price < 0)
                    throw new FormatException("Input must be a valid number format!");

                string qtyStr = Ask("Quantity");
                if (!int.TryParse(qtyStr, out int qty) || qty < 0)
                    throw new FormatException("Input must be a valid number format!");

                string thrStr = Ask("Low Stock Threshold (press Enter for default: 5)");
                int threshold = string.IsNullOrWhiteSpace(thrStr) ? 5
                    : int.TryParse(thrStr, out int t) && t >= 0 ? t : 5;

                var product = new Product(nextProductId++, name, price, qty, catId, supId, threshold);
                products.Add(product);
                OK($"Product '{product.Name}' added successfully! (ID: {product.Id})");
            }
            catch (Exception ex) { Err(ex.Message); }
            Pause();
        }

        static void ViewProducts()
        {
            Clear();
            BoxTop(); BoxRow("  📋 VIEW ALL PRODUCTS"); BoxBot();
            var sorted = products.OrderBy(p => p.CategoryId).ToList();
            PrintProductTable(sorted);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"\n  Total: {products.Count} products");
            Console.ResetColor();
            Pause();
        }

        static void SearchProduct()
        {
            Clear();
            BoxTop(); BoxRow("  🔍 SEARCH PRODUCT"); BoxBot();
            try
            {
                string kw = Ask("Enter product name or ID");
                if (string.IsNullOrWhiteSpace(kw))
                    throw new ArgumentException("Search keyword cannot be empty.");

                List<Product> results;
                if (int.TryParse(kw, out int searchId))
                    results = products.Where(p => p.Id == searchId).ToList();
                else
                    results = products.Where(p => p.Name.ToLower().Contains(kw.ToLower())).ToList();

                if (results.Count == 0)
                    Warn($"No products matched '{kw}'.");
                else
                {
                    OK($"{results.Count} result(s) found:");
                    PrintProductTable(results);
                }
            }
            catch (Exception ex) { Err(ex.Message); }
            Pause();
        }

        static void UpdateProduct()
        {
            Clear();
            BoxTop(); BoxRow("  ✏️  UPDATE PRODUCT"); BoxBot();
            try
            {
                PrintProductTable(products);
                string idStr = Ask("Enter Product ID to update");
                if (!int.TryParse(idStr, out int id))
                    throw new FormatException("Invalid ID format!");

                var p = products.FirstOrDefault(x => x.Id == id);
                if (p == null) throw new ArgumentException($"Product ID {id} not found.");

                Console.WriteLine($"\n  Editing: {p.Name} — press Enter to keep current value");

                string name = Ask($"New Name [{p.Name}]");
                if (!string.IsNullOrWhiteSpace(name)) p.Name = name;

                string priceStr = Ask($"New Price [₱{p.Price:N2}]");
                if (!string.IsNullOrWhiteSpace(priceStr))
                {
                    if (!decimal.TryParse(priceStr, out decimal price) || price < 0)
                        throw new FormatException("Input must be a valid number format!");
                    p.Price = price;
                }

                string thrStr = Ask($"New Low Stock Threshold [{p.LowStockThreshold}]");
                if (!string.IsNullOrWhiteSpace(thrStr))
                {
                    if (!int.TryParse(thrStr, out int thr) || thr < 0)
                        throw new FormatException("Threshold must be a non-negative integer.");
                    p.LowStockThreshold = thr;
                }

                OK($"Product '{p.Name}' updated successfully!");
            }
            catch (Exception ex) { Err(ex.Message); }
            Pause();
        }

        static void DeleteProduct()
        {
            Clear();
            BoxTop(); BoxRow("  🗑️  DELETE PRODUCT"); BoxBot();
            try
            {
                PrintProductTable(products);
                string idStr = Ask("Enter Product ID to delete");
                if (!int.TryParse(idStr, out int id))
                    throw new FormatException("Invalid ID format!");

                var p = products.FirstOrDefault(x => x.Id == id);
                if (p == null) throw new ArgumentException($"Product ID {id} not found.");

                Warn($"You are about to permanently delete: {p.Name} (ID: {p.Id})");
                string confirm = Ask("Type YES to confirm");
                if (confirm != "YES") { Console.WriteLine("\n  Deletion cancelled."); Pause(); return; }

                products.Remove(p);
                OK($"Product '{p.Name}' deleted successfully!");
            }
            catch (Exception ex) { Err(ex.Message); }
            Pause();
        }

        // ════════════════════════════════════════════════════════
        // TRANSACTIONS & REPORTS
        // ════════════════════════════════════════════════════════

        static void ReportsMenu()
        {
            while (true)
            {
                Clear();
                BoxTop();
                BoxRow("                   NEXUS HUB INVENTORY");
                BoxRow("             📊 TRANSACTIONS & REPORTS");
                BoxMid();
                BoxRow("     [1] Restock Product");
                BoxRow("     [2] Deduct Stock");
                BoxRow("     [3] View Transaction History");
                BoxRow("     [4] Show Low-Stock Items");
                BoxRow("     [5] Compute Total Inventory Value");
                BoxRow("     [0] Back to Main Menu");
                BoxBot();

                string choice = Ask("Select an option");
                switch (choice)
                {
                    case "1": RestockProduct();     break;
                    case "2": DeductStock();        break;
                    case "3": ViewTransactions();   break;
                    case "4": ShowLowStock();       break;
                    case "5": ShowInventoryValue(); break;
                    case "0": return;
                    default: Err("Invalid input!"); Pause(); break;
                }
            }
        }

        static void RestockProduct()
        {
            Clear();
            BoxTop(); BoxRow("  📥 RESTOCK PRODUCT"); BoxBot();
            try
            {
                PrintProductTable(products);
                string idStr = Ask("Enter Product ID to restock");
                if (!int.TryParse(idStr, out int id))
                    throw new FormatException("Invalid ID format!");

                var p = products.FirstOrDefault(x => x.Id == id);
                if (p == null) throw new ArgumentException($"Product ID {id} not found.");

                string amtStr = Ask("Quantity to add");
                if (!int.TryParse(amtStr, out int amount) || amount <= 0)
                    throw new FormatException("Input must be a valid number format!");

                p.StockQuantity += amount;

                transactions.Add(new TransactionRecord(
                    nextTransactionId++, p.Id, p.Name,
                    TransactionType.Restock, amount, currentUser.Username));

                OK($"Restocked! +{amount} units added to '{p.Name}'. New stock: {p.StockQuantity}");
            }
            catch (Exception ex) { Err(ex.Message); }
            Pause();
        }

        static void DeductStock()
        {
            Clear();
            BoxTop(); BoxRow("  📤 DEDUCT STOCK"); BoxBot();
            try
            {
                PrintProductTable(products);
                string idStr = Ask("Enter Product ID to deduct from");
                if (!int.TryParse(idStr, out int id))
                    throw new FormatException("Invalid ID format!");

                var p = products.FirstOrDefault(x => x.Id == id);
                if (p == null) throw new ArgumentException($"Product ID {id} not found.");

                string amtStr = Ask("Quantity to deduct");
                if (!int.TryParse(amtStr, out int amount) || amount <= 0)
                    throw new FormatException("Input must be a valid number format!");
                if (p.StockQuantity < amount)
                    throw new InvalidOperationException(
                        $"Not enough stock. Current: {p.StockQuantity}, Requested: {amount}.");

                p.StockQuantity -= amount;

                transactions.Add(new TransactionRecord(
                    nextTransactionId++, p.Id, p.Name,
                    TransactionType.Deduct, amount, currentUser.Username));

                OK($"Deducted! -{amount} units from '{p.Name}'. Remaining: {p.StockQuantity}");

                if (p.IsLowStock || p.OutOfStock)
                    Warn($"'{p.Name}' is now {(p.OutOfStock ? "OUT OF STOCK" : "LOW ON STOCK")}! " +
                         $"(Qty: {p.StockQuantity}, Threshold: {p.LowStockThreshold})");
            }
            catch (Exception ex) { Err(ex.Message); }
            Pause();
        }

        static void ViewTransactions()
        {
            Clear();
            BoxTop(); BoxRow("  📜 TRANSACTION HISTORY"); BoxBot();

            if (transactions.Count == 0) { Warn("No transactions recorded yet."); Pause(); return; }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n  {"#",-4} {"Type",-10} {"Product",-22} {"Qty",6} {"By",-12} {"Date & Time"}");
            Console.WriteLine("  " + new string('─', 72));
            Console.ResetColor();

            foreach (var t in transactions)
            {
                Console.ForegroundColor = t.TxnType == TransactionType.Restock
                    ? ConsoleColor.Green : ConsoleColor.Yellow;

                Console.WriteLine($"  {t.Id,-4} {t.TypeLabel,-10} {Clip(t.ProductName, 21),-22} " +
                                  $"{t.Quantity,6} {Clip(t.PerformedBy, 11),-12} {t.Date:MM/dd/yyyy HH:mm:ss}");
            }

            Console.ResetColor();
            Console.WriteLine("  " + new string('─', 72));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"\n  Total transactions: {transactions.Count}");
            Console.ResetColor();
            Pause();
        }

        static void ShowLowStock()
        {
            Clear();
            BoxTop(); BoxRow("  ⚠️  LOW-STOCK ITEMS"); BoxBot();

            var low = products.Where(p => p.IsLowStock || p.OutOfStock).ToList();
            if (low.Count == 0) { OK("All products are adequately stocked."); Pause(); return; }

            Warn($"{low.Count} product(s) are at or below their alert threshold:\n");
            PrintProductTable(low);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n  Please consider restocking these items.");
            Console.ResetColor();
            Pause();
        }

        static void ShowInventoryValue()
        {
            Clear();
            BoxTop(); BoxRow("  💰 COMPUTE TOTAL INVENTORY VALUE"); BoxBot();

            if (products.Count == 0) { Warn("No products in inventory."); Pause(); return; }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n  {"Product",-28} {"Qty",6} {"Unit Price",12} {"Total Value",14}");
            Console.WriteLine("  " + new string('─', 62));
            Console.ResetColor();

            decimal grandTotal = 0;
            foreach (var p in products)
            {
                Console.ForegroundColor = p.IsLowStock || p.OutOfStock
                    ? ConsoleColor.Yellow : ConsoleColor.Gray;
                Console.WriteLine($"  {Clip(p.Name, 27),-28} {p.StockQuantity,6} " +
                                  $"₱{p.Price,11:N2} ₱{p.TotalValue,13:N2}");
                grandTotal += p.TotalValue;
            }

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  " + new string('═', 62));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  {"GRAND TOTAL",-46} ₱{grandTotal,13:N2}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  " + new string('═', 62));
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"\n  Products tracked: {products.Count}   |   Computed: {DateTime.Now:MM/dd/yyyy HH:mm}");
            Console.ResetColor();
            Pause();
        }
    }
}
