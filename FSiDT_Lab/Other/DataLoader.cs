using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;

namespace FSiDT_Lab
{
    public static class DataLoader
    {
        public static List<DataRow>? LoadValues()
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

        private static List<DataRow> ReadNumbersFromFile(string filePath)
        {
            var result = new List<DataRow>();

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    result.Add
                    (
                        new DataRow()
                        { 
                            Index = i + 1,
                            Values = line.Replace('.', ',')
                                .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(ParseSelector)
                                .ToList()
                        }
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
