using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static CISStock.ApplicationContext;
using static CISStock.ConsignmentNote;

namespace CISStock
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    public class Service1 : IService1
    {
        public bool AuthenticateUser(string username, string password)
        {
            using (var dbContext = new ApplicationContext()) 
            {
                var user = dbContext.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);

                return user != null; // Если пользователь найден, возвращаем true, иначе false
            }
        }

        public bool RegisterUser(string username, string password, string firstName, string lastName, string role)
        {
            using (var dbContext = new ApplicationContext()) 
            {
                // Проверяем, существует ли пользователь с таким же именем
                if (dbContext.Users.Any(u => u.UserName == username))
                {
                    return false; // Пользователь уже существует
                }

                //// Поиск роли в базе данных
                //var rolename = dbContext.Roles.FirstOrDefault(r => r.RoleName == role);

                //// Если роль не найдена, создаем новую роль
                //if (rolename == null)
                //{
                //    role = new Role { RoleName = rolename };
                //    dbContext.Roles.Add(rolename);
                //    dbContext.SaveChanges();
                //}

                // Создаем нового пользователя
                var user = new User
                {
                    UserName = username,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                    
                };

                // Добавляем пользователя в базу данных
                dbContext.Users.Add(user);
                dbContext.SaveChanges();

                return true; // Регистрация успешна
            }
        }

        public List<Invoice> GetInvoices()
        {
            using (var context = new ApplicationContext())
            {
                // Загрузить все накладные из базы данных
                List<Invoice> invoices = context.Invoices.ToList();
                return invoices;
            }
        }
        public List<DisplayInvoice> GetDisplayInvoices()
        {
            using (var context = new ApplicationContext())
            {
                var displayInvoices = context.Invoices
                    .Include(i => i.Supplier) // Убедитесь, что Supplier загружен
                    .Select(i => new DisplayInvoice(i.InvoiceId, i.InvoiceDate, i.Supplier != null ? i.Supplier.SupplierName : ""))
                    .ToList();

                return displayInvoices;
            }
        }
        public bool SaveInvoice(Invoice invoice)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    // При необходимости выполните другие действия (например, добавление поставщиков, товаров)
                    context.Invoices.Add(invoice);
                    context.SaveChanges();
                    return true; // В случае успешного сохранения
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок, например, запись в лог или возврат false
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception:");
                    Console.WriteLine(ex.InnerException.Message);
                }
                return false;
            }
        }

        public List<Supplier> GetSuppliers()
        {
            using (var context = new ApplicationContext())
            {
                List<Supplier> suppliers = context.Suppliers.ToList();
                return suppliers;
            }
        }

        public bool SaveSupplier(SupplierDto supplierDto)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    // Проверяем, существует ли поставщик с тем же идентификатором
                    if (context.Suppliers.Any(s => s.SupplierId == supplierDto.SupplierId))
                    {
                        Console.WriteLine("Поставщик с таким идентификатором уже существует.");
                        return false;
                    }

                    // Проверяем, существует ли поставщик с тем же именем
                    if (context.Suppliers.Any(s => s.SupplierName == supplierDto.SupplierName))
                    {
                        Console.WriteLine("Поставщик с таким именем уже существует.");
                        return false;
                    }

                    // Преобразуем SupplierDto в Supplier перед сохранением
                    var supplier = new Supplier
                    {
                        SupplierId = supplierDto.SupplierId,
                        SupplierName = supplierDto.SupplierName
                    };

                    context.Suppliers.Add(supplier);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //public Invoice GetInvoiceById(int invoiceId)
        //{
        //    using (var context = new ApplicationContext())
        //    {
        //        try
        //        {
        //            // Загрузка накладной с учетом связанных данных
        //            Invoice invoice = context.Invoices
        //                .Include(i => i.Supplier)
        //                .Include(i => i.InvoiceDate)
        //                .Include(i => i.Products)
        //                .SingleOrDefault(i => i.InvoiceId == invoiceId);

        //            if (invoice == null)
        //            {
        //                // Накладная с указанным идентификатором не найдена
        //                Console.WriteLine($"Invoice with ID {invoiceId} not found.");
        //            }

        //            return invoice;
        //        }
        //        catch (Exception ex)
        //        {
        //            // Обработка ошибок, например, запись в лог или возврат null
        //            Console.WriteLine($"Error while fetching invoice: {ex.Message}");
        //            return null;
        //        }
        //    }
        //}
        public InvoiceDTO GetInvoiceDTOById(int invoiceId)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    Invoice invoice = context.Invoices
                        .Include(i => i.Supplier)
                        .Include(i => i.Products)
                        .SingleOrDefault(i => i.InvoiceId == invoiceId);

                    if (invoice == null)
                    {
                        // Накладная с указанным идентификатором не найдена
                        Console.WriteLine($"Invoice with ID {invoiceId} not found.");
                        return null;
                    }

                    // Преобразование в DTO для передачи через WCF
                    return MapToDTO(invoice);
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок, например, запись в лог или возврат null
                Console.WriteLine($"Error while fetching invoice: {ex.Message}");
                return null;
            }
        }
        public InvoiceDTO MapToDTO(Invoice invoice)
        {
            return new InvoiceDTO
            {
                InvoiceId = invoice.InvoiceId,
                InvoiceDate = invoice.InvoiceDate,
                SupplierId = invoice.SupplierId,
                SupplierName = invoice.Supplier.SupplierName,
                Products = invoice.Products.Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Quantity = p.Quantity
                }).ToList()
            };
        }
        public bool SaveCustomer(CustomerDto customerDto)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    // Проверяем, существует ли Клиент с тем же идентификатором
                    if (context.Customers.Any(s => s.CustomerId == customerDto.CustomerId))
                    {
                        Console.WriteLine("Клиент с таким идентификатором уже существует.");
                        return false;
                    }

                    // Проверяем, существует ли Клиент с тем же именем
                    if (context.Customers.Any(s => s.CustomerName == customerDto.CustomerName))
                    {
                        Console.WriteLine("Клиент с таким именем уже существует.");
                        return false;
                    }

                    // Преобразуем CustomerDto в Customer перед сохранением
                    var customer = new Customer
                    {
                        CustomerId = customerDto.CustomerId,
                        CustomerName = customerDto.CustomerName
                    };

                    context.Customers.Add(customer);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


    }
}
