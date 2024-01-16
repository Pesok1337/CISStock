using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client.SrvsReference;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            
        }
        private void SalesMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CustomersMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InvoicesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LoadInvoices();
        }

        public void LoadInvoices()
        {
            try
            {
                SrvsReference.Service1Client client = new SrvsReference.Service1Client();
                var displayInvoices = client.GetDisplayInvoices();
                invoicesDataGrid.ItemsSource = displayInvoices;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                MessageBox.Show("Ошибка при обновлении списка накладных.");
            }
        }

        private void ProductsOnStockMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ContractorsMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void CreateInvoice_Click(object sender, RoutedEventArgs e)
        {
            InvoiceWindow invoiceWindow = new InvoiceWindow();
            invoiceWindow.Show();

        }

        private void CreateSale_Click(object sender, RoutedEventArgs e)
        {
            SaleWindow saleWindow = new SaleWindow();
            saleWindow.Show();
        }

        private void UpdateInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SrvsReference.Service1Client client = new SrvsReference.Service1Client();
                var displayInvoices = client.GetDisplayInvoices();
                invoicesDataGrid.ItemsSource = displayInvoices;
                MessageBox.Show("Список накладных обновлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
                MessageBox.Show("Ошибка при обновлении списка накладных.");
            }
        }

        private void DeleteInvoice_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DataGridInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedRow = (DisplayInvoice)invoicesDataGrid.SelectedItem;

            if (selectedRow != null)
            {
                int invoiceId = selectedRow.InvoiceId;

                SrvsReference.Service1Client client = new SrvsReference.Service1Client();
                var invoice = client.GetInvoiceDTOById(invoiceId);

                InvoiceWindow invoiceWindow = new InvoiceWindow(invoice.InvoiceId);
                invoiceWindow.Show();
            }
        }
        private void DataGridSale_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {


            var selectedRow = (DisplaySale)salesDataGrid.SelectedItem;

            if (selectedRow != null)
            {
                int saleId = selectedRow.SaleId;

                SrvsReference.Service1Client client = new SrvsReference.Service1Client();
                var sale = client.GetSaleDTOById(saleId);

                SaleWindow saleWindow = new SaleWindow(sale.SaleId);
                saleWindow.Show();
            }
        }

        private void CreateSupplier_Click(object sender, RoutedEventArgs e)
        {
            SupplierWindow supplierWindow = new SupplierWindow();

            supplierWindow.Show();
        }

        private void UpdateSupplier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SrvsReference.Service1Client client = new SrvsReference.Service1Client();

                var suppliers = client.GetSuppliers();

                suppliersDataGrid.ItemsSource = suppliers;

                MessageBox.Show("Список поставщиков обновлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Ошибка при обновлении списка поставщиков.");
            }

        }
        private void DeleteSupplier_Click(object sender, RoutedEventArgs e)
        {
            var selectedSupplier = (ConsignmentNoteSupplier)suppliersDataGrid.SelectedItem;

            Service1Client client = new Service1Client();

            bool result = client.DeleteSupplier(selectedSupplier);

            if (result)
            {
                MessageBox.Show("Поставщик успешно удален.");
            }
            else
            {
                MessageBox.Show("Ошибка при удалении поставщика.");
            }

            client.Close();
        }
        private void CreateCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow customerWindow = new CustomerWindow();

            customerWindow.Show();
        }
        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SrvsReference.Service1Client client = new SrvsReference.Service1Client();

                var customers = client.GetCustomers();

                CustomersDataGrid.ItemsSource = customers;

                MessageBox.Show("Список поставщиков обновлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Ошибка при обновлении списка поставщиков.");
            }
        }
        private void DeleteCustomers_Click(object sender, RoutedEventArgs e)
        {

        }
        private void UpdateSale_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SrvsReference.Service1Client client = new SrvsReference.Service1Client();
                var displaySales = client.GetDisplaySales();
                salesDataGrid.ItemsSource = displaySales;
                MessageBox.Show("Список накладных обновлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                MessageBox.Show("Ошибка при обновлении списка накладных.");
            }
        }
        private void DeleteSale_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateProductOnStock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SrvsReference.Service1Client client = new SrvsReference.Service1Client();

                var productOnStock = client.GetProductsOnStock();

                productOnStockDataGrid.ItemsSource = productOnStock;

                MessageBox.Show("Список товаров на складе обновлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Ошибка при обновлении списка товаров на складе.");
            }
        }
    }
}

