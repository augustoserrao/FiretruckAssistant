using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FiretruckAssistant
{
    /// <summary>
    /// Interaction logic for NewFireWindow.xaml
    /// </summary>
    public partial class NewFireWindow : Window
    {
        FireTruckAssistantVM vm = FireTruckAssistantVM.GetInstance();

        public NewFireWindow()
        {
            InitializeComponent();
            DataContext = vm;
            vm.ResetFire();
        }

        private void Button_Add_Event_Clicked(object sender, RoutedEventArgs e)
        {
            if (vm.AddFire())
            {
                Close();
            }
        }

        private void Button_Add_Street_Clicked(object sender, RoutedEventArgs e)
        {
            vm.AddStreet();
        }

        private void Corner_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only numbers to be typed
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}
