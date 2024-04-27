using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuickPick.UI.Views.Hex;

public class HexagonButton : Button
{
    Hexagon _hexagon;
   // ImageAwesome Image;

    double hexScale = 1.1;
    double iconScale = .3;
    public HexagonButton()
    {
        this.BorderThickness = new Thickness(0);
        Padding = new Thickness(-5);
        Background = Brushes.Transparent;

        // Create a Grid to hold the Hexagon and the icon
        Grid grid = new Grid();

        // Create the Hexagon shape
        _hexagon = new Hexagon()
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };

        // Add the Hexagon to the Grid
        grid.Children.Add(_hexagon);

        //// Create the FontAwesome icon
        //Image = new ImageAwesome
        //{            
        //    Foreground = Brushes.White,
        //    HorizontalAlignment = HorizontalAlignment.Center,
        //    VerticalAlignment = VerticalAlignment.Center,
        //    Width = this.Width * iconScale,
        //    Height = this.Height * iconScale
        //};

        // Add the icon to the Grid
        //grid.Children.Add(Image);

        // Set the Content of the Button to the Grid
        this.Content = grid;

        SizeChanged += HexagonButton_SizeChanged; ;
    }

    private void HexagonButton_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        // Set the Width and Height of the Hexagon to be the same as the ActualWidth and ActualHeight of the HexagonButton
        _hexagon.Width = this.ActualWidth * hexScale;
        _hexagon.Height = this.ActualHeight * hexScale;

        //Image.Width = this.ActualWidth * iconScale;
    }
    //public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
    //    "Icon", typeof(EFontAwesomeIcon), typeof(HexagonButton), new PropertyMetadata(default(EFontAwesomeIcon), OnIconChanged));


    //private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //{
    //    var button = (HexagonButton)d;
    //    button.Image.Icon = (EFontAwesomeIcon)e.NewValue;
    //    button.Image.Width = button.ActualWidth * button.iconScale;
    //  //  button.Image.Height = button.ActualHeight * button.iconScale;
    //    button.HexagonButton_SizeChanged(button, null);
    //}
    //public EFontAwesomeIcon FontIcon
    //{
    //    get { return (EFontAwesomeIcon)GetValue(IconProperty); }
    //    set { SetValue(IconProperty, value); }
    //}

}