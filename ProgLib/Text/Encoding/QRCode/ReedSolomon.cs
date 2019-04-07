using System;

namespace ProgLib.Text.Encoding.QRCode
{
	internal class ReedSolomon
	{
	    internal static Int32 INCORRECTABLE_ERROR = -1;

	    internal static readonly Byte[] ExpToInt =
		{
			   1,   2,   4,   8,  16,  32,  64, 128,  29,  58, 116, 232, 205, 135,  19,  38,
			  76, 152,  45,  90, 180, 117, 234, 201, 143,   3,   6,  12,  24,  48,  96, 192,
			 157,  39,  78, 156,  37,  74, 148,  53, 106, 212, 181, 119, 238, 193, 159,  35,
			  70, 140,   5,  10,  20,  40,  80, 160,  93, 186, 105, 210, 185, 111, 222, 161,
			  95, 190,  97, 194, 153,  47,  94, 188, 101, 202, 137,  15,  30,  60, 120, 240,
			 253, 231, 211, 187, 107, 214, 177, 127, 254, 225, 223, 163,  91, 182, 113, 226,
			 217, 175,  67, 134,  17,  34,  68, 136,  13,  26,  52, 104, 208, 189, 103, 206,
			 129,  31,  62, 124, 248, 237, 199, 147,  59, 118, 236, 197, 151,  51, 102, 204,
			 133,  23,  46,  92, 184, 109, 218, 169,  79, 158,  33,  66, 132,  21,  42,  84,
			 168,  77, 154,  41,  82, 164,  85, 170,  73, 146,  57, 114, 228, 213, 183, 115,
			 230, 209, 191,  99, 198, 145,  63, 126, 252, 229, 215, 179, 123, 246, 241, 255,
			 227, 219, 171,  75, 150,  49,  98, 196, 149,  55, 110, 220, 165,  87, 174,  65,
			 130,  25,  50, 100, 200, 141,   7,  14,  28,  56, 112, 224, 221, 167,  83, 166,
			  81, 162,  89, 178, 121, 242, 249, 239, 195, 155,  43,  86, 172,  69, 138,   9,
			  18,  36,  72, 144,  61, 122, 244, 245, 247, 243, 251, 235, 203, 139,  11,  22,
			  44,  88, 176, 125, 250, 233, 207, 131,  27,  54, 108, 216, 173,  71, 142,   1,

			        2,   4,   8,  16,  32,  64, 128,  29,  58, 116, 232, 205, 135,  19,  38,
			  76, 152,  45,  90, 180, 117, 234, 201, 143,   3,   6,  12,  24,  48,  96, 192,
			 157,  39,  78, 156,  37,  74, 148,  53, 106, 212, 181, 119, 238, 193, 159,  35,
			  70, 140,   5,  10,  20,  40,  80, 160,  93, 186, 105, 210, 185, 111, 222, 161,
			  95, 190,  97, 194, 153,  47,  94, 188, 101, 202, 137,  15,  30,  60, 120, 240,
			 253, 231, 211, 187, 107, 214, 177, 127, 254, 225, 223, 163,  91, 182, 113, 226,
			 217, 175,  67, 134,  17,  34,  68, 136,  13,  26,  52, 104, 208, 189, 103, 206,
			 129,  31,  62, 124, 248, 237, 199, 147,  59, 118, 236, 197, 151,  51, 102, 204,
			 133,  23,  46,  92, 184, 109, 218, 169,  79, 158,  33,  66, 132,  21,  42,  84,
			 168,  77, 154,  41,  82, 164,  85, 170,  73, 146,  57, 114, 228, 213, 183, 115,
			 230, 209, 191,  99, 198, 145,  63, 126, 252, 229, 215, 179, 123, 246, 241, 255,
			 227, 219, 171,  75, 150,  49,  98, 196, 149,  55, 110, 220, 165,  87, 174,  65,
			 130,  25,  50, 100, 200, 141,   7,  14,  28,  56, 112, 224, 221, 167,  83, 166,
			  81, 162,  89, 178, 121, 242, 249, 239, 195, 155,  43,  86, 172,  69, 138,   9,
			  18,  36,  72, 144,  61, 122, 244, 245, 247, 243, 251, 235, 203, 139,  11,  22,
			  44,  88, 176, 125, 250, 233, 207, 131,  27,  54, 108, 216, 173,  71, 142,   1
		};

