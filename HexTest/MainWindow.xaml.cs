using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HexTest
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int length = 2;
        int maxNumber = 12;

        public MainWindow()
        {
            InitializeComponent();
            //CreateNewHex();

        }

        private void CreateNewHex()
        {
            Canvas?.Children.Clear();
            var positions = new HexGridCalculator().CreateHexPositions(length, maxNumber);
            // Assuming you have a Canvas or Grid control named 'container' in your XAML
            foreach (Point pos in positions)
            {
                // Create a visual representation of the cell
                Ellipse hexagon = new Ellipse();
                hexagon.Width = 25;
                hexagon.Height = 25;
                hexagon.Fill = Brushes.LightGray;
                hexagon.Stroke = Brushes.Black;

                // Scale and position the ellipse
                Canvas.SetLeft(hexagon, pos.X * 25);
                Canvas.SetTop(hexagon, pos.Y * 25);

                // Add the hexagon to the visual tree
                
                Canvas?.Children.Add(hexagon);
                UpdateLayout();
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            maxNumber++;
            CreateNewHex();

        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            maxNumber--;
            CreateNewHex();

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                length = int.TryParse(e.ToString(), out int result) ? result : length;
                CreateNewHex();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}