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
        public Point CalculatePositionForThumbnailView(Point buttonLocation, double xToWindowCenter, double yToWindowCenter, int i, double width, double height)
        {
            // TAKE NOTE:
            // X increases when going right.
            // Y increases when going down.

            Point adjustedStartPoint = buttonLocation;
            double normalized_X_DistanceInPercentages = (xToWindowCenter - _halfSize) / -UI_SIZE;  // for example: turns -75 to -150 and 75 to 0; 
            double normalized_Y_DistanceInPercentages = (yToWindowCenter - _halfSize) / -UI_SIZE;  // for example: turns -75 to -150 and 75 to 0;

            double xOffset = normalized_X_DistanceInPercentages * -width;
            double yOffset = normalized_Y_DistanceInPercentages * -height;

            // add offset for height of the thumbnail
            adjustedStartPoint.X += xOffset;
            adjustedStartPoint.Y += yOffset;

            double maxDistance = _halfSize;
            double extraSpace = 50;

            // Calculate the proportion of the current distance to the max distance
            double proportionX = (double)xToWindowCenter / maxDistance;
            double proportionY = (double)yToWindowCenter / maxDistance;

            // Adjust the start point based on the proportion
            adjustedStartPoint.X += extraSpace * proportionX;
            adjustedStartPoint.Y += extraSpace * proportionY;

            if (i != 0) // if the thumbnail is not the first.
            {
                // adjust the start point based on the index
                bool isleftToCenter = xToWindowCenter < 0;
                if (isleftToCenter)
                    adjustedStartPoint.X -= i * width;
                else
                    adjustedStartPoint.X += i * width;


            }
            return adjustedStartPoint;
        }


        public Point CalculatePositionForThumbnailView(Point PreviousThumbnailLocation, double xToWindowCenter, double previousWidth, double width)
        {
            bool isleftToCenter = xToWindowCenter < 0;
            if (isleftToCenter)
            {
                return new Point(PreviousThumbnailLocation.X - width, PreviousThumbnailLocation.Y);
            }
            return new Point(PreviousThumbnailLocation.X + previousWidth, PreviousThumbnailLocation.Y);
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
