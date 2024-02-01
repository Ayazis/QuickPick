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

	// In charge of creating the RECT and it's position for the thumbnailpreview.
	// The windows API for createing a Thumbnail requires a RECT.
	public class ThumbnailRectCreator : IThumbnailPositioner
	{
		public double dpiScaling = 1;
		/// <summary>
		/// Creates the RECT for the actual thumbnailpreview.
		/// </summary>
		/// <param name="buttonCenter"></param>
		/// <param name="xToWindowCenter"></param>
		/// <param name="yToWindowCenter"></param>

		/// <param name="i"></param>
		/// <param name="aspectRatio"></param>
		/// <returns></returns>
		public Point CalculatePositionForThumbnailView(Point buttonCenter, double xToWindowCenter, double yToWindowCenter, int i, double width, double height)
		{
			const int EXTRA_MARGIN = 20;
			double X = CalculateThumbnailX(buttonCenter.X, xToWindowCenter, width + EXTRA_MARGIN);
			double Y = CalculateThumbnailY(buttonCenter.Y, yToWindowCenter, height + EXTRA_MARGIN);
			bool isLefToCenter = xToWindowCenter < 0;


			return CalculateRectPosition(X, Y, width + EXTRA_MARGIN, height + EXTRA_MARGIN, i, isLefToCenter);
		}

		private double CalculateThumbnailX(double startPosition, double xDistanceToWindowCenter, double width)
		{
			// Position should increasingly shift depending on xDistanceToWindowCenter          
			double shiftCoEfficient = 2; // increase for more horizontal shifting
			double horizontalShiftAmount = xDistanceToWindowCenter * shiftCoEfficient;
			double shiftedPosition = startPosition + horizontalShiftAmount;

			double offset = width / 2; // Default position is the thumbnail centered in the middle.            
			double offsetCorrection = 0 * dpiScaling; // 25 For some reason things tend to be a bit more to the left than they should be. This corrects that. // Todo: Adjust for DPI?
			double correctedOffset = offset - offsetCorrection;

			double offSetPosition = shiftedPosition - correctedOffset;
			return offSetPosition;
		}


		private double CalculateThumbnailY(double buttonYLocation, double verticalDistance, double height)
		{
			// Normalize verticalDistance to a range of 0 to 1
			double normalizedDistance = ((-verticalDistance + 75) / 150) * dpiScaling;

			// Adjust Offset based on normalizedDistance
			double offset = height * dpiScaling * normalizedDistance;

			double offsetCorrection = 35 * dpiScaling; // 35 For some reason things tend to be higher than they should be. This corrects that. // Todo: Adjust for DPI?
			double correctedOffset = offset - offsetCorrection;
			double finalYPosition = buttonYLocation - correctedOffset;

			return finalYPosition;

		}

		private Point CalculateRectPosition(double x, double y, double width, double Height,
		   int i, bool isLeftToCenter)
		{

			double left = x;
			double top = y;
			double right = x + width;
			double bottom = y + Height;

			if (isLeftToCenter)
			{
				left -= i * width;
				right -= i * width;
			}
			else
			{
				left += i * width;
				right += i * width;
			}

			return new Point(left, top);
		}
	}

}
