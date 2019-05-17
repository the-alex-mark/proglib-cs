using System;
using System.IO;
using System.Text;

namespace ProgLib.IO
{
    /// <summary>
    /// Предоставляет изменяемый массив байтов.
    /// Этот класс не наследуется.
    /// </summary>
    public sealed class BytesBuilder : IDisposable
    {
        /// <summary>
        /// Инициализирует пустой конструктор, готовый к заполнению.
        /// </summary>
        public BytesBuilder()
        {

        }

        /// <summary>
        /// Инициализирует конструктор из байтового набора данных.
        /// </summary>
        /// <param name="data"></param>
        public BytesBuilder(Byte[] data)
        {
            _store.Close();
            _store.Dispose();
            _store = new MemoryStream(data);
        }

        /// <summary>
        /// Инициализирует конструктор из строкового представления (Base64) существующего экземпляра.
        /// </summary>
        /// <param name="base64"></param>
        public BytesBuilder(String base64)
        {
            _store.Close();
            _store.Dispose();
            _store = new MemoryStream(Convert.FromBase64String(base64));
        }

        #region Constants

        /// <summary>
        /// "true" в двоичной форме
        /// </summary>
        private const byte _streamTrue = (byte)1;
        /// <summary>
        /// "false" в байтовой форме
        /// </summary>
        private const byte _streamFalse = (byte)0;

        #endregion

        #region Fields

        /// <summary>
        /// Содержит фактические байты
        /// </summary>
        private MemoryStream _store = new MemoryStream();
        
        #endregion

        #region Properties

