using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ProgLib.Windows.Material
{
    static class DrawHelper
    {
        public static GraphicsPath CreateRoundRect(float X, float Y, float Width, float Height, float Radius)
        {
            GraphicsPath GP = new GraphicsPath();
            GP.AddLine(X + Radius, Y, X + Width - (Radius * 2), Y);
            GP.AddArc(X + Width - (Radius * 2), Y, Radius * 2, Radius * 2, 270, 90);
            GP.AddLine(X + Width, Y + Radius, X + Width, Y + Height - (Radius * 2));
            GP.AddArc(X + Width - (Radius * 2), Y + Height - (Radius * 2), Radius * 2, Radius * 2, 0, 90);
            GP.AddLine(X + Width - (Radius * 2), Y + Height, X + Radius, Y + Height);
            GP.AddArc(X, Y + Height - (Radius * 2), Radius * 2, Radius * 2, 90, 90);
            GP.AddLine(X, Y + Height - (Radius * 2), X, Y + Radius);
            GP.AddArc(X, Y, Radius * 2, Radius * 2, 180, 90);
            GP.CloseFigure();
            return GP;
        }

        public static GraphicsPath CreateRoundRect(Rectangle Rectangle, float Radius)
        {
            return CreateRoundRect(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height, Radius);
        }

        public static Color BlendColor(Color BackgroundColor, Color FrontColor, double Blend)
        {
            Double ratio = Blend / 255d;
            Double invRatio = 1d - ratio;
            Int32 r = (int)((BackgroundColor.R * invRatio) + (FrontColor.R * ratio));
            Int32 g = (int)((BackgroundColor.G * invRatio) + (FrontColor.G * ratio));
            Int32 b = (int)((BackgroundColor.B * invRatio) + (FrontColor.B * ratio));
            return Color.FromArgb(r, g, b);
        }

        public static Color BlendColor(Color BackgroundColor, Color FrontColor)
        {
            return BlendColor(BackgroundColor, FrontColor, FrontColor.A);
        }
    }
}