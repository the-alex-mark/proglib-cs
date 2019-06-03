using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Drawing
{
    public class FullColor
    {
        public FullColor(KnownColor KnownColor)
        {
            this.Value = Color.FromKnownColor(KnownColor);
        }

        public FullColor(Color BaseColor)
        {
            this.Value = BaseColor;
        }

        #region Properties

        private Color Value
        {
            get;
            set;
        }

        /// <summary>
        /// Представляет цвет, являющийся значением null.
        /// </summary>
        public static readonly FullColor Empty = default(FullColor);

        /// <summary>
        /// Получает значение Alpha этой структуры <see cref="FullColor"/>.
        /// </summary>
        public Byte Alpha
        {
            get { return Value.A; }
        }

        /// <summary>
        /// Получает значение Red этой структуры <see cref="FullColor"/>.
        /// </summary>
        public Byte Red
        {
            get { return Value.R; }
        }

        /// <summary>
        /// Получает значение Green этой структуры <see cref="FullColor"/>.
        /// </summary>
        public Byte Green
        {
            get { return Value.G; }
        }

        /// <summary>
        /// Получает значение Blue этой структуры <see cref="FullColor"/>.
        /// </summary>
        public Byte Blue
        {
            get { return Value.B; }
        }

        /// <summary>
        /// Получает значение Hue этой структуры <see cref="FullColor"/>.
        /// </summary>
        public Int32 Hue
        {
            get { return Convert.ToHSB(Value).H; }
        }

        /// <summary>
        /// Получает значение Saturation этой структуры <see cref="FullColor"/>.
        /// </summary>
        public Double Saturation
        {
            get { return Convert.ToHSB(Value).S; }
        }

        /// <summary>
        /// Получает значение Brightness этой структуры <see cref="FullColor"/>.
        /// </summary>
        public Double Brightness
        {
            get { return Convert.ToHSB(Value).B; }
        }

        /// <summary>
        /// Получает значение Lightness этой структуры <see cref="FullColor"/>.
        /// </summary>
        public Double Lightness
        {
            get { return Convert.ToHSL(Value).L; }
        }

        /// <summary>
        /// Получает значение Cyan этой структуры <see cref="FullColor"/>.
        /// </summary>
        public Int32 Cyan
        {
            get { return Convert.ToCMYK(Value).C; }
        }

        /// <summary>
        /// Получает значение Magenta этой структуры <see cref="FullColor"/>.
        /// </summary>
        public Int32 Magenta
        {
            get { return Convert.ToCMYK(Value).M; }
        }

        /// <summary>
        /// Получает значение Yellow этой структуры <see cref="FullColor"/>.
        /// </summary>
        public Int32 Yellow
        {
            get { return Convert.ToCMYK(Value).Y; }
        }

        /// <summary>
        /// Получает значение Key этой структуры <see cref="FullColor"/>.
        /// </summary>
        public Int32 Key
        {
            get { return Convert.ToCMYK(Value).K; }
        }
        
        #endregion

        /// <summary>
        /// Создает структуру <see cref="FullColor"/> из указанного, предварительно определенного цвета.
        /// </summary>
        /// <param name="KnownColor">Элемент перечисления <see cref="KnownColor"/>.</param>
        /// <returns></returns>
        public static FullColor FromKnownColor(KnownColor KnownColor)
        {
            return new FullColor(KnownColor);
        }

        /// <summary>
        /// Создает структуру <see cref="FullColor"/> из указанного имени предопределенного цвета.
        /// </summary>
		/// <param name="Name">Строка, которая является именем предопределенного цвета. Допустимые имена те же, что и у элементов перечня <see cref="KnownColor"/>.</param>
		/// <returns></returns>
		public static FullColor FromName(String Name)
        {
            return new FullColor(Color.FromName(Name));
        }

        /// <summary>
        /// Создает структуру <see cref="FullColor"/> из указанных 8-разрядных значений цветов (красный, зеленый, синий).
        /// Значение альфа неявно определено как 255 (полностью непрозрачно).
        /// </summary>
        /// <param name="Red">Значение красного компонента. Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Значение зеленого компонента. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Значение синего компонента. Допустимые значения - от 0 до 255.</param>
        /// <exception cref="ArgumentException">Параметр Red, Green или Blue меньше 0 или больше 255.</exception>
        /// <returns></returns>
        public static FullColor FromArgb(Int32 Red, Int32 Green, Int32 Blue)
        {
            return new FullColor(Color.FromArgb(Red, Green, Blue));
        }

        /// <summary>
        /// Создает структуру <see cref="FullColor"/> из указанной структуры System.Drawing.Color, но с новым определенным значением альфа.
        /// </summary>
        /// <param name="Alpha">Значение альфа для нового цвета System.Drawing.Color. Допустимые значения — от 0 до 255.</param>
        /// <param name="BaseColor">Цвет System.Drawing.Color, на основе которого будет создан новый цвет <see cref="FullColor"/>.</param>
        /// <exception cref="ArgumentException">Alpha меньше 0 или больше 255.</exception>
        /// <returns></returns>
        public static FullColor FromArgb(Int32 Alpha, Color BaseColor)
        {
            return new FullColor(Color.FromArgb(Alpha, BaseColor));
        }

        /// <summary>
        /// Создает структуру <see cref="FullColor"/> из четырех значений компонентов ARGB (альфа, красный, зеленый и синий).
        /// </summary>
        /// <param name="Alpha">Компонент альфа. Допустимые значения — от 0 до 255.</param>
        /// <param name="Red">Красный компонент. Допустимые значения — от 0 до 255.</param>
        /// <param name="Green">Зеленый компонент. Допустимые значения — от 0 до 255.</param>
        /// <param name="Blue">Синий компонент. Допустимые значения — от 0 до 255.</param>
        /// <returns></returns>
        public static FullColor FromArgb(Int32 Alpha, Int32 Red, Int32 Green, Int32 Blue)
        {
            return new FullColor(Color.FromArgb(Alpha, Red, Green, Blue));
        }

        public static FullColor FromHsb(Int32 Hue, Int32 Saturation, Int32 Brightness)
        {
            return new FullColor(Convert.ToColor(new HSB(Hue, Saturation, Brightness)));
        }
    }
}
