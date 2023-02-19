using System;
using System.Runtime.InteropServices;
using System.Windows; 
using Graphics = System.Drawing.Graphics;

namespace QuickPick.Logic
{
    public class MousePosition
    {
        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }
        public static Point GetCursorPosition()
        {
            POINT lpPoint;

            GetCursorPos(out lpPoint);

            lpPoint = CorrectForScaling(lpPoint);

            return lpPoint;
        }
        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);       

        private static POINT CorrectForScaling(POINT lpPoint)
        {
            // Todo: Multiple screens, multiple scaling!
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                float dpiX = graphics.DpiX;

                var scalingFactor =  dpiX / 96;

                lpPoint.X = (int)(lpPoint.X / scalingFactor);
                lpPoint.Y = (int)(lpPoint.Y / scalingFactor);

                return lpPoint;
            }
        }

     
    }

}