	    internal static readonly Byte[] IntToExp =
		{
			   0,   0,   1,  25,   2,  50,  26, 198,   3, 223,  51, 238,  27, 104, 199,  75,
			   4, 100, 224,  14,  52, 141, 239, 129,  28, 193, 105, 248, 200,   8,  76, 113,
			   5, 138, 101,  47, 225,  36,  15,  33,  53, 147, 142, 218, 240,  18, 130,  69,
			  29, 181, 194, 125, 106,  39, 249, 185, 201, 154,   9, 120,  77, 228, 114, 166,
			   6, 191, 139,  98, 102, 221,  48, 253, 226, 152,  37, 179,  16, 145,  34, 136,
			  54, 208, 148, 206, 143, 150, 219, 189, 241, 210,  19,  92, 131,  56,  70,  64,
			  30,  66, 182, 163, 195,  72, 126, 110, 107,  58,  40,  84, 250, 133, 186,  61,
			 202,  94, 155, 159,  10,  21, 121,  43,  78, 212, 229, 172, 115, 243, 167,  87,
			   7, 112, 192, 247, 140, 128,  99,  13, 103,  74, 222, 237,  49, 197, 254,  24,
			 227, 165, 153, 119,  38, 184, 180, 124,  17,  68, 146, 217,  35,  32, 137,  46,
			  55,  63, 209,  91, 149, 188, 207, 205, 144, 135, 151, 178, 220, 252, 190,  97,
			 242,  86, 211, 171,  20,  42,  93, 158, 132,  60,  57,  83,  71, 109,  65, 162,
			  31,  45,  67, 216, 183, 123, 164, 118, 196,  23,  73, 236, 127,  12, 111, 246,
			 108, 161,  59,  82,  41, 157,  85, 170, 251,  96, 134, 177, 187, 204,  62,  90,
			 203,  89,  95, 176, 156, 169, 160,  81,  11, 245,  22, 235, 122, 117,  44, 215,
			  79, 174, 213, 233, 230, 231, 173, 232, 116, 214, 244, 234, 168,  80,  88, 175
		};
        
	    internal static Int32 CorrectData
			(
			Byte[] ReceivedData,    // полученный буфер данных с данными и кодом исправления ошибок
            Int32 DataLength,       // длина данных в буфере (обратите внимание, что иногда массив длиннее данных)
            Int32 ErrCorrCodewords  // количество кодовых слов коррекции ошибок
            )
		{
            // Вычислить вектор синдрома
            Int32[] Syndrome = CalculateSyndrome(ReceivedData, DataLength, ErrCorrCodewords);

            // полученные данные не содержат ошибок
            // Примечание: это не должно произойти, потому что мы вызываем этот метод, только если ошибка была обнаружена
            if (Syndrome == null) return 0;

            // Изменен Берлекэмпа-Мэсси
            // Вычислить Сигму и омегу
            Int32[] Sigma = new Int32[ErrCorrCodewords / 2 + 2];
            Int32[] Omega = new Int32[ErrCorrCodewords / 2 + 1];
            Int32 ErrorCount = CalculateSigmaMBM(Sigma, Omega, Syndrome, ErrCorrCodewords);

            // Данные не могут быть исправлены
            if (ErrorCount <= 0) return INCORRECTABLE_ERROR;

            // Ищите положение ошибки с помощью поиска Chien
            Int32[] ErrorPosition = new Int32[ErrorCount];
		    if(!ChienSearch(ErrorPosition, DataLength, ErrorCount, Sigma)) return INCORRECTABLE_ERROR;

            // Корректный массив данных на основе массива позиций
            ApplyCorrection(ReceivedData, DataLength, ErrorCount, ErrorPosition, Sigma, Omega);

            // Возвращение графа ошибку, прежде чем она была исправлена
            return ErrorCount;
		}

        // Расчет вектора синдрома
        // S0 = R0 + R1 +        R2 + ....        + Rn
        // S1 = R0 + R1 * A**1 + R2 * A**2 + .... + Rn * A**n
        // S2 = R0 + R1 * A**2 + R2 * A**4 + .... + Rn * A**2n
        // ....
        // Sm = R0 + R1 * A**m + R2 * A**2m + .... + Rn * A**mn

