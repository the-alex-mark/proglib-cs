using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms
{
    public static class FormExpansion
    {
        /// <summary>
        /// Возвращает дубликат элемента управления
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Control"></param>
        /// <returns></returns>
        public static T Clone<T>(this T Control) where T : Control
        {
            T Result = (T)Activator.CreateInstance(Control.GetType());

            // Дублирование всех свойств
            PropertyDescriptorCollection _properties = TypeDescriptor.GetProperties(Control);

            foreach (PropertyDescriptor _property in _properties)
            {
                if (!_property.IsReadOnly)
                {
                    if (_property.Name != "WindowTarget")
                        _property.SetValue(Result, _property.GetValue(Control));
                }
            }

            // Перебор вложенных элементов управления
            foreach (Control _control in Control.Controls)
            {
                Control Child = _control.Clone();
                Child.Parent = Result;
            }

            return Result;
        }

        /// <summary>
        /// Возвращает <see cref="ContentAlignment"/> в виде <see cref="TextFormatFlags"/>
        /// </summary>
        /// <param name="Alignment"></param>
        /// <returns></returns>
        public static TextFormatFlags ToTextFormatFlags(this ContentAlignment Alignment)
        {
            switch (Alignment)
            {
                case ContentAlignment.BottomLeft: return TextFormatFlags.Bottom | TextFormatFlags.Left;
                case ContentAlignment.BottomCenter: return TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.BottomRight: return TextFormatFlags.Bottom | TextFormatFlags.Right;
                case ContentAlignment.MiddleLeft: return TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                case ContentAlignment.MiddleCenter: return TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.MiddleRight: return TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                case ContentAlignment.TopLeft: return TextFormatFlags.Top | TextFormatFlags.Left;
                case ContentAlignment.TopCenter: return TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.TopRight: return TextFormatFlags.Top | TextFormatFlags.Right;
            }
            throw new InvalidEnumArgumentException();
        }
    }
}
