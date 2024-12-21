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
        public Admin(User currentuser)
        {
            InitializeComponent();
            this.currentUser = currentuser;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDB();
        }
        private void LoadDB()
        {
            currentUser = Prn212examContext.Ins.Users.FirstOrDefault(u => u.UserId == currentUser.UserId);

            // Hiển thị tên người dùng
            if (currentUser != null)
            {
                var userNameLabel = (Label)FindName("lblUserName");
                userNameLabel.Content = $"Role: {currentUser.Role} - Id: {currentUser.UserId} - Username: {currentUser.Username}";
            }
            // tải danh sách ng dùng
            var user = Prn212examContext.Ins.Users.ToList();
            dgUser.ItemsSource = user;

        }
        private void dgUser_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            lblStatusMessage.Visibility = Visibility.Collapsed;

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
            
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                new MainWindow().Show();
                this.Close();
            }
        }

        private async void btnInsertUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = txtUserName.Text;
                var existingUser = Prn212examContext.Ins.Users.FirstOrDefault(u => u.Username == username);

                if (existingUser != null)
                {
                    lblStatusMessage.Content = "Username already exists. Please choose another one.";
                    lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatusMessage.Visibility = Visibility.Visible;
                    return;
                }

                User newUser = new User
                {
                    Username = username,
                    Password = "123",
                    Role = rbtnStudent.IsChecked == true ? "Student" : rbtnTeacher.IsChecked == true ? "Teacher" : "Admin",
                    IsActive = chkIsActive.IsChecked == true
                };

                Prn212examContext.Ins.Users.Add(newUser);
                Prn212examContext.Ins.SaveChanges();

                lblStatusMessage.Content = "User inserted successfully.";
                lblStatusMessage.Foreground = new SolidColorBrush(Colors.Green);
                lblStatusMessage.Visibility = Visibility.Visible;
                LoadDB();
                await Task.Delay(3000);
                lblStatusMessage.Visibility = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                lblStatusMessage.Content = $"Error: {ex.Message}";

                lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                lblStatusMessage.Visibility = Visibility.Visible;
                await Task.Delay(3000);
                lblStatusMessage.Visibility= Visibility.Collapsed;
            }
        }



        private async void btnUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int userId = int.Parse(txtUserId.Text);
                string newUsername = txtUserName.Text;
                var existingUser = Prn212examContext.Ins.Users.FirstOrDefault(u =>u.UserId != userId && u.Username == newUsername);

                if (existingUser != null)
                {
                    lblStatusMessage.Content = "Username already exists. Please choose another one.";
                    lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatusMessage.Visibility = Visibility.Visible;
                    return;
                }
                User? userToUpdate = dgUser.SelectedItem as User;
                if (userToUpdate == null)
                {
                    lblStatusMessage.Content = "Please select a user to update.";
                    lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatusMessage.Visibility = Visibility.Visible;
                    return;
                }

                userToUpdate.Username = txtUserName.Text;
                userToUpdate.Role = rbtnStudent.IsChecked == true ? "Student" : rbtnTeacher.IsChecked == true ? "Teacher" : "Admin";
                userToUpdate.IsActive = chkIsActive.IsChecked == true;

                Prn212examContext.Ins.SaveChanges();

                lblStatusMessage.Content = "User updated successfully.";
                lblStatusMessage.Foreground = new SolidColorBrush(Colors.Green);
                lblStatusMessage.Visibility = Visibility.Visible;
                LoadDB();
                await Task.Delay(3000);
                lblStatusMessage.Visibility = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                lblStatusMessage.Content = $"Error: {ex.Message}";
                lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                lblStatusMessage.Visibility = Visibility.Visible;
                await Task.Delay(3000);
                lblStatusMessage.Visibility = Visibility.Collapsed;
            }
        }

        private async void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User? userToDelete = dgUser.SelectedItem as User;
                if (userToDelete == null)
                {
                    lblStatusMessage.Content = "Please select a user to delete.";
                    lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                    lblStatusMessage.Visibility = Visibility.Visible;
                    return;
                }

                var result = MessageBox.Show($"Xóa người dùng '{userToDelete.Username}'." +
                    $" việc xóa cũng đồng thời xóa các TestResults, StudentAnswers, Questions, Tests, Courses có liên quan." +
                    $"Bạnn có muốn xóa?",
                                              "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    Prn212examContext.Ins.TestResults.RemoveRange(Prn212examContext.Ins.TestResults.Where(tr => tr.StudentId == userToDelete.UserId));
                    Prn212examContext.Ins.StudentAnswers.RemoveRange(Prn212examContext.Ins.StudentAnswers.Where(sa => sa.StudentId == userToDelete.UserId));
                    Prn212examContext.Ins.Questions.RemoveRange(Prn212examContext.Ins.Questions.Where(q => q.Test.CreatorId == userToDelete.UserId));
                    Prn212examContext.Ins.Tests.RemoveRange(Prn212examContext.Ins.Tests.Where(t => t.CreatorId == userToDelete.UserId));
                    Prn212examContext.Ins.Courses.RemoveRange(Prn212examContext.Ins.Courses.Where(c => c.CreatorId == userToDelete.UserId));

                    Prn212examContext.Ins.Users.Remove(userToDelete);
                    Prn212examContext.Ins.SaveChanges();

                    lblStatusMessage.Content = "User deleted successfully.";
                    lblStatusMessage.Foreground = new SolidColorBrush(Colors.Green);
                    lblStatusMessage.Visibility = Visibility.Visible;
                    LoadDB();
                    await Task.Delay(3000);
                    lblStatusMessage.Visibility = Visibility.Collapsed;

                }
            }
            catch (Exception ex)
            {
                lblStatusMessage.Content = $"Error: {ex.Message}";
                lblStatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                lblStatusMessage.Visibility = Visibility.Visible;
                await Task.Delay(3000);
                lblStatusMessage.Visibility = Visibility.Collapsed;
            }
        }


    }
}
