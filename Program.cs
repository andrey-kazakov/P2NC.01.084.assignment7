using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace assignment7_AndreiKazakov
{
    internal static class Program
    {
        static char[] correctAnswers =
        {
            'B', 'D', 'A', 'A', 'C',
            'A', 'B', 'A', 'C', 'D',
            'B', 'C', 'D', 'A', 'D',
            'C', 'C', 'B', 'D', 'A',
        };
        const int CORRECT_ANSWERS_TO_PASS = 15;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            char[] readAnswers = ExtractAnswers(RequestAndReadFile());

            if (readAnswers.Length != correctAnswers.Length)
            {
                MessageBox.Show(
                    $"The file contains {readAnswers.Length} answers (one on each line), but should contain {correctAnswers.Length}",
                    "Incorrect amount",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Environment.Exit(1);
                return;
            }

            int[] incorrectIndexes = CompareAnswers(readAnswers);
            int correctAnswersCount = correctAnswers.Length - incorrectIndexes.Length;

            bool isPassed = correctAnswersCount >= CORRECT_ANSWERS_TO_PASS;

            string report = $"Correctly answered questions: {correctAnswersCount}\n";
            report += $"Incorrectly answered questions: {incorrectIndexes.Length}\n";

            if (incorrectIndexes.Length > 0)
            {
                report += $"Incorrect answer numbers: ";
                report += string.Join(", ", incorrectIndexes.Select(index => (index + 1).ToString()).ToArray());
            }

            MessageBox.Show(
                (isPassed ? "You've passed the exam!\n" : "You've failed the exam!\n") + report,
                "Result: " + (isPassed ? "pass" : "fail"),
                MessageBoxButtons.OK,
                isPassed ? MessageBoxIcon.Information : MessageBoxIcon.Error
            );
            Environment.Exit(isPassed ? 0 : 1);
        }

        static int[] CompareAnswers(char[] readAnswers)
        {
            List<int> incorrectIndexes = new List<int>() { };

            for (int i = 0; i < correctAnswers.Length; i++)
            {
                if (correctAnswers[i] != readAnswers[i])
                {
                    incorrectIndexes.Add(i);
                }
            }

            return incorrectIndexes.ToArray();
        }

        static char[] ExtractAnswers(string content)
        {
            return content
                .Trim()
                .ToUpper()
                .Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim()[0])
                .ToArray();
        }

        static string RequestAndReadFile()
        {
            var filePath = string.Empty;
            var fileContent = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;

                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
                else
                {
                    Environment.Exit(1);
                }
            }

            return fileContent;
        }
    }
}
