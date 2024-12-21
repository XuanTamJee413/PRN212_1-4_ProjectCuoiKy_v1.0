using ProjectCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
using static System.Collections.Specialized.BitVector32;

namespace ProjectCuoiKy
{
    /// <summary>
    /// Interaction logic for TakeTest.xaml
    /// </summary>
    public partial class TakeTest : Window
    {
        private User? currentUser;
        private Test? currentTest;

        private Dictionary<int, string> selectedAnswers = new Dictionary<int, string>();

        public TakeTest(User currentUser, Test test)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            this.currentTest = test;
            this.WindowState = WindowState.Maximized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblTestName.Content = currentTest.TestName;
            lblDueTime.Content = currentTest.DueTime.ToString() + " phut";

            LoadQuestions();
        }
        private void LoadQuestions()
        {
            var questions = Prn212examContext.Ins.Questions
                .Where(q => q.TestId == currentTest.TestId)
                .ToList();

            var random = new Random();
            foreach (var question in questions)
            {
                question.A = question.CorrectAnswer;
                question.B = question.WrongAnswer1;
                question.C = question.WrongAnswer2;
                question.D = question.WrongAnswer3;
                var swapAnswers = new List<string>
                {
                    question.A,
                    question.B,
                    question.C,
                    question.D
                };

                swapAnswers = swapAnswers.OrderBy(a => random.Next()).ToList();

                question.A = swapAnswers[0];
                question.B = swapAnswers[1];
                question.C = swapAnswers[2];
                question.D = swapAnswers[3];

                StudentAnswer sa = new StudentAnswer {
                    TestId = currentTest.TestId,
                    StudentId = currentUser.UserId,
                    QuestionId = question.QuestionId,
                    ChosenAnswer = "InittialEmptyBecauseNotSelectedFromUser",
                    IsCorrect = false
                };
                Prn212examContext.Ins.StudentAnswers.Add(sa);
                Prn212examContext.Ins.SaveChanges();
            }

            questionsList.ItemsSource = questions;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to submit your test?", "Confirm Submission", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var answer = Prn212examContext.Ins.StudentAnswers.Where(sa => sa.TestId == currentTest.TestId && sa.StudentId == currentUser.UserId).ToList();
                int correctCount = 0;
                foreach (var item in answer)
                {
                    if (item.IsCorrect == true)
                    {
                        correctCount++;
                    }
                }
                var mark = answer.Count > 0 ? Math.Round( (correctCount / (double)answer.Count) * 10,2) : 0;
                TestResult testResult = new TestResult { 
                    TestId = currentTest.TestId,
                    StudentId = currentUser.UserId,
                    Score = (decimal)mark,
                    CompletionTime = DateTime.Now,
                };
                Prn212examContext.Ins.TestResults.Add(testResult);
                Prn212examContext.Ins.SaveChanges();
                MessageBox.Show($"Your test has been submitted. You answered: {correctCount} / {answer.Count} questions correctly.\nTest Result: [{mark}] mark");
                Close();
            }
        }
        
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;

            if (radioButton != null)
            {
                int questionId = int.Parse(radioButton.GroupName.ToString());
                string chosenAnswer = radioButton.Content.ToString();

                if (selectedAnswers.ContainsKey(questionId))
                {
                    selectedAnswers[questionId] = chosenAnswer;
                }
                else
                {
                    selectedAnswers.Add(questionId, chosenAnswer);
                }

                var existingAnswer = Prn212examContext.Ins.StudentAnswers
                    .FirstOrDefault(sa => sa.TestId == currentTest.TestId && sa.StudentId == currentUser.UserId && sa.QuestionId == questionId);

                if (existingAnswer != null)
                {
                    existingAnswer.ChosenAnswer = chosenAnswer;
                    existingAnswer.IsCorrect = IsAnswerCorrect(questionId, chosenAnswer);
                }
                else
                {
                    var studentAnswer = new StudentAnswer
                    {
                        TestId = currentTest.TestId,
                        StudentId = currentUser.UserId,
                        QuestionId = questionId,
                        ChosenAnswer = chosenAnswer,
                        IsCorrect = IsAnswerCorrect(questionId, chosenAnswer)
                    };

                    Prn212examContext.Ins.StudentAnswers.Add(studentAnswer);
                }

                Prn212examContext.Ins.SaveChanges();
            }
        }

        private bool IsAnswerCorrect(int questionId, string chosenAnswer)
        {
            var question = Prn212examContext.Ins.Questions.FirstOrDefault(q => q.QuestionId == questionId);
            if (question != null)
            {
                return question.CorrectAnswer == chosenAnswer;
            }
            return false;
        }

    }
}
