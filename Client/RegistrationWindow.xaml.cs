using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SrvsReference.Service1Client authServiceClient = new SrvsReference.Service1Client();
                // Получите значения из полей ввода
                string username = txtUsername.Text;
                string password = txtPassword.Password;
                string firstName = txtFirstName.Text;
                string lastName = txtLastName.Text;
                string selectedRole = cmbRoles.Text; // или cmbRoles.SelectedItem, в зависимости от вашей логики

                // Создайте экземпляр клиента WCF

                // Вызовите метод регистрации
                bool registrationResult =
                    authServiceClient.RegisterUser(username, password, firstName, lastName, selectedRole);

                if (registrationResult)
                {
                    MessageBox.Show("Регистрация успешна!");
                    this.Close(); // Закройте окно после успешной регистрации
                }
                else
                {
                    MessageBox.Show("Ошибка регистрации. Пользователь с таким именем уже существует.");
                }
            }
            catch (CommunicationException ex)
            {
                MessageBox.Show($"Ошибка связи с сервером WCF: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }

            MessageBox.Show("Регистрация успешна!");
                this.Close(); // Закройте окно после успешной регистрации
            }
        }
    }
