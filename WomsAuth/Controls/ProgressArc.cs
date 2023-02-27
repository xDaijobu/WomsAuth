using Microsoft.Maui.Graphics;
using System.Diagnostics;

namespace WomsAuth.Controls
{
    public class ProgressArc : IDrawable
    {
        public double Progress { get; set; } = 100;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Angle of the arc in degrees
            var endAngle = 90 - (int)Math.Round(Progress * 360, MidpointRounding.AwayFromZero);
            // Drawing code goes here
            
            canvas.StrokeColor = Color.FromRgba("6599ff");
            canvas.FillColor = Color.FromRgba("6599ff");
            canvas.StrokeSize = 80;

            Debug.WriteLine($"The rect width is {dirtyRect.Width} and height is {dirtyRect.Height}");
            //canvas.DrawArc(5, 5, (dirtyRect.Width - 10), (dirtyRect.Height - 10), 90, endAngle, false, false);
            var currentWidth = (dirtyRect.Width - 10);
            var currentHeight = (dirtyRect.Height - 10);
            //canvas.FillArc(dirtyRect, 90, endAngle, false);

            // luar ny
            canvas.DrawArc(dirtyRect, 90, endAngle, false, false);
            
            
            // dlm
            //canvas.FillArc(dirtyRect, 90, endAngle, false);
        }
    }
}
