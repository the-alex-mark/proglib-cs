using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ProgLib.Text.Encoding.QRCode
{
    public enum ErrorSpotControl
	    {
	        None,
	        White,
	        Black,
	        Alternate,
	    }

    /// <summary>
    /// Преобразование QR-код bool массив в растровое изображение.
    /// </summary>
    public static class QRCodeToBitmap
	{
        // Белые и черные кисти
        private static SolidBrush BrushWhite = new SolidBrush(Color.White);
	    private static SolidBrush BrushBlack = new SolidBrush(Color.Black);

        /// <summary>
        /// Создать растровое изображение с QR Code массив 
        /// </summary>
        /// <param name="QRCode">QRCode</param>
        /// <param name="ModuleSize">Размер модуля в пикселях</param>
        /// <param name="QuietZone">Тихая зона в пикселях</param>
        /// <returns>Растровое изображение QRCode</returns>
        public static Bitmap CreateBitmap(QRCode QRCode, Int32 ModuleSize, Int32 QuietZone)
		{
            // размер растрового изображения QRCode
            Int32 BitmapDimension = QRCode.QRCodeImageDimension(ModuleSize, QuietZone);

            // Создать объект изображения и сделать его белым
            Bitmap ImageBitmap = new Bitmap(BitmapDimension, BitmapDimension);
		    Graphics Graphics = Graphics.FromImage(ImageBitmap);
		    Graphics.FillRectangle(BrushWhite, 0, 0, BitmapDimension, BitmapDimension);

            // Ярлык
            Int32 Dimension = QRCode.QRCodeDimension;
		    Boolean[,] Code = QRCode.QRCodeMatrix;
            Int32 PosX = QuietZone;
            Int32 PosY = QuietZone;

            // Краска QR-код изображения
            for (int Row = 0; Row < Dimension; Row++)
			{
			    for (int Col = 0; Col < Dimension; Col++)
				{
				    if (Code[Row, Col]) Graphics.FillRectangle(BrushBlack, PosX, PosY, ModuleSize, ModuleSize);
				    PosX += ModuleSize;
				}

			    PosX = QuietZone;
			    PosY += ModuleSize;
			}

            // Возврат растрового изображения
            return ImageBitmap;
		}

        /// <summary>
        /// Создать растровое изображение с QR Code массив более твердый или люк фон
        /// </summary>
        /// <param name="QRCode">QRCode</param>
        /// <param name="ModuleSize">Размер модуля в пикселях</param>
        /// <param name="QuietZone">Тихая зона в пикселях</param>
        /// <param name="Background">SolidBrush или HatchBrush фон</param>
        /// <param name="ImageWidth">Ширина выходного изображения</param>
        /// <param name="ImageHeight">Высота выходного изображения</param>
        /// <param name="QRCodeCenterPosX">QRCode позиция X </param>
        /// <param name="QRCodeCenterPosY">QRCode позиция Y</param>
        /// <param name="Rotation">Вращение QRCode в градусах</param>
        /// <returns>Растровое изображение QRCode</returns>
        public static Bitmap CreateBitmap(QRCode QRCode, Int32 ModuleSize, Int32 QuietZone, Brush Background, Int32 ImageWidth, Int32 ImageHeight, Double QRCodeCenterPosX, Double QRCodeCenterPosY, Double Rotation)
		{
            // Создать фоновое растровое изображение и раскрасить его кистью
            Bitmap BackgroundBitmap = new Bitmap(ImageWidth, ImageHeight);
		    Graphics Graphics = Graphics.FromImage(BackgroundBitmap);
		    Graphics.FillRectangle(Background, 0, 0, ImageWidth, ImageHeight);

            // Возвращение в QR растровый код закрасили фон
            return CreateBitmap(QRCode, ModuleSize, QuietZone, BackgroundBitmap, QRCodeCenterPosX, QRCodeCenterPosY, Rotation);
		}

        /// <summary>
        /// Создать растровое изображение с QR Code массив более твердый или люк фон
        /// </summary>
        /// <param name="QRCode">QRCode</param>
        /// <param name="ModuleSize">Размер модуля в пикселях</param>
        /// <param name="QuietZone">Тихая зона в пикселях</param>
        /// <param name="Background">Фон растрового изображения</param>
        /// <param name="QRCodeCenterPosX">QRCode позиция X </param>
        /// <param name="QRCodeCenterPosY">QRCode позиция Y</param>
        /// <param name="Rotation">Вращение QRCode в градусах</param>
        /// <returns>Растровое изображение QRCode</returns>
        public static Bitmap CreateBitmap(QRCode QRCode, Int32 ModuleSize, Int32 QuietZone, Bitmap Background, Double QRCodeCenterPosX, Double QRCodeCenterPosY, Double Rotation)
		{
            // Размер растрового изображения QRCode
            Int32 QRCodeImageDimension = QRCode.QRCodeImageDimension(ModuleSize, QuietZone);

		    // transformation matrix
		    Matrix Matrix = new Matrix();
		    Matrix.Translate((float) QRCodeCenterPosX , (float) QRCodeCenterPosY);
		    if (Rotation != 0) Matrix.Rotate((float) Rotation);

            // Создание копии фона
            Bitmap OutputImage = new Bitmap(Background);

            // Создание графического объекта
            Graphics Graphics = Graphics.FromImage(OutputImage);

            // Присоединить матрицу преобразования
            Graphics.Transform = Matrix;

            // Положение верхнего левого угла QRCode относительно центра QRCode
            Double QRCodePos = -0.5 * QRCodeImageDimension;

            // очистить область для QRCode
            Graphics.FillRectangle(BrushWhite, (float) QRCodePos, (float) QRCodePos, QRCodeImageDimension, QRCodeImageDimension);

		    // Ярлык
		    Int32 Dimension = QRCode.QRCodeDimension;
		    Boolean[,] Code = QRCode.QRCodeMatrix;

            // Добавить тихую зону
            QRCodePos += QuietZone;

            // Краска QR-код изображения
            for (int Row = 0; Row < Dimension; Row++)
			    for (int Col = 0; Col < Dimension; Col++)
			    {
			        if (Code[Row, Col]) Graphics.FillRectangle(BrushBlack, (float) (QRCodePos + Col * ModuleSize), (float) (QRCodePos + Row * ModuleSize), ModuleSize, ModuleSize);
			    }

		    return OutputImage;
		}

        /// <summary>
        /// Создать растровое изображение с QR Code массив более твердый или люк фон
        /// </summary>
        /// <param name="QRCode">QRCode</param>
        /// <param name="ModuleSize">Размер модуля в пикселях</param>
        /// <param name="QuietZone">Quiet zone in pixels</param>
        /// <param name="Background">Фон растрового изображения</param>
        /// <param name="QRCodeCenterPosX">QRCode позиция X </param>
        /// <param name="QRCodeCenterPosY">QRCode позиция Y</param>
        /// <param name="Rotation">Вращение QRCode в градусах</param>
        /// <param name="CameraDistance">Расстояние перспективной камеры в пикселях</param>
        /// <param name="ViewXRotation">Вид камеры вращение оси X в градусах</param>
        /// <returns>Растровое изображение QRCode</returns>
        public static Bitmap CreateBitmap(QRCode QRCode, Int32 ModuleSize, Int32 QuietZone, Bitmap Background, Double QRCodeCenterPosX, Double QRCodeCenterPosY, Double Rotation, Double CameraDistance, Double ViewXRotation)
		{
            // Cоздать перспективный объект
            Perspective Perspective = new Perspective(QRCodeCenterPosX, QRCodeCenterPosY, Rotation, CameraDistance, ViewXRotation);

            // Cоздание графического объекта
            Graphics Graphics = Graphics.FromImage(Background);

            // размер растрового изображения QRCode
            Int32 QRCodeImageDimension = QRCode.QRCodeImageDimension(ModuleSize, QuietZone);

            // Половинный размер изображения
            Double HalfDimension = 0.5 * QRCodeImageDimension;

            // Полигон
            PointF[] Polygon = new PointF[4];
		    Perspective.GetPolygon(-HalfDimension, -HalfDimension, QRCodeImageDimension, Polygon);

            // Очистить область для QRCode
            Graphics.FillPolygon(BrushWhite, Polygon);

		    // Ярлык
		    Int32 Dimension = QRCode.QRCodeDimension;
		    Boolean[,] Code = QRCode.QRCodeMatrix;

            // Добавить тихую зону
            Double QRCodePos = -HalfDimension + QuietZone;

            // краска QRCode изображения
            for (int Row = 0; Row < Dimension; Row++)
			    for (int Col = 0; Col < Dimension; Col++)
			    {
			        if (Code[Row, Col])
				    {
				        Perspective.GetPolygon(QRCodePos + Col * ModuleSize, QRCodePos + Row * ModuleSize, ModuleSize, Polygon);
				        Graphics.FillPolygon(BrushBlack, Polygon);
				    }
			    }

		    return Background;
		}

        /// <summary>
        /// Добавление областей ошибок для тестирования
        /// </summary>
        /// <param name="ImageBitmap">Растровое изображение</param>
        /// <param name="ErrorControl">Перечисление контроля ошибок</param>
        /// <param name="ErrorDiameter">Диаметр пятна ошибки</param>
        /// <param name="ErrorSpotsCount">Количество пятен ошибок</param>
        public static void AddErrorSpots(Bitmap ImageBitmap, ErrorSpotControl ErrorControl, Double ErrorDiameter, Double ErrorSpotsCount)
		{
            // Генератор случайных чисел
            Random RandNum = new Random();

            // Создание графического объекта
            Graphics Graphics = Graphics.FromImage(ImageBitmap);

		    Double XRange = ImageBitmap.Width - ErrorDiameter - 4;
		    Double YRange = ImageBitmap.Height - ErrorDiameter - 4;
		    SolidBrush SpotBrush = ErrorControl == ErrorSpotControl.Black ? BrushBlack : BrushWhite;

		    for (int Index = 0; Index < ErrorSpotsCount; Index++)
			{
			    Double XPos = RandNum.NextDouble() * XRange;
			    Double YPos = RandNum.NextDouble() * YRange;
			    if (ErrorControl == ErrorSpotControl.Alternate) SpotBrush = (Index & 1) == 0 ? BrushWhite : BrushBlack;
			    Graphics.FillEllipse(SpotBrush, (float) XPos, (float) YPos, (float) ErrorDiameter, (float) ErrorDiameter);
			}

		    return;
		}
	}
}
