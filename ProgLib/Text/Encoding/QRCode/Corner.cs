using System;

namespace ProgLib.Text.Encoding.QRCode
{
    /// <summary>
    /// QR угол три искатели шаблон класса
    /// </summary>
    internal class Corner
	{
	    internal Finder TopLeftFinder;
	    internal Finder TopRightFinder;
	    internal Finder BottomLeftFinder;

	    internal Double TopLineDeltaX;
	    internal Double TopLineDeltaY;
	    internal Double TopLineLength;
	    internal Double LeftLineDeltaX;
	    internal Double LeftLineDeltaY;
	    internal Double LeftLineLength;

        /// <summary>
        /// QR-уголок конструктора
        /// </summary>
        /// <param name="TopLeftFinder"></param>
        /// <param name="TopRightFinder"></param>
        /// <param name="BottomLeftFinder"></param>
        private Corner(Finder TopLeftFinder, Finder TopRightFinder, Finder BottomLeftFinder)
	    {
		    // Save three finders
		    this.TopLeftFinder = TopLeftFinder;
		    this.TopRightFinder = TopRightFinder;
		    this.BottomLeftFinder = BottomLeftFinder;

		    // Top line slope
		    TopLineDeltaX = TopRightFinder.Col - TopLeftFinder.Col;
		    TopLineDeltaY = TopRightFinder.Row - TopLeftFinder.Row;

		    // Top line length
		    TopLineLength = Math.Sqrt(TopLineDeltaX * TopLineDeltaX + TopLineDeltaY * TopLineDeltaY);

		    // Left line slope
		    LeftLineDeltaX = BottomLeftFinder.Col - TopLeftFinder.Col;
		    LeftLineDeltaY = BottomLeftFinder.Row - TopLeftFinder.Row;

		    // Left line length
		    LeftLineLength = Math.Sqrt(LeftLineDeltaX * LeftLineDeltaX + LeftLineDeltaY * LeftLineDeltaY);
		    return;
	    }

        /// <summary>
        /// Проверить QR-угол на валидность
        /// </summary>
        /// <param name="TopLeftFinder"></param>
        /// <param name="TopRightFinder"></param>
        /// <param name="BottomLeftFinder"></param>
        /// <returns></returns>
        internal static Corner CreateCorner(Finder TopLeftFinder, Finder TopRightFinder, Finder BottomLeftFinder)
	    {
		    // Try all three possible permutation of three finders
		    for (int Index = 0; Index < 3; Index++)
		    {
			    // TestCorner runs three times to test all posibilities rotate top left, top right and bottom left
			    if (Index != 0)
				{
				    Finder Temp = TopLeftFinder;
				    TopLeftFinder = TopRightFinder;
				    TopRightFinder = BottomLeftFinder;
				    BottomLeftFinder = Temp;
				}

			    // Top line slope
			    Double TopLineDeltaX = TopRightFinder.Col - TopLeftFinder.Col;
                Double TopLineDeltaY = TopRightFinder.Row - TopLeftFinder.Row;

                // Left line slope
                Double LeftLineDeltaX = BottomLeftFinder.Col - TopLeftFinder.Col;
                Double LeftLineDeltaY = BottomLeftFinder.Row - TopLeftFinder.Row;

                // Top line length
                Double TopLineLength = Math.Sqrt(TopLineDeltaX * TopLineDeltaX + TopLineDeltaY * TopLineDeltaY);

                // Left line length
                Double LeftLineLength = Math.Sqrt(LeftLineDeltaX * LeftLineDeltaX + LeftLineDeltaY * LeftLineDeltaY);

			    // The short side must be at least 80% of the long side
			    if (Math.Min(TopLineLength, LeftLineLength) < QRDecoder.CORNER_SIDE_LENGTH_DEV * Math.Max(TopLineLength, LeftLineLength)) continue;

                // Top line vector
                Double TopLineSin = TopLineDeltaY / TopLineLength;
                Double TopLineCos = TopLineDeltaX / TopLineLength;

                // Rotate lines such that top line is parallel to x axis left line after rotation
                Double NewLeftX = TopLineCos * LeftLineDeltaX + TopLineSin * LeftLineDeltaY;
                Double NewLeftY = -TopLineSin * LeftLineDeltaX + TopLineCos * LeftLineDeltaY;

			    // New left line X should be zero (or between +/- 4 deg)
			    if (Math.Abs(NewLeftX / LeftLineLength) > QRDecoder.CORNER_RIGHT_ANGLE_DEV) continue;

			    // Swap top line with left line
			    if (NewLeftY < 0)
			    {
				    // Swap top left with bottom right
				    Finder TempFinder = TopRightFinder;
				    TopRightFinder = BottomLeftFinder;
				    BottomLeftFinder = TempFinder;
			    }

			    return new Corner(TopLeftFinder, TopRightFinder, BottomLeftFinder);
		    }

		    return null;
	    }
        
        /// <summary>
        /// Проверить QR-угол на валидность
        /// </summary>
        /// <returns></returns>
        internal Int32 InitialVersionNumber()
	    {
		    // Version number based on top line
		    Double TopModules = 7;

		    // top line is mostly horizontal
		    if (Math.Abs(TopLineDeltaX) >= Math.Abs(TopLineDeltaY))
		    {
			    TopModules += TopLineLength * TopLineLength /
                        (Math.Abs(TopLineDeltaX) * 0.5 * (TopLeftFinder.HModule + TopRightFinder.HModule));			
		    }
		    // Top line is mostly vertical
		    else
		    {
			    TopModules += TopLineLength * TopLineLength /
                        (Math.Abs(TopLineDeltaY) * 0.5 * (TopLeftFinder.VModule + TopRightFinder.VModule));			
		    }

		    // Version number based on left line
		    Double LeftModules = 7;

		    // Left line is mostly vertical
		    if (Math.Abs(LeftLineDeltaY) >= Math.Abs(LeftLineDeltaX))
		    {
			    LeftModules += LeftLineLength * LeftLineLength /
				    (Math.Abs(LeftLineDeltaY) * 0.5 * (TopLeftFinder.VModule + BottomLeftFinder.VModule));			
		    }
		    // Left line is mostly horizontal
		    else
			{
			    LeftModules += LeftLineLength * LeftLineLength /
				    (Math.Abs(LeftLineDeltaX) * 0.5 * (TopLeftFinder.HModule + BottomLeftFinder.HModule));			
			}

		    // Version (there is rounding in the calculation)
		    Int32 Version = ((int) Math.Round(0.5 * (TopModules + LeftModules)) - 15) / 4;

		    // Not a valid corner
		    if(Version < 1 || Version > 40) throw new ApplicationException("Угол недопустим (номер версии должен быть от 1 до 40)");

		    // Exit with version number
		    return Version;
		}
	}
}
