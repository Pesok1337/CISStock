using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using static CISStock.ConsignmentNote;

namespace CISStock
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        bool AuthenticateUser(string username, string password);

        [OperationContract]
        bool RegisterUser(string username, string password, string firstName, string lastName, string role);

        [OperationContract]
        List<Invoice> GetInvoices();

        [OperationContract]
        bool DeleteSupplier(Supplier Supplier);
        //[OperationContract]
        //bool SaveInvoice(Invoice invoice);

        //[OperationContract]
        //bool UpdateInvoice(Invoice updatedInvoice);

        [OperationContract]
        bool SaveInvoiceDTO(InvoiceDTO invoiceDTO);
        [OperationContract]
        List<Sale> GetSales();

        [OperationContract]
        bool UpdateSaleDTO(SaleDTO updatedSaleDTO);
        [OperationContract]
        bool SaveSaleDTO(SaleDTO SaleDTO);

        [OperationContract]
        bool UpdateInvoiceDTO(InvoiceDTO updatedInvoiceDTO);

        //[OperationContract]
        //bool CheckInvoice(int InvoiceId);

        [OperationContract]
        List<Supplier> GetSuppliers();

        [OperationContract]
        bool SaveSupplier(SupplierDto supplierDto);

        [OperationContract]
        bool SaveCustomer(CustomerDto сustomerDto);

        [OperationContract]
        List<Customer> GetCustomers();

        //[OperationContract]
        //Invoice GetInvoiceById(int invoiceId);

        //[OperationContract]
        //string GetSupplierById(int supplierId);

        [OperationContract]
        List<DisplayInvoice> GetDisplayInvoices();

        [OperationContract]
        InvoiceDTO GetInvoiceDTOById(int invoiceId);

        [OperationContract]
        List<DisplaySale> GetDisplaySales();

        [OperationContract]
        SaleDTO GetSaleDTOById(int saleId);

        [OperationContract]
        bool AddOrUpdateProductOnStock(ProductDTO productDTO);

        [OperationContract]
        bool RemoveProductFromStock(string productOnStockName, int quantity);

        [OperationContract]
        List<ProductOnStock> GetProductsOnStock();

        // TODO: Добавьте здесь операции служб
    }
    [DataContract]
    public class SupplierDto
    {
        [DataMember]
        public int SupplierId { get; set; }

        [DataMember]
        public string SupplierName { get; set; }
    }
    [DataContract]
    public class CustomerDto
    {
        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public string CustomerName { get; set; }
    }
    [DataContract]
    public class DisplayInvoice
    {
        [DataMember]
        public int InvoiceId { get; set; }
        [DataMember]
        public DateTime InvoiceDate { get; set; }
        [DataMember]
        public string SupplierName { get; set; }

        public DisplayInvoice() { } 

        public DisplayInvoice(int invoiceId, DateTime invoiceDate, string supplierName)
        {
            InvoiceId = invoiceId;
            InvoiceDate = invoiceDate;
            SupplierName = supplierName;
        }
    }
    [DataContract]
    public class InvoiceDTO
    {
        [DataMember]
        public int InvoiceId { get; set; }

        [DataMember]
        public DateTime InvoiceDate { get; set; }

        [DataMember]
        public int SupplierId { get; set; }

        [DataMember]
        public string SupplierName { get; set; }

        [DataMember]
        public List<ProductDTO> Products { get; set; }
    }

    [DataContract]
    public class ProductDTO
    {
        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }

    [DataContract]
    public class DisplaySale
    {
        [DataMember]
        public int SaleId { get; set; }
        [DataMember]
        public DateTime SaleDate { get; set; }
        [DataMember]
        public string CustomerName { get; set; }

        public DisplaySale() { }

        public DisplaySale(int saleId, DateTime saleDate, string customerName)
        {
            SaleId = saleId;
            SaleDate = saleDate;
            CustomerName = customerName;
        }
    }
    [DataContract]
    public class SaleDTO
    {
        [DataMember]
        public int SaleId { get; set; }

        [DataMember]
        public DateTime SaleDate { get; set; }

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public List<ProductDTO> Products { get; set; }
    }

    [DataContract]
    public class ProductOnStockDTO
    {
        [DataMember]
        public int ProductOnStockId { get; set; }

        [DataMember]
        public string ProductOnStockName { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }

}
