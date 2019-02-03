//Group 4 Final Assignment Bee Breeding
using System.Windows;


namespace BeePathfinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //initialization of VM class with vm field
        VM vm = new VM();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void BtnInput_Click(object sender, RoutedEventArgs e)
        {
            LblOutput.Content = "The Shortest Path Is: " + vm.Calculate();
            LblOutput.Visibility = Visibility.Visible;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TxtInputA.Text = "";
            TxtInputB.Text = "";
            LblOutput.Content = "";
            LblOutput.Visibility = Visibility.Hidden;
        }
    }
}
