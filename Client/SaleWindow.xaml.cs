using Client.SrvsReference;
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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для SaleWindow.xaml
    /// </summary>
    public partial class SaleWindow : Window
    {
        private SrvsReference.Service1Client client;
        private List<ProductDTO> products = new List<ProductDTO>();
        public SaleWindow()
        {
            InitializeComponent();
            Initialize();
        }

        public SaleWindow(int SaleId)
        {
            InitializeComponent();
            Initialize();
            LoadSale(SaleId);
        }

        private void Initialize()
        {
            client = new SrvsReference.Service1Client();
            LoadCustomers();
            // Инициализация DataGrid для товаров
            InitializeProductsGrid();
        }
        private void LoadSale(int saleId)
        {
            try
            {
                var saleDTO = client.GetSaleDTOById(saleId);
                datePicker.SelectedDate = saleDTO.SaleDate;
                customerComboBox.Text = saleDTO.CustomerName;
                SaleNumberLabel.Content = saleDTO.SaleId;

                // Присваиваем источник данных
                products = saleDTO.Products.ToList(); 
                productsDataGrid.ItemsSource = products;

                productsDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void LoadCustomers()
        {
            // Загрузка поставщиков из WCF
            try
            {
                SrvsReference.Service1Client client = new SrvsReference.Service1Client();
                var customers = client.GetCustomers();
                customerComboBox.ItemsSource = customers;
                customerComboBox.DisplayMemberPath = "CustomerName";
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

        private void SaveSaleButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем новую накладную
                var newSale = new SaleDTO
                {
                    SaleDate = datePicker.SelectedDate ?? DateTime.Now,
                    CustomerId = (customerComboBox.SelectedItem as ConsignmentNoteCustomer)?.CustomerId ?? 0,
                    Products = products.ToArray(),
                };

                bool success;
                // Проверяем, является ли это созданием новой накладной или редактированием существующей
                if (SaleNumberLabel.Content == null)
                {
                    success = client.SaveSaleDTO(newSale);
                }
                else
                {
                    // Редактируем существующую накладную
                    newSale.SaleId = (int)SaleNumberLabel.Content;
                    success = client.UpdateSaleDTO(newSale);
                    
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

