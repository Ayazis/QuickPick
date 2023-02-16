using QuickPick.Logic;
using QuickPick.Logic.ActiveApps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Win32Interop.WinHandles;

namespace QuickPick.SRC.Logic
{
	public static class ActiveApps
	{
		[DebuggerDisplay("{WindowText}")]
		public class OpenApplicationObject
		{
			public IntPtr Handle;			
			public Bitmap Thumbnail;
			public string WindowText;
			public OpenApplicationObject(IntPtr handle, string windowText, Bitmap thumbnail)
			{
				Handle = handle;
				Thumbnail = thumbnail;
				WindowText = windowText;
			}		
		}
	
		public static IEnumerable<OpenApplicationObject> GetAllOpenWindows()
		{
			foreach (var process in Process.GetProcesses())
			{
				IntPtr hWnd = process.MainWindowHandle;
				if (hWnd != IntPtr.Zero)
				{
					Image thumbnail = ThumbnailGetter.GetThumbnail(hWnd);
					if (thumbnail != null)
					{
						var bitmap = new Bitmap(thumbnail);
						yield return new OpenApplicationObject(hWnd, process.MainWindowTitle, bitmap);						
					}
				}
			}
			
		}
	}
}

