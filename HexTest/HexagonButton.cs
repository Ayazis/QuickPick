using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuickPick.UI.Views.Hex;
public class HexagonButton : Button
{
    public Hexagon HexagonShape;
    // ImageAwesome Image;

    double hexScale = 1.1;
    double iconScale = .3;


    private static Style _noHooverOverStyle;

    public HexagonButton()
    {

        this.BorderThickness = new Thickness(0);
        Padding = new Thickness(-5);
        Background = Brushes.Transparent;

        // Create a Grid to hold the Hexagon and the icon
        Grid grid = new Grid();

        // Create the Hexagon shape
        HexagonShape = new Hexagon()
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };



        // Add the Hexagon to the Grid
        grid.Children.Add(HexagonShape);

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

        Style = CreateStyle();
    }


    private Style CreateStyle()
    {
        if (_noHooverOverStyle != null)
            return _noHooverOverStyle;

        // Create a new style
        Style noHooverOverStyle = new Style(typeof(Button));

        // Set OverridesDefaultStyle to true
        noHooverOverStyle.Setters.Add(new Setter { Property = Control.OverridesDefaultStyleProperty, Value = true });

        // Create a new control template
        ControlTemplate template = new ControlTemplate(typeof(Button));

        // Create a border for the template
        FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
        border.Name = "border";
        border.SetValue(BackgroundProperty, Brushes.Transparent);

        // Create a content presenter for the template
        FrameworkElementFactory presenter = new FrameworkElementFactory(typeof(ContentPresenter));
        presenter.SetValue(ContentPresenter.ContentProperty, new TemplateBindingExtension(ContentProperty));
        presenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, new TemplateBindingExtension(HorizontalContentAlignmentProperty));
        presenter.SetValue(ContentPresenter.VerticalAlignmentProperty, new TemplateBindingExtension(VerticalContentAlignmentProperty));

        // Add the content presenter to the border
        border.AppendChild(presenter);

        // Set the visual tree of the template
        template.VisualTree = border;

        // Add the template to the style
        noHooverOverStyle.Setters.Add(new Setter { Property = TemplateProperty, Value = template });

        _noHooverOverStyle = noHooverOverStyle;
        return noHooverOverStyle;
    }
    private void HexagonButton_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        // Set the Width and Height of the Hexagon to be the same as the ActualWidth and ActualHeight of the HexagonButton
        HexagonShape.Width = this.ActualWidth * hexScale;
        HexagonShape.Height = this.ActualHeight * hexScale;

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
