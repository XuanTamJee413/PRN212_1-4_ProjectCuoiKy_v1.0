using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for Teacher.xaml
    /// </summary>
    public partial class Teacher : Window
    {
        private User? currentUser;
        public Teacher(User? currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUser();
            LoadCourse();
            LoadDB();
        }
        private void LoadDB()
        {
            if (currentUser != null)
            {
                var userNameLabel = (Label)FindName("lblUserName");
                userNameLabel.Content = $"Role: {currentUser.Role} - Id: {currentUser.UserId} - Username: {currentUser.Username}";
            }
            var courses = Prn212examContext.Ins.Courses.Include(s => s.Creator).ToList();
            dgCourses.ItemsSource = courses;

            var tests = Prn212examContext.Ins.Tests.Include(q => q.Creator).
                Include(qs => qs.Course).ToList();
            dgTests.ItemsSource = tests;

            var testResults = Prn212examContext.Ins.TestResults.Include(q => q.Test).Include(tr => tr.Student).ToList();
            dgTestResult.ItemsSource = testResults;
        }
        private void LoadUser()
        {
            var user = Prn212examContext.Ins.Users.Select(x => x.Username).ToList();
            cbxCourseCreadtedBy.ItemsSource = user;
            cbxCourseCreadtedBy.SelectedIndex = 0;

            cbxTestCreadtedBy.ItemsSource = user;
            cbxTestCreadtedBy.SelectedIndex = 0;
        }
        private void LoadCourse()
        {
            var courses = Prn212examContext.Ins.Courses.Select(x => x.CourseName).ToList();
            cbxCourseName.ItemsSource = courses;
            cbxCourseName.SelectedIndex = 0;

            courses.Add("All");
            cbxFilterTestByCourse.ItemsSource = courses;
            cbxFilterTestByCourse.SelectedItem = "All";
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                new MainWindow().Show();
                this.Close();
            }
        }
        private void dgCourses_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgCourses.SelectedItem is Course selectedCourse)
            {
                txtCourseId.Text = selectedCourse.CourseId.ToString();
                txtCourseName.Text = selectedCourse.CourseName;
                cbxCourseCreadtedBy.SelectedItem = selectedCourse.Creator?.Username;
            }
        }

        private void btnInsertCourse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var courseName = txtCourseName.Text;
                var createdBy = cbxCourseCreadtedBy.SelectedItem?.ToString();

                if (string.IsNullOrWhiteSpace(courseName) || string.IsNullOrWhiteSpace(createdBy))
                {
                    MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var user = Prn212examContext.Ins.Users.FirstOrDefault(u => u.Username == createdBy);
                if (user == null) return;

                var newCourse = new Course
                {
                    CourseName = courseName,
                    CreatorId = user.UserId 
                };

                Prn212examContext.Ins.Courses.Add(newCourse);
                Prn212examContext.Ins.SaveChanges();
                LoadDB();
                MessageBox.Show("Course added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadCourse();
        }

        private void btnUpdateCourse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgCourses.SelectedItem is Course selectedCourse)
                {
                    var courseName = txtCourseName.Text;
                    var createdBy = cbxCourseCreadtedBy.SelectedItem?.ToString();

                    if (string.IsNullOrWhiteSpace(courseName) || string.IsNullOrWhiteSpace(createdBy))
                    {
                        MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var user = Prn212examContext.Ins.Users.FirstOrDefault(u => u.Username == createdBy);
                    if (user == null) return;

                    selectedCourse.CourseName = courseName;
                    selectedCourse.CreatorId = user.UserId;

                    Prn212examContext.Ins.Courses.Update(selectedCourse);
                    Prn212examContext.Ins.SaveChanges();
                    LoadDB();
                    MessageBox.Show("Course updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Please select a course to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadCourse();
        }

        private void btnDeleteCourse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgCourses.SelectedItem is Course selectedCourse)
                {
                    var result = MessageBox.Show($"Xóa course: '{selectedCourse.CourseName}', hành động này cũng sẽ xóa các dữ liệu liên quan.",
                                                 "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        Prn212examContext.Ins.TestResults.RemoveRange(Prn212examContext.Ins.TestResults.Where(tr => tr.Test.CourseId == selectedCourse.CourseId));
                        Prn212examContext.Ins.StudentAnswers.RemoveRange(Prn212examContext.Ins.StudentAnswers.Where(sa => sa.Test.CourseId == selectedCourse.CourseId));
                        Prn212examContext.Ins.Questions.RemoveRange(Prn212examContext.Ins.Questions.Where(q => q.Test.CourseId == selectedCourse.CourseId));
                        Prn212examContext.Ins.Tests.RemoveRange(Prn212examContext.Ins.Tests.Where(t => t.CourseId == selectedCourse.CourseId));

                        Prn212examContext.Ins.Courses.Remove(selectedCourse);
                        Prn212examContext.Ins.SaveChanges();
                        LoadDB();
                        MessageBox.Show("Course deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a course to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgTests_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgTests.SelectedItem is Test selectedTest)
            {
                txtTestId.Text = selectedTest.TestId.ToString();
                txtTestName.Text = selectedTest.TestName;
                cbxCourseName.SelectedItem = selectedTest.Course?.CourseName;
                cbxTestCreadtedBy.SelectedItem = selectedTest.Creator?.Username;
                chkStatus.IsChecked = selectedTest.Status;
                txtTestKey.Text = selectedTest.TestKey;
                dpkStartDate.SelectedDate = selectedTest.StartDate;
                dpkEndDate.SelectedDate = selectedTest.EndDate;
                dpkDueTime.Text = selectedTest.DueTime.ToString();
            }
        }

        private void btnViewTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var testId = int.Parse(txtTestId.Text);
                new TestDetail(testId).Show();
            }catch(Exception ex)
            {
                MessageBox.Show($"Please select a test first!!?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void btnInsertTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var testName = txtTestName.Text;
                var courseName = cbxCourseName.SelectedItem?.ToString();
                var createdBy = cbxTestCreadtedBy.SelectedItem?.ToString();
                var testKey = txtTestKey.Text;
                var startDate = dpkStartDate.SelectedDate;
                var endDate = dpkEndDate.SelectedDate;
                var dueTime = dpkDueTime.Text;
                var status = chkStatus.IsChecked ?? false;

                if (string.IsNullOrWhiteSpace(testName) || string.IsNullOrWhiteSpace(courseName) || string.IsNullOrWhiteSpace(createdBy))
                {
                    MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var existingTestKey = Prn212examContext.Ins.Tests.FirstOrDefault(u => u.TestKey == testKey);

                if (existingTestKey != null)
                {
                    MessageBox.Show("Test key already exists. Try another one!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var course = Prn212examContext.Ins.Courses.FirstOrDefault(c => c.CourseName == courseName);
                var user = Prn212examContext.Ins.Users.FirstOrDefault(u => u.Username == createdBy);

                if (course == null || user == null) return;

                var newTest = new Test
                {
                    TestName = testName,
                    CourseId = course.CourseId,
                    CreatorId = user.UserId,
                    TestKey = testKey,
                    StartDate = startDate,
                    EndDate = endDate,
                    DueTime = int.Parse(dueTime),
                    Status = status
                };

                Prn212examContext.Ins.Tests.Add(newTest);
                Prn212examContext.Ins.SaveChanges();
                LoadDB();
                MessageBox.Show("Test added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.InnerException.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnUpdateTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgTests.SelectedItem is Test selectedTest)
                {
                    var testId = int.Parse(txtTestId.Text);
                    var testName = txtTestName.Text;
                    var courseName = cbxCourseName.SelectedItem?.ToString();
                    var createdBy = cbxTestCreadtedBy.SelectedItem?.ToString();
                    var testKey = txtTestKey.Text;
                    var startDate = dpkStartDate.SelectedDate;
                    var endDate = dpkEndDate.SelectedDate;
                    var dueTime = dpkDueTime.Text;
                    var status = chkStatus.IsChecked ?? false;

                    if (string.IsNullOrWhiteSpace(testName) || string.IsNullOrWhiteSpace(courseName) || string.IsNullOrWhiteSpace(createdBy))
                    {
                        MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var existingTestKey = Prn212examContext.Ins.Tests.FirstOrDefault(u => u.TestId != testId && u.TestKey == testKey);

                    if (existingTestKey != null)
                    {
                        MessageBox.Show("Test key already exists. try another one!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }


                    var course = Prn212examContext.Ins.Courses.FirstOrDefault(c => c.CourseName == courseName);
                    var user = Prn212examContext.Ins.Users.FirstOrDefault(u => u.Username == createdBy);

                    if (course == null || user == null) return;

                    selectedTest.TestName = testName;
                    selectedTest.CourseId = course.CourseId;
                    selectedTest.CreatorId = user.UserId;
                    selectedTest.TestKey = testKey;
                    selectedTest.StartDate = startDate;
                    selectedTest.EndDate = endDate;
                    selectedTest.DueTime = int.Parse(dueTime);
                    selectedTest.Status = status;

                    Prn212examContext.Ins.Tests.Update(selectedTest);
                    Prn212examContext.Ins.SaveChanges();
                    LoadDB();
                    MessageBox.Show("Test updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Please select a test to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.InnerException.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnDeleteTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgTests.SelectedItem is Test selectedTest)
                {
                    var result = MessageBox.Show($"Xóa Tests: '{selectedTest.TestName}' cũng sẽ xóa các dữ liệu liên quan.",
                                                 "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        Prn212examContext.Ins.TestResults.RemoveRange(Prn212examContext.Ins.TestResults.Where(tr => tr.TestId == selectedTest.TestId));
                        Prn212examContext.Ins.StudentAnswers.RemoveRange(Prn212examContext.Ins.StudentAnswers.Where(sa => sa.TestId == selectedTest.TestId));
                        Prn212examContext.Ins.Questions.RemoveRange(Prn212examContext.Ins.Questions.Where(q => q.TestId == selectedTest.TestId));

                        Prn212examContext.Ins.Tests.Remove(selectedTest);
                        Prn212examContext.Ins.SaveChanges();
                        LoadDB();
                        MessageBox.Show("Test deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a test to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgTestResult_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgTestResult.SelectedItem is TestResult selectedTR)
            {
                lblTestResultId.Content = selectedTR.ResultId;
                lblTestId.Content = selectedTR.TestId;
                lblTestName.Content = selectedTR.Test.TestName;
                lblStudentId.Content = selectedTR.StudentId;
                lblStudentName.Content = selectedTR.Student.Username;
                lblScore.Content = selectedTR.Score;
                dpkDoneTime.SelectedDate = Prn212examContext.Ins.TestResults.Find(selectedTR.ResultId).CompletionTime;
            }
        }

        private void btnSearchTest_Click(object sender, RoutedEventArgs e)
        {
            string search = tbxSearchTest.Text.Trim().ToLower();
            string filterTest = cbxFilterTestByCourse.SelectedItem?.ToString();

            var rawSource = Prn212examContext.Ins.Tests
                .Include(t => t.Creator)
                .Include(t => t.Course)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                rawSource = rawSource.Where(t => t.TestName.ToLower().Contains(search) ||
                                         t.Creator.Username.ToLower().Contains(search) ||
                                         t.Course.CourseName.ToLower().Contains(search));
            }

            if (!string.IsNullOrEmpty(filterTest) && filterTest != "All")
            {
                rawSource = rawSource.Where(t => t.Course.CourseName == filterTest);
            }

            var filteredTests = rawSource.ToList();
            dgTests.ItemsSource = filteredTests;
        }

        private void tbxSearchTest_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSearchTest_Click(sender, e);
        }

        private void cbxFilterTestByCourse_SelectionChanged(object sender, RoutedEventArgs e)
        {
            btnSearchTest_Click(sender, e);
        }
    }
}
