using Olympics.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Olympics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PartecipationsView : Window
    {
        private PartecipationsViewModel vm;
        public PartecipationsView()
        {
            InitializeComponent();
            vm = new PartecipationsViewModel();
            DataContext = vm;
        }

        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            vm.FirstPage();
        }

        private void PrevPage_Click(object sender, RoutedEventArgs e)
        {
            vm.PrevPage();
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            vm.NextPage();
        }

        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            vm.LastPage();
        }

        private void AzzeraFiltri_Click(object sender, RoutedEventArgs e)
        {
            vm.AzzeraFiltri();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            vm.About();
        }
    }
}
