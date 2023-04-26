using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QuickPickNet6;
internal class TrayIcon
{
	private NotifyIcon _trayIcon;
	private ContextMenuStrip _contextMenu;

	public void CreateTrayIcon()
	{
		_contextMenu = new ContextMenuStrip();
		_contextMenu.Items.Add("Settings", null, OnSettingsClick);
		_contextMenu.Items.Add(new ToolStripSeparator());
		_contextMenu.Items.Add("Exit", null, OnExitClick);

		_trayIcon = new NotifyIcon
		{
			Icon = CreateIcon(),
			Visible = true,
			ContextMenuStrip = _contextMenu
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

	public void OnExitClick(object sender, EventArgs e)
	{
		_trayIcon.Visible = false;
		_trayIcon.Dispose();
		System.Windows.Application.Current.Shutdown();
	}

	public delegate void SettingsMenuClickedEventHandler(object sender, EventArgs e);

	public static event SettingsMenuClickedEventHandler SettingsMenuClicked;

	private void OnSettingsClick(object sender, EventArgs e)
	{
		SettingsMenuClicked?.Invoke(this, e);
	}

}