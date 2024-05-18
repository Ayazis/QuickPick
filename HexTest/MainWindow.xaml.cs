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
        int length = 50;
        int maxNumber = 48;

        HexGridCreator _hexGridCreator = new(new HexPositionsCalculator());
        public MainWindow()
        {
            InitializeComponent();
            tbCount.Text = maxNumber.ToString();
            //CreateNewHex();        
        }


        public void DrawHexagonalGrid()
        {
            _hexGridCreator.CreateHexButtonsInHoneyCombStructure(this.Canvas.Width, length, maxNumber);
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
                DrawHexagonalGrid();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}