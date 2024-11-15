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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
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
                cbxCourseCreadtedBy.SelectedValue = c.CreatorId;
            }
        }

        private void lstCourses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCourses.SelectedItem != null)
            {
                string selectedCourseName = dgCourses.SelectedItem.ToString();
                var course = Prn212quizDbContext.Ins.Courses.FirstOrDefault(c => c.CourseName == selectedCourseName);

                if (course != null)
                {
                    // Lấy các bài kiểm tra của học phần được chọn
                    var quizzes = Prn212quizDbContext.Ins.Tests
                        .Where(t => t.CourseId == course.CourseId)
                        .ToList();

                    dgQuizzes.Items.Clear(); // Xóa các bài kiểm tra cũ
                    foreach (var quiz in quizzes)
                    {
                        dgQuizzes.Items.Add(quiz.TestKey);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy học phần.");
                }
            }
        }

        private void lstQuizzes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgQuizzes.SelectedItem != null)
            {
                string selectedQuizKey = dgQuizzes.SelectedItem.ToString();
                var quiz = Prn212quizDbContext.Ins.Tests
                    .FirstOrDefault(t => t.TestKey == selectedQuizKey);

                if (quiz != null)
                {
                    // Lấy danh sách các câu hỏi trong bài kiểm tra
                    var studentAnswers = Prn212quizDbContext.Ins.StudentAnswers
                        .Where(sa => sa.TestId == quiz.TestId)
                        .ToList();

                    foreach (var sa in studentAnswers)
                    {
                        var term = Prn212quizDbContext.Ins.Terms
                            .FirstOrDefault(t => t.TermId == sa.TermId);

                        MessageBox.Show($"Thuật ngữ: {term.TermText}, Câu trả lời: {sa.ChosenAnswer}, Đúng/Sai: {sa.IsCorrect}");
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy Bài kiểm tra");
                }
            }
        }

        private void dgCourses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgQuizzes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnQuizDetail_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSetDetail_Click(object sender, RoutedEventArgs e)
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

        private void btnViewCourse_Click(object sender, RoutedEventArgs e)
        {

        }

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