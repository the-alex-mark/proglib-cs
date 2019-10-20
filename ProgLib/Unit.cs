using System;
using System.Linq;

namespace ProgLib
{
    public class Unit
    {
        // -- Системы счисления ------------------------------------------------------------------------------------------------------------------------------------------

        private static Int32 ToNumber(Char Number)
        {
            switch (Number)
            {
                case '1': return 1;
                case '2': return 2;
                case '3': return 3;
                case '4': return 4;
                case '5': return 5;
                case '6': return 6;
                case '7': return 7;
                case '8': return 8;
                case '9': return 9;

                case 'A': return 10;
                case 'B': return 11;
                case 'C': return 12;
                case 'D': return 13;
                case 'E': return 14;
                case 'F': return 15;

                default: return 0;
            }
        }

        // Decimal
        public static Binary DecimalToBinary(Decimal Value)
        {
            String _result = Convert.ToString((int)Value, 2);
            return (from i in _result.ToArray() select Convert.ToInt32(i - '0')).ToArray();
        }
        public static Octal DecimalToOctal(Decimal Value)
        {
            return Convert.ToInt32(Convert.ToString((int)Value, 8));
        }
        public static Hexadecimal DecimalToHexadecimal(Decimal Value)
        {
            return Convert.ToString((int)Value, 16);
        }

        // Binary
        public static Decimal BinaryToDecimal(Binary Value)
        {
            Decimal Decimal = 0;
            for (int i = 0; i < Value.Length; i++)
                Decimal += Value.Values[(Value.Length - 1) - i] * (int)Math.Pow(2, i);

            return Decimal;
        }
        public static Octal BinaryToOctal(Binary Value)
        {
            return DecimalToOctal(BinaryToDecimal(Value));
        }
        public static Hexadecimal BinaryToHexadecimal(Binary Value)
        {
            return DecimalToHexadecimal(BinaryToDecimal(Value));
        }

        // Octal
        public static Decimal OctalToDecimal(Octal Value)
        {
            String _value = Value.ToString();
            Decimal Decimal = 0;
            for (int i = 0; i < _value.Length; i++)
                Decimal += Convert.ToInt32(_value[(_value.Length - 1) - i] - '0') * (int)Math.Pow(8, i);

            return Decimal;
        }
        public static Binary OctalToBinary(Octal Value)
        {
            return DecimalToBinary(OctalToDecimal(Value));
        }
        public static Hexadecimal OctalToHexadecimal(Octal Value)
        {
            return DecimalToHexadecimal(OctalToDecimal(Value));
        }
        
        // Hexadecimal
        public static Decimal HexadecimalToDecimal(Hexadecimal Value)
        {
            Decimal Decimal = 0;
            for (int i = 0; i < Value.Value.Length; i++)
                Decimal += ToNumber(Value.Value[(Value.Value.Length - 1) - i]) * (int)Math.Pow(16, i);

            return Decimal;
        }
        public static Binary HexadecimalToBinary(Hexadecimal Value)
        {
            return DecimalToBinary(HexadecimalToDecimal(Value));
        }
        public static Hexadecimal HexadecimalToHexadecimal(Hexadecimal Value)
        {
            return DecimalToHexadecimal(HexadecimalToDecimal(Value));
        }



        // -- Типы данных ------------------------------------------------------------------------------------------------------------------------------------------------

        // Эксабайт EB
        public static Double ExabyteToPetabyte(Double Value)
        {
            return Value * Math.Pow(1024, 1);
        }
        public static Double ExabyteToTerabyte(Double Value)
        {
            return Value * Math.Pow(1024, 2);
        }
        public static Double ExabyteToGigabyte(Double Value)
        {
            return Value * Math.Pow(1024, 3);
        }
        public static Double ExabyteToMegabyte(Double Value)
        {
            return Value * Math.Pow(1024, 4);
        }
        public static Double ExabyteToKilobyte(Double Value)
        {
            return Value * Math.Pow(1024, 5);
        }
        public static Double ExabyteToByte(Double Value)
        {
            return Value * Math.Pow(1024, 6);
        }
        
