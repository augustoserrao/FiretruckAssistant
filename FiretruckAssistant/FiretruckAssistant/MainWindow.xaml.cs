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

namespace FiretruckAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FireTruckAssistantVM vm = FireTruckAssistantVM.GetInstance();
        NewFireWindow newFireWindow = null;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void Button_New_Fire_Clicked(object sender, RoutedEventArgs e)
        {
            if (newFireWindow == null)
            {
                newFireWindow = new NewFireWindow();
                newFireWindow.Closed += newFireWindowClosed;
                newFireWindow.Show();
            }
        }

        private void newFireWindowClosed(object sender, EventArgs e)
        {
            newFireWindow = null;
        }
    }
}
