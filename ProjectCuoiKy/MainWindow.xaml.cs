using Microsoft.EntityFrameworkCore;
using ProjectCuoiKy.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectCuoiKy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private User? currentUser;
        public MainWindow()
        {
            InitializeComponent();
        }
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDB();
        }
        private void LoadDB()
        {
            currentUser =  null;

            if (currentUser != null)
            {
                var userNameLabel = (Label)FindName("lblUserName");
                userNameLabel.Content = $"Xin Chào: {currentUser.Username}";
            }

            // Tải danh sách học phần
            var courses = Prn212examContext.Ins.Courses.Include(s => s.Creator).ToList();
            dgCourses.ItemsSource = courses;

            // Tải danh sách bài kiểm tra
            var quizzes = Prn212examContext.Ins.Tests.Include(q => q.Creator).Include(qs => qs.Course).Where(t => t.Status == false).ToList();
            dgTests.ItemsSource = quizzes;
        }
        private void btnLoginAsAdmin_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = Prn212examContext.Ins.Users.FirstOrDefault(u => u.Username.Equals("admin"));
            new Admin(currentUser).Show();
            Close();
        }

        private void btnLoginAsTeacher_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = Prn212examContext.Ins.Users.FirstOrDefault(u => u.Username.Equals("teacher"));
            new Teacher(currentUser).Show();
            Close();
        }

        private void btnLoginAsStudent_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = Prn212examContext.Ins.Users.FirstOrDefault(u => u.Username.Equals("student"));
            new Student(currentUser).Show();
            Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login(this);
            loginWindow.ShowDialog();
        }

        private void dgTests_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgTests.SelectedItem is Test selectedTest)
            {
                txtTestId.Text = selectedTest.TestId.ToString();
                txtTestName.Text = selectedTest.TestName;
                txtCourseName.Text = selectedTest.Course.CourseName;
                txtTestCreadtedBy.Text = selectedTest.Creator.Username;
                lblStatus.Content = selectedTest.Status?? false ? "Active: quiz dành cho kiểm tra" : "Inactive: quiz dành cho ôn tập";
            }
        }

        private void btnViewQuestion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var testId = int.Parse(txtTestId.Text);
                new JustViewQuestion(testId).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hãy chọn một bài để xem câu hỏi ôn tập!!?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}