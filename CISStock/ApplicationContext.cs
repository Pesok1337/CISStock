using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CISStock
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Users> users { get; set; } = null;
        public DbSet<UserRoles> userRoles { get; set; } = null;
        public DbSet<Roles> roles { get; set; } = null;
        public DbSet<Stock> stock { get; set; } = null;
        public DbSet<Products> products { get; set; } = null;
        public DbSet<PurchaseOrders> purchaseOrders { get; set; } = null;
        public DbSet<PurchaseOrderDetails> purchaseOrderDetails { get; set; } = null;
        public DbSet<Sales> sales { get; set; } = null;
        public DbSet<Suppliers> suppliers { get; set; } = null;

        public ApplicationContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost; port=5432; Database=Stock; UserName=postgres; Password=admin");
            //base.OnConfiguring(optionsBuilder);
        }

        [Table("Users")]
        public class Users
        {
            [Column("UserID")]
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public string UserName { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }
        [Table("UserRoles")]
        public class UserRoles
        {
            [Column("UserRolesID")]
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            List<Users> Users { get; set; } = new List<Users>();
            List<Roles> Roles { get; set; } = new List<Roles>();
        }
        [Table("Roles")]
        public class Roles
        {
            [Column("RolesID")]
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public string RoleName { get; set; }
            public string Description { get; set; }
        }
        [Table("Stock")]
        public class Stock
        {
            [Column("StockID")]
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            List<Products> Products { get; set; } = new List<Products>();
            public int QuantityOnHand { get; set; }
            public DateTime LastPurchaseDate { get; set; }
        }
        [Table("Sales")]
        public class Sales
        {
            [Column("SaleID")]
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            List<Products> ProductID { get; set; } = new List<Products>();
            public DateTime SaleDate { get; set; }
            public int QuantitySold { get; set; }
            public decimal UnitPrice { get; set; }

        }
        [Table("Products")]
        public class Products
        {
            [Column("ProductID")]
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public string ProductName { get; set; }
            public string Description { get; set; }
        }
        [Table("Suppliers")]
        public class Suppliers
        {
            [Column("SupplierID")]
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public string SupplierName { get; set; }
            public string ContactPerson { get; set; }
            // Другие свойства
        }
        [Table("PurchaseOrders")]
        public class PurchaseOrders
        {
            [Column("PurchaseOrderID")]
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            List<Suppliers> SupplierID { get; set; } = new List<Suppliers>();
            public DateTime OrderDate { get; set; }
            // Другие свойства
        }
        [Table("PurchaseOrderDetails")]
        public class PurchaseOrderDetails
        {
            [Column("PurchaseOrderDetailID")]
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            List<PurchaseOrders> PurchaseOrderID { get; set; } = new List<PurchaseOrders>();
            List<Products> ProductID { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            // Другие свойства
        }
    }
}
