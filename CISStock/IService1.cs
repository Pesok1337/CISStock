using System;
using System.Collections.Generic;
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
        bool SaveInvoice(Invoice invoice);

        [OperationContract]
        List<Supplier> GetSuppliers();

        [OperationContract]
        bool SaveSupplier(SupplierDto supplierDto);

        [OperationContract]
        bool SaveCustomer(CustomerDto сustomerDto);

        //[OperationContract]
        //Invoice GetInvoiceById(int invoiceId);

        //[OperationContract]
        //string GetSupplierById(int supplierId);

        [OperationContract]
        List<DisplayInvoice> GetDisplayInvoices();

        [OperationContract]
        InvoiceDTO GetInvoiceDTOById(int invoiceId);

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

}