        internal static Int32[] CalculateSyndrome
			(
            Byte[] ReceivedData,    // полученный буфер данных с данными и кодом исправления ошибок
            Int32 DataLength,       // длина данных в буфере (обратите внимание, что иногда массив длиннее данных)
            Int32 ErrCorrCodewords  // количество кодовых слов коррекции ошибок
            )
		{
            // Выделить вектор синдрома
            Int32[] Syndrome = new Int32[ErrCorrCodewords];

            // Индикатор ошибки сброса
            Boolean Error = false;

            // syndrome[zero] частный случай
            // Total = Data[0] + Data[1] + ... Data[n]
            Int32 Total = ReceivedData[0];
		    for (int SumIndex = 1; SumIndex < DataLength; SumIndex++) Total = ReceivedData[SumIndex] ^ Total;
		    Syndrome[0] = Total;
		    if (Total != 0) Error = true;

            // Все другие синдромы
            for (int Index = 1; Index < ErrCorrCodewords;  Index++)
			{
			    // Total = Data[0] + Data[1] * Alpha + Data[2] * Alpha ** 2 + ... Data[n] * Alpha ** n
			    Total = ReceivedData[0];
			    for (int IndexT = 1; IndexT < DataLength; IndexT++) Total = ReceivedData[IndexT] ^ MultiplyIntByExp(Total, Index);
			    Syndrome[Index] = Total;
			    if (Total != 0) Error = true;
			}

            // Если есть ошибка возврата синдрома вектор в противном случае возвращает null
            return Error ? Syndrome : null;
		}

        // Изменен Берлекэмпа-Мэсси
        internal static Int32 CalculateSigmaMBM(Int32[] Sigma, Int32[] Omega, Int32[] Syndrome, Int32 ErrCorrCodewords)
		{
            Int32[] PolyC = new Int32[ErrCorrCodewords];
            Int32[] PolyB = new Int32[ErrCorrCodewords];
		    PolyC[1] = 1;
		    PolyB[0] = 1;
            Int32 ErrorControl = 1;
            Int32 ErrorCount = 0;       // L
            Int32 m = -1;

		    for (int ErrCorrIndex = 0; ErrCorrIndex < ErrCorrCodewords; ErrCorrIndex++)
			    {
                // Рассчитать расхождение
                Int32 Dis = Syndrome[ErrCorrIndex];
			    for(int i = 1; i <= ErrorCount; i++) Dis ^= Multiply(PolyB[i], Syndrome[ErrCorrIndex - i]);

			    if (Dis != 0)
				{
                    Int32 DisExp = IntToExp[Dis];
                    Int32[] WorkPolyB = new Int32[ErrCorrCodewords];

				    for (int Index = 0; Index <= ErrCorrIndex; Index++) WorkPolyB[Index] = PolyB[Index] ^ MultiplyIntByExp(PolyC[Index], DisExp);
                    Int32 js = ErrCorrIndex - m;
				    if (js > ErrorCount)
					{
					    m = ErrCorrIndex - ErrorCount;
					    ErrorCount = js;
					    if (ErrorCount > ErrCorrCodewords / 2) return INCORRECTABLE_ERROR;
					    for (int Index = 0; Index <= ErrorControl; Index++) PolyC[Index] = DivideIntByExp(PolyB[Index], DisExp);
					    ErrorControl = ErrorCount;
					}
				    PolyB = WorkPolyB;
				}

                // Сдвиг полинома вправо
                Array.Copy(PolyC, 0, PolyC, 1, Math.Min(PolyC.Length - 1, ErrorControl));
			    PolyC[0] = 0;
			    ErrorControl++;
			    }

		    PolynomialMultiply(Omega, PolyB, Syndrome);
		    Array.Copy(PolyB, 0, Sigma, 0, Math.Min(PolyB.Length, Sigma.Length));

		    return ErrorCount;
		}

        // Chien search - это быстрый алгоритм для определения корней многочленов, определенных над конечным полем.
        // Наиболее типичным использованием поиска Цзянь является нахождение корней полиномов локатора ошибок, встречающихся при декодировании кодов Рида-Соломона и кодов BCH.
        private static Boolean ChienSearch(Int32[]	ErrorPosition, Int32 DataLength, Int32 ErrorCount, Int32[]	Sigma)
		{
            // Последняя ошибка
            Int32 LastPosition = Sigma[1];

            // Одна ошибка
            if (ErrorCount == 1)
			{
                // Положение вне диапазона
                if (IntToExp[LastPosition] >= DataLength) return false;

                // Сохранение единственной позиции ошибки в массиве позиций
                ErrorPosition[0] = LastPosition;
			    return true;
			}

            // Мы начинаем с последней позиции ошибки
            Int32 PosIndex = ErrorCount - 1;
		    for (int DataIndex = 0; DataIndex < DataLength; DataIndex++)
			{
                Int32 DataIndexInverse = 255 - DataIndex;
                Int32 Total = 1;
			    for (int Index = 1; Index <= ErrorCount; Index++) Total ^= MultiplyIntByExp(Sigma[Index], (DataIndexInverse * Index) % 255);
			    if (Total != 0) continue;

                Int32 Position = ExpToInt[DataIndex];
			    LastPosition ^=  Position;
			    ErrorPosition[PosIndex--] = Position;
			    if (PosIndex == 0)
				{
                    // Положение вне диапазона
                    if (IntToExp[LastPosition] >= DataLength) return false;
				    ErrorPosition[0] = LastPosition;
				    return true;
				}
			}

            // Поиски не увенчались успехом
            return false;
		}

