using FontAwesome5;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuickPick.UI.Views.Hex;
public class HexagonButton : Button
{
    Hexagon _hexagonShapeBackground = new();
    public Hexagon Hexagon { get; private set; } = new Hexagon() { Fill = Brushes.Transparent };
    ImageAwesome Image;
    public Grid Grid = new Grid();
    double hexScale = 1.0;
    double iconScale = .3;

    private static Style _noHooverOverStyle;

    public HexagonButton()
    {
        BorderThickness = new Thickness(0);
        Padding = new Thickness(-5);
        Background = Brushes.Transparent;


        AddHexaonShapeAndIcon();

        SizeChanged += HexagonButton_SizeChanged;


        _noHooverOverStyle = CreateStyle();
        Style = _noHooverOverStyle;
    }

    private void AddHexaonShapeAndIcon()
    {
        // Create the FontAwesome icon
        SetDefaultImage();

        // Add the Hexagon to the Grid
        Grid.Children.Add(_hexagonShapeBackground);

        //Add the icon to the Grid
        Grid.Children.Add(Image);

        Grid.Children.Add(Hexagon);


        // Set the Content of the Button to the Grid
        this.Content = Grid;
    }

    private void SetDefaultImage()
    {
        Image = new ImageAwesome
        {
            Foreground = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Width = this.Width * iconScale,
            Height = this.Height * iconScale,
            //Icon = EFontAwesomeIcon.Regular_Eye
        };
    }

    private static Style CreateStyle()
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
        _hexagonShapeBackground.Width = this.ActualWidth * hexScale;
        _hexagonShapeBackground.Height = this.ActualHeight * hexScale;
        Hexagon.Width = this.ActualWidth * hexScale;
        Hexagon.Height = this.ActualHeight * hexScale;


        // Update icon size
        UpdateIconSize();

        UpdateLayout();
    }
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        "Icon", typeof(EFontAwesomeIcon), typeof(HexagonButton), new PropertyMetadata(default(EFontAwesomeIcon), OnIconChanged));
    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var button = (HexagonButton)d;
        button.Image.Icon = (EFontAwesomeIcon)e.NewValue;
        button.Image.Width = button.ActualWidth * button.iconScale;
        button.Image.Height = button.ActualHeight * button.iconScale;
        button.HexagonButton_SizeChanged(button, null);
    }
    public EFontAwesomeIcon FontIcon
    {
        get { return (EFontAwesomeIcon)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    private void UpdateIconSize()
    {
        Image.Width = this.ActualWidth * iconScale;
        Image.Height = this.ActualHeight * iconScale;
    }


}