        /// <summary>
        /// Количество байт
        /// </summary>
        public int Length
        {
            get { return (int)_store.Length; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Добавьте строку необработанных байтов в хранилище
        /// </summary>
        /// <param name="b"></param>
        private void AddBytes(byte[] b)
        {
            _store.Write(b, 0, b.Length);
        }

        private void AddBytes(byte[] b, int coint)
        {
            _store.Write(b, 0, coint);
        }

        /// <summary>
        /// Reads a specific number of bytes from the store
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private byte[] GetBytes(int length)
        {
            byte[] data = new byte[length];
            if (length > 0)
            {
                int read = _store.Read(data, 0, length);
                if (read != length)
                {
                    throw new ApplicationException("Buffer did not contain " + length + " bytes");
                }
            }
            return data;
        }

        #endregion
        
        /// <summary>
        /// Добавляет данные типа <see cref="Boolean"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        public void Append(Boolean Value)
        {
            _store.WriteByte(Value ? _streamTrue : _streamFalse);
        }

        /// <summary>
        /// Добавляет данные типа <see cref="Byte"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        public void Append(Byte Value)
        {
            _store.WriteByte(Value);
        }

        /// <summary>
        /// Добавляет массив типа <see cref="Byte"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="AddLength">Если "true" - длина добавляется перед значением
        /// (это позволяет извлекать отдельные элементы обратно в исходную форму ввода).
        /// </param>
        public void Append(Byte[] Value, Boolean AddLength = true)
        {
            if (AddLength) Append(Value.Length);
            AddBytes(Value);
        }

        /// <summary>
        /// Добавляет массив типа <see cref="Byte"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Count">Количество добавляемых байтов</param>
        /// <param name="AddLength">Если "true" - длина добавляется перед значением
        /// (это позволяет извлекать отдельные элементы обратно в исходную форму ввода).</param>
        public void Append(Byte[] Value, Int32 Count, Boolean AddLength = true)
        {
            if (AddLength) Append(Value.Length);
            AddBytes(Value, Count);
        }

        /// <summary>
        /// Добавляет данные типа <see cref="Char"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        public void Append(Char Value)
        {
            _store.WriteByte((byte)Value);
        }

        /// <summary>
        /// Добавляет массив типа <see cref="Char"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="AddLength">Если "true" - длина добавляется перед значением
        /// (это позволяет извлекать отдельные элементы обратно в исходную форму ввода).</param>
        public void Append(Char[] Value, Boolean AddLength = true)
        {
            if (AddLength) Append(Value.Length);
            Append(Encoding.Unicode.GetBytes(Value));
        }

        /// <summary>
        /// Добавляет данные типа <see cref="DateTime"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        public void Append(DateTime Value)
        {
            Append(Value.Ticks);
        }

        /// <summary>
        /// Добавляет данные типа <see cref="Decimal"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        public void Append(Decimal Value)
        {
            // GetBits always returns four ints.
            // We store them in a specific order so that they can be recovered later.
            int[] bits = decimal.GetBits(Value);
            Append(bits[0]);
            Append(bits[1]);
            Append(bits[2]);
            Append(bits[3]);
        }

        /// <summary>
        /// Добавляет данные типа <see cref="Double"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        public void Append(Double Value)
        {
            AddBytes(BitConverter.GetBytes(Value));
        }

        /// <summary>
        /// Добавляет данные типа <see cref="Single"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        public void Append(Single Value)
        {
            AddBytes(BitConverter.GetBytes(Value));
        }

        /// <summary>
        /// Добавляет данные типа <see cref="Guid"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        public void Append(Guid Value)
        {
            Append(Value.ToByteArray());
        }

        /// <summary>
        /// Добавляет данные типа <see cref="Int16"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        public void Append(Int16 Value)
        {
            AddBytes(BitConverter.GetBytes(Value));
        }

        /// <summary>
        /// Добавляет данные типа <see cref="Int32"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        public void Append(Int32 Value)
        {
            AddBytes(BitConverter.GetBytes(Value));
        }

        /// <summary>
        /// Добавляет данные типа <see cref="Int64"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        public void Append(Int64 Value)
        {
            AddBytes(BitConverter.GetBytes(Value));
        }

        /// <summary>
        /// Добавляет данные типа <see cref="String"/> в текущий поток.
        /// </summary>
        /// <param name="Value">Value to append to existing builder data</param>
        /// <param name="AddLength">Если "true" - длина добавляется перед значением
        /// (это позволяет извлекать отдельные элементы обратно в исходную форму ввода).</param>
        public void Append(String Value, Boolean AddLength = true)
        {
            byte[] data = Encoding.Unicode.GetBytes(Value);
            if (AddLength) Append(data.Length);
            AddBytes(data);
        }

        /// <summary>
        /// Добавляет данные типа <see cref="Int64"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        public void Append(UInt16 Value)
        {
            AddBytes(BitConverter.GetBytes(Value));
        }

        /// <summary>
        /// Добавляет данные типа <see cref="UInt32"/> в текущий поток.
        /// </summary>
        /// <param name="Value"></param>
        public void Append(UInt32 Value)
        {
            AddBytes(BitConverter.GetBytes(Value));
        }

        /// <summary>
        /// Добавляет данные типа <see cref="UInt64"/> в текущий поток.
        /// </summary>
        /// <param name="ul"></param>
        public void Append(UInt64 Value)
        {
            AddBytes(BitConverter.GetBytes(Value));
        }

        /// <summary>
        /// Получает данные типа <see cref="Boolean"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public Boolean GetBoolean()
        {
            return _store.ReadByte() == _streamTrue;
        }

        /// <summary>
        /// Получает данные типа <see cref="Byte"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public Byte GetByte()
        {
            return (byte)_store.ReadByte();
        }

        /// <summary>
        /// Получает массив типа <see cref="Byte"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public Byte[] GetByteArray()
        {
            int length = GetInt();
            return GetBytes(length);
        }

        /// <summary>
        /// Получает данные типа <see cref="Char"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public Char GetChar()
        {
            return (char)_store.ReadByte();
        }

        /// <summary>
        /// Получает массив типа <see cref="Char"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public Char[] GetCharArray()
        {
            int length = GetInt();
            return System.Text.Encoding.Unicode.GetChars(GetBytes(length));
        }

        /// <summary>
        /// Получает данные типа <see cref="DateTime"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateTime()
        {
            return new DateTime(GetLong());
        }

        /// <summary>
        /// Получает данные типа <see cref="Decimal"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public Decimal GetDecimal()
        {
            // GetBits always returns four ints.
            // We store them in a specific order so that they can be recovered later.
            int[] bits = new int[] { GetInt(), GetInt(), GetInt(), GetInt() };
            return new decimal(bits);
        }

        /// <summary>
        /// Получает данные типа <see cref="Double"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public Double GetDouble()
        {
            return BitConverter.ToDouble(GetBytes(8), 0);
        }

        /// <summary>
        /// Получает данные типа <see cref="Single"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public Single GetFloat()
        {
            return BitConverter.ToSingle(GetBytes(4), 0);
        }

        /// <summary>
        /// Получает данные типа <see cref="Guid"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public Guid GetGuid()
        {
            return new Guid(GetByteArray());
        }

        /// <summary>
        /// Получает данные типа <see cref="Int16"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public Int16 GetShort()
        {
            return BitConverter.ToInt16(GetBytes(2), 0);
        }

        /// <summary>
        /// Получает данные типа <see cref="Int32"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public Int32 GetInt()
        {
            return BitConverter.ToInt32(GetBytes(4), 0);
        }

        /// <summary>
        /// Получает данные типа <see cref="Int64"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public Int64 GetLong()
        {
            return BitConverter.ToInt64(GetBytes(8), 0);
        }

        /// <summary>
        /// Получает данные типа <see cref="String"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public String GetString()
        {
            int length = GetInt();
            return System.Text.Encoding.Unicode.GetString(GetBytes(length));
        }

        /// <summary>
        /// Получает данные типа <see cref="UInt16"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public UInt16 GetUshort()
        {
            return BitConverter.ToUInt16(GetBytes(2), 0);
        }

        /// <summary>
        /// Получает данные типа <see cref="UInt32"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public UInt32 GetUint()
        {
            return BitConverter.ToUInt32(GetBytes(4), 0);
        }

        /// <summary>
        /// Получает данные типа <see cref="UInt64"/> из текущего потока.
        /// </summary>
        /// <returns></returns>
        public UInt64 GetUlong()
        {
            return BitConverter.ToUInt64(GetBytes(8), 0);
        }

        /// <summary>
        /// Очищает данные.
        /// </summary>
        public void Clear()
        {
            _store.Close();
            _store.Dispose();
            _store = new MemoryStream();
        }

        /// <summary>
        /// Задаёт начальное значение для положения в текущем потоке.
        /// </summary>
        public void Rewind()
        {
            _store.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Задаёт указанное значение для положения в текущем потоке.
        /// WARNING:
        /// При добавлении в построитель объектов переменного размера результаты чтения после поиска ненулевого значения непредсказуемы.
        /// Построитель хранит не только объекты - для некоторых он также хранит дополнительную информацию.
        /// </summary>
        /// <param name="position"></param>
        public void Seek(int position)
        {
            _store.Seek((long)position, SeekOrigin.Begin);
        }

        /// <summary>
        /// Возвращает поток в виде массива байтов.
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            byte[] data = new byte[Length];
            Array.Copy(_store.GetBuffer(), data, Length);
            return data;
        }

        /// <summary>
        /// Возвращает данные класса <see cref="BytesBuilder"/> в строковом представлении (Base64).
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Convert.ToBase64String(ToArray());
        }
                
        /// <summary>
        /// Освобождает все ресурсы, используемые объектом <see cref="BytesBuilder"/>.
        /// </summary>
        public void Dispose()
        {
            _store.Close();
            _store.Dispose();
        }
    }
}