        public static Double ExabyteToExabite(Double Value)
        {
            return Value * 8;
        }
        public static Double ExabyteToPetabit(Double Value)
        {
            return ExabyteToPetabyte(Value) * 8;
        }
        public static Double ExabyteToTerabit(Double Value)
        {
            return ExabyteToTerabyte(Value) * 8;
        }
        public static Double ExabyteToGigabit(Double Value)
        {
            return ExabyteToGigabyte(Value) * 8;
        }
        public static Double ExabyteToMegabit(Double Value)
        {
            return ExabyteToMegabyte(Value) * 8;
        }
        public static Double ExabyteToKilobit(Double Value)
        {
            return ExabyteToKilobyte(Value) * 8;
        }
        public static Double ExabyteToBit(Double Value)
        {
            return ExabyteToByte(Value) * 8;
        }

        // Петабайт PB
        public static Double PetabyteToExabyte(Double Value)
        {
            return Value / Math.Pow(1024, 1);
        }
        public static Double PetabyteToTerabyte(Double Value)
        {
            return Value * Math.Pow(1024, 1);
        }
        public static Double PetabyteToGigabyte(Double Value)
        {
            return Value * Math.Pow(1024, 2);
        }
        public static Double PetabyteToMegabyte(Double Value)
        {
            return Value * Math.Pow(1024, 3);
        }
        public static Double PetabyteToKilobyte(Double Value)
        {
            return Value * Math.Pow(1024, 4);
        }
        public static Double PetabyteToByte(Double Value)
        {
            return Value * Math.Pow(1024, 5);
        }
        
        public static Double PetabyteToExabite(Double Value)
        {
            return PetabyteToExabyte(Value) * 8;
        }
        public static Double PetabyteToPetabit(Double Value)
        {
            return Value * 8;
        }
        public static Double PetabyteToTerabit(Double Value)
        {
            return PetabyteToTerabyte(Value) * 8;
        }
        public static Double PetabyteToGigabit(Double Value)
        {
            return PetabyteToGigabyte(Value) * 8;
        }
        public static Double PetabyteToMegabit(Double Value)
        {
            return PetabyteToMegabyte(Value) * 8;
        }
        public static Double PetabyteToKilobit(Double Value)
        {
            return PetabyteToKilobyte(Value) * 8;
        }
        public static Double PetabyteToBit(Double Value)
        {
            return PetabyteToByte(Value) * 8;
        }

        // Терабайт TB
        public static Double TerabyteToExabyte(Double Value)
        {
            return Value / Math.Pow(1024, 2);
        }
        public static Double TerabyteToPetabyte(Double Value)
        {
            return Value / Math.Pow(1024, 1);
        }
        public static Double TerabyteToGigabyte(Double Value)
        {
            return Value * Math.Pow(1024, 1);
        }
        public static Double TerabyteToMegabyte(Double Value)
        {
            return Value * Math.Pow(1024, 2);
        }
        public static Double TerabyteToKilobyte(Double Value)
        {
            return Value * Math.Pow(1024, 3);
        }
        public static Double TerabyteToByte(Double Value)
        {
            return Value * Math.Pow(1024, 4);
        }
        
        public static Double TerabyteToExabite(Double Value)
        {
            return TerabyteToExabyte(Value) * 8;
        }
        public static Double TerabyteToPetabit(Double Value)
        {
            return TerabyteToPetabyte(Value) * 8;
        }
        public static Double TerabyteToTerabit(Double Value)
        {
            return Value * 8;
        }
        public static Double TerabyteToGigabit(Double Value)
        {
            return TerabyteToGigabyte(Value) * 8;
        }
        public static Double TerabyteToMegabit(Double Value)
        {
            return TerabyteToMegabyte(Value) * 8;
        }
        public static Double TerabyteToKilobit(Double Value)
        {
            return TerabyteToKilobyte(Value) * 8;
        }
        public static Double TerabyteToBit(Double Value)
        {
            return TerabyteToByte(Value) * 8;
        }

        // Гигабайт GB
        public static Double GigabyteToExabyte(Double Value)
        {
            return Value / Math.Pow(1024, 3);
        }
        public static Double GigabyteToPetabyte(Double Value)
        {
            return Value / Math.Pow(1024, 2);
        }
        public static Double GigabyteToTerabyte(Double Value)
        {
            return Value / Math.Pow(1024, 1);
        }
        public static Double GigabyteToMegabyte(Double Value)
        {
            return Value * Math.Pow(1024, 1);
        }
        public static Double GigabyteToKilobyte(Double Value)
        {
            return Value * Math.Pow(1024, 2);
        }
        public static Double GigabyteToByte(Double Value)
        {
            return Value * Math.Pow(1024, 3);
        }
        
