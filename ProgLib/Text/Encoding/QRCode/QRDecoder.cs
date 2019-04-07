using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace ProgLib.Text.Encoding.QRCode
{
    public class QRDecoder : QRCode
	{
	    internal Int32 ImageWidth;
	    internal Int32 ImageHeight;
	    internal Boolean[,] BlackWhiteImage;
	    internal List<Finder> FinderList;
	    internal List<Finder> AlignList;
	    internal List<Byte[]> DataArrayList;

	    internal Boolean Trans4Mode;

        // Коэффициенты преобразования из QR-модулей в пиксели изображения
        internal Double Trans3a;
	    internal Double Trans3b;
	    internal Double Trans3c;
	    internal Double Trans3d;
	    internal Double Trans3e;
	    internal Double Trans3f;

        // Матрица преобразования на основе трех пальцев плюс еще одна точка
        internal Double Trans4a;
	    internal Double Trans4b;
	    internal Double Trans4c;
	    internal Double Trans4d;
	    internal Double Trans4e;
	    internal Double Trans4f;
	    internal Double Trans4g;
	    internal Double Trans4h;

	    internal const Double SIGNATURE_MAX_DEVIATION = 0.25;
	    internal const Double HOR_VERT_SCAN_MAX_DISTANCE = 2.0;
	    internal const Double MODULE_SIZE_DEVIATION = 0.5; // 0.75;
	    internal const Double CORNER_SIDE_LENGTH_DEV = 0.8;
	    internal const Double CORNER_RIGHT_ANGLE_DEV = 0.25; // about Sin(4 deg)
	    internal const Double ALIGNMENT_SEARCH_AREA = 0.3;
        
        /// <summary>
        /// Конструктор
        /// </summary>
	    public QRDecoder() {}
        
        /// <summary>
        /// Декодировать QRCode  булевой матрицы
        /// </summary>
        /// <param name="InputImage"></param>
        /// <returns></returns>
        public Byte[][] ImageDecoder(Bitmap	InputImage)
	    {
		    Int32 Start;

		    try
		    {
                // Пустой вывод строки данных
                DataArrayList = new List<Byte[]>();
                
                // Сохранение размера изображения
                ImageWidth = InputImage.Width;
			    ImageHeight = InputImage.Height;
                
			    Start = Environment.TickCount;

                // Преобразование входного изображения в черно-белое логическое изображениепреобразование входного изображения в черно-белое логическое изображение
                if (!ConvertImageToBlackAndWhite(InputImage)) return null;
            
                // Горизонтальный поиск искателей
                if (!HorizontalFindersSearch()) return null;
            
                // Вертикальный поиск искателей
                VerticalFindersSearch();
                
                // Удаление неиспользуемых искателей
                if (!RemoveUnusedFinders()) return null;
		    }
		    catch { return null; }
            
            // Поиск всех возможных трёх вариантов поиска
            Int32 Index1End = FinderList.Count - 2;
            Int32 Index2End = FinderList.Count - 1;
            Int32 Index3End = FinderList.Count;

		    for (int Index1 = 0; Index1 < Index1End; Index1++)
            {
                for (int Index2 = Index1 + 1; Index2 < Index2End; Index2++)
                {
                    for (int Index3 = Index2 + 1; Index3 < Index3End; Index3++)
                    {
                        try
                        {
                            // Поиск трёх искателей, расположенных в форме L
                            Corner Corner = Corner.CreateCorner(FinderList[Index1], FinderList[Index2], FinderList[Index3]);

                            // Недопустимый угол
                            if (Corner == null) continue;

                            // Получение информации о угле (версия, код ошибки и маска)
                            if (!GetQRCodeCornerInfo(Corner)) continue;

                            // декодирование угла, используя три пальца
                            if (DecodeQRCodeCorner(Corner)) continue;
                            
                            // QRCode Версия 1 имеет метку (другими словами расшифровать не удалось)
                            if (QRCodeVersion == 1) continue;

                            // найти внизу справа метку
                            if (!FindAlignmentMark(Corner)) continue;
                            
                            // Расшифровка, используя 4 очка
                            foreach (Finder Align in AlignList)
                            {
                                // Вычисления преобразований на основе 3 Finders, а внизу справа метку 
                                SetTransMatrix(Corner, Align.Row, Align.Col);

                                // Декодируйте угол тремя пальцами и одной меткой выравнивания
                                if (DecodeQRCodeCorner(Corner)) break;
                            }
                        }
                        catch { continue; }
                    }
                }
            }

            // Не найден выход
            if (DataArrayList.Count == 0) { return null; }

            // Успешный выход
            return DataArrayList.ToArray();
		}

        /// <summary>
        /// Результат форматирования для отображения
        /// </summary>
        /// <param name="DataByteArray"></param>
        /// <returns></returns>
        public static string QRCodeResult(Byte[][] DataByteArray)
		{
            // Не QRCode
            if (DataByteArray == null) return string.Empty;

            // Изображение имеет один QRCode
            if (DataByteArray.Length == 1) return ByteArrayToStr(DataByteArray[0]);

            // Изображение имеет более одного QRCode
            StringBuilder Str = new StringBuilder();
		    for (int Index = 0; Index < DataByteArray.Length; Index++)
			{
			    if (Index != 0) Str.Append("\r\n");
			    Str.AppendFormat("QR Code {0}\r\n", Index + 1);
			    Str.Append(ByteArrayToStr(DataByteArray[Index]));
			}

		    return Str.ToString();
		}
        
        /// <summary>
        /// Преобразовывает изображение в черно-белую булеву матрицу
        /// </summary>
        /// <param name="InputImage"></param>
        /// <returns></returns>
        internal Boolean ConvertImageToBlackAndWhite(Bitmap InputImage)
		{
            // Биты изображения замка
            BitmapData BitmapData = InputImage.LockBits(new Rectangle(0, 0, ImageWidth, ImageHeight), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            // Адрес первой строки
            IntPtr BitArrayPtr = BitmapData.Scan0;

            // Длина в байтах одной строки сканирования
            Int32 ScanLineWidth = BitmapData.Stride;
		    if (ScanLineWidth < 0) { return false; }

            // Всего байт изображения
            Int32 TotalBytes = ScanLineWidth * ImageHeight;
		    Byte[] BitmapArray = new Byte[TotalBytes];

            // Скопируйте значения RGB в массив.
            Marshal.Copy(BitArrayPtr, BitmapArray, 0, TotalBytes);

            // Разблокировать изображение
            InputImage.UnlockBits(BitmapData);

            // Выделить серое изображение
            Byte[,] GrayImage = new Byte[ImageHeight, ImageWidth];
            Int32[] GrayLevel = new Int32[256];

            // Преобразовать в серый
            Int32 Delta = ScanLineWidth - 3 * ImageWidth;
            Int32 BitmapPtr = 0;
		    for (int Row = 0; Row < ImageHeight; Row++)
			{
			    for (int Col = 0; Col < ImageWidth; Col++)
				{
                    Int32 Module = (30 * BitmapArray[BitmapPtr] + 59 * BitmapArray[BitmapPtr + 1] + 11 * BitmapArray[BitmapPtr + 2]) / 100;
				    GrayLevel[Module]++;
				    GrayImage[Row, Col] = (byte)Module;
				    BitmapPtr += 3;
				}
			    BitmapPtr += Delta;
			}

            // Серый уровень отсечки между Черным и белым
            Int32 LevelStart;
            Int32 LevelEnd;
		    for (LevelStart = 0; LevelStart < 256 && GrayLevel[LevelStart] == 0; LevelStart++);
		    for (LevelEnd = 255; LevelEnd >= LevelStart && GrayLevel[LevelEnd] == 0; LevelEnd--);
		    LevelEnd++;
		    if (LevelEnd - LevelStart < 2) { return false; }

            Int32 CutoffLevel = (LevelStart + LevelEnd) / 2;

            // создать логическое изображение white = false, black = true
            BlackWhiteImage = new bool[ImageHeight, ImageWidth];
		    for(int Row = 0; Row < ImageHeight; Row++)
			    for(int Col = 0; Col < ImageWidth; Col++)
				    BlackWhiteImage[Row, Col] = GrayImage[Row, Col] < CutoffLevel;
            
            // Выход
            return true;
		}
        
        /// <summary>
        /// Поиск по строкам для шлакоблоков
        /// </summary>
        /// <returns></returns>
        internal Boolean HorizontalFindersSearch()
		{
            // Создать пустой список искателей
            FinderList = new List<Finder>();

            // Поиск шаблонов finder
            Int32[] ColPos = new Int32[ImageWidth + 1];
            Int32 PosPtr = 0;

            // Сканирование по одной строке за раз
            for (int Row = 0; Row < ImageHeight; Row++)
			{
                // Ищите первый черный пиксель
                Int32 Col;
			    for (Col = 0; Col < ImageWidth && !BlackWhiteImage[Row, Col]; Col++);
			    if (Col == ImageWidth) continue;

                // Первый черный
                PosPtr = 0;
			    ColPos[PosPtr++] = Col;

                // Петля для пар
                for (;;)
				{
                    // Ищи рядом белый
                    // Если черный все пути к краю, рядом белый через край
                    for (; Col < ImageWidth && BlackWhiteImage[Row, Col]; Col++);
				    ColPos[PosPtr++] = Col;
				    if (Col == ImageWidth) break;

                    // Ищите следующий черный
                    for (; Col < ImageWidth && !BlackWhiteImage[Row, Col]; Col++);
				    if (Col == ImageWidth) break;
				    ColPos[PosPtr++] = Col;
				}

                // У нас должно быть не менее 6 позиций
                if (PosPtr < 6) continue;

                // Массив длины сборки
                Int32 PosLen = PosPtr - 1;
                Int32[] Len = new Int32[PosLen];
			    for (int Ptr = 0; Ptr < PosLen; Ptr++) Len[Ptr] = ColPos[Ptr + 1] - ColPos[Ptr];

                // Тестовая подпись
                Int32 SigLen = PosPtr - 5;
			    for (int SigPtr = 0; SigPtr < SigLen; SigPtr += 2)
				{
				    if(TestFinderSig(ColPos, Len, SigPtr, out double ModuleSize))
					    FinderList.Add(new Finder(Row, ColPos[SigPtr + 2], ColPos[SigPtr + 3], ModuleSize));
				}
			}

            // Искатели не найдены
            if (FinderList.Count < 3) { return false; }

		    // Выход
		    return true;
		}
        
        /// <summary>
        /// Поиск блоков выравнивания по строкам
        /// </summary>
        /// <param name="AreaLeft"></param>
        /// <param name="AreaTop"></param>
        /// <param name="AreaWidth"></param>
        /// <param name="AreaHeight"></param>
        /// <returns></returns>
        internal Boolean HorizontalAlignmentSearch(Int32 AreaLeft, Int32 AreaTop, Int32 AreaWidth, Int32 AreaHeight)
		{
            // Создать пустой список искателей
            AlignList = new List<Finder>();

            // Поиск шаблонов finder
            Int32[] ColPos = new Int32[AreaWidth + 1];
            Int32 PosPtr = 0;

            // Область справа и снизу
            Int32 AreaRight = AreaLeft + AreaWidth;
            Int32 AreaBottom = AreaTop + AreaHeight;

            // Сканирование по одной строке за раз
            for (int Row = AreaTop; Row < AreaBottom; Row++)
			{
                // Ищите первый черный пиксель
                Int32 Col;
			    for (Col = AreaLeft; Col < AreaRight && !BlackWhiteImage[Row, Col]; Col++);
			    if (Col == AreaRight) continue;

                // Первый черный
                PosPtr = 0;
			    ColPos[PosPtr++] = Col;

                // Петля для пар
                for (;;)
				{
                    // Ищи рядом белый
                    // Если черный все пути к краю, рядом белый через край
                    for (; Col < AreaRight && BlackWhiteImage[Row, Col]; Col++);
				    ColPos[PosPtr++] = Col;
				    if (Col == AreaRight) break;

                    // Ищите следующий черный
                    for (; Col < AreaRight && !BlackWhiteImage[Row, Col]; Col++);
				    if (Col == AreaRight) break;
				    ColPos[PosPtr++] = Col;
				}

                // У нас должно быть не менее 6 позиций
                if (PosPtr < 6) continue;

                // Массив длины сборки
                Int32 PosLen = PosPtr - 1;
                Int32[] Len = new Int32[PosLen];
			    for (int Ptr = 0; Ptr < PosLen; Ptr++) Len[Ptr] = ColPos[Ptr + 1] - ColPos[Ptr];

                // Тестовая подпись
                Int32 SigLen = PosPtr - 5;
			    for(int SigPtr = 0; SigPtr < SigLen; SigPtr += 2)
				{
				    if(TestAlignSig(ColPos, Len, SigPtr, out double ModuleSize))
					    AlignList.Add(new Finder(Row, ColPos[SigPtr + 2], ColPos[SigPtr + 3], ModuleSize));
				}
			}

		    // Выход
		    return AlignList.Count != 0;
		}

	////////////////////////////////////////////////////////////////////
	// search column by column for finders blocks
	////////////////////////////////////////////////////////////////////

	internal void VerticalFindersSearch()
		{
		// active columns
		bool[] ActiveColumn = new bool[ImageWidth];
		foreach(Finder HF in FinderList)
			{
			for(int Col = HF.Col1; Col < HF.Col2; Col++) ActiveColumn[Col] = true;
			}

		// look for finder patterns
		int[] RowPos = new int[ImageHeight + 1];
		int PosPtr = 0;

		// scan one column at a time
		for(int Col = 0; Col < ImageWidth; Col++)
			{
			// not active column
			if(!ActiveColumn[Col]) continue;

			// look for first black pixel
			int Row;
			for(Row = 0; Row < ImageHeight && !BlackWhiteImage[Row, Col]; Row++);
			if(Row == ImageWidth) continue;

			// first black
			PosPtr = 0;
			RowPos[PosPtr++] = Row;

			// loop for pairs
			for(;;)
				{
				// look for next white
				// if black is all the way to the edge, set next white after the edge
				for(; Row < ImageHeight && BlackWhiteImage[Row, Col]; Row++);
				RowPos[PosPtr++] = Row;
				if(Row == ImageHeight) break;

				// look for next black
				for(; Row < ImageHeight && !BlackWhiteImage[Row, Col]; Row++);
				if(Row == ImageHeight) break;
				RowPos[PosPtr++] = Row;
				}

			// we must have at least 6 positions
			if(PosPtr < 6) continue;

			// build length array
			int PosLen = PosPtr - 1;
			int[] Len = new int[PosLen];
			for(int Ptr = 0; Ptr < PosLen; Ptr++) Len[Ptr] = RowPos[Ptr + 1] - RowPos[Ptr];

			// test signature
			int SigLen = PosPtr - 5;
			for(int SigPtr = 0; SigPtr < SigLen; SigPtr += 2)
				{
				if(!TestFinderSig(RowPos, Len, SigPtr, out double ModuleSize)) continue;
				foreach(Finder HF in FinderList)
					{
					HF.Match(Col, RowPos[SigPtr + 2], RowPos[SigPtr + 3], ModuleSize);
					}
				}
			}

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// search column by column for finders blocks
	////////////////////////////////////////////////////////////////////

	internal void VerticalAlignmentSearch
			(
			int AreaLeft,
			int AreaTop,
			int AreaWidth,
			int AreaHeight
			)
		{
		// active columns
		bool[] ActiveColumn = new bool[AreaWidth];
		foreach(Finder HF in AlignList)
			{
			for(int Col = HF.Col1; Col < HF.Col2; Col++) ActiveColumn[Col - AreaLeft] = true;
			}

		// look for finder patterns
		int[] RowPos = new int[AreaHeight + 1];
		int PosPtr = 0;

		// area right and bottom
		int AreaRight = AreaLeft + AreaWidth;
		int AreaBottom = AreaTop + AreaHeight;

		// scan one column at a time
		for(int Col = AreaLeft; Col < AreaRight; Col++)
			{
			// not active column
			if(!ActiveColumn[Col - AreaLeft]) continue;

			// look for first black pixel
			int Row;
			for(Row = AreaTop; Row < AreaBottom && !BlackWhiteImage[Row, Col]; Row++);
			if(Row == AreaBottom) continue;

			// first black
			PosPtr = 0;
			RowPos[PosPtr++] = Row;

			// loop for pairs
			for(;;)
				{
				// look for next white
				// if black is all the way to the edge, set next white after the edge
				for(; Row < AreaBottom && BlackWhiteImage[Row, Col]; Row++);
				RowPos[PosPtr++] = Row;
				if(Row == AreaBottom) break;

				// look for next black
				for(; Row < AreaBottom && !BlackWhiteImage[Row, Col]; Row++);
				if(Row == AreaBottom) break;
				RowPos[PosPtr++] = Row;
				}

			// we must have at least 6 positions
			if(PosPtr < 6) continue;

			// build length array
			int PosLen = PosPtr - 1;
			int[] Len = new int[PosLen];
			for(int Ptr = 0; Ptr < PosLen; Ptr++) Len[Ptr] = RowPos[Ptr + 1] - RowPos[Ptr];

			// test signature
			int SigLen = PosPtr - 5;
			for(int SigPtr = 0; SigPtr < SigLen; SigPtr += 2)
				{
				if(!TestAlignSig(RowPos, Len, SigPtr, out double ModuleSize)) continue;
				foreach(Finder HF in AlignList)
					{
					HF.Match(Col, RowPos[SigPtr + 2], RowPos[SigPtr + 3], ModuleSize);
					}
				}
			}

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// search column by column for finders blocks
	////////////////////////////////////////////////////////////////////

	internal bool RemoveUnusedFinders()
		{
		// remove all entries without a match
		for(int Index = 0; Index < FinderList.Count; Index++)
			{
			if(FinderList[Index].Distance == double.MaxValue)
				{
				FinderList.RemoveAt(Index);
				Index--;
				}
			}

		// list is now empty or has less than three finders
		if(FinderList.Count < 3)
			{
			return false;
			}

		// keep best entry for each overlapping area
		for(int Index = 0; Index < FinderList.Count; Index++)
			{
			Finder Finder = FinderList[Index];
			for(int Index1 = Index + 1; Index1 < FinderList.Count; Index1++)
				{
				Finder Finder1 = FinderList[Index1];
				if(!Finder.Overlap(Finder1)) continue;
				if(Finder1.Distance < Finder.Distance)
					{
					Finder = Finder1;
					FinderList[Index] = Finder;
					}
				FinderList.RemoveAt(Index1);
				Index1--;
				}
			}

		// list is now empty or has less than three finders
		if(FinderList.Count < 3)
			{
			return false;
			}

		// exit
		return true;
		}

	////////////////////////////////////////////////////////////////////
	// search column by column for finders blocks
	////////////////////////////////////////////////////////////////////

	internal bool RemoveUnusedAlignMarks()
		{
		// remove all entries without a match
		for(int Index = 0; Index < AlignList.Count; Index++)
			{
			if(AlignList[Index].Distance == double.MaxValue)
				{
				AlignList.RemoveAt(Index);
				Index--;
				}
			}

		// keep best entry for each overlapping area
		for(int Index = 0; Index < AlignList.Count; Index++)
			{
			Finder Finder = AlignList[Index];
			for(int Index1 = Index + 1; Index1 < AlignList.Count; Index1++)
				{
				Finder Finder1 = AlignList[Index1];
				if(!Finder.Overlap(Finder1)) continue;
				if(Finder1.Distance < Finder.Distance)
					{
					Finder = Finder1;
					AlignList[Index] = Finder;
					}
				AlignList.RemoveAt(Index1);
				Index1--;
				}
			}

		// exit
		return AlignList.Count != 0;
		}

	////////////////////////////////////////////////////////////////////
	// test finder signature 1 1 3 1 1
	////////////////////////////////////////////////////////////////////

	internal bool TestFinderSig
			(
			int[] Pos,
			int[] Len,
			int Index,
			out double Module
			)
		{
		Module = (Pos[Index + 5] - Pos[Index]) / 7.0;
		double MaxDev = SIGNATURE_MAX_DEVIATION * Module;
		if(Math.Abs(Len[Index] - Module) > MaxDev) return false;
		if(Math.Abs(Len[Index + 1] - Module) > MaxDev) return false;
		if(Math.Abs(Len[Index + 2] - 3 * Module) > MaxDev) return false;
		if(Math.Abs(Len[Index + 3] - Module) > MaxDev) return false;
		if(Math.Abs(Len[Index + 4] - Module) > MaxDev) return false;
		return true;		
		}

	////////////////////////////////////////////////////////////////////
	// test alignment signature n 1 1 1 n
	////////////////////////////////////////////////////////////////////

	internal bool TestAlignSig
			(
			int[] Pos,
			int[] Len,
			int Index,
			out double Module
			)
		{
		Module = (Pos[Index + 4] - Pos[Index + 1]) / 3.0;
		double MaxDev = SIGNATURE_MAX_DEVIATION * Module;
		if(Len[Index] < Module -  MaxDev) return false;
		if(Math.Abs(Len[Index + 1] - Module) > MaxDev) return false;
		if(Math.Abs(Len[Index + 2] - Module) > MaxDev) return false;
		if(Math.Abs(Len[Index + 3] - Module) > MaxDev) return false;
		if(Len[Index + 4] < Module - MaxDev) return false;
		return true;		
		}

	////////////////////////////////////////////////////////////////////
	// Build corner list
	////////////////////////////////////////////////////////////////////

	internal List<Corner> BuildCornerList()
		{
		// empty list
		List<Corner> Corners = new List<Corner>();

		// look for all possible 3 finder patterns
		int Index1End = FinderList.Count - 2;
		int Index2End = FinderList.Count - 1;
		int Index3End = FinderList.Count;
		for(int Index1 = 0; Index1 < Index1End; Index1++)
			for(int Index2 = Index1 + 1; Index2 < Index2End; Index2++)
				for(int Index3 = Index2 + 1; Index3 < Index3End; Index3++)
			{
			// find 3 finders arranged in L shape
			Corner Corner = Corner.CreateCorner(FinderList[Index1], FinderList[Index2], FinderList[Index3]);

			// add corner to list
			if(Corner != null) Corners.Add(Corner);
			}

		// exit
		return Corners.Count == 0 ? null : Corners;
		}

	////////////////////////////////////////////////////////////////////
	// Get QR Code corner info
	////////////////////////////////////////////////////////////////////

	internal bool GetQRCodeCornerInfo
			(
			Corner Corner
			)
		{
		try
			{
			// initial version number
			QRCodeVersion = Corner.InitialVersionNumber();

			// qr code dimension
			QRCodeDimension = 17 + 4 * QRCodeVersion;
            
			// set transformation matrix
			SetTransMatrix(Corner);

			// if version number is 7 or more, get version code
			if(QRCodeVersion >= 7)
				{
				int Version = GetVersionOne();
				if(Version == 0)
					{
					Version = GetVersionTwo();
					if(Version == 0) return false;
					}

				// QR Code version number is different than initial version
				if(Version != QRCodeVersion)
					{
					// initial version number and dimension
					QRCodeVersion = Version;

					// qr code dimension
					QRCodeDimension = 17 + 4 * QRCodeVersion;
                    
					// set transformation matrix
					SetTransMatrix(Corner);
					}
				}

			// get format info arrays
			int FormatInfo = GetFormatInfoOne();
			if(FormatInfo < 0)
				{
				FormatInfo = GetFormatInfoTwo();
				if(FormatInfo < 0) return false;
				}

			// set error correction code and mask code
			ErrorCorrection = FormatInfoToErrCode(FormatInfo >> 3);
			MaskCode = FormatInfo & 7;

			// successful exit
			return true;
			}
            
		catch (Exception Ex)
			{
			// failed exit
			return false;
			}
		}

	////////////////////////////////////////////////////////////////////
	// Search for QR Code version
	////////////////////////////////////////////////////////////////////

	internal bool DecodeQRCodeCorner
			(
			Corner Corner
			)
		{
		try
			{
			// create base matrix
			BuildBaseMatrix();

			// create data matrix and test fixed modules
			ConvertImageToMatrix();

			// based on version and format information
			// set number of data and error correction codewords length  
			SetDataCodewordsLength();

			// apply mask as per get format information step
			ApplyMask(MaskCode);

			// unload data from binary matrix to byte format
			UnloadDataFromMatrix();

			// restore blocks (undo interleave)
			RestoreBlocks();

			// calculate error correction
			// in case of error try to correct it
			CalculateErrorCorrection();

			// decode data
			byte[] DataArray = DecodeData();
			DataArrayList.Add(DataArray);
            

			// successful exit
			return true;
			}
            
		catch (Exception Ex)
			{
			// failed exit
			return false;
			}
		}

	internal void SetTransMatrix
			(
			Corner	Corner
			)
		{
		// save
		int BottomRightPos = QRCodeDimension - 4;

		// transformation matrix based on three finders
		double[,] Matrix1 = new double[3, 4];
		double[,] Matrix2 = new double[3, 4];

		// build matrix 1 for horizontal X direction
		Matrix1[0, 0] = 3;
		Matrix1[0, 1] = 3;
		Matrix1[0, 2] = 1;
		Matrix1[0, 3] = Corner.TopLeftFinder.Col;

		Matrix1[1, 0] = BottomRightPos;
		Matrix1[1, 1] = 3;
		Matrix1[1, 2] = 1;
		Matrix1[1, 3] = Corner.TopRightFinder.Col;

		Matrix1[2, 0] = 3;
		Matrix1[2, 1] = BottomRightPos;
		Matrix1[2, 2] = 1;
		Matrix1[2, 3] = Corner.BottomLeftFinder.Col;

		// build matrix 2 for Vertical Y direction
		Matrix2[0, 0] = 3;
		Matrix2[0, 1] = 3;
		Matrix2[0, 2] = 1;
		Matrix2[0, 3] = Corner.TopLeftFinder.Row;

		Matrix2[1, 0] = BottomRightPos;
		Matrix2[1, 1] = 3;
		Matrix2[1, 2] = 1;
		Matrix2[1, 3] = Corner.TopRightFinder.Row;

		Matrix2[2, 0] = 3;
		Matrix2[2, 1] = BottomRightPos;
		Matrix2[2, 2] = 1;
		Matrix2[2, 3] = Corner.BottomLeftFinder.Row;

		// solve matrix1
		SolveMatrixOne(Matrix1);
		Trans3a = Matrix1[0, 3];
		Trans3c = Matrix1[1, 3];
		Trans3e = Matrix1[2, 3];

		// solve matrix2
		SolveMatrixOne(Matrix2);
		Trans3b = Matrix2[0, 3];
		Trans3d = Matrix2[1, 3];
		Trans3f = Matrix2[2, 3];

		// reset trans 4 mode
		Trans4Mode = false;
		return;
		}

	internal void SolveMatrixOne
			(
			double[,] Matrix
			)
		{
		for(int Row = 0; Row < 3; Row++)
			{
		    // If the element is zero, make it non zero by adding another row
		    if(Matrix[Row, Row] == 0)
			    {
		        int Row1;
				for (Row1 = Row + 1; Row1 < 3 && Matrix[Row1, Row] == 0; Row1++);
			    if (Row1 == 3) throw new ApplicationException("Solve linear equations failed");

                for(int Col = Row; Col < 4; Col++) Matrix[Row, Col] += Matrix[Row1, Col];
	            }

			// make the diagonal element 1.0
			for(int Col = 3; Col > Row; Col--) Matrix[Row, Col] /= Matrix[Row, Row];

			// subtract current row from next rows to eliminate one value
			for(int Row1 = Row + 1; Row1 < 3; Row1++)
				{
				for (int Col = 3; Col > Row; Col--) Matrix[Row1, Col] -= Matrix[Row, Col] * Matrix[Row1, Row];
				}
			}

		// go up from last row and eliminate all solved values
	    Matrix[1, 3] -= Matrix[1, 2] * Matrix[2, 3];
	    Matrix[0, 3] -= Matrix[0, 2] * Matrix[2, 3];
	    Matrix[0, 3] -= Matrix[0, 1] * Matrix[1, 3];
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Get image pixel color
	////////////////////////////////////////////////////////////////////

	internal bool GetModule
			(
			int		Row,
			int		Col
			)
		{
		// get module based on three finders
		if(!Trans4Mode)
			{
			int Trans3Col = (int) Math.Round(Trans3a * Col + Trans3c * Row + Trans3e, 0, MidpointRounding.AwayFromZero);
			int Trans3Row = (int) Math.Round(Trans3b * Col + Trans3d * Row + Trans3f, 0, MidpointRounding.AwayFromZero);
			return BlackWhiteImage[Trans3Row, Trans3Col];
			}

		// get module based on three finders plus one alignment mark
		double W = Trans4g * Col + Trans4h * Row + 1.0;
		int Trans4Col = (int) Math.Round((Trans4a * Col + Trans4b * Row + Trans4c) / W, 0, MidpointRounding.AwayFromZero);
		int Trans4Row = (int) Math.Round((Trans4d * Col + Trans4e * Row + Trans4f) / W, 0, MidpointRounding.AwayFromZero);
		return BlackWhiteImage[Trans4Row, Trans4Col];
		}

	////////////////////////////////////////////////////////////////////
	// search row by row for finders blocks
	////////////////////////////////////////////////////////////////////

	internal bool FindAlignmentMark
			(
			Corner Corner
			)
		{
		// alignment mark estimated position
		int AlignRow = QRCodeDimension - 7;
		int AlignCol = QRCodeDimension - 7;
		int ImageCol = (int) Math.Round(Trans3a * AlignCol + Trans3c * AlignRow + Trans3e, 0, MidpointRounding.AwayFromZero);
		int ImageRow = (int) Math.Round(Trans3b * AlignCol + Trans3d * AlignRow + Trans3f, 0, MidpointRounding.AwayFromZero);
        
		// search area
		int Side = (int) Math.Round(ALIGNMENT_SEARCH_AREA * (Corner.TopLineLength + Corner.LeftLineLength), 0, MidpointRounding.AwayFromZero);
		
		int AreaLeft = ImageCol - Side / 2;
		int AreaTop = ImageRow - Side / 2;
		int AreaWidth = Side;
		int AreaHeight = Side;

		#if DEBUGEX
		DisplayBottomRightCorder(AreaLeft, AreaTop, AreaWidth, AreaHeight);
		#endif

		// horizontal search for finders
		if(!HorizontalAlignmentSearch(AreaLeft, AreaTop, AreaWidth, AreaHeight)) return false;

		// vertical search for finders
		VerticalAlignmentSearch(AreaLeft, AreaTop, AreaWidth, AreaHeight);

		// remove unused alignment entries
		if(!RemoveUnusedAlignMarks()) return false;

		// successful exit
		return true;
		}

	internal void DisplayBottomRightCorder
			(
			int AreaLeft,
			int AreaTop,
			int AreaWidth,
			int AreaHeight
			)
		{
		SolidBrush BrushWhite = new SolidBrush(Color.White);
		SolidBrush BrushBlack = new SolidBrush(Color.Black);
		const int Module = 4;

		// create picture object and make it white
		Bitmap ImageBitmap = new Bitmap(Module * AreaWidth, Module * AreaHeight);
		Graphics Graphics = Graphics.FromImage(ImageBitmap);
		Graphics.FillRectangle(BrushWhite, 0, 0, Module * AreaWidth, Module * AreaHeight);

		// shortcut
		int PosX = 0;
		int PosY = 0;

		// paint QR Code image
		for(int Row = 0; Row < AreaHeight; Row++)
			{
			for(int Col = 0; Col < AreaWidth; Col++)
				{
				if(BlackWhiteImage[AreaTop + Row, AreaLeft + Col]) Graphics.FillRectangle(BrushBlack, PosX, PosY, Module, Module);
				PosX += Module;
				}
			PosX = 0;
			PosY += Module;
			}

		string FileName = "AlignImage.png";
		try
			{
			FileStream fs = new FileStream(FileName, FileMode.Create);
			ImageBitmap.Save(fs, ImageFormat.Png);
			fs.Close();
			}
		catch(IOException)
			{
			FileName = null;
			}

		// start image editor
		if(FileName != null) Process.Start(FileName);
		return;
		}

	internal void SetTransMatrix
			(
			Corner	Corner,
			double ImageAlignRow,
			double ImageAlignCol
			)
		{
		// top right and bottom left QR code position
		int FarFinder = QRCodeDimension - 4;
		int FarAlign = QRCodeDimension - 7;

		double[,] Matrix = new double[8, 9];

		Matrix[0, 0] = 3.0;
		Matrix[0, 1] = 3.0;
		Matrix[0, 2] = 1.0;
		Matrix[0, 6] = -3.0 * Corner.TopLeftFinder.Col;
		Matrix[0, 7] = -3.0 * Corner.TopLeftFinder.Col;
		Matrix[0, 8] = Corner.TopLeftFinder.Col;

		Matrix[1, 0] = FarFinder;
		Matrix[1, 1] = 3.0;
		Matrix[1, 2] = 1.0;
		Matrix[1, 6] = -FarFinder * Corner.TopRightFinder.Col;
		Matrix[1, 7] = -3.0 * Corner.TopRightFinder.Col;
		Matrix[1, 8] = Corner.TopRightFinder.Col;

		Matrix[2, 0] = 3.0;
		Matrix[2, 1] = FarFinder;
		Matrix[2, 2] = 1.0;
		Matrix[2, 6] = -3.0 * Corner.BottomLeftFinder.Col;
		Matrix[2, 7] = -FarFinder * Corner.BottomLeftFinder.Col;
		Matrix[2, 8] = Corner.BottomLeftFinder.Col;

		Matrix[3, 0] = FarAlign;
		Matrix[3, 1] = FarAlign;
		Matrix[3, 2] = 1.0;
		Matrix[3, 6] = -FarAlign * ImageAlignCol;
		Matrix[3, 7] = -FarAlign * ImageAlignCol;
		Matrix[3, 8] = ImageAlignCol;

		Matrix[4, 3] = 3.0;
		Matrix[4, 4] = 3.0;
		Matrix[4, 5] = 1.0;
		Matrix[4, 6] = -3.0 * Corner.TopLeftFinder.Row;
		Matrix[4, 7] = -3.0 * Corner.TopLeftFinder.Row;
		Matrix[4, 8] = Corner.TopLeftFinder.Row;

		Matrix[5, 3] = FarFinder;
		Matrix[5, 4] = 3.0;
		Matrix[5, 5] = 1.0;
		Matrix[5, 6] = -FarFinder * Corner.TopRightFinder.Row;
		Matrix[5, 7] = -3.0 * Corner.TopRightFinder.Row;
		Matrix[5, 8] = Corner.TopRightFinder.Row;

		Matrix[6, 3] = 3.0;
		Matrix[6, 4] = FarFinder;
		Matrix[6, 5] = 1.0;
		Matrix[6, 6] = -3.0 * Corner.BottomLeftFinder.Row;
		Matrix[6, 7] = -FarFinder * Corner.BottomLeftFinder.Row;
		Matrix[6, 8] = Corner.BottomLeftFinder.Row;

		Matrix[7, 3] = FarAlign;
		Matrix[7, 4] = FarAlign;
		Matrix[7, 5] = 1.0;
		Matrix[7, 6] = -FarAlign * ImageAlignRow;
		Matrix[7, 7] = -FarAlign * ImageAlignRow;
		Matrix[7, 8] = ImageAlignRow;

		for(int Row = 0; Row < 8; Row++)
			{
		    // If the element is zero, make it non zero by adding another row
		    if(Matrix[Row, Row] == 0)
			    {
		        int Row1;
				for (Row1 = Row + 1; Row1 < 8 && Matrix[Row1, Row] == 0; Row1++);
			    if (Row1 == 8) throw new ApplicationException("Solve linear equations failed");

                for(int Col = Row; Col < 9; Col++) Matrix[Row, Col] += Matrix[Row1, Col];
	            }

			// make the diagonal element 1.0
			for(int Col = 8; Col > Row; Col--) Matrix[Row, Col] /= Matrix[Row, Row];

			// subtract current row from next rows to eliminate one value
			for(int Row1 = Row + 1; Row1 < 8; Row1++)
				{
				for (int Col = 8; Col > Row; Col--) Matrix[Row1, Col] -= Matrix[Row, Col] * Matrix[Row1, Row];
				}
			}

		// go up from last row and eliminate all solved values
		for(int Col = 7; Col > 0; Col--) for(int Row = Col - 1; Row >= 0; Row--)
		    {
		    Matrix[Row, 8] -= Matrix[Row, Col] * Matrix[Col, 8];
			}

		Trans4a = Matrix[0, 8];
		Trans4b = Matrix[1, 8];
		Trans4c = Matrix[2, 8];
		Trans4d = Matrix[3, 8];
		Trans4e = Matrix[4, 8];
		Trans4f = Matrix[5, 8];
		Trans4g = Matrix[6, 8];
		Trans4h = Matrix[7, 8];

		// set trans 4 mode
		Trans4Mode = true;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Get version code bits top right
	////////////////////////////////////////////////////////////////////

	internal int GetVersionOne()
		{
		int VersionCode = 0;
		for(int Index = 0; Index < 18; Index++)
			{
			if(GetModule(Index / 3, QRCodeDimension - 11 + (Index % 3))) VersionCode |= 1 << Index;
			}
		return TestVersionCode(VersionCode);
		}

	////////////////////////////////////////////////////////////////////
	// Get version code bits bottom left
	////////////////////////////////////////////////////////////////////

	internal int GetVersionTwo()
		{
		int VersionCode = 0;
		for(int Index = 0; Index < 18; Index++)
			{
			if(GetModule(QRCodeDimension - 11 + (Index % 3), Index / 3)) VersionCode |= 1 << Index;
			}
		return TestVersionCode(VersionCode);
		}

	////////////////////////////////////////////////////////////////////
	// Test version code bits
	////////////////////////////////////////////////////////////////////

	internal int TestVersionCode
			(
			int VersionCode
			)
		{
		// format info
		int Code = VersionCode >> 12;

		// test for exact match
		if(Code >= 7 && Code <= 40 && QRCode.VersionCodeArray[Code - 7] == VersionCode)
			{
			return Code;
			}

		// look for a match
		int BestInfo = 0;
		int Error = int.MaxValue;
		for(int Index = 0; Index < 34; Index++)
			{
			// test for exact match
			int ErrorBits = VersionCodeArray[Index] ^ VersionCode;
			if(ErrorBits == 0) return VersionCode >> 12;

			// count errors
			int ErrorCount = CountBits(ErrorBits);

			// save best result
			if(ErrorCount < Error)
				{
				Error = ErrorCount;
				BestInfo = Index;
				}
			}

		return Error <= 3 ? BestInfo + 7 : 0;
		}

        /// <summary>
        /// Получить информацию о формате в левом верхнем углу
        /// </summary>
        /// <returns></returns>
        public Int32 GetFormatInfoOne()
		{
            Int32 Info = 0;
		    for(int Index = 0; Index < 15; Index++)
			{
			    if (GetModule(FormatInfoOne[Index, 0], FormatInfoOne[Index, 1])) Info |= 1 << Index;
			}

		    return TestFormatInfo(Info);
		}
        
        /// <summary>
        /// Получить информацию о формате вокруг верхнего правого и нижнего левого углов
        /// </summary>
        /// <returns></returns>
        internal Int32 GetFormatInfoTwo()
		{
            Int32 Info = 0;
		    for (int Index = 0; Index < 15; Index++)
			{
                Int32 Row = FormatInfoTwo[Index, 0];
			    if (Row < 0) Row += QRCodeDimension;
                Int32 Col = FormatInfoTwo[Index, 1];
			    if (Col < 0) Col += QRCodeDimension;
			    if (GetModule(Row, Col)) Info |= 1 << Index;
			}

		    return TestFormatInfo(Info);
		}
        
        /// <summary>
        /// Биты информации о формате теста
        /// </summary>
        /// <param name="FormatInfo"></param>
        /// <returns></returns>
        internal Int32 TestFormatInfo(Int32 FormatInfo)
		{
            // Информация о формате
            Int32 Info = (FormatInfo ^ 0x5412) >> 10;

            // Тест на точное соответствие
            if (QRCode.FormatInfoArray[Info] == FormatInfo) { return Info; }

            // Ищите совпадения
            Int32 BestInfo = 0;
            Int32 Error = Int32.MaxValue;
		    for (int Index = 0; Index < 32; Index++)
		    {
                Int32 ErrorCount = CountBits(QRCode.FormatInfoArray[Index] ^ FormatInfo);
		        if (ErrorCount < Error)
			    {
			        Error = ErrorCount;
			        BestInfo = Index;
			    }
		    }

		    return Error <= 3 ? BestInfo : -1;
		}
        
        /// <summary>
        /// Количество битов
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        internal int CountBits(int Value)
		{
		    Int32 Count = 0;
		    for (int Mask = 0x4000; Mask != 0; Mask >>= 1) if ((Value & Mask) != 0) Count++;

		    return Count;
		}
        
        /// <summary>
        /// Преобразование изображения в матрицу qr-кода и тестирование фиксированных модулей
        /// </summary>
        internal void ConvertImageToMatrix()
		{
            // Цикл для всех модулей
            Int32 FixedCount = 0;
            Int32 ErrorCount = 0;
		    for(int Row = 0; Row < QRCodeDimension; Row++) for(int Col = 0; Col < QRCodeDimension; Col++)
			{
                // модуль (Row, Col) нет фиксированного модуля
                if ((BaseMatrix[Row, Col] & Fixed) == 0)
				{
				    if (GetModule(Row, Col)) BaseMatrix[Row, Col] |= Black;
				}

                // Фиксированный модуль
                else
                {
                    // Всего фиксированных модулей
                    FixedCount++;

                    // Проверка на ошибку
                    if ((GetModule(Row, Col) ? Black : White) != (BaseMatrix[Row, Col] & 1)) ErrorCount++;
				}
			}
        
		    if (ErrorCount > FixedCount * ErrCorrPercent[(int) ErrorCorrection] / 100)
			    throw new ApplicationException("Исправлена ошибка модулей");
            
		    return;
		}

        /// <summary>
        /// Выгружает данные матрицы из базовой матрицы
        /// </summary>
        internal void UnloadDataFromMatrix()
		{
            // Инициализация указателя входного массива
            Int32 Ptr = 0;
            Int32 PtrEnd = 8 * MaxCodewords;
		    CodewordsArray = new Byte[MaxCodewords];

            // Нижний правый угол выходной матрицы
            Int32 Row = QRCodeDimension - 1;
            Int32 Col = QRCodeDimension - 1;

            // Состояние шага
            Int32 State = 0;
		    for(;;) 
			    {
                // Текущий модуль-данные
                if ((MaskMatrix[Row, Col] & NonData) == 0)
				    {
                    // Выгрузить текущий модуль
                    if ((MaskMatrix[Row, Col] & 1) != 0) CodewordsArray[Ptr >> 3] |= (byte) (1 << (7 - (Ptr & 7)));
				    if(++Ptr == PtrEnd) break;
				    }

                // Настоящий модуль non данные и вертикальная линия времени условие дальше
                else if (Col == 6) Col--;
                
                // Обновление положения матрицы до следующего модуля
                switch (State)
				{
                    case 0:
					    Col--;
					    State = 1;
					    continue;
                        
                    case 1:
					    Col++;
					    Row--;
                        if (Row >= 0)
						{
						    State = 0;
						    continue;
						}
                        Col -= 2;
					    Row = 0;
					    State = 2;
					    continue;
                        
                    case 2:
					    Col--;
					    State = 3;
					    continue;
                        
                    case 3:
					    Col++;
					    Row++;
                        if (Row < QRCodeDimension)
						{
						    State = 2;
						    continue;
						}
                        Col -= 2;
					    Row = QRCodeDimension - 1;
					    State = 0;
					    continue;
				}
			}

		    return;
		}
        
        /// <summary>
        /// Восстанавливает чередующиеся данные и блоки коррекции ошибок
        /// </summary>
        internal void RestoreBlocks()
		{
            // Выделение временного массива кодовых слов
            Byte[] TempArray = new Byte[MaxCodewords];

            // Всего блоков
            Int32 TotalBlocks = BlocksGroup1 + BlocksGroup2;

            // Создание массива исходных блоков данных
            Int32[] Start = new Int32[TotalBlocks];
		    for (int Index = 1; Index < TotalBlocks; Index++) Start[Index] = Start[Index - 1] + (Index <= BlocksGroup1 ? DataCodewordsGroup1 : DataCodewordsGroup2);

            // Шаг первый. Чередование на основе длины группы один
            Int32 PtrEnd = DataCodewordsGroup1 * TotalBlocks;

            // Восстановление группы один и два
            Int32 Ptr;
            Int32 Block = 0;
		    for (Ptr = 0; Ptr < PtrEnd; Ptr++)
			{
			    TempArray[Start[Block]] = CodewordsArray[Ptr];
			    Start[Block]++;
			    Block++;

			    if (Block == TotalBlocks) Block = 0;
			}

            // Восстановить вторую группу
            if (DataCodewordsGroup2 > DataCodewordsGroup1)
			{
                // Шаг первый. Чередование на основе длины группы один
                PtrEnd = MaxDataCodewords;

			    Block = BlocksGroup1;
			    for(; Ptr < PtrEnd; Ptr++)
				{
				    TempArray[Start[Block]] = CodewordsArray[Ptr];
				    Start[Block]++;
				    Block++;

				    if (Block == TotalBlocks) Block = BlocksGroup1;
				}
			}

            // Создание массива блоков коррекции ошибок начальная точка
            Start[0] = MaxDataCodewords;
		    for (int Index = 1; Index < TotalBlocks; Index++) Start[Index] = Start[Index - 1] + ErrCorrCodewords;

            // Восстановить все группы
            PtrEnd = MaxCodewords;
		    Block = 0;
		    for (; Ptr < PtrEnd; Ptr++)
			{
			    TempArray[Start[Block]] = CodewordsArray[Ptr];
			    Start[Block]++;
			    Block++;

			    if (Block == TotalBlocks) Block = 0;
			}

            // Сохранить результат
            CodewordsArray = TempArray;
		    return;
		}
        
        /// <summary>
        /// Вычисленяет исправления ошибок
        /// </summary>
        protected void CalculateErrorCorrection()
		{
            // Общее количество ошибок 
            Int32 TotalErrorCount = 0;

            // Задать полиномиальный массив генератора
            Byte[] Generator = GenArray[ErrCorrCodewords - 7];

            // Буфер вычисления коррекции ошибок
            Int32 BufSize = Math.Max(DataCodewordsGroup1, DataCodewordsGroup2) + ErrCorrCodewords;
		    Byte[] ErrCorrBuff = new Byte[BufSize];

            // Начальное количество кодовых слов данных 
            Int32 DataCodewords = DataCodewordsGroup1;
            Int32 BuffLen = DataCodewords + ErrCorrCodewords;

            // Указатель кодовых слов
            Int32 DataCodewordsPtr = 0;

            // Буфер кодовых слов коррекции ошибок указатель
            Int32 CodewordsArrayErrCorrPtr = MaxDataCodewords;

            // Цикл по одному блоку за раз
            Int32 TotalBlocks = BlocksGroup1 + BlocksGroup2;
		    for (int BlockNumber = 0; BlockNumber < TotalBlocks; BlockNumber++)
			{
                // Переключиться на кодовые слова данных группы 2
                if (BlockNumber == BlocksGroup1)
				{
				    DataCodewords = DataCodewordsGroup2;
				    BuffLen = DataCodewords + ErrCorrCodewords;
				}

                // Скопируйте следующий блок кодовых слов в буфер и освободить оставшуюся часть
                Array.Copy(CodewordsArray, DataCodewordsPtr, ErrCorrBuff, 0, DataCodewords);
			    Array.Copy(CodewordsArray, CodewordsArrayErrCorrPtr, ErrCorrBuff, DataCodewords, ErrCorrCodewords);

                // Сделать дубликат
                Byte[] CorrectionBuffer = (byte[]) ErrCorrBuff.Clone();

                // Коррекция ошибок полиномиальное деление
                ReedSolomon.PolynominalDivision(ErrCorrBuff, BuffLen, Generator, ErrCorrCodewords);

                // Проверка на ошибку
                Int32 Index;
			    for (Index = 0; Index < ErrCorrCodewords && ErrCorrBuff[DataCodewords + Index] == 0; Index++);
			    if (Index < ErrCorrCodewords)
				{
                    // Исправить ошибку
                    Int32 ErrorCount = ReedSolomon.CorrectData(CorrectionBuffer, BuffLen, ErrCorrCodewords);
				    if(ErrorCount <= 0) { throw new ApplicationException("Данные повреждены. Исправление ошибки не удалось"); }

				    TotalErrorCount += ErrorCount;

                    // Исправить данные
                    Array.Copy(CorrectionBuffer, 0, CodewordsArray, DataCodewordsPtr, DataCodewords);
				}

                // Время обновления кодовых слов для следующего буфера
                DataCodewordsPtr += DataCodewords;

                // Обновить указатель				
                CodewordsArrayErrCorrPtr += ErrCorrCodewords;
			}

		    return;
		}
        
        /// <summary>
        /// Преобразует битовый массив в массив байтов
        /// </summary>
        /// <returns></returns>
        internal Byte[] DecodeData()
		{
            // Начальное состояние битового буфера
            BitBuffer = (uint)((CodewordsArray[0] << 24) | (CodewordsArray[1] << 16) | (CodewordsArray[2] << 8) | CodewordsArray[3]);
		    BitBufferLen = 32;
		    CodewordsPtr = 4;

            // Выделить список байтов данных
            List<Byte> DataSeg = new List<Byte>();

            // Данные могут быть составлены из блоков
            for (;;)
			{
                // Первые 4 бита индикатор режима
                EncodingMode EncodingMode = (EncodingMode) ReadBitsFromCodewordsArray(4);

                // Конец данных
                if (EncodingMode <= 0) break;

                // Длина прочитанных данных
                Int32 DataLength = ReadBitsFromCodewordsArray(DataLengthBits(EncodingMode));
			    if (DataLength < 0) { throw new ApplicationException("Преждевременный конец данных (DataLengh)"); }

                // Сохранить начало сегмента
                Int32 SegStart = DataSeg.Count;

                // Переключатель на основе режима кодирования
                // Числовой индикатор кода 0001, цифробуквенное 0010, байт 0100
                switch (EncodingMode)
				{
                    // Числовой режим
                    case EncodingMode.Numeric:
                        // Кодировать цифры в группы по 2
                        Int32 NumericEnd = (DataLength / 3) * 3;

					    for (int Index = 0; Index < NumericEnd; Index += 3)
						{
                            Int32 Temp = ReadBitsFromCodewordsArray(10);
						    if (Temp < 0) { throw new ApplicationException("Преждевременный конец данных (Numeric 1)"); }

						    DataSeg.Add(DecodingTable[Temp / 100]);
						    DataSeg.Add(DecodingTable[(Temp % 100) / 10]);
						    DataSeg.Add(DecodingTable[Temp % 10]);
						}

                        // У нас остался один персонаж
                        if (DataLength - NumericEnd == 1)
                        {
                            Int32 Temp = ReadBitsFromCodewordsArray(4);
						    if(Temp < 0) { throw new ApplicationException("Преждевременный конец данных (Numeric 2)"); }
						    DataSeg.Add(DecodingTable[Temp]);
						}

                        // У нас осталось два персонажа
                        else if (DataLength - NumericEnd == 2)
                        {
                            Int32 Temp = ReadBitsFromCodewordsArray(7);
						    if(Temp < 0) { throw new ApplicationException("Преждевременный конец данных (Numeric 3)"); }

						    DataSeg.Add(DecodingTable[Temp / 10]);
						    DataSeg.Add(DecodingTable[Temp % 10]);
						}
					    break;

                    // Буквенно-цифровой режим
                    case EncodingMode.AlphaNumeric:
                        // Кодировать цифры в группы по 2
                        Int32 AlphaNumEnd = (DataLength / 2) * 2;

					    for(int Index = 0; Index < AlphaNumEnd; Index += 2)
						{
                            Int32 Temp = ReadBitsFromCodewordsArray(11);
						    if(Temp < 0) { throw new ApplicationException("Преждевременный конец данных (Alpha Numeric 1)"); }

						    DataSeg.Add(DecodingTable[Temp / 45]);
						    DataSeg.Add(DecodingTable[Temp % 45]);
						}

                        // У нас остался один персонаж
                        if (DataLength - AlphaNumEnd == 1)
						{
                            Int32 Temp = ReadBitsFromCodewordsArray(6);
						    if(Temp < 0) { throw new ApplicationException("Преждевременный конец данных (Alpha Numeric 2)"); }

						    DataSeg.Add(DecodingTable[Temp]);
						}
					    break;

				    // Байтовый режим
				    case EncodingMode.Byte:
                        // Добавить данные после режима и количества символов
                        for (int Index = 0; Index < DataLength; Index++)
						{
                            Int32 Temp = ReadBitsFromCodewordsArray(8);
						    if (Temp < 0) { throw new ApplicationException("Преждевременный конец данных (byte mode)"); }

						    DataSeg.Add((byte) Temp);
						}
					    break;

				    default:
					    throw new ApplicationException(string.Format("Режим кодирования не поддерживается {0}", EncodingMode.ToString()));
				    }

			    if (DataLength != DataSeg.Count - SegStart)
                    throw new ApplicationException("Длина кодировки данных по ошибке");
			    }

		    // Сохранение данных
		    return DataSeg.ToArray();
		}
        
        /// <summary>
        /// Чтение данных из массива кодовых слов
        /// </summary>
        /// <param name="Bits"></param>
        /// <returns></returns>
        internal Int32 ReadBitsFromCodewordsArray(Int32 Bits)
		{
		    if (Bits > BitBufferLen) return -1;
                Int32 Data = (int)(BitBuffer >> (32 - Bits));

		    BitBuffer <<= Bits;
		    BitBufferLen -= Bits;

		    while (BitBufferLen <= 24 && CodewordsPtr < MaxDataCodewords)
		    {
			    BitBuffer |= (uint)(CodewordsArray[CodewordsPtr++] << (24 - BitBufferLen));
			    BitBufferLen += 8;
		    }

		    return Data;
		}
	}
}
