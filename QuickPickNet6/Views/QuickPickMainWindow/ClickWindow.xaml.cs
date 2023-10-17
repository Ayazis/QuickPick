using System;
using System.Collections.Generic;
using System.Windows;
using QuickPick.PinnedApps;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Ayazis.KeyHooks;
using System.Windows.Media.Animation;
using MouseAndKeyBoardHooks;
using Ayazis.Utilities;
using ThumbnailLogic;
using System.Linq;
using System.Windows.Input;
using Microsoft.VisualBasic;
using QuickPick.UI.Views.Thumbnail;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using QuickPick.UI;
using System.Runtime.CompilerServices;

namespace QuickPick;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class ClickWindow : Window
{
    public static ClickWindow _instance;
    private QuickPickMainWindowModel _qpm = new QuickPickMainWindowModel();
    private IntPtr _quickPickWindowHandle;
    private List<ThumbnailView> _currentThumbnails = new List<ThumbnailView>();
    public static ThumbnailTimer ThumbnailTimer;

    private ThumbnailRectCreator _thumbnailRectCreator = new();
    public Storyboard HideAnimation { get; private set; }
    public Storyboard ShowAnimation { get; private set; }
    public ClickWindow()
    {
        ThumbnailTimer = new(HideThumbnails);
        InitializeComponent();
        this.PreviewMouseWheel += ClickWindow_PreviewMouseWheel;
       // this.MouseUp += ClickWindow_MouseLeftButtonUp;
        DataContext = _qpm;

        HideAnimation = TryFindResource("hideMe") as Storyboard;
        ShowAnimation = TryFindResource("showMe") as Storyboard;
       // this.Deactivated += HandleFocusLost;
       // this.LostFocus += HandleFocusLost;

        SetQuickPicksMainWindowHandle();
        UpdateLayout();
        _instance = this;

        // EnableBlur();  // The blurreffect works, but only on window level, creating a squared blurry area...
    }

    public void HandleFocusLost(object sender, EventArgs e)
    {
		HideWindow();
	}