        public static Double GigabyteToExabite(Double Value)
        {
            return GigabyteToExabyte(Value) * 8;
        }
        public static Double GigabyteToPetabit(Double Value)
        {
            return GigabyteToPetabyte(Value) * 8;
        }
        public static Double GigabyteToTerabit(Double Value)
        {
            return GigabyteToTerabyte(Value) * 8;
        }
        public static Double GigabyteToGigabit(Double Value)
        {
            return Value * 8;
        }
        public static Double GigabyteToMegabit(Double Value)
        {
            return GigabyteToMegabyte(Value) * 8;
        }
        public static Double GigabyteToKilobit(Double Value)
        {
            return GigabyteToKilobyte(Value) * 8;
        }
        public static Double GigabyteToBit(Double Value)
        {
            return GigabyteToByte(Value) * 8;
        }

        // Мегабайт MB
        public static Double MegabyteToExabyte(Double Value)
        {
            return Value / Math.Pow(1024, 4);
        }
        public static Double MegabyteToPetabyte(Double Value)
        {
            return Value / Math.Pow(1024, 3);
        }
        public static Double MegabyteToTerabyte(Double Value)
        {
            return Value / Math.Pow(1024, 2);
        }
        public static Double MegabyteToGigabyte(Double Value)
        {
            return Value / Math.Pow(1024, 1);
        }
        public static Double MegabyteToKilobyte(Double Value)
        {
            return Value * Math.Pow(1024, 1);
        }
        public static Double MegabyteToByte(Double Value)
        {
            return Value * Math.Pow(1024, 2);
        }
        
        public static Double MegabyteToExabite(Double Value)
        {
            return MegabyteToExabyte(Value) * 8;
        }
        public static Double MegabyteToPetabit(Double Value)
        {
            return MegabyteToPetabyte(Value) * 8;
        }
        public static Double MegabyteToTerabit(Double Value)
        {
            return MegabyteToTerabyte(Value) * 8;
        }
        public static Double MegabyteToGigabit(Double Value)
        {
            return MegabyteToGigabyte(Value) * 8;
        }
        public static Double MegabyteToMegabit(Double Value)
        {
            return Value * 8;
        }
        public static Double MegabyteToKilobit(Double Value)
        {
            return MegabyteToKilobyte(Value) * 8;
        }
        public static Double MegabyteToBit(Double Value)
        {
            return MegabyteToByte(Value) * 8;
        }

        // Килобайт KB
        public static Double KilobyteToExabyte(Double Value)
        {
            return Value / Math.Pow(1024, 5);
        }
        public static Double KilobyteToPetabyte(Double Value)
        {
            return Value / Math.Pow(1024, 4);
        }
        public static Double KilobyteToTerabyte(Double Value)
        {
            return Value / Math.Pow(1024, 3);
        }
        public static Double KilobyteToGigabyte(Double Value)
        {
            return Value / Math.Pow(1024, 2);
        }
        public static Double KilobyteToMegabyte(Double Value)
        {
            return Value / Math.Pow(1024, 1);
        }
        public static Double KilobyteToByte(Double Value)
        {
            return Value * Math.Pow(1024, 1);
        }
        
        public static Double KilobyteToExabite(Double Value)
        {
            return KilobyteToExabyte(Value) * 8;
        }
        public static Double KilobyteToPetabit(Double Value)
        {
            return KilobyteToPetabyte(Value) * 8;
        }
        public static Double KilobyteToTerabit(Double Value)
        {
            return KilobyteToTerabyte(Value) * 8;
        }
        public static Double KilobyteToGigabit(Double Value)
        {
            return KilobyteToGigabyte(Value) * 8;
        }
        public static Double KilobyteToMegabit(Double Value)
        {
            return KilobyteToMegabyte(Value) * 8;
        }
        public static Double KilobyteToKilobit(Double Value)
        {
            return Value * 8;
        }
        public static Double KilobyteToBit(Double Value)
        {
            return KilobyteToByte(Value) * 8;
        }

