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
    /// Логика взаимодействия для CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        public CustomerWindow()
        {
            InitializeComponent();
        }

        private async void SaveCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            using (SrvsReference.Service1Client client = new Service1Client())
            {
                // Создайте CustomerDto из данных на форме
                var customerDto = new CustomerDto
                {
                    CustomerName = CustomerNameTextBox.Text
                };

                // Асинхронно вызываем метод для сохранения
                bool isSaved = await Task.Run(() => client.SaveCustomer(customerDto));

                if (isSaved)
                {
                    MessageBox.Show("Клиент успешно сохранен.");
                    Close();
                }
                else
                {
                    MessageBox.Show("Ошибка при сохранении Клиента.");
                }
            }

        }
    }
}
