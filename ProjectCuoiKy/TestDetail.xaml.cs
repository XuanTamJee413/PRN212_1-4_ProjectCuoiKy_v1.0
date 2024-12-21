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
    /// Interaction logic for TestDetail.xaml
    /// </summary>
    public partial class TestDetail : Window
    {
        public int testId;
        public TestDetail(int testId)
        {
            InitializeComponent();
            this.testId = testId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCbxTest();
            LoadDB(testId);
        }
        private void LoadDB(int currentTestId)
        {
            var questions = Prn212examContext.Ins.Questions.Include(s => s.Test).Where(q => q.Test.TestId == currentTestId).ToList();
            dgvQuestionList.ItemsSource = questions;
        }
        private void LoadCbxTest()
        {
            var user = Prn212examContext.Ins.Tests.Select(x => x.TestName).ToList();
            cbxTestList.ItemsSource = user;
            cbxTestList.SelectedItem = Prn212examContext.Ins.Tests.FirstOrDefault(x => x.TestId == testId).TestName;
        }

        private void dgvQuestionList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgvQuestionList.SelectedItem is Question selectedQuestion)
            {
                txtQuestionId.Text = selectedQuestion.QuestionId.ToString();
                txtQuestionText.Text = selectedQuestion.QuestionText;
                txtCorrectAnswer.Text = selectedQuestion.CorrectAnswer;
                txtWrongAnswer1.Text = selectedQuestion.WrongAnswer1;
                txtWrongAnswer2.Text = selectedQuestion.WrongAnswer2;
                txtWrongAnswer3.Text = selectedQuestion.WrongAnswer3;
            }
        }

        private void cbxTestList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxTestList.SelectedItem != null)
            {
                string selectedTestName = cbxTestList.SelectedItem.ToString();
                var questions = Prn212examContext.Ins.Questions
                    .Include(q => q.Test)
                    .Where(q => q.Test.TestName == selectedTestName)
                    .ToList();
                dgvQuestionList.ItemsSource = questions;
            }
        }

        private void btnInsertQuestion_Click(object sender, RoutedEventArgs e)
        {
            var newQuestion = new Question
            {
                QuestionText = txtQuestionText.Text,
                CorrectAnswer = txtCorrectAnswer.Text,
                WrongAnswer1 = txtWrongAnswer1.Text,
                WrongAnswer2 = txtWrongAnswer2.Text,
                WrongAnswer3 = txtWrongAnswer3.Text,
                TestId = Prn212examContext.Ins.Tests
                            .Where(t => t.TestId == testId)
                            .Select(t => t.TestId)
                            .FirstOrDefault()
            };

            Prn212examContext.Ins.Questions.Add(newQuestion);
            Prn212examContext.Ins.SaveChanges();

            LoadDB(testId);
        }

        private void btnUpdateQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtQuestionId.Text, out int questionId))
            {
                var questionToUpdate = Prn212examContext.Ins.Questions
                    .FirstOrDefault(q => q.QuestionId == questionId);

                if (questionToUpdate != null)
                {
                    questionToUpdate.QuestionText = txtQuestionText.Text;
                    questionToUpdate.CorrectAnswer = txtCorrectAnswer.Text;
                    questionToUpdate.WrongAnswer1 = txtWrongAnswer1.Text;
                    questionToUpdate.WrongAnswer2 = txtWrongAnswer2.Text;
                    questionToUpdate.WrongAnswer3 = txtWrongAnswer3.Text;

                    Prn212examContext.Ins.SaveChanges();

                    var currentTestDetails = cbxTestList.SelectedItem.ToString();
                    LoadDB(testId);
                }
            }
        }

        private void btnDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtQuestionId.Text, out int questionId))
            {
                var questionToDelete = Prn212examContext.Ins.Questions
                    .FirstOrDefault(q => q.QuestionId == questionId);

                if (questionToDelete != null)
                {
                    var result = MessageBox.Show(
                "Việc xóa Question này cũng đồng thời xóa các answers có liên quan. Bạnn có muốn xóa?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        Prn212examContext.Ins.StudentAnswers.RemoveRange(Prn212examContext.Ins.StudentAnswers.Where(sa => sa.QuestionId == questionId));
                        Prn212examContext.Ins.Questions.Remove(questionToDelete);
                        Prn212examContext.Ins.SaveChanges();

                        var currentTestDetails = cbxTestList.SelectedItem.ToString();
                        LoadDB(testId);
                    }
                }
            }
        }

    }
}