        // Байт B
        public static Double ByteToExabyte(Double Value)
        {
            return Value / Math.Pow(1024, 6);
        }
        public static Double ByteToPetabyte(Double Value)
        {
            return Value / Math.Pow(1024, 5);
        }
        public static Double ByteToTerabyte(Double Value)
        {
            return Value / Math.Pow(1024, 4);
        }
        public static Double ByteToGigabyte(Double Value)
        {
            return Value / Math.Pow(1024, 3);
        }
        public static Double ByteToMegabyte(Double Value)
        {
            return Value / Math.Pow(1024, 2);
        }
        public static Double ByteToKilobyte(Double Value)
        {
            return Value / Math.Pow(1024, 1);
        }
        
        public static Double ByteToExabite(Double Value)
        {
            return ByteToExabyte(Value) * 8;
        }
        public static Double ByteToPetabit(Double Value)
        {
            return ByteToPetabyte(Value) * 8;
        }
        public static Double ByteToTerabit(Double Value)
        {
            return ByteToTerabyte(Value) * 8;
        }
        public static Double ByteToGigabit(Double Value)
        {
            return ByteToGigabyte(Value) * 8;
        }
        public static Double ByteToMegabit(Double Value)
        {
            return ByteToMegabyte(Value) * 8;
        }
        public static Double ByteToKilobit(Double Value)
        {
            return ByteToKilobyte(Value) * 8;
        }
        public static Double ByteToBit(Double Value)
        {
            return Value * 8;
        }
        
        // Эксабит Ebit
        public static Double ExabiteToPetabit(Double Value)
        {
            return Value * Math.Pow(1024, 1);
        }
        public static Double ExabiteToTerabit(Double Value)
        {
            return Value * Math.Pow(1024, 2);
        }
        public static Double ExabiteToGigabit(Double Value)
        {
            return Value * Math.Pow(1024, 3);
        }
        public static Double ExabiteToMegabit(Double Value)
        {
            return Value * Math.Pow(1024, 4);
        }
        public static Double ExabiteToKilobit(Double Value)
        {
            return Value * Math.Pow(1024, 5);
        }
        public static Double ExabiteToBit(Double Value)
        {
            return Value * Math.Pow(1024, 6);
        }

        public static Double ExabiteToExabyte(Double Value)
        {
            return Value / 8;
        }
        public static Double ExabiteToPetabyte(Double Value)
        {
            return ExabiteToPetabit(Value) / 8;
        }
        public static Double ExabiteToTerabyte(Double Value)
        {
            return ExabiteToTerabit(Value) / 8;
        }
        public static Double ExabiteToGigabyte(Double Value)
        {
            return ExabiteToGigabit(Value) / 8;
        }
        public static Double ExabiteToMegabyte(Double Value)
        {
            return ExabiteToMegabit(Value) / 8;
        }
        public static Double ExabiteToKilobyte(Double Value)
        {
            return ExabiteToKilobit(Value) / 8;
        }
        public static Double ExabiteToByte(Double Value)
        {
            return ExabiteToBit(Value) / 8;
        }

        // Петабит Pbit
        public static Double PetabitToExabite(Double Value)
        {
            return Value / Math.Pow(1024, 1);
        }
        public static Double PetabitToTerabit(Double Value)
        {
            return Value * Math.Pow(1024, 1);
        }
        public static Double PetabitToGigabit(Double Value)
        {
            return Value * Math.Pow(1024, 2);
        }
        public static Double PetabitToMegabit(Double Value)
        {
            return Value * Math.Pow(1024, 3);
        }
        public static Double PetabitToKilobit(Double Value)
        {
            return Value * Math.Pow(1024, 4);
        }
        public static Double PetabitToBit(Double Value)
        {
            return Value * Math.Pow(1024, 5);
        }

        public static Double PetabitToExabyte(Double Value)
        {
            return PetabitToExabite(Value) / 8;
        }
        public static Double PetabitToPetabyte(Double Value)
        {
            return Value / 8;
        }
        public static Double PetabitToTerabyte(Double Value)
        {
            return PetabitToTerabit(Value) / 8;
        }
        public static Double PetabitToGigabyte(Double Value)
        {
            return PetabitToGigabit(Value) / 8;
        }
        public static Double PetabitToMegabyte(Double Value)
        {
            return PetabitToMegabit(Value) / 8;
        }
        public static Double PetabitToKilobyte(Double Value)
        {
            return PetabitToKilobit(Value) / 8;
        }
        public static Double PetabitToByte(Double Value)
        {
            return PetabitToBit(Value) / 8;
        }

