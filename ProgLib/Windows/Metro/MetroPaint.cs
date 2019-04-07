using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Windows.Metro
{
    public class MetroPaint
    {

        public sealed class BorderColor
        {
            public static Color Form(Theme Theme)
            {
                if (Theme == Theme.Dark)
                    return Color.FromArgb(153, 153, 153);

                return Color.FromArgb(153, 153, 153);
            }

            public static class Button
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Hover(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(102, 102, 102);
                }

                public static Color Press(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(238, 238, 238);

                    return Color.FromArgb(51, 51, 51);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(109, 109, 109);

                    return Color.FromArgb(155, 155, 155);
                }
            }

            public static class CheckBox
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Hover(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(204, 204, 204);

                    return Color.FromArgb(51, 51, 51);
                }

                public static Color Press(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(85, 85, 85);

                    return Color.FromArgb(204, 204, 204);
                }
            }

            public static class ComboBox
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Hover(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(204, 204, 204);

                    return Color.FromArgb(51, 51, 51);
                }

                public static Color Press(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(85, 85, 85);

                    return Color.FromArgb(204, 204, 204);
                }
            }

            public static class ProgressBar
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Hover(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Press(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(109, 109, 109);

                    return Color.FromArgb(155, 155, 155);
                }
            }

            public static class TabControl
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Hover(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Press(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(109, 109, 109);

                    return Color.FromArgb(155, 155, 155);
                }
            }

            public static class Toggle
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Hover(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(204, 204, 204);

                    return Color.FromArgb(51, 51, 51);
                }

                public static Color Press(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(85, 85, 85);

                    return Color.FromArgb(204, 204, 204);
                }
            }
        }

        public sealed class BackColor
        {
            public static Color Form(Theme Theme)
            {
                if (Theme == Theme.Dark)
                    return Color.FromArgb(17, 17, 17);

                return Color.FromArgb(255, 255, 255);
            }

            public sealed class Button
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(34, 34, 34);

                    return Color.FromArgb(238, 238, 238);
                }

                public static Color Hover(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(102, 102, 102);
                }

                public static Color Press(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(238, 238, 238);

                    return Color.FromArgb(51, 51, 51);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(80, 80, 80);

                    return Color.FromArgb(204, 204, 204);
                }
            }

            public sealed class TrackBar
            {
                public sealed class Thumb
                {
                    public static Color Normal(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(153, 153, 153);

                        return Color.FromArgb(102, 102, 102);
                    }

                    public static Color Hover(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(204, 204, 204);

                        return Color.FromArgb(17, 17, 17);
                    }

                    public static Color Press(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(204, 204, 204);

                        return Color.FromArgb(17, 17, 17);
                    }

                    public static Color Disabled(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(85, 85, 85);

                        return Color.FromArgb(179, 179, 179);
                    }
                }

                public sealed class Bar
                {
                    public static Color Normal(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(204, 204, 204);
                    }

                    public static Color Hover(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(204, 204, 204);
                    }

                    public static Color Press(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(204, 204, 204);
                    }

                    public static Color Disabled(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(34, 34, 34);

                        return Color.FromArgb(230, 230, 230);
                    }
                }
            }

            public sealed class ScrollBar
            {
                public sealed class Thumb
                {
                    public static Color Normal(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(221, 221, 221);
                    }

                    public static Color Hover(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(204, 204, 204);

                        return Color.FromArgb(17, 17, 17);
                    }

                    public static Color Press(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(204, 204, 204);

                        return Color.FromArgb(17, 17, 17);
                    }

                    public static Color Disabled(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(221, 221, 221);
                    }
                }

                public sealed class Bar
                {
                    public static Color Normal(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Hover(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Press(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Disabled(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }
                }
            }

            public sealed class ProgressBar
            {
                public sealed class Bar
                {
                    public static Color Normal(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Hover(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Press(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Disabled(Theme Theme)
                    {
                        if (Theme == Theme.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(221, 221, 221);
                    }
                }
            }
        }

        public sealed class ForeColor
        {
            public sealed class Button
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(204, 204, 204);

                    return Color.FromArgb(0, 0, 0);
                }

                public static Color Hover(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(17, 17, 17);

                    return Color.FromArgb(255, 255, 255);
                }

                public static Color Press(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(17, 17, 17);

                    return Color.FromArgb(255, 255, 255);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(109, 109, 109);

                    return Color.FromArgb(136, 136, 136);
                }
            }

            public static Color Form(Theme Theme)
            {
                if (Theme == Theme.Dark)
                    return Color.FromArgb(255, 255, 255);

                return Color.FromArgb(0, 0, 0);
            }

            public sealed class Tile
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(255, 255, 255);

                    return Color.FromArgb(255, 255, 255);
                }

                public static Color Hover(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(255, 255, 255);

                    return Color.FromArgb(255, 255, 255);
                }

                public static Color Press(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(255, 255, 255);

                    return Color.FromArgb(255, 255, 255);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(209, 209, 209);

                    return Color.FromArgb(209, 209, 209);
                }
            }

            public sealed class Link
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(0, 0, 0);
                }

                public static Color Hover(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(93, 93, 93);

                    return Color.FromArgb(128, 128, 128);
                }

                public static Color Press(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(93, 93, 93);

                    return Color.FromArgb(128, 128, 128);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(51, 51, 51);

                    return Color.FromArgb(209, 209, 209);
                }
            }

            public sealed class Label
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(0, 0, 0);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(51, 51, 51);

                    return Color.FromArgb(209, 209, 209);
                }
            }

            public sealed class CheckBox
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(17, 17, 17);
                }

                public static Color Hover(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Press(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(93, 93, 93);

                    return Color.FromArgb(136, 136, 136);
                }
            }

            public sealed class ComboBox
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Hover(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(17, 17, 17);
                }

                public static Color Press(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(93, 93, 93);

                    return Color.FromArgb(136, 136, 136);
                }
            }

            public sealed class ProgressBar
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(0, 0, 0);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(51, 51, 51);

                    return Color.FromArgb(209, 209, 209);
                }
            }

            public sealed class TabControl
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(0, 0, 0);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(51, 51, 51);

                    return Color.FromArgb(209, 209, 209);
                }
            }

            public sealed class Toggle
            {
                public static Color Normal(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(17, 17, 17);
                }

                public static Color Hover(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Press(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Disabled(Theme Theme)
                {
                    if (Theme == Theme.Dark)
                        return Color.FromArgb(93, 93, 93);

                    return Color.FromArgb(136, 136, 136);
                }
            }
        }

        public sealed class ControlBox
        {
            public static class Hide
            {
                public static Image Normal(Theme Theme)
                {
                    Bitmap Hide = new Bitmap(25, 20);
                    using (Graphics G = Graphics.FromImage(Hide))
                    {
                        if (Theme == Theme.Dark)
                        {
                            G.Clear(Color.FromArgb(17, 17, 17));
                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(9, 14), new Point(16, 14));
                        }
                        else
                        {
                            G.Clear(Drawing.MetroColors.White);
                            G.DrawLine(new Pen(Drawing.MetroColors.Black, 2), new Point(9, 14), new Point(16, 14));
                        }
                    }

                    return Hide;
                }

                public static Image Hover(Theme Theme)
                {
                    Bitmap Hide = new Bitmap(25, 20);
                    using (Graphics G = Graphics.FromImage(Hide))
                    {
                        if (Theme == Theme.Dark)
                        {
                            G.Clear(Color.FromArgb(170, 170, 170));
                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(9, 14), new Point(16, 14));
                        }
                        else
                        {
                            G.Clear(Color.FromArgb(102, 102, 102));
                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(9, 14), new Point(16, 14));
                        }
                    }

                    return Hide;
                }

                public static Image Press(Theme Theme, Color StyleColor)
                {
                    Bitmap Hide = new Bitmap(25, 20);
                    using (Graphics G = Graphics.FromImage(Hide))
                    {
                        if (Theme == Theme.Dark)
                        {
                            G.Clear(StyleColor);
                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(9, 14), new Point(16, 14));
                        }
                        else
                        {
                            G.Clear(StyleColor);
                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(9, 14), new Point(16, 14));
                        }
                    }

                    return Hide;
                }
            }

            public static class Minimum
            {
                public static Image Normal(Theme Theme)
                {
                    Bitmap Minimum = new Bitmap(25, 20);
                    using (Graphics G = Graphics.FromImage(Minimum))
                    {
                        if (Theme == Theme.Dark)
                        {
                            G.Clear(Color.FromArgb(17, 17, 17));
                            G.DrawRectangle(new Pen(Drawing.MetroColors.White, 1), new Rectangle(8, 6, 8, 8));
                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(8, 7), new Point(16, 7));
                        }
                        else
                        {
                            G.Clear(Drawing.MetroColors.White);
                            G.DrawRectangle(new Pen(Drawing.MetroColors.Black, 1), new Rectangle(8, 6, 8, 8));
                            G.DrawLine(new Pen(Drawing.MetroColors.Black, 2), new Point(8, 7), new Point(16, 7));
                        }
                    }

                    return Minimum;
                }

                public static Image Hover(Theme Theme)
                {
                    Bitmap Minimum = new Bitmap(25, 20);
                    using (Graphics G = Graphics.FromImage(Minimum))
                    {
                        if (Theme == Theme.Dark)
                        {
                            G.Clear(Color.FromArgb(170, 170, 170));
                            G.DrawRectangle(new Pen(Drawing.MetroColors.White, 1), new Rectangle(8, 6, 8, 8));
                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(8, 7), new Point(16, 7));
                        }
                        else
                        {
                            G.Clear(Color.FromArgb(102, 102, 102));
                            G.DrawRectangle(new Pen(Drawing.MetroColors.White, 1), new Rectangle(8, 6, 8, 8));
                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(8, 7), new Point(16, 7));
                        }
                    }

                    return Minimum;
                }

                public static Image Press(Theme Theme, Color StyleColor)
                {
                    Bitmap Minimum = new Bitmap(25, 20);
                    using (Graphics G = Graphics.FromImage(Minimum))
                    {
                        if (Theme == Theme.Dark)
                        {
                            G.Clear(StyleColor);
                            G.DrawRectangle(new Pen(Drawing.MetroColors.White, 1), new Rectangle(8, 6, 8, 8));
                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(8, 7), new Point(16, 7));
                        }
                        else
                        {
                            G.Clear(StyleColor);
                            G.DrawRectangle(new Pen(Drawing.MetroColors.White, 1), new Rectangle(8, 6, 8, 8));
                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(8, 7), new Point(16, 7));
                        }
                    }

                    return Minimum;
                }
            }

            public static class Close
            {
                public static Image Normal(Theme Theme)
                {
                    Bitmap Close = new Bitmap(25, 20);
                    using (Graphics G = Graphics.FromImage(Close))
                    {
                        if (Theme == Theme.Dark)
                        {
                            G.Clear(Color.FromArgb(17, 17, 17));
                            G.SmoothingMode = SmoothingMode.AntiAlias;

                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(8, 6), new Point(16, 14));
                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(16, 6), new Point(8, 14));
                        }
                        else
                        {
                            G.Clear(Drawing.MetroColors.White);
                            G.SmoothingMode = SmoothingMode.AntiAlias;

                            G.DrawLine(new Pen(Drawing.MetroColors.Black, 2), new Point(8, 6), new Point(16, 14));
                            G.DrawLine(new Pen(Drawing.MetroColors.Black, 2), new Point(16, 6), new Point(8, 14));
                        }

                    }

                    return Close;
                }

                public static Image Hover(Theme Theme)
                {
                    Bitmap Close = new Bitmap(25, 20);
                    using (Graphics G = Graphics.FromImage(Close))
                    {
                        if (Theme == Theme.Dark)
                        {
                            G.Clear(Color.FromArgb(170, 170, 170));
                            G.SmoothingMode = SmoothingMode.AntiAlias;

                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(8, 6), new Point(16, 14));
                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(16, 6), new Point(8, 14));
                        }
                        else
                        {
                            G.Clear(Color.FromArgb(102, 102, 102));
                            G.SmoothingMode = SmoothingMode.AntiAlias;

                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(8, 6), new Point(16, 14));
                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(16, 6), new Point(8, 14));
                        }
                    }

                    return Close;
                }

                public static Image Press(Theme Theme, Color StyleColor)
                {
                    Bitmap Close = new Bitmap(25, 20);
                    using (Graphics G = Graphics.FromImage(Close))
                    {
                        if (Theme == Theme.Dark)
                        {
                            G.Clear(StyleColor);
                            G.SmoothingMode = SmoothingMode.AntiAlias;

                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(8, 6), new Point(16, 14));
                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(16, 6), new Point(8, 14));
                        }
                        else
                        {
                            G.Clear(StyleColor);
                            G.SmoothingMode = SmoothingMode.AntiAlias;

                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(8, 6), new Point(16, 14));
                            G.DrawLine(new Pen(Drawing.MetroColors.White, 2), new Point(16, 6), new Point(8, 14));
                        }
                    }

                    return Close;
                }
            }

            public static class Resize
            {
                public static Image Normal(Theme Theme)
                {
                    Bitmap Resize = new Bitmap(10, 10);
                    using (Graphics G = Graphics.FromImage(Resize))
                    {
                        if (Theme == Theme.Dark)
                        {
                            G.Clear(Color.FromArgb(17, 17, 17));

                            G.FillRectangle(new SolidBrush(Color.FromArgb(109, 109, 109)), new Rectangle(8, 0, 2, 2));
                            G.FillRectangle(new SolidBrush(Color.FromArgb(109, 109, 109)), new Rectangle(4, 4, 2, 2));
                            G.FillRectangle(new SolidBrush(Color.FromArgb(109, 109, 109)), new Rectangle(8, 4, 2, 2));
                            G.FillRectangle(new SolidBrush(Color.FromArgb(109, 109, 109)), new Rectangle(0, 8, 2, 2));
                            G.FillRectangle(new SolidBrush(Color.FromArgb(109, 109, 109)), new Rectangle(4, 8, 2, 2));
                            G.FillRectangle(new SolidBrush(Color.FromArgb(109, 109, 109)), new Rectangle(8, 8, 2, 2));
                        }
                        else
                        {
                            G.Clear(Drawing.MetroColors.White);

                            G.FillRectangle(new SolidBrush(Color.FromArgb(136, 136, 136)), new Rectangle(8, 0, 2, 2));
                            G.FillRectangle(new SolidBrush(Color.FromArgb(136, 136, 136)), new Rectangle(4, 4, 2, 2));
                            G.FillRectangle(new SolidBrush(Color.FromArgb(136, 136, 136)), new Rectangle(8, 4, 2, 2));
                            G.FillRectangle(new SolidBrush(Color.FromArgb(136, 136, 136)), new Rectangle(0, 8, 2, 2));
                            G.FillRectangle(new SolidBrush(Color.FromArgb(136, 136, 136)), new Rectangle(4, 8, 2, 2));
                            G.FillRectangle(new SolidBrush(Color.FromArgb(136, 136, 136)), new Rectangle(8, 8, 2, 2));
                        }

                    }

                    return Resize;
                }
            }
        }
    }
}