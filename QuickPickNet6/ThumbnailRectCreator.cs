using System;
using System.Windows;
using ThumbnailLogic;

namespace QuickPick.UI
{
    public class ThumbnailRectCreator
    {

        private const double MAX_DIMENSION = 180.0;

        /// <summary>
        /// Creates the RECT for the actual thumbnailpreview.
        /// </summary>
        /// <param name="buttonCenter"></param>
        /// <param name="xToWindowCenter"></param>
        /// <param name="yToWindowCenter"></param>
        /// <param name="dpiScaling"></param>
        /// <param name="i"></param>
        /// <param name="aspectRatio"></param>
        /// <returns></returns>
        public RECT CreateRectForThumbnail(Point buttonCenter, double xToWindowCenter, double yToWindowCenter, double dpiScaling, int i, double aspectRatio)
        {
            var dimensions = CalculateThumbnailDimensions(aspectRatio);
            double thumbnailX = CalculateThumbnailX(buttonCenter.X, xToWindowCenter, dimensions.Width);
            double thumbnailY = CalculateThumbnailY(buttonCenter.Y, yToWindowCenter, dimensions.Height);

            return CalculateRECT(thumbnailX, thumbnailY, dimensions, dpiScaling, i, xToWindowCenter < 0);
        }

        private (double Width, double Height) CalculateThumbnailDimensions(double aspectRatio)
        {
            double width, height;
            bool isLandscape = aspectRatio > 1;

            if (isLandscape)
            {
                width = MAX_DIMENSION; 
                height = width / aspectRatio;
            }
            else
            {
                height = MAX_DIMENSION;
                width = height * aspectRatio;
            }

            return (Width: width, Height: height);
        }

        private double CalculateThumbnailX(double startPosition, double xDistanceToWindowCenter, double width)
        {
            // Position should increasingly shift depending on xDistanceToWindowCenter          
            double shiftCoEfficient = 2; // increase for more horizontal shifting
            double horizontalShiftAmount = xDistanceToWindowCenter * shiftCoEfficient;
            double shiftedPosition = startPosition + horizontalShiftAmount;

            double offset = width / 2; // Default position is the thumbnail centered in the middle.            
            double offSetPosition = shiftedPosition - offset;
            return offSetPosition;
        }


        private double CalculateThumbnailY(double buttonCenterY, double yToCenter, double height)
        {
            double coefficient = 1.2; // Increase to increase distance from thumbnail to the center
            double thumbnailY = buttonCenterY + (yToCenter * coefficient);
            double verticalOffset = -30;
            thumbnailY += verticalOffset;

            if (yToCenter < 0)
            {
                thumbnailY -= (height / 2);
            }

            return thumbnailY;
        }

        private RECT CalculateRECT(double thumbnailX, double thumbnailY, (double Width, double Height) dimensions, double dpiScaling, int i, bool isLeftToCenter)
        {
            int left = (int)(thumbnailX * dpiScaling);
            if (isLeftToCenter)
            {
                left -= (int)(i * MAX_DIMENSION);
            }
            else
            {
                left += (int)(i * MAX_DIMENSION);
            }

            int top = (int)(thumbnailY * dpiScaling);
            int right = (int)((thumbnailX + dimensions.Width) * dpiScaling);
            int bottom = (int)((thumbnailY + dimensions.Height) * dpiScaling);

            return new RECT(left, top, right, bottom);
        }
    }

}