        // Терабит Tbit
        public static Double TerabitToExabite(Double Value)
        {
            return Value / Math.Pow(1024, 2);
        }
        public static Double TerabitToPetabit(Double Value)
        {
            return Value / Math.Pow(1024, 1);
        }
        public static Double TerabitToGigabit(Double Value)
        {
            return Value * Math.Pow(1024, 1);
        }
        public static Double TerabitToMegabit(Double Value)
        {
            return Value * Math.Pow(1024, 2);
        }
        public static Double TerabitToKilobit(Double Value)
        {
            return Value * Math.Pow(1024, 3);
        }
        public static Double TerabitToBit(Double Value)
        {
            return Value * Math.Pow(1024, 4);
        }

        public static Double TerabitToExabyte(Double Value)
        {
            return TerabitToExabite(Value) / 8;
        }
        public static Double TerabitToPetabyte(Double Value)
        {
            return TerabitToPetabit(Value) / 8;
        }
        public static Double TerabitToTerabyte(Double Value)
        {
            return Value / 8;
        }
        public static Double TerabitToGigabyte(Double Value)
        {
            return TerabitToGigabit(Value) / 8;
        }
        public static Double TerabitToMegabyte(Double Value)
        {
            return TerabitToMegabit(Value) / 8;
        }
        public static Double TerabitToKilobyte(Double Value)
        {
            return TerabitToKilobit(Value) / 8;
        }
        public static Double TerabitToByte(Double Value)
        {
            return TerabitToBit(Value) / 8;
        }

        // Гигабит Gbit
        public static Double GigabitToExabite(Double Value)
        {
            return Value / Math.Pow(1024, 3);
        }
        public static Double GigabitToPetabit(Double Value)
        {
            return Value / Math.Pow(1024, 2);
        }
        public static Double GigabitToTerabit(Double Value)
        {
            return Value / Math.Pow(1024, 1);
        }
        public static Double GigabitToMegabit(Double Value)
        {
            return Value * Math.Pow(1024, 1);
        }
        public static Double GigabitToKilobit(Double Value)
        {
            return Value * Math.Pow(1024, 2);
        }
        public static Double GigabitToBit(Double Value)
        {
            return Value * Math.Pow(1024, 3);
        }

        public static Double GigabitToExabyte(Double Value)
        {
            return GigabitToExabite(Value) / 8;
        }
        public static Double GigabitToPetabyte(Double Value)
        {
            return GigabitToPetabit(Value) / 8;
        }
        public static Double GigabitToTerabyte(Double Value)
        {
            return GigabitToTerabit(Value) / 8;
        }
        public static Double GigabitToGigabyte(Double Value)
        {
            return Value / 8;
        }
        public static Double GigabitToMegabyte(Double Value)
        {
            return GigabitToMegabit(Value) / 8;
        }
        public static Double GigabitToKilobyte(Double Value)
        {
            return GigabitToKilobit(Value) / 8;
        }
        public static Double GigabitToByte(Double Value)
        {
            return GigabitToBit(Value) / 8;
        }

        // Мегабит Mbit
        public static Double MegabitToExabite(Double Value)
        {
            return Value / Math.Pow(1024, 4);
        }
        public static Double MegabitToPetabit(Double Value)
        {
            return Value / Math.Pow(1024, 3);
        }
        public static Double MegabitToTerabit(Double Value)
        {
            return Value / Math.Pow(1024, 2);
        }
        public static Double MegabitToGigabit(Double Value)
        {
            return Value / Math.Pow(1024, 1);
        }
        public static Double MegabitToKilobit(Double Value)
        {
            return Value * Math.Pow(1024, 1);
        }
        public static Double MegabitToBit(Double Value)
        {
            return Value * Math.Pow(1024, 2);
        }