	    private static void ApplyCorrection(Byte[] ReceivedData, Int32 DataLength, Int32 ErrorCount, Int32[]	ErrorPosition, Int32[]	Sigma, Int32[]	Omega)
		{
		    for (int ErrIndex = 0; ErrIndex < ErrorCount; ErrIndex++)
			{
                Int32 ps = ErrorPosition[ErrIndex];
                Int32 zlog = 255 - IntToExp[ps];
                Int32 OmegaTotal = Omega[0];
			    for (int Index = 1; Index < ErrorCount; Index++) OmegaTotal ^= MultiplyIntByExp(Omega[Index], (zlog * Index) % 255);
                Int32 SigmaTotal = Sigma[1];
			    for (int j = 2; j < ErrorCount; j += 2) SigmaTotal ^= MultiplyIntByExp(Sigma[j + 1], (zlog * j) % 255);
			    ReceivedData[DataLength - 1 - IntToExp[ps]] ^= (byte) MultiplyDivide(ps, OmegaTotal, SigmaTotal);
			}

		    return;
		}

	    internal static void PolynominalDivision(Byte[] Polynomial, Int32 PolyLength, Byte[] Generator, Int32 ErrCorrCodewords)
		{
            Int32 DataCodewords = PolyLength - ErrCorrCodewords;

            // Коррекция ошибок полиномиальное деление
            for (int Index = 0; Index < DataCodewords; Index++)
			{
                // Текущее первое кодовое слово равно нулю
                if (Polynomial[Index] == 0) continue;

                // Текущее первое кодовое слово не равно нулю
                Int32 Multiplier = IntToExp[Polynomial[Index]];

                // Петля для коэффициентов коррекции ошибок
                for (int GeneratorIndex = 0; GeneratorIndex < ErrCorrCodewords; GeneratorIndex++)
				{
				    Polynomial[Index + 1 + GeneratorIndex] = (byte) (Polynomial[Index + 1 + GeneratorIndex] ^ ExpToInt[Generator[GeneratorIndex] + Multiplier]);
				}
			}

		    return;
		}

	    internal static Int32 Multiply(Int32 Int1, Int32 Int2)
		{
		    return (Int1 == 0 || Int2 == 0) ? 0 : ExpToInt[IntToExp[Int1] + IntToExp[Int2]];
		}

	    internal static Int32 MultiplyIntByExp(Int32 Int, Int32 Exp)
		{
		    return Int == 0 ? 0 : ExpToInt[IntToExp[Int] + Exp];
		}

	    internal static Int32 MultiplyDivide(Int32 Int1, Int32 Int2, Int32 Int3)
		{
		    return (Int1 == 0 || Int2 == 0) ? 0 : ExpToInt[(IntToExp[Int1] + IntToExp[Int2] - IntToExp[Int3] + 255) % 255];
		}

	    internal static Int32 DivideIntByExp(Int32 Int, Int32 Exp)
		{
		    return Int == 0 ? 0 : ExpToInt[IntToExp[Int] - Exp + 255];
		}

	    internal static void PolynomialMultiply(Int32[] Result, Int32[] Poly1, Int32[] Poly2)
		{
		    Array.Clear(Result, 0, Result.Length);
		    for (int Index1 = 0; Index1 < Poly1.Length; Index1++)
			    {
			    if (Poly1[Index1] == 0) continue;
                Int32 loga = IntToExp[Poly1[Index1]];
                Int32 Index2End = Math.Min(Poly2.Length, Result.Length - Index1);
			    // = Sum(Poly1[Index1] * Poly2[Index2]) for all Index2
			    for (int Index2 = 0; Index2 < Index2End; Index2++)
				    if(Poly2[Index2] != 0) Result[Index1 + Index2] ^= ExpToInt[loga + IntToExp[Poly2[Index2]]];
			    }

		    return;
		}
	}
}
