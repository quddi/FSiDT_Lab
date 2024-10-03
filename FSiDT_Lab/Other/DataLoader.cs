using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;

namespace EM_Lab_1
{
    public static class DataLoader
    {
        public static List<List<double>>? LoadValues()
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;

                try
                {
                    return ReadNumbersFromFile(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при читанні файлу: {ex.Message}");
                }
            }

            return null;
        }

        private static List<List<double>> ReadNumbersFromFile(string filePath)
        {
            var result = new List<List<double>>();

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    result.Add
                    (
                        line.Replace('.', ',')
                        .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(ParseSelector)
                        .ToList()
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при зчитуванні файлу: {ex.Message}");
            }

            return result;

            static double ParseSelector(string input)
            {
                var parseResult = double.TryParse(input, out double result);

                if (parseResult == false) MessageBox.Show($"Was unable to parse value: [{input}]");

                return parseResult ? result : 0;
            }
        }
    }
}
