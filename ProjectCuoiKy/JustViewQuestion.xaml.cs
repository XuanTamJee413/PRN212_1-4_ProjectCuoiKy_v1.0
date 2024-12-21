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
    /// Interaction logic for JustViewQuestion.xaml
    /// </summary>
    public partial class JustViewQuestion : Window
    {
        private int testId;
        public JustViewQuestion(int testId)
        {
            InitializeComponent();
            this.testId = testId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblTestName.Content = Prn212examContext.Ins.Tests.Where(t => t.TestId == testId).FirstOrDefault().TestName;

            LoadQuestions();
        }
        private void LoadQuestions()
        {
            var questions = Prn212examContext.Ins.Questions
                .Where(q => q.TestId == testId)
                .ToList();

            var random = new Random();
            foreach (var question in questions)
            {
                question.A = question.CorrectAnswer;
                question.B = question.WrongAnswer1;
                question.C = question.WrongAnswer2;
                question.D = question.WrongAnswer3;
                var tronAnswers = new List<string>
                {
                    question.A,
                    question.B,
                    question.C,
                    question.D
                };

                tronAnswers = tronAnswers.OrderBy(a => random.Next()).ToList();

                question.A = tronAnswers[0];
                question.B = tronAnswers[1];
                question.C = tronAnswers[2];
                question.D = tronAnswers[3];
            }

            questionsList.ItemsSource = questions;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
