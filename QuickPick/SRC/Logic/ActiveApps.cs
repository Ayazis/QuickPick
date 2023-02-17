using QuickPick.Logic;
using QuickPick.Logic.ActiveApps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

using System.Windows.Media.Imaging;

using WPFImage = System.Windows.Controls.Image;

namespace QuickPick.SRC.Logic
{
	public static class ActiveApps
	{
		[DebuggerDisplay("{WindowText}")]
		public class OpenApplicationObject
		{
			public IntPtr Handle;			
			public WPFImage Thumbnail;
			public string WindowText;
			public OpenApplicationObject(IntPtr handle, string windowText, WPFImage thumbnail)
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
						WPFImage wpfImage = ConvertToUIElement(thumbnail);
						yield return new OpenApplicationObject(hWnd, process.MainWindowTitle, wpfImage);						
					}
				}
			}
			
		}

		public static WPFImage ConvertToUIElement(Image image)
		{
			using (var stream = new MemoryStream())
			{
				// save the image to the stream in PNG format
				image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

				// create a new BitmapImage and set its source to the stream
				var bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.StreamSource = new MemoryStream(stream.ToArray());
				bitmapImage.EndInit();

				// create a new Image control and set its source to the BitmapImage
				WPFImage wpfImage = new WPFImage();
				wpfImage.Source = bitmapImage;

				// wpfImage is now a UIElement that can be used in your WPF application
				return wpfImage;
			}
		}
	}
}

