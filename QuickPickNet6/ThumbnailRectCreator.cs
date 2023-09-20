using System;
using System.Windows;
using ThumbnailLogic;

namespace QuickPick.UI
{ 
    public class ThumbnailRectCreator
    {
        
        private const double DEFAULT_DIMENSION = 200.0;

        public RECT CreateRectForThumbnail(Point buttonCenter, double xToCenter, double ytoCenter, double dpiScaling, int i, double aspectRatio)
        {
            var dimensions = CalculateThumbnailDimensions(aspectRatio);
            double thumbnailX = CalculateThumbnailX(buttonCenter.X, xToCenter, DEFAULT_DIMENSION);
            double thumbnailY = CalculateThumbnailY(buttonCenter.Y, ytoCenter, DEFAULT_DIMENSION);

            return CalculateRECT(thumbnailX, thumbnailY, dimensions, dpiScaling, i, xToCenter < 0);
        }

        private (double Width, double Height) CalculateThumbnailDimensions(double aspectRatio)
        {
            double width, height;
            bool isLandscape = aspectRatio > 1;

            if (isLandscape)
            {
                width = DEFAULT_DIMENSION-20;
                height = width / aspectRatio;
            }
            else
            {
                height = DEFAULT_DIMENSION-20;
                width = height * aspectRatio;
            }

            return (Width: width, Height: height);
        }

        private double CalculateThumbnailX(double buttonCenterX, double xToCenter, double width)
        {
            double thumbnailX = buttonCenterX + xToCenter;

            // The closer the thumbnail is to the center (xToCenter near 0), the more it should be shifted to align its center.
            double centeringFactor = Math.Abs(xToCenter) / width;
            double shift = (0.8- centeringFactor) * (width / 2);

            if (xToCenter < 0)
            {
                thumbnailX -= (width - shift);
            }
            else
            {
                thumbnailX -= shift;
            }

            return thumbnailX;
        }


        private double CalculateThumbnailY(double buttonCenterY, double yToCenter, double height)
        {
            double coefficient = 1;
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
                left -= (int)(i * DEFAULT_DIMENSION);
            }
            else
            {
                left += (int)(i * DEFAULT_DIMENSION);
            }

            int top = (int)(thumbnailY * dpiScaling);
            int right = (int)((thumbnailX + dimensions.Width) * dpiScaling);
            int bottom = (int)((thumbnailY + dimensions.Height) * dpiScaling);

            return new RECT(left, top, right, bottom);
        }
    }

}
