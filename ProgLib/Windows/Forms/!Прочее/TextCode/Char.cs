using System;

namespace ProgLib.Windows.Forms
{
    /// <summary>
    /// Char and style
    /// </summary>
    public struct TextCodeChar
    {
        /// <summary>
        /// Unicode character
        /// </summary>
        public char c;
        /// <summary>
        /// Style bit mask
        /// </summary>
        /// <remarks>Bit 1 in position n means that this char will rendering by FastColoredTextBox.Styles[n]</remarks>
        public StyleIndex style;

        public TextCodeChar(char c)
        {
            this.c = c;
            style = StyleIndex.None;
        }
    }
}
