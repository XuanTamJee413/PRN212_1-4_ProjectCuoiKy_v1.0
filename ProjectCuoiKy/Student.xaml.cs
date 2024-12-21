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
    /// Interaction logic for Student.xaml
    /// </summary>
    public partial class Student : Window
    {
        private User currentUser;
        public Student(User user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDB();
        }
        private void LoadDB()
        {
            //load user name
            if (currentUser != null)
            {
                var userNameLabel = (Label)FindName("lblUserName");
                userNameLabel.Content = $"Role: {currentUser.Role} - Id: {currentUser.UserId} - Username: {currentUser.Username}";
            }

            // load danh sach test
            var tests = Prn212examContext.Ins.Tests.Include(q => q.Creator).Include(qs => qs.Course).ToList();
            dgTests.ItemsSource = tests;

            // load danh sach test result
            var testResults = Prn212examContext.Ins.TestResults.Include(t => t.Test).Include(tr => tr.Student).Where(tr => tr.StudentId == currentUser.UserId).ToList();
            dgTestResult.ItemsSource = testResults;
        }

        private void dgTests_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgTests.SelectedItem is Test selectedTest)
            {
                lblTestId.Content = selectedTest.TestId.ToString();
                lblTestName.Content = selectedTest.TestName;
                lblDueTime.Content = selectedTest.DueTime.ToString() + " phut";
            }
        }


        private void btnTakeTest_Click(object sender, RoutedEventArgs e)
        {
            if (dgTests.SelectedItem is Test selectedTest)
            {
                int selectedTestId = selectedTest.TestId;
                bool testStatus = Prn212examContext.Ins.Tests.FirstOrDefault(t => t.TestId == selectedTestId).Status ?? false;
                if (!testStatus)
                {
                    new JustViewQuestion(selectedTestId).Show();
                    return;
                }
                string enteredTestKey = txtTestKey.Text.Trim();

                if (selectedTest.TestKey == enteredTestKey)
                {

                    var existingAnswers = Prn212examContext.Ins.StudentAnswers
                        .Where(sa => sa.StudentId == currentUser.UserId && sa.TestId == selectedTest.TestId)
                        .ToList();

                    if (existingAnswers.Any())
                    {
                        var result = MessageBox.Show(
                            "Bạn đã làm bài này trước đó! nếu bạn làm lại, bài trước đó sẽ bị HỦY. Bạn có muốn tiếp tịc???",
                            "Confirm",
                            MessageBoxButton.YesNo
                        );

                        if (result == MessageBoxResult.Yes)
                        {
                            Prn212examContext.Ins.StudentAnswers.RemoveRange(existingAnswers);
                            Prn212examContext.Ins.SaveChanges();

                            TakeTest takeTestWindow = new TakeTest(currentUser, selectedTest);
                            takeTestWindow.ShowDialog();
                            MessageBox.Show("Đã có dữ liệu mới!!. nhấn ok để load");
                            LoadDB();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        TakeTest takeTestWindow = new TakeTest(currentUser, selectedTest);
                        takeTestWindow.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Test key is incorrect.!!!");
                }
            }
            else
            {
                MessageBox.Show("Please select a test to start.");
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

        private void dgTestResult_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgTestResult.SelectedItem is TestResult selectedTR)
            {
                lblResultId.Content = selectedTR.ResultId.ToString();
                lblTestIdResult.Content = selectedTR.TestId.ToString();
                lblTestScore.Content = selectedTR.Score.ToString();
                lblCompiledTime.Content = selectedTR.CompletionTime.ToString();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một test để hiển thị?!");
            }
        }

        private void btnReTakeTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var testToRetake = Prn212examContext.Ins.Tests.FirstOrDefault(t => t.TestId == int.Parse(lblTestIdResult.Content.ToString()));
                int selectedTestId = testToRetake.TestId;
                bool testStatus = Prn212examContext.Ins.Tests.FirstOrDefault(t => t.TestId == selectedTestId).Status ?? false;
                if (!testStatus)
                {
                    new JustViewQuestion(selectedTestId).Show();
                    return;
                }
                string enteredTestKey = txtTestRetakeKey.Text.Trim();

                if (testToRetake.TestKey == enteredTestKey)
                {

                    var existingAnswers = Prn212examContext.Ins.StudentAnswers
                        .Where(sa => sa.StudentId == currentUser.UserId && sa.TestId == testToRetake.TestId)
                        .ToList();

                    if (existingAnswers.Any())
                    {
                        var result = MessageBox.Show(
                            "Bạn đã làm bài này trước đó! nếu bạn làm lại, bài trước đó sẽ bị HỦY. Bạn có muốn tiếp tịc???",
                            "Confirm",
                            MessageBoxButton.YesNo
                        );

                        if (result == MessageBoxResult.Yes)
                        {
                            Prn212examContext.Ins.StudentAnswers.RemoveRange(existingAnswers);
                            Prn212examContext.Ins.SaveChanges();

                            TakeTest takeTestWindow = new TakeTest(currentUser, testToRetake);
                            takeTestWindow.ShowDialog();
                            MessageBox.Show("Đã có dữ liệu mới!!. nhấn ok để load");
                            LoadDB();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        TakeTest takeTestWindow = new TakeTest(currentUser, testToRetake);
                        takeTestWindow.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Test key is incorrect.!!!");
                }
            }
            catch (Exception ex)
            {
                {
                    MessageBox.Show("Please select a test to start.");
                }
            }
        }
    }
}
