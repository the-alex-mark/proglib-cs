using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ProgLib.Drawing.Drawing2D
{
    public static class CustomGraphicsPath
    {
        /// <summary>
        /// Добавляет прямоугольник с загруглёнными углами, заданный структурой <see cref="System.Drawing.Rectangle"/>.
        /// </summary>
        /// <param name="Radius"></param>
        /// <param name="Rectangle"></param>
        /// <returns></returns>
        public static System.Drawing.Drawing2D.GraphicsPath Superellipse(Radius Radius, Rectangle Rectangle)
        {
            System.Drawing.Drawing2D.GraphicsPath GP = new System.Drawing.Drawing2D.GraphicsPath();

            if (Radius.LeftTop != 0)
                GP.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, Radius.LeftTop * 2, Radius.LeftTop * 2), 180, 90);
            else GP.AddLine(new Point(Rectangle.X, Rectangle.Y), new Point(Rectangle.X, Rectangle.Y));

            if (Radius.RightTop != 0)
                GP.AddArc(new Rectangle(Rectangle.Width - Radius.RightTop * 2, Rectangle.Y, Radius.RightTop * 2, Radius.RightTop * 2), 270, 90);
            else GP.AddLine(new Point(Rectangle.Width, Rectangle.Y), new Point(Rectangle.Width, Rectangle.Y));

            if (Radius.RightBottom != 0)
                GP.AddArc(new Rectangle(Rectangle.Width - Radius.RightBottom * 2, Rectangle.Height - Radius.RightBottom * 2, Radius.RightBottom * 2, Radius.RightBottom * 2), 0, 90);
            else GP.AddLine(new Point(Rectangle.Width, Rectangle.Height), new Point(Rectangle.Width, Rectangle.Height));

            if (Radius.LeftBottom != 0)
                GP.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - Radius.LeftBottom * 2, Radius.LeftBottom * 2, Radius.LeftBottom * 2), 90, 90);
            else GP.AddLine(new Point(Rectangle.X, Rectangle.Height), new Point(Rectangle.X, Rectangle.Height));

            GP.CloseFigure();

            return GP;
        }

        /// <summary>
        /// Добавляет окружность по градусам от определённого градуса.
        /// </summary>
        /// <param name="Center"></param>
        /// <param name="StartAngle"></param>
        /// <param name="SweepAngle"></param>
        /// <param name="Radius"></param>
        /// <returns></returns>
        public static System.Drawing.Drawing2D.GraphicsPath EllipseByDegrees(Point Center, Int32 StartAngle, Int32 SweepAngle, Int32 Radius)
        {
            System.Drawing.Drawing2D.GraphicsPath GP = new System.Drawing.Drawing2D.GraphicsPath();
            GP.AddArc(new Rectangle(Center.X - Radius, Center.Y - Radius, Radius * 2, Radius * 2), StartAngle, SweepAngle);
            GP.AddLine(
                new Point(((int)Math.Cos(SweepAngle) * Radius + Center.X + Radius) - Radius, (int)Math.Sin(SweepAngle) * Radius + Center.Y),
                Center);

            GP.CloseFigure();
            return GP;
        }

        /// <summary>
        /// Добавляет окружность по градусам от определённого градуса.
        /// </summary>
        /// <param name="Rectangle"></param>
        /// <param name="StartAngle"></param>
        /// <param name="SweepAngle"></param>
        /// <returns></returns>
        public static System.Drawing.Drawing2D.GraphicsPath EllipseByDegrees(Rectangle Rectangle, Int32 StartAngle, Int32 SweepAngle)
        {
            Point Center = new Point(Rectangle.Width / 2, Rectangle.Height / 2);
            Int32 Radius = Rectangle.Width / 2;

            System.Drawing.Drawing2D.GraphicsPath GP = new System.Drawing.Drawing2D.GraphicsPath();
            GP.AddArc(new Rectangle(Center.X - Radius, Center.Y - Radius, Radius * 2, Radius * 2), StartAngle, SweepAngle);
            GP.AddLine(
                new Point(((int)Math.Cos(180) * Radius + Center.X + Radius) - Radius, (int)Math.Sin(180) * Radius + Center.Y),
                Center);

            GP.CloseFigure();
            return GP;
        }

        /// <summary>
        /// Смешивает цвета (значение переменной "Blend" должно быть от 0 до 255)
        /// </summary>
        /// <param name="BackgroundColor"></param>
        /// <param name="FrontColor"></param>
        /// <param name="Blend">Значение должно быть от 0 до 255</param>
        /// <returns></returns>
        public static Color BlendColor(Color BackgroundColor, Color FrontColor, Int32 Blend)
        {
            if (Enumerable.Range(0, 256).Contains(Blend))
            {
                Double Ratio = Blend / 255d;
                Int32 R = (int)((BackgroundColor.R * (1d - Ratio)) + (FrontColor.R * Ratio));
                Int32 G = (int)((BackgroundColor.G * (1d - Ratio)) + (FrontColor.G * Ratio));
                Int32 B = (int)((BackgroundColor.B * (1d - Ratio)) + (FrontColor.B * Ratio));

                return Color.FromArgb(R, G, B);
            }
            else
            {
                throw new Exception("Значение переменной \"Blend\" должно быть от 0 до 255");
            }
        }
    }
}
