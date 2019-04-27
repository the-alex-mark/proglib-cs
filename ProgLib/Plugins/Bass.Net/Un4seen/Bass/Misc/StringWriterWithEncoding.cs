using System;
using System.IO;
using System.Text;

namespace Un4seen.Bass.Misc
{
	internal class StringWriterWithEncoding : StringWriter
	{
		public StringWriterWithEncoding(Encoding encoding)
		{
			this._encoding = encoding;
		}

		public override Encoding Encoding
		{
			get
			{
				return this._encoding;
			}
		}

		private Encoding _encoding;
	}
}
