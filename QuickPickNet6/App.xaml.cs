﻿using Ayazis.KeyHooks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Application = System.Windows.Application;
using System.Windows.Forms;
using System.Windows;
using System.Diagnostics;
using System.Collections.ObjectModel;
using QuickPick.PinnedApps;

namespace QuickPick;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{	
	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);	       
		new TrayIconManager().CreateTrayIcon();		
	}
}
