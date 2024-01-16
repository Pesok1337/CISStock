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
using Client.SrvsReference;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для SupplierWindow.xaml
    /// </summary>
    public partial class SupplierWindow : Window
    {
        public SupplierWindow()
        {
            InitializeComponent();
        }

        private async void SaveSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            using (SrvsReference.Service1Client client = new Service1Client())
            {
                // Создайте SupplierDto из данных на форме
                var supplierDto = new SupplierDto
                {
                    SupplierName = supplierNameTextBox.Text
                };

                // Асинхронно вызываем метод для сохранения
                bool isSaved = await Task.Run(() => client.SaveSupplier(supplierDto));

                if (isSaved)
                {
                    MessageBox.Show("Поставщик успешно сохранен.");
                    Close();
                }
                else
                {
                    MessageBox.Show("Ошибка при сохранении поставщика.");
                }
            }
            
        }
    }
}
