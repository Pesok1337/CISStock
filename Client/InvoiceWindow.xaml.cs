using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Client.SrvsReference;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для invoice.xaml
    /// </summary>
    public partial class InvoiceWindow : Window
    {
        private SrvsReference.Service1Client client;
        private List<ProductDTO> products = new List<ProductDTO>();
        public InvoiceWindow()
        {
            InitializeComponent();
            Initialize();
        }

        public InvoiceWindow(int invoiceId)
        {
            InitializeComponent();
            Initialize();
            LoadInvoice(invoiceId);
        }

        private void Initialize()
        {
            client = new SrvsReference.Service1Client();
            LoadSuppliers();
            // Инициализация DataGrid для товаров
            InitializeProductsGrid();
        }
        private void LoadInvoice(int invoiceId)
        {
            try
            {
                var invoiceDTO = client.GetInvoiceDTOById(invoiceId);
                datePicker.SelectedDate = invoiceDTO.InvoiceDate;
                supplierComboBox.Text = invoiceDTO.SupplierName;
                invoiceNumberLabel.Content = invoiceDTO.InvoiceId;

                // Присваиваем источник данных
                products = invoiceDTO.Products.ToList(); // Преобразуем массив в List
                productsDataGrid.ItemsSource = products;

                productsDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Обработка ошибок при загрузке накладной
            }
        }


        private void LoadSuppliers()
        {
            // Загрузка поставщиков из WCF
            try
            {
                SrvsReference.Service1Client client = new SrvsReference.Service1Client();
                var suppliers = client.GetSuppliers();
                supplierComboBox.ItemsSource = suppliers;
                supplierComboBox.DisplayMemberPath = "SupplierName";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void InitializeProductsGrid()
        {
            // Инициализация DataGrid для товаров
            productsDataGrid.AutoGenerateColumns = false;
            productsDataGrid.ItemsSource = products;
            // Создание столбцов для отображения товаров
            var productNameColumn = new DataGridTextColumn
            {
                Header = "Наименование товара",
                Binding = new System.Windows.Data.Binding("ProductName")
            };
            var quantityColumn = new DataGridTextColumn
            {
                Header = "Количество",
                Binding = new System.Windows.Data.Binding("Quantity")
            };
            productsDataGrid.Columns.Add(productNameColumn);
            productsDataGrid.Columns.Add(quantityColumn);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            products.Add(new ProductDTO { ProductName = "Новый товар", Quantity = 0 });
            productsDataGrid.Items.Refresh();
        }

        private void SaveInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем новую накладную
                var newInvoice = new InvoiceDTO
                {
                    InvoiceDate = datePicker.SelectedDate ?? DateTime.Now,
                    SupplierId = (supplierComboBox.SelectedItem as ConsignmentNoteSupplier)?.SupplierId ?? 0,
                    Products = products.ToArray(),
                };

                bool success;
                // Проверяем, является ли это созданием новой накладной или редактированием существующей
                if (invoiceNumberLabel.Content == null)
                {
                    success = client.SaveInvoiceDTO(newInvoice);
                }
                else
                {
                    // Редактируем существующую накладную
                    newInvoice.InvoiceId = (int)invoiceNumberLabel.Content;
                    success = client.UpdateInvoiceDTO(newInvoice);
                }

                if (success)
                {
                    MessageBox.Show("Накладная успешно сохранена!");
                    Close();
                }
                else
                {
                    MessageBox.Show("Не удалось сохранить накладную. Проверьте лог ошибок.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
