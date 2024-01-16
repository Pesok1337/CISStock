using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static CISStock.ApplicationContext;

namespace CISStock
{
    public class ConsignmentNote
    {
        [Table("invoice")]
        public class Invoice
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int InvoiceId { get; set; }

            [Column("InvoiceDate")]
            public DateTime InvoiceDate { get; set; }

            // Связь с товарами в накладной
            public virtual ICollection<Product> Products { get; set; }

            // Связь с поставщиком
            [ForeignKey("Supplier")]
            public int SupplierId { get; set; }
            public virtual Supplier Supplier { get; set; }
        }

        [Table("sale")]
        public class Sale
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int SaleId { get; set; }

            [Column("SaleDate")]
            public DateTime SaleDate { get; set; }

            // Связь с товарами в продаже
            public virtual ICollection<Product> Products { get; set; }

            // Связь с покупателем
            [ForeignKey("Customer")]
            public int CustomerId { get; set; }
            public virtual Customer Customer { get; set; }
        }

        [Table("products")] //Товары
        public class Product
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ProductId { get; set; }

            [Column("ProductName")]
            public string ProductName { get; set; }

            [Column("Quantity")]
            public int Quantity { get; set; }

            // Связь с накладной
            [ForeignKey("Invoice")]
            public int? InvoiceId { get; set; }
            public virtual Invoice Invoice { get; set; }
            // Связь с накладной
            [ForeignKey("Sale")]
            public int? SaleId { get; set; }
            public virtual Sale Sale { get; set; }
        }
        [Table("products_on_stock")] //Товары на складе
        public class ProductOnStock
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ProductOnStockId { get; set; }

            [Column("ProductOnStockName")]
            public string ProductOnStockName { get; set; }

            [Column("Quantity")]
            public int Quantity { get; set; }

        }
        [Table("supplier")]
        public class Supplier
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int SupplierId { get; set; }

            [Column("SupplierName")]
            public string SupplierName { get; set; }

            // Связь с накладными
            public virtual ICollection<Invoice> Invoices { get; set; }
        }
        [Table("customers")]
        public class Customer
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int CustomerId { get; set; }

            [Column("CustomerName")]
            public string CustomerName { get; set; }

        }

    }
}
