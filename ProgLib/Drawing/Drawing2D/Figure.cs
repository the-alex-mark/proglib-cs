using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ProgLib.Drawing.Drawing2D
{
    /// <summary>
    /// Предоставляет различные сложные фигуры.
    /// </summary>
    public static class Figure
    {
        /// <summary>
        /// Прямоугольник с закруглёнными углами.
        /// </summary>
        /// <param name="Radius">Величина округления углов (в пикселях)</param>
        /// <param name="Bounds">Прямоугольник, представляющий границы для изображаемого элемента</param>
        /// <returns></returns>
        public static GraphicsPath Superellipse(Radius Radius, Rectangle Bounds)
        {
            GraphicsPath GP = new GraphicsPath();
            Bounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.X + Bounds.Width, Bounds.Y + Bounds.Height);

            if (Radius.LeftTop != 0)
                GP.AddArc(new Rectangle(Bounds.X, Bounds.Y, Radius.LeftTop * 2, Radius.LeftTop * 2), 180, 90);
            else GP.AddLine(new Point(Bounds.X, Bounds.Y), new Point(Bounds.X, Bounds.Y));

            if (Radius.RightTop != 0)
                GP.AddArc(new Rectangle(Bounds.Width - Radius.RightTop * 2, Bounds.Y, Radius.RightTop * 2, Radius.RightTop * 2), 270, 90);
            else GP.AddLine(new Point(Bounds.Width, Bounds.Y), new Point(Bounds.Width, Bounds.Y));

            if (Radius.RightBottom != 0)
                GP.AddArc(new Rectangle(Bounds.Width - Radius.RightBottom * 2, Bounds.Height - Radius.RightBottom * 2, Radius.RightBottom * 2, Radius.RightBottom * 2), 0, 90);
            else GP.AddLine(new Point(Bounds.Width, Bounds.Height), new Point(Bounds.Width, Bounds.Height));

            if (Radius.LeftBottom != 0)
                GP.AddArc(new Rectangle(Bounds.X, Bounds.Height - Radius.LeftBottom * 2, Radius.LeftBottom * 2, Radius.LeftBottom * 2), 90, 90);
            else GP.AddLine(new Point(Bounds.X, Bounds.Height), new Point(Bounds.X, Bounds.Height));

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
        public static GraphicsPath EllipseByDegrees(Point Center, Int32 StartAngle, Int32 SweepAngle, Int32 Radius)
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
        public static GraphicsPath EllipseByDegrees(Rectangle Rectangle, Int32 StartAngle, Int32 SweepAngle)
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
        /// Смешвает два цвета.
        /// </summary>
        /// <param name="BackgroundColor"></param>
        /// <param name="FrontColor"></param>
        /// <param name="Blend">Величина смешивания (от 0 до 255)</param>
        /// <returns></returns>
        public static Color BlendColor(Color BackgroundColor, Color FrontColor, Int32 Blend)
        {
            if (Enumerable.Range(0, 256).Contains(Blend))
            {
                Double Ratio = Blend / 255D;
                Int32 R = (int)((BackgroundColor.R * (1D - Ratio)) + (FrontColor.R * Ratio));
                Int32 G = (int)((BackgroundColor.G * (1D - Ratio)) + (FrontColor.G * Ratio));
                Int32 B = (int)((BackgroundColor.B * (1D - Ratio)) + (FrontColor.B * Ratio));

                return Color.FromArgb(R, G, B);
            }
            else
            {
                throw new Exception("Значение переменной \"Blend\" должно быть от 0 до 255!");
            }
        }
    }
}
