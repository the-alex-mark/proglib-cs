using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProgLib.Text
{
    public class StringTable
    {
        public IList<Object> Columns { get; set; }
        public IList<Object[]> Rows { get; protected set; }

        public TableOptions Options { get; protected set; }

        public StringTable(params String[] Columns) : this(new TableOptions { Columns = new List<String>(Columns) })
        {

        }

        public StringTable(DataTable Table)
        {
            if (Table == null) return;

            List<String> Rows;
            List<String> Columns = new List<String>();

            for (int j = 0; j < Table.Columns.Count; j++)
                Columns.Add(Table.Columns[j].ColumnName);

            StringTable T = new StringTable(Columns.ToArray());

            for (int i = 0; i < Table.Rows.Count; i++)
            {
                Rows = new List<String>();

                for (int j = 0; j < Table.Columns.Count; j++)
                    Rows.Add(Table.Rows[i][j].ToString());

                T.AddRow(Rows.ToArray());
            }
            this.Columns = T.Columns;
            this.Rows = T.Rows;
        }

        public StringTable(TableOptions Options)
        {
            this.Options = Options ?? throw new ArgumentNullException("options");
            Rows = new List<object[]>();
            Columns = new List<object>(Options.Columns);
        }
        
        /// <summary>
        /// Добавляет столбцы в таблицу
        /// </summary>
        /// <param name="Names"></param>
        /// <returns></returns>
        public void AddColumn(IEnumerable<String> Names)
        {
            foreach (String Name in Names)
                Columns.Add(Name);
        }

        /// <summary>
        /// Добавляет строку в таблицу
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public void AddRow(params Object[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (!Columns.Any())
                throw new Exception("Добавьте пожалуйста столбцы.");

            if (Columns.Count != values.Length)
                throw new Exception(
                    $"Число столбцов в строке ({Columns.Count}) не соответствует значениям ({values.Length}");

            Rows.Add(values);
        }

        public static StringTable From<T>(IEnumerable<T> values)
        {
            var table = new StringTable();

            var columns = GetColumns<T>();

            table.AddColumn(columns);

            foreach (var propertyValues in values.Select(value => columns.Select(column => GetColumnValue<T>(value, column))))
                table.AddRow(propertyValues.ToArray());

            return table;
        }

        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();

            // найдите самый длинный столбец, выполнив поиск в каждой строке
            var columnLengths = ColumnLengths();

            // создание строкового формата с заполнением
            String format = Enumerable.Range(0, Columns.Count)
                .Select(i => " | {" + i + ",-" + columnLengths[i] + "}")
                .Aggregate((s, a) => s + a) + " |";

            // find the longest formatted line
            var maxRowLength = Math.Max(0, Rows.Any() ? Rows.Max(row => string.Format(format, row).Length) : 0);
            String columnHeaders = string.Format(format, Columns.ToArray());

            // самая длинная строка больше отформатированного заголовка столбца и самой длинной строки
            var longestLine = Math.Max(maxRowLength, columnHeaders.Length);

            // добавить каждую строку
            var results = Rows.Select(row => string.Format(format, row)).ToList();

            // создание разделителя
            String divider = " " + String.Join("", Enumerable.Repeat("-", longestLine - 1)) + " ";

            builder.AppendLine(divider);
            builder.AppendLine(columnHeaders);

            foreach (String row in results)
            {
                builder.AppendLine(divider);
                builder.AppendLine(row);
            }

            builder.AppendLine(divider);

            if (Options.EnableCount)
            {
                builder.AppendLine("");
                builder.AppendFormat(" Count: {0}", Rows.Count);
            }

            return builder.ToString();
        }

        public String ToMarkDownString()
        {
            return ToMarkDownString('|');
        }

        private String ToMarkDownString(Char delimiter)
        {
            var builder = new StringBuilder();

            // найдите самый длинный столбец, выполнив поиск в каждой строке
            var columnLengths = ColumnLengths();

            // создание строкового формата с заполнением
            var format = Format(columnLengths, delimiter);

            // найти самую длинную отформатированную строку
            var columnHeaders = string.Format(format, Columns.ToArray());

            // добавить каждую строку
            var results = Rows.Select(row => string.Format(format, row)).ToList();

            // создание разделителя
            var divider = Regex.Replace(columnHeaders, @"[^|]", "-");

            builder.AppendLine(columnHeaders);
            builder.AppendLine(divider);
            results.ForEach(row => builder.AppendLine(row));

            return builder.ToString();
        }

        public String ToMinimalString()
        {
            return ToMarkDownString(char.MinValue);
        }

        public String ToStringAlternative()
        {
            var builder = new StringBuilder();

            // найдите самый длинный столбец, выполнив поиск в каждой строке
            var columnLengths = ColumnLengths();

            // создание строкового формата с заполнением
            var format = Format(columnLengths);

            // найти самую длинную отформатированную строку
            var columnHeaders = string.Format(format, Columns.ToArray());

            // добавить каждую строку
            var results = Rows.Select(row => string.Format(format, row)).ToList();

            // создание разделителя
            var divider = Regex.Replace(columnHeaders, @"[^|]", "-");
            var dividerPlus = divider.Replace("|", "+");

            builder.AppendLine(dividerPlus);
            builder.AppendLine(columnHeaders);

            foreach (var row in results)
            {
                builder.AppendLine(dividerPlus);
                builder.AppendLine(row);
            }
            builder.AppendLine(dividerPlus);

            return builder.ToString();
        }

        private String Format(List<Int32> columnLengths, Char delimiter = '|')
        {
            String delimiterStr = delimiter == Char.MinValue ? String.Empty : delimiter.ToString();
            String format = (Enumerable.Range(0, Columns.Count)
                .Select(i => " " + delimiterStr + " {" + i + ",-" + columnLengths[i] + "}")
                .Aggregate((s, a) => s + a) + " " + delimiterStr).Trim();
            return format;
        }

        private List<Int32> ColumnLengths()
        {
            List<Int32> columnLengths = Columns
                .Select((t, i) => Rows.Select(x => x[i])
                    .Union(new[] { Columns[i] })
                    .Where(x => x != null)
                    .Select(x => x.ToString().Length).Max())
                .ToList();
            return columnLengths;
        }

        /// <summary>
        /// Возвращает таблицу в виде типе данных <see cref="String"/>
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public String Result(TableFormat format = TableFormat.Default)
        {
            String Table = "";

            switch (format)
            {
                case TableFormat.Default:
                    Table = ToString();
                    break;
                case TableFormat.MarkDown:
                    Table = ToMarkDownString();
                    break;
                case TableFormat.Alternative:
                    Table = ToStringAlternative();
                    break;
                case TableFormat.Minimal:
                    Table = ToMinimalString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }

            return Table;
        }

        private static IEnumerable<String> GetColumns<T>()
        {
            return typeof(T).GetProperties().Select(x => x.Name).ToArray();
        }

        private static Object GetColumnValue<T>(Object target, String column)
        {
            return typeof(T).GetProperty(column).GetValue(target, null);
        }
    }

    public class TableOptions
    {
        public IEnumerable<String> Columns { get; set; } = new List<String>();
        public Boolean EnableCount { get; set; } = true;
    }
    public enum TableFormat
    {
        Default = 0,
        MarkDown = 1,
        Alternative = 2,
        Minimal = 3
    }
}
