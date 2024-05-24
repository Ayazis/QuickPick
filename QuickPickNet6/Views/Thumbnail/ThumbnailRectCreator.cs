using System;
using System.Diagnostics;
using System.DirectoryServices.Protocols;
using System.Windows;
using ThumbnailLogic;

namespace QuickPick.UI
{
	public class DpiSafeThumbnailPositioner : IThumbnailPositioner
	{
		// this is the hardcoded size of the circular panel , found in the main ui xaml file. Todo: fetch dynamically
		static readonly double UI_SIZE = 150;
		readonly double _halfSize = UI_SIZE / 2;

		//</inheritdoc>
		public Point CalculatePositionForThumbnailView(Point startPoint, double xToWindowCenter, double yToWindowCenter, int i, double width, double height)
		{
			// TAKE NOTE:
			// X increases when going right.
			// Y increases when going down.

			Point adjustedStartPoint = startPoint;
			double normalized_X_DistanceInPercentages = (xToWindowCenter - _halfSize) / -UI_SIZE;  // for example: turns -75 to -150 and 75 to 0; 
			double normalized_Y_DistanceInPercentages = (yToWindowCenter - _halfSize) / -UI_SIZE;  // for example: turns -75 to -150 and 75 to 0;

			double xOffset = normalized_X_DistanceInPercentages * -width;
			double yOffset = normalized_Y_DistanceInPercentages * -height;

			// add offset for height of the thumbnail
			adjustedStartPoint.X += xOffset;
			adjustedStartPoint.Y += yOffset;

			// At this point everything is placed semi-correct. We will need to increase the distance between the thumbnail and the center.
			// this means that what is on top should be higher, and what is on the bottom should be lower.
			// this means that what is on the left should be more to the left, and what is on the right should be more to the right.
			// we will have to move it gradually, -75 == -100% and 75 == 100%.            

			double maxDistance = _halfSize;
			double extraSpace = 50;

			// Calculate the proportion of the current distance to the max distance
			double proportionX = (double)xToWindowCenter / maxDistance;
			double proportionY = (double)yToWindowCenter / maxDistance;

			// Adjust the start point based on the proportion
			adjustedStartPoint.X += extraSpace * proportionX;
			adjustedStartPoint.Y += extraSpace * proportionY;

			// adjust the start point based on the index
			bool isleftToCenter = xToWindowCenter < 0;
			if (isleftToCenter)
				adjustedStartPoint.X -= i * width;
			else
				adjustedStartPoint.X += i * width;

			return adjustedStartPoint;
		}
	}



	public interface IThumbnailPositioner
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buttonCenter">the point of the center of the ApplinkButton, as fullscreen coordinates.</param>
		/// <param name="xToWindowCenter">The horizontal WPF distance of the button to the center of the main UI.</param>
		/// <param name="yToWindowCenter">The vertical WPF distance of the button to the center of the main UI.</param>
		/// <param name="thumbnailIndex">The index of the thumbnail within all the app's thumbnails.</param>
		/// <param name="width">Width of the thumbnail.</param>
		/// <param name="height">Height of the thumbnail.</param>
		/// <returns></returns>
		Point CalculatePositionForThumbnailView(Point buttonCenter, double xToWindowCenter, double yToWindowCenter, int thumbnailIndex, double width, double height);
	}
}
