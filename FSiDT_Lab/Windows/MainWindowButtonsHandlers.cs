using EM_Lab_1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSiDT_Lab
{
    public partial class MainWindow
    {
        private void UploadDataButtonClickHandler(object _, RoutedEventArgs __)
        {
            _currentData = DataLoader.LoadValues()!;
        }
    }
}
