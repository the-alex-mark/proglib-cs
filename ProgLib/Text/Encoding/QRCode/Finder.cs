using System;

namespace ProgLib.Text.Encoding.QRCode
{
    /// <summary>
    /// Класс поиска QR-кода
    /// </summary>
    internal class Finder
	{
	    // Horizontal scan
	    internal Int32 Row;
	    internal Int32 Col1;
	    internal Int32 Col2;
	    internal Double HModule;

	    // Vertical scan
	    internal Int32 Col;
	    internal Int32 Row1;
	    internal Int32 Row2;
	    internal Double VModule;

	    internal Double Distance;
	    internal Double ModuleSize;

        /// <summary>
        /// Конструктор при горизонтальном сканировании
        /// </summary>
        internal Finder(Int32 Row, Int32 Col1, Int32 Col2, Double HModule)
		{
		    this.Row = Row;
		    this.Col1 = Col1;
		    this.Col2 = Col2;
		    this.HModule = HModule;
		    Distance = Double.MaxValue;

		    return;
		}

	    /// <summary>
	    /// Совпадение при вертикальном сканировании
	    /// </summary>
	    internal void Match(Int32 Col, Int32 Row1, Int32 Row2, Double VModule)
		{
		    // Test if horizontal and vertical are not related
		    if (Col < Col1 || Col >= Col2 || Row < Row1 || Row >= Row2) return;

		    // Module sizes must be about the same
		    if (Math.Min(HModule, VModule) < Math.Max(HModule, VModule) * QRDecoder.MODULE_SIZE_DEVIATION) return;

            // Calculate distance
            Double DeltaX = Col - 0.5 * (Col1 + Col2);
            Double DeltaY = Row - 0.5 * (Row1 + Row2);
            Double Delta = Math.Sqrt(DeltaX * DeltaX + DeltaY * DeltaY);

		    // Distance between two points must be less than 2 pixels
		    if (Delta > QRDecoder.HOR_VERT_SCAN_MAX_DISTANCE) return;

		    // New result is better than last result
		    if (Delta < Distance)
			{
			    this.Col = Col;
			    this.Row1 = Row1;
			    this.Row2 = Row2;
			    this.VModule = VModule;
			    ModuleSize = 0.5 * (HModule + VModule);
			    Distance = Delta;
			}

		    return;
		}

        /// <summary>
        /// Горизонтальное и вертикальное сканирование перекрываются
        /// </summary>
        internal bool Overlap(Finder Other)
		{
		    return Other.Col1 < Col2 && Other.Col2 >= Col1 && Other.Row1 < Row2 && Other.Row2 >= Row1;
		}

        /// <summary>
        /// Finder в строку
        /// </summary>
        public override string ToString()
		{
		    if (Distance == Double.MaxValue)
			{
			    return String.Format("Finder: Row: {0}, Col1: {1}, Col2: {2}, HModule: {3:0.00}", Row, Col1, Col2, HModule);
			}

		    return String.Format("Finder: Row: {0}, Col: {1}, Module: {2:0.00}, Distance: {3:0.00}", Row, Col, ModuleSize, Distance);
		}
	}
}
