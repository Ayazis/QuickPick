using QuickPick.UI.Views.Hex;
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
            tbCount.Text = maxNumber.ToString();
            //CreateNewHex();        
        }


        public void DrawHexagonalGrid(List<HexPositionsCalculator.Point> grid, double size)
        {
            Canvas?.Children.Clear();
            foreach (var point in grid)
            {
                double x = size/1.75 * (3.0 / 2 * point.Q);
                double y = size/1.75 * (Math.Sqrt(3) * (point.R + 0.5 * point.Q));
                var hexagon = new HexagonButton() { Width = size, Height = size };
                //hexagon.Fill = Brushes.LightGray;
                //hexagon.Stroke = Brushes.Black;
                Canvas.Children.Add(hexagon);
                Canvas.SetLeft(hexagon, x);
                Canvas.SetTop(hexagon, y);
            }
        }



    private void IncreaseCount()
        {
            maxNumber++;
            tbCount.Text = maxNumber.ToString();
        }

        private void DecreaseCount()
        {
            maxNumber--;
            tbCount.Text = maxNumber.ToString();
        }
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            IncreaseCount();

        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            DecreaseCount();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                length = int.TryParse(e.ToString(), out int result) ? result : length;
                var grid = HexPositionsCalculator.GenerateHexagonalGridFixed(maxNumber);
                DrawHexagonalGrid(grid, 50);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}