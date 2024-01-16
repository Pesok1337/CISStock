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
        public bool DeleteSupplier(Supplier Supplier)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    var supplier = context.Suppliers.Find(Supplier.SupplierId);

                    if (supplier == null)
                    {
                        return false;
                    }

                    context.Suppliers.Remove(supplier);

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

        //public bool SaveInvoiceDTO(InvoiceDTO invoiceDTO)
        //{
        //    try
        //    {
        //        using (var context = new ApplicationContext())
        //        {
        //            // Преобразуем InvoiceDTO обратно в Invoice
        //            var invoice = MapToEntity(invoiceDTO);

        //            // При необходимости выполните другие действия (например, добавление поставщиков, товаров)
        //            context.Invoices.Add(invoice);
        //            context.SaveChanges();
        //            return true; // В случае успешного сохранения
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Обработка ошибок, например, запись в лог или возврат false
        //        Console.WriteLine(ex.Message);
        //        if (ex.InnerException != null)
        //        {
        //            Console.WriteLine("Inner Exception:");
        //            Console.WriteLine(ex.InnerException.Message);
        //        }
        //        return false;
        //    }
        //}
        private void SaveProducts(IEnumerable<Product> products, ApplicationContext context)
        {
            foreach (var product in products)
            {
                // Проверяем, что товар не связан ни с накладной, ни с продажей
                if (product.Invoice == null && product.Sale == null)
                {
                    context.Products.Add(product);
                    // Устанавливаем связь с накладной или продажей
                    if (product.InvoiceId.HasValue)
                    {
                        product.Invoice = context.Invoices.Find(product.InvoiceId);
                    }
                    else if (product.SaleId.HasValue)
                    {
                        product.Sale = context.Sales.Find(product.SaleId);
                    }
                }
            }

            // Сохраняем изменения в товарах
            context.SaveChanges();
        }


        public bool SaveInvoiceDTO(InvoiceDTO invoiceDTO)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    // Преобразуем InvoiceDTO обратно в Invoice
                    var invoice = MapToEntity(invoiceDTO);

                    // Устанавливаем связь для каждого продукта
                    foreach (var product in invoice.Products)
                    {
                        product.Invoice = invoice;
                    }

                    // Сохраняем товары
                    SaveProducts(invoice.Products, context);

                    foreach (var product in invoiceDTO.Products)
                    {
                        AddOrUpdateProductOnStock(product);
                    }

                    // Добавляем накладную
                    context.Invoices.Add(invoice);
                    context.SaveChanges();

                    return true; // В случае успешного сохранения
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception:");
                    Console.WriteLine(ex.InnerException.Message);
                }
                return false;
            }
        }


        public bool SaveSaleDTO(SaleDTO saleDTO)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    // Преобразуем SaleDTO обратно в Sale
                    var sale = MapToEntity(saleDTO);

                    foreach (var product in sale.Products)
                    {
                        product.Sale = sale;
                    }

                    // Сохраняем товары
                    SaveProducts(sale.Products, context);
                    foreach (var product in saleDTO.Products)
                    {
                        RemoveProductFromStock(product.ProductName, product.Quantity);
                    }
                    // Добавляем продажу
                    context.Sales.Add(sale);
                    context.SaveChanges();

                    return true; // В случае успешного сохранения
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception:");
                    Console.WriteLine(ex.InnerException.Message);
                }
                return false;
            }
        }



        public bool UpdateInvoiceDTO(InvoiceDTO updatedInvoiceDTO)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    // Преобразуем UpdatedInvoiceDTO обратно в Invoice
                    var updatedInvoice = MapToEntity(updatedInvoiceDTO);

                    // Получаем существующую накладную из базы данных
                    var existingInvoice = context.Invoices
                        .Include(i => i.Products)
                        .SingleOrDefault(i => i.InvoiceId == updatedInvoice.InvoiceId);

                    if (existingInvoice == null)
                    {
                        // Накладная не найдена, можно выполнить какие-то дополнительные действия или вернуть false
                        return false;
                    }

                    // Обновляем свойства существующей накладной
                    existingInvoice.InvoiceDate = updatedInvoice.InvoiceDate;
                    existingInvoice.SupplierId = updatedInvoice.SupplierId;

                    // Удаляем текущие товары у существующей накладной
                    context.Products.RemoveRange(existingInvoice.Products);

                    // Добавляем новые товары из обновленной накладной
                    foreach (var product in updatedInvoice.Products)
                    {
                        existingInvoice.Products.Add(product);
                    }

                    // Сохраняем изменения
                    context.SaveChanges();

                    return true; // В случае успешного обновления
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
        public Invoice MapToEntity(InvoiceDTO invoiceDTO)
        {
            // Создаем новый объект Invoice и заполняем его данными из InvoiceDTO
            var invoice = new Invoice
            {
                InvoiceId = invoiceDTO.InvoiceId,
                InvoiceDate = invoiceDTO.InvoiceDate,
                SupplierId = invoiceDTO.SupplierId,
                // Преобразуем ProductDTO в Product и добавляем их к накладной
                Products = invoiceDTO.Products.Select(productDTO => new Product
                {
                    ProductId = productDTO.ProductId,
                    ProductName = productDTO.ProductName,
                    Quantity = productDTO.Quantity,
                    InvoiceId = invoiceDTO.InvoiceId
                }).ToList()
            };

            return invoice;
        }

        //
        public List<Sale> GetSales()
        {
            using (var context = new ApplicationContext())
            {
                // Загрузить все накладные из базы данных
                List<Sale> sales = context.Sales.ToList();
                return sales;
            }
        }
        public List<DisplaySale> GetDisplaySales()
        {
            using (var context = new ApplicationContext())
            {
                var displaySales = context.Sales
                    .Include(i => i.Customer) // Убедитесь, что Supplier загружен
                    .Select(i => new DisplaySale(i.SaleId, i.SaleDate, i.Customer != null ? i.Customer.CustomerName : ""))
                    .ToList();

                return displaySales;
            }
        }

        //public bool SaveSaleDTO(SaleDTO saleDTO)
        //{
        //    try
        //    {
        //        using (var context = new ApplicationContext())
        //        {
        //            // Преобразуем saleDTO обратно в sale
        //            var sale = MapToEntity(saleDTO);

        //            // При необходимости выполните другие действия (например, добавление поставщиков, товаров)
        //            context.Sales.Add(sale);
        //            context.SaveChanges();
        //            return true; // В случае успешного сохранения
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Обработка ошибок, например, запись в лог или возврат false
        //        Console.WriteLine(ex.Message);
        //        if (ex.InnerException != null)
        //        {
        //            Console.WriteLine("Inner Exception:");
        //            Console.WriteLine(ex.InnerException.Message);
        //        }
        //        return false;
        //    }
        //}
        

        public bool UpdateSaleDTO(SaleDTO updatedSaleDTO)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    // Преобразуем UpdatedInvoiceDTO обратно в Invoice
                    var updatedSale = MapToEntity(updatedSaleDTO);

                    // Получаем существующую накладную из базы данных
                    var existingSale = context.Sales
                        .Include(i => i.Products)
                        .SingleOrDefault(i => i.SaleId == updatedSale.SaleId);

                    if (existingSale == null)
                    {
                        // Накладная не найдена, можно выполнить какие-то дополнительные действия или вернуть false
                        return false;
                    }

                    // Обновляем свойства существующей накладной
                    existingSale.SaleDate = updatedSale.SaleDate;
                    existingSale.CustomerId = updatedSale.CustomerId;

                    // Удаляем текущие товары у существующей накладной
                    context.Products.RemoveRange(existingSale.Products);

                    // Добавляем новые товары из обновленной накладной
                    foreach (var product in updatedSale.Products)
                    {
                        existingSale.Products.Add(product);
                    }

                    // Сохраняем изменения
                    context.SaveChanges();

                    return true; // В случае успешного обновления
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
        //
        public SaleDTO GetSaleDTOById(int saleId)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    Sale sale = context.Sales
                        .Include(i => i.Customer)
                        .Include(i => i.Products)
                        .SingleOrDefault(i => i.SaleId == saleId);

                    if (sale == null)
                    {
                        // Накладная с указанным идентификатором не найдена
                        Console.WriteLine($"Sale with ID {saleId} not found.");
                        return null;
                    }

                    // Преобразование в DTO для передачи через WCF
                    return MapToDTO(sale);
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок, например, запись в лог или возврат null
                Console.WriteLine($"Error while fetching invoice: {ex.Message}");
                return null;
            }
        }
        public SaleDTO MapToDTO(Sale sale)
        {
            return new SaleDTO
            {
                SaleId = sale.SaleId,
                SaleDate = sale.SaleDate,
                CustomerId = sale.CustomerId,
                CustomerName = sale.Customer.CustomerName,
                Products = sale.Products.Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Quantity = p.Quantity
                }).ToList()
            };
        }
        //public Sale MapToEntity(SaleDTO saleDTO)
        //{
        //    // Создаем новый объект sale и заполняем его данными из saleDTO
        //    var sale = new Sale
        //    {
        //        SaleId = saleDTO.SaleId,
        //        SaleDate = saleDTO.SaleDate,
        //        CustomerId = saleDTO.CustomerId,
        //        // Преобразуем ProductDTO в Product и добавляем их к накладной
        //        Products = saleDTO.Products.Select(productDTO => new Product
        //        {
        //            ProductId = productDTO.ProductId,
        //            ProductName = productDTO.ProductName,
        //            Quantity = productDTO.Quantity
        //        }).ToList()
        //    };

        //    return sale;
        //}
        public Sale MapToEntity(SaleDTO saleDTO)
        {
            // Создаем новый объект sale и заполняем его данными из saleDTO
            var sale = new Sale
            {
                SaleId = saleDTO.SaleId,
                SaleDate = saleDTO.SaleDate,
                CustomerId = saleDTO.CustomerId,
                // Преобразуем ProductDTO в Product и добавляем их к накладной
                Products = saleDTO.Products.Select(productDTO =>
                {
                    var product = new Product
                    {
                        ProductId = productDTO.ProductId,
                        ProductName = productDTO.ProductName,
                        Quantity = productDTO.Quantity,
                        SaleId = saleDTO.SaleId  // Установите SaleId для связи
                    };
                    return product;
                }).ToList()
            };

            return sale;
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
        public List<Customer> GetCustomers()
        {
            using (var context = new ApplicationContext())
            {
                List<Customer> customers = context.Customers.ToList();
                return customers;
            }
        }

        public bool AddOrUpdateProductOnStock(ProductDTO productDTO)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    var productOnStock = context.ProductOnStocks.SingleOrDefault(p => p.ProductOnStockName == productDTO.ProductName);

                    if (productOnStock == null)
                    {
                        // Если товар не существует, добавляем его в таблицу
                        productOnStock = new ProductOnStock
                        {
                            ProductOnStockName = productDTO.ProductName,
                            Quantity = productDTO.Quantity
                        };
                        context.ProductOnStocks.Add(productOnStock);
                    }
                    else
                    {
                        // Если товар уже существует, увеличиваем его количество
                        productOnStock.Quantity += productDTO.Quantity;
                    }

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool RemoveProductFromStock(string productOnStockName, int quantity)
        {
            try
            {
                using (var context = new ApplicationContext())
                {
                    var productOnStock = context.ProductOnStocks.SingleOrDefault(p => p.ProductOnStockName == productOnStockName);

                    if (productOnStock == null || productOnStock.Quantity < quantity)
                    {
                        // Если товара не существует или его количество меньше запрашиваемого, возвращаем ошибку
                        return false;
                    }

                    // Уменьшаем количество товара
                    productOnStock.Quantity -= quantity;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public List<ProductOnStock> GetProductsOnStock()
        {
            using (var context = new ApplicationContext())
            {
                //// Возвращаем все товары на складе
                //return context.ProductOnStocks.Select(p => new ProductOnStockDTO
                //{
                //    ProductOnStockId = p.ProductOnStockId,
                //    ProductOnStockName = p.ProductOnStockName,
                //    Quantity = p.Quantity
                //}).ToList();
                List<ProductOnStock> productOnStockDTO = context.ProductOnStocks.ToList();
                return productOnStockDTO;
            }
        }

    }
}
