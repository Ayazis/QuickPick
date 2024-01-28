using System.Diagnostics;
using System.Windows;
using ThumbnailLogic;

namespace QuickPick.UI
{

    // In charge of creating the RECT and it's position for the thumbnailpreview.
    // The windows API for createing a Thumbnail requires a RECT.
    public static class ThumbnailRectCreator
    {   
        public static double dpiScaling = 1;
        /// <summary>
        /// Creates the RECT for the actual thumbnailpreview.
        /// </summary>
        /// <param name="buttonCenter"></param>
        /// <param name="xToWindowCenter"></param>
        /// <param name="yToWindowCenter"></param>
       
        /// <param name="i"></param>
        /// <param name="aspectRatio"></param>
        /// <returns></returns>
        public static RECT CalculatePositionForThumbnailView(Point buttonCenter, double xToWindowCenter, double yToWindowCenter,  int i, double width, double height)
        {   
            const int EXTRA_MARGIN = 20;
            double X = CalculateThumbnailX(buttonCenter.X, xToWindowCenter, width + EXTRA_MARGIN);
            double Y = CalculateThumbnailY(buttonCenter.Y, yToWindowCenter, height + EXTRA_MARGIN);
            bool isLefToCenter = xToWindowCenter < 0;


            return CalculateRectPosition(X, Y, width+EXTRA_MARGIN, height+EXTRA_MARGIN, i, isLefToCenter);
        }

        private static double CalculateThumbnailX(double startPosition, double xDistanceToWindowCenter, double width)
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


        private static double CalculateThumbnailY(double buttonYLocation, double verticalDistance, double height)
        {            
            // Normalize verticalDistance to a range of 0 to 1
            double normalizedDistance = ((-verticalDistance + 75) / 150) * dpiScaling; 

            // Adjust Offset based on normalizedDistance
            double offset = height * dpiScaling * normalizedDistance;

            double offsetCorrection = 35 *dpiScaling; // 35 For some reason things tend to be higher than they should be. This corrects that. // Todo: Adjust for DPI?
            double correctedOffset = offset - offsetCorrection;
            double finalYPosition = buttonYLocation -correctedOffset;

            return finalYPosition;

        }

        private static RECT CalculateRectPosition(double x, double y, double width, double Height,
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

            var newRect = new RECT((int)left, (int)top, (int)right, (int)bottom);
            return newRect;
        }
    }

}