	private void ClickWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (MouseIsOutsideVisibleCircle())
            HideWindow();
    }

    private void ClickWindow_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var collection = _qpm.PinnedApps;
        if (e.Delta > 0)
        {       // Scrolled up
            if (collection.Count > 1)
            {
                collection.Move(0, collection.Count - 1);
                //AppLink firstItem = collection[0];
                //collection.RemoveAt(0);
                //collection.Add(firstItem);
            }
        }
        else
        {
            // Scrolled down
            // Scrolled down
            if (collection.Count > 1)
            {
                collection.Move(collection.Count - 1, 0);
            }
        }
    }

    public bool MouseIsOutsideVisibleCircle()
    {
        // Get the current mouse position
        System.Drawing.Point mousePosition = MousePosition.GetCursorPosition();

        // Calculate the circle's radius and center point
        double circleRadius = this.Applinks.Width / 2;
        Point circleCenter = new Point(Left + Width / 2, Top - Height / 2);

        // Calculate the distance between the mouse position and the circle's center
        double deltaX = mousePosition.X - circleCenter.X;
        double deltaY = mousePosition.Y - circleCenter.Y;
        double distanceToCenter = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));

        // Determine if the mouse is outside the circle
        bool isOutside = distanceToCenter > circleRadius;

        return isOutside;
    }

    public void UpdateTaskbarShortCuts()
    {
        List<AppLink> apps = AppLinkRetriever.GetPinnedAppsAndActiveWindows();

        foreach (var app in apps)
        {
            var handle = ActiveWindows.GetActiveWindowOnCurentDesktop(app.TargetPath);
            if (handle != default)
                app.HasWindowActiveOnCurrentDesktop = true;
        }


        // Group the apps by filePath
        var grouped = apps.GroupBy(g => g.TargetPath);
        foreach (var group in grouped)
        {
            // Foreach 
            var openWindows = group.ToList();
            var main = openWindows.First();
            main.WindowHandles = openWindows.SelectMany(s => s.WindowHandles).Distinct().ToList();
        }


        _qpm.PinnedApps = new ObservableCollection<AppLink>(apps);
        _qpm.NotifyPropertyChanged(nameof(_qpm.PinnedApps));
    }

    public static void HideWindow()
    {
        try
        {
            _instance.Deactivated -= _instance.HandleFocusLost;
            _instance.LostFocus -= _instance.HandleFocusLost;
            _instance.HideAnimation.Begin(_instance);

        }
        catch (Exception ex)
        {
            Logs.Logger.Log(ex);
        }
    }

    public void ShowWindow()
    {
        try
        {
            Visibility = Visibility.Visible;
            var mousePosition = MousePosition.GetCursorPosition();
            Left = mousePosition.X - ActualWidth / 2;
            Top = mousePosition.Y - ActualHeight / 2;
            ShowAnimation.Begin(this);
        }
        catch (Exception ex)
        {
            Logs.Logger.Log(ex);
        }
    }

    private void SetQuickPicksMainWindowHandle()
    {
        // Getting the window handle only works when the app is shown in the taskbar.
        // hHe handle remains usable after setting this to false.
        ShowInTaskbar = true;
        Show();
        Process currentProcess = Process.GetCurrentProcess();
        _quickPickWindowHandle = currentProcess.MainWindowHandle;
        ShowInTaskbar = false;
        Hide();

    }
    private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        ThumbnailTimer.StopTimer();
        HideThumbnails();
        // todo: Move logic out of xaml.xs
        var button = (System.Windows.Controls.Button)sender;
        AppLink pinnedApp = button.DataContext as AppLink;

        CreateThumbnails(pinnedApp, button);
    }

    private void CreateThumbnails(AppLink pinnedApp, System.Windows.Controls.Button button)
    {
        // Get the center of the button relative to its container (the window)
        var buttonCenter = button.TransformToAncestor(this)
                                .Transform(new Point(button.ActualWidth / 2, button.ActualHeight / 2));

        // Get the center of the window
        double windowCenterX = this.ActualWidth / 2;
        double windowCenterY = this.ActualHeight / 2;

        // Calculate the button's position relative to the window's center
        double xToWindowCenter = buttonCenter.X - windowCenterX;
        double ytoWindowCenter = buttonCenter.Y - windowCenterY;

        // Get DPI information
        PresentationSource source = PresentationSource.FromVisual(this);
        var m = source.CompositionTarget.TransformToDevice;
        double dpiScaling = m.M11;

        for (int i = 0; i < pinnedApp.WindowHandles.Count; i++)
        {
            ProcessThumbnail(i);
        };
        
        //local function for readability
        void ProcessThumbnail(int i)
        {
            IntPtr currentWindowHandle = pinnedApp.WindowHandles[i];

            // Create thumbnailRelation
            var newThumbnail = ThumbnailCreator.GetThumbnailRelations(currentWindowHandle, _quickPickWindowHandle);
            if (newThumbnail == default)
                return;

            double aspectRatio = ThumbnailCreator.GetWindowAspectRatio(currentWindowHandle);
            RECT rect = _thumbnailRectCreator.CreateRectForThumbnail(buttonCenter, xToWindowCenter, ytoWindowCenter, dpiScaling, i, aspectRatio);

            string windowTitle = ActiveWindows.GetWindowTitle(currentWindowHandle);
            var thumbnailContext = new ThumbnailDataContext(newThumbnail, rect, currentWindowHandle, windowTitle);
            var thumbnailView = new ThumbnailView(thumbnailContext, dpiScaling);

            this.ThumbnailCanvas.Children.Add(thumbnailView);
            _currentThumbnails.Add(thumbnailView);

            // Set the position of ThumbnailView based on translated coordinates.
            Canvas.SetLeft(thumbnailView, rect.Left / dpiScaling - 20);
            Canvas.SetTop(thumbnailView, rect.Top / dpiScaling - 20);
            thumbnailView.FadeIn();
        }
    }

    private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        ThumbnailTimer.StartTimer();
    }


    private void HideThumbnails()
    {
        foreach (var thumbnailView in _currentThumbnails)
        {
            if (thumbnailView != default)
                thumbnailView.Hide();
        }
        _currentThumbnails.Clear();
    }


    internal void EnableBlur()
    {
        var windowHelper = new WindowInteropHelper(this);

        var accent = new AccentPolicy();
        accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

        var accentStructSize = Marshal.SizeOf(accent);

        var accentPtr = Marshal.AllocHGlobal(accentStructSize);
        Marshal.StructureToPtr(accent, accentPtr, false);

        var data = new WindowCompositionAttributeData();
        data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
        data.SizeOfData = accentStructSize;
        data.Data = accentPtr;

        SetWindowCompositionAttribute(windowHelper.Handle, ref data);

        Marshal.FreeHGlobal(accentPtr);
    }
    internal enum AccentState
    {
        ACCENT_DISABLED = 1,
        ACCENT_ENABLE_GRADIENT = 0,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_INVALID_STATE = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    internal enum WindowCompositionAttribute
    {
        // ...
        WCA_ACCENT_POLICY = 19
        // ...
    }
    [DllImport("user32.dll")]
    internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
}
