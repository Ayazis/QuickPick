﻿using System;
using System.Collections.Generic;
using System.Windows;
using QuickPick.PinnedApps;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Ayazis.KeyHooks;
using System.Windows.Media.Animation;
using MouseAndKeyBoardHooks;
using Ayazis.Utilities;
using Utilities.Mouse_and_Keyboard;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using Utilities.Utilities.VirtualDesktop;
using Utilities.VirtualDesktop;
using System.Threading.Tasks;
using ThumbnailLogic;
using System.Windows.Media.Media3D;

namespace QuickPick;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class ClickWindow : Window
{
    private static ClickWindow _instance;
    private QuickPickMainWindowModel _qpm = new QuickPickMainWindowModel();
    private IntPtr _quickPickWindowHandle;
    private IntPtr _currentThumbnail;

    public Storyboard HideAnimation { get; private set; }
    public Storyboard ShowAnimation { get; private set; }
    public ClickWindow()
    {
        InitializeComponent();
        DataContext = _qpm;

        HideAnimation = TryFindResource("hideMe") as Storyboard;
        ShowAnimation = TryFindResource("showMe") as Storyboard;


        SetQuickPicksMainWindowHandle();
        UpdateLayout();
        _instance = this;
    }

    private void HotKeys_LeftMouseClicked()
    {
        if (MouseIsOutsideWindow())
            HideAnimation.Begin(this);
    }
    public bool MouseIsOutsideWindow()
    {
        var mouse = MousePosition.GetCursorPosition();

        bool isOutside = (mouse.X < this.Left || mouse.X > this.Left + this.ActualWidth)
                        || (mouse.Y < this.Top || mouse.Y > this.Top + this.ActualHeight);

        return isOutside;
    }

    public void UpdateTaskbarShortCuts()
    {
        List<TaskbarShortCut> apps = TaskbarApps.GetPinnedAppsAndActiveWindows();

        foreach (var app in apps)
        {
            var handle = ActiveWindows.GetActiveWindowOnCurentDesktop(app.TargetPath);
            if (handle != default)
                app.HasWindowActiveOnCurrentDesktop = true;
        }

        _qpm.PinnedApps = new ObservableCollection<TaskbarShortCut>(apps);
        _qpm.NotifyPropertyChanged(nameof(_qpm.PinnedApps));


    }

    public static void HideWindow()
    {
        try
        {
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
            this.Left = mousePosition.X - (this.ActualWidth / 2);
            this.Top = mousePosition.Y - (this.ActualHeight / 2);
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
        var button = ((System.Windows.Controls.Button)sender);
        TaskbarShortCut pinnedApp = button.DataContext as TaskbarShortCut;
        var windowHandle = pinnedApp.WindowHandle;


        double sizeFactor = 0.2;
        var positionRelativeToWindow = button.TranslatePoint(new Point(0, 0), this);

        double x = positionRelativeToWindow.X;
        double y = positionRelativeToWindow.Y;
        double width = 1920 * sizeFactor;
        double height = 1080 * sizeFactor;

        _currentThumbnail = ThumbnailCreator.GetThumbnailRelations(windowHandle, _quickPickWindowHandle);
        if (_currentThumbnail == default)
            return;
        RECT rect = new RECT((int)x, (int)y, (int)(x + width), (int)(y + height));

        ThumbnailCreator.FadeInThumbnail(_currentThumbnail, rect);
    }

    private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (_currentThumbnail != default)
            ThumbnailCreator.DwmUnregisterThumbnail(_currentThumbnail);

    }
}
