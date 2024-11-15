using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectCuoiKy.Models;
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
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        private User? currentUser;
        public Admin()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDB();
        }
        private void LoadDB()
        {
            currentUser = Prn212quizDbContext.Ins.Users.FirstOrDefault(u => u.UserId == 1);

            // Hiển thị tên người dùng
            if (currentUser != null)
            {
                var userNameLabel = (Label)FindName("lblUserName");
                userNameLabel.Content = $"Xin Chào: {currentUser.Username}";
            }
            // tải danh sách ng dùng
            var user = Prn212quizDbContext.Ins.Users.ToList();
            dgUser.ItemsSource = user;

        }
        private void dgUser_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            User user = dgUser.SelectedItem as User;
            if (user != null)
            {
                txtUserId.Text = user.UserId.ToString();
                txtUserName.Text = user.Username.ToString();
                rbtnStudent.IsChecked = user.Role == "Student";
                rbtnTeacher.IsChecked = user.Role == "Teacher";
                rbtnAdmin.IsChecked = user.Role == "Admin";
                chkIsActive.IsChecked = user.IsActive;
            }
            else
            {
                MessageBox.Show("Không tìm thấy User!");
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {

        }


        private void btnInsertUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdateUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
