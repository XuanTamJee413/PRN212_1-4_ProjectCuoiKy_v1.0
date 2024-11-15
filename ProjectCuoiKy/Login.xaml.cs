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

namespace ProjectCuoiKy
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txbUsername.Text;
            string password = txbPassword.Password; 

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                txbErrorMessage.Text = "Vui lòng nhập cả tên người dùng và mật khẩu.";
                txbSuccessMessage.Text = "";
            }
            else
            {
                if (username == "1" && password == "1")
                {
                    txbErrorMessage.Text = ""; 
                    txbSuccessMessage.Text = "Đăng nhập thành công!";
                    await Task.Delay(1000); 
                    this.Close();
                }
                else
                {
                    txbErrorMessage.Text = "Tên người dùng hoặc mật khẩu không đúng.";
                }
            }
        }

        private async void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txbUsername.Clear();
            txbPassword.Clear();
            txbSuccessMessage.Text = "";
            txbErrorMessage.Text = "Nhập thông tin đăng nhập";
            await Task.Delay(1000);
            txbErrorMessage.Text = "";
        }
    }
}