        public static Double MegabitToExabyte(Double Value)
        {
            return MegabitToExabite(Value) / 8;
        }
        public static Double MegabitToPetabyte(Double Value)
        {
            return MegabitToPetabit(Value) / 8;
        }
        public static Double MegabitToTerabyte(Double Value)
        {
            return MegabitToTerabit(Value) / 8;
        }
        public static Double MegabitToGigabyte(Double Value)
        {
            return MegabitToGigabit(Value) / 8;
        }
        public static Double MegabitToMegabyte(Double Value)
        {
            return Value / 8;
        }
        public static Double MegabitToKilobyte(Double Value)
        {
            return MegabitToKilobit(Value) / 8;
        }
        public static Double MegabitToByte(Double Value)
        {
            return MegabitToBit(Value) / 8;
        }

        // Килобит Kbit
        public static Double KilobitToExabite(Double Value)
        {
            return Value / Math.Pow(1024, 5);
        }
        public static Double KilobitToPetabit(Double Value)
        {
            return Value / Math.Pow(1024, 4);
        }
        public static Double KilobitToTerabit(Double Value)
        {
            return Value / Math.Pow(1024, 3);
        }
        public static Double KilobitToGigabit(Double Value)
        {
            return Value / Math.Pow(1024, 2);
        }
        public static Double KilobitToMegabit(Double Value)
        {
            return Value / Math.Pow(1024, 1);
        }
        public static Double KilobitToBit(Double Value)
        {
            return Value * Math.Pow(1024, 1);
        }

        public static Double KilobitToExabyte(Double Value)
        {
            return KilobitToExabite(Value) / 8;
        }
        public static Double KilobitToPetabyte(Double Value)
        {
            return KilobitToPetabit(Value) / 8;
        }
        public static Double KilobitToTerabyte(Double Value)
        {
            return KilobitToTerabit(Value) / 8;
        }
        public static Double KilobitToGigabyte(Double Value)
        {
            return KilobitToGigabit(Value) / 8;
        }
        public static Double KilobitToMegabyte(Double Value)
        {
            return KilobitToMegabit(Value) / 8;
        }
        public static Double KilobitToKilobyte(Double Value)
        {
            return Value / 8;
        }
        public static Double KilobitToByte(Double Value)
        {
            return KilobitToBit(Value) / 8;
        }

        // Бит Bit
        public static Double BitToExabite(Double Value)
        {
            return Value / Math.Pow(1024, 6);
        }
        public static Double BitToPetabit(Double Value)
        {
            return Value / Math.Pow(1024, 5);
        }
        public static Double BitToTerabit(Double Value)
        {
            return Value / Math.Pow(1024, 4);
        }
        public static Double BitToGigabit(Double Value)
        {
            return Value / Math.Pow(1024, 3);
        }
        public static Double BitToMegabit(Double Value)
        {
            return Value / Math.Pow(1024, 2);
        }
        public static Double BitToKilobit(Double Value)
        {
            return Value / Math.Pow(1024, 1);
        }

        public static Double BitToExabyte(Double Value)
        {
            return BitToExabite(Value) / 8;
        }
        public static Double BitToPetabyte(Double Value)
        {
            return BitToPetabit(Value) / 8;
        }
        public static Double BitToTerabyte(Double Value)
        {
            return BitToTerabit(Value) / 8;
        }
        public static Double BitToGigabyte(Double Value)
        {
            return BitToGigabit(Value) / 8;
        }
        public static Double BitToMegabyte(Double Value)
        {
            return BitToMegabit(Value) / 8;
        }
        public static Double BitToKilobyte(Double Value)
        {
            return BitToKilobit(Value) / 8;
        }
        public static Double BitToByte(Double Value)
        {
            return Value / 8;
        }



        // -- Температура ------------------------------------------------------------------------------------------------------------------------------------------------

        // Градусы Цельсия °C
        public static Double CelsiusToFahrenheit(Double Value)
        {
            return Value * 9 / 5 + 32;
        }
        public static Double CelsiusToKelvin(Double Value)
        {
            return Value + 273.15;
        }

        // Градус Фаренгейта °F
        public static Double FahrenheitToCelsius(Double Value)
        {
            return ((Value - 32) * 5) / 9;
        }
        public static Double FahrenheitToKelvin(Double Value)
        {
            return FahrenheitToCelsius(Value) + 273.15;
        }

        // Кельвин K
        public static Double KelvinToCelsius(Double Value)
        {
            return Value - 273.15;
        }
        public static Double KelvinToFahrenheit(Double Value)
        {
            return CelsiusToFahrenheit(KelvinToCelsius(Value));
        }
    }
}