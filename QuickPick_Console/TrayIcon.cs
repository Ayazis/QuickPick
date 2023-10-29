using QuickPick.UI.Views.Settings;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace QuickPick;
public class TrayIconManager
{
	private NotifyIcon _trayIcon;
	private ContextMenuStrip _contextMenu;

	public void CreateTrayIcon()
	{
		_contextMenu = new ContextMenuStrip();
		_contextMenu.Items.Add("Settings", null, OnSettingsClick);
		_contextMenu.Items.Add(new ToolStripSeparator());
		_contextMenu.Items.Add("Exit", null, OnExitClick);
		Assembly assembly = Assembly.GetExecutingAssembly();
		string currentVersion = assembly?.GetName()?.Version?.ToString() ?? string.Empty;
		_trayIcon = new NotifyIcon
		{
			Icon = CreateIcon(),
			Visible = true,
			ContextMenuStrip = _contextMenu
			,
			Text= $"QuickPick {currentVersion}"
		};
	}
	private Icon CreateIcon()
	{
		var currentPath = AppDomain.CurrentDomain.BaseDirectory;
		var IconPath = $@"{currentPath}\Assets\QP_Icon_32px.png";
		if (!File.Exists(IconPath))
			return new Icon(SystemIcons.Warning, 40, 40);

		var bitmap = new Bitmap(IconPath);
		var iconHandle = bitmap.GetHicon();

		return Icon.FromHandle(iconHandle);
	}

	public void RemoveTrayIcon()
	{
		if (_trayIcon != null)
		{
			_trayIcon.Visible = false;
			_trayIcon.Dispose();
			_trayIcon = null;
		}
	}
	public void OnExitClick(object sender, EventArgs e)
	{
		Application.Exit(); // handling this event is done in program.cs
	}

	public delegate void SettingsMenuClickedEventHandler(object sender, EventArgs e);

	public static event SettingsMenuClickedEventHandler SettingsMenuClicked;

	private void OnSettingsClick(object sender, EventArgs e)
	{
		new SettingsWindow().Show();
	}

}