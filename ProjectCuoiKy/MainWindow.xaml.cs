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
            LoadUser();
            LoadCourse();
            Admin admin = new Admin();
            admin.Show();
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

            // Tải danh sách học phần
            var courses = Prn212quizDbContext.Ins.Courses.Include(s => s.Creator).ToList();
            dgCourses.ItemsSource = courses;

            var user = Prn212quizDbContext.Ins.Users.Select(u => u.Username).ToList();

            // Tải danh sách bài kiểm tra
            var quizzes = Prn212quizDbContext.Ins.Tests.Include(qu => qu.Creator).
                Include(qs => qs.Course).ToList();
            dgQuizzes.ItemsSource = quizzes;
        }

        private void LoadUser()
        {
            var user = Prn212quizDbContext.Ins.Users.Select(x => x.Username).ToList();
            cbxCourseCreadtedBy.ItemsSource = user;
            cbxCourseCreadtedBy.SelectedIndex = 0;
            cbxTestCreadtedBy.ItemsSource = user;
            cbxTestCreadtedBy.SelectedIndex = 0;
        }

        private void LoadCourse()
        {
            var course = Prn212quizDbContext.Ins.Courses.Select(s => s.CourseName).ToList();
            cbxTestName.ItemsSource = course;
            cbxTestName.SelectedIndex = 0;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.ShowDialog();
        }
        private void dgCourses_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Course c = dgCourses.SelectedItem as Course;
            if (c != null) { 
                txtCourseId.Text = c.CourseId.ToString();
                txtCourseName.Text = c.CourseName;
                cbxCourseCreadtedBy.SelectedValue = Prn212quizDbContext.Ins.Users.FirstOrDefault(s => s.UserId.Equals(c.CreatorId)).Username;
            }
            else
            {
                MessageBox.Show("Không tìm thấy Course!");
            }
        }

        private void dgQuizzes_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Test t = dgQuizzes.SelectedItem as Test;
            if (t != null)
            {
                txtTestId.Text = t.TestId.ToString();
                cbxTestName.SelectedValue = Prn212quizDbContext.Ins.Courses.FirstOrDefault(s => s.CourseId.Equals(t.CourseId)).CourseName;
                cbxTestCreadtedBy.SelectedValue = Prn212quizDbContext.Ins.Users.FirstOrDefault(s => s.UserId.Equals(t.CreatorId)).Username;
                chkTimerEnabled.IsChecked = t.TimerEnabled;
                txtTestKey.Text = t.TestKey.ToString();
            }
            else
            {
                MessageBox.Show("Không tìm thấy Quiz!");
            }
        }

        //Event cho Course
        private void btnViewCourse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnInsertCourse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdateCourse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteCourse_Click(object sender, RoutedEventArgs e)
        {

        }

        // event cho test
        private void btnViewTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnInsertTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdateTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteTest_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}