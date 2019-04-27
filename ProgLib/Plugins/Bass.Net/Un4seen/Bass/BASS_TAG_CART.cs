using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BASS_TAG_CART
	{
		public string Version
		{
			get
			{
				if (this.version == null)
				{
					return "0000";
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.version, 0, this.version.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.version, 0, this.version.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					if (BassNet.UseBrokenLatin1Behavior)
					{
						this.version = Encoding.Default.GetBytes("0000");
						return;
					}
					this.version = Encoding.ASCII.GetBytes("0000");
					return;
				}
				else
				{
					this.version = new byte[4];
					if (BassNet.UseBrokenLatin1Behavior)
					{
						Encoding.Default.GetBytes(value + "0000", 0, 4, this.version, 0);
						return;
					}
					Encoding.ASCII.GetBytes(value + "0000", 0, 4, this.version, 0);
					return;
				}
			}
		}

		public string Title
		{
			get
			{
				if (this.title == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.title, 0, this.title.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.title, 0, this.title.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.title = new byte[64];
					return;
				}
				this.title = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.title, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.title, 0);
			}
		}

		public string Artist
		{
			get
			{
				if (this.artist == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.artist, 0, this.artist.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.artist, 0, this.artist.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.artist = new byte[64];
					return;
				}
				this.artist = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.artist, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.artist, 0);
			}
		}

		public string CutID
		{
			get
			{
				if (this.cutID == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.cutID, 0, this.cutID.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.cutID, 0, this.cutID.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.cutID = new byte[64];
					return;
				}
				this.cutID = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.cutID, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.cutID, 0);
			}
		}

		public string ClientID
		{
			get
			{
				if (this.clientID == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.clientID, 0, this.clientID.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.clientID, 0, this.clientID.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.clientID = new byte[64];
					return;
				}
				this.clientID = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.clientID, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.clientID, 0);
			}
		}

		public string Category
		{
			get
			{
				if (this.category == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.category, 0, this.category.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.category, 0, this.category.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.category = new byte[64];
					return;
				}
				this.category = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.category, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.category, 0);
			}
		}

		public string Classification
		{
			get
			{
				if (this.classification == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.classification, 0, this.classification.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.classification, 0, this.classification.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.classification = new byte[64];
					return;
				}
				this.classification = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.classification, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.classification, 0);
			}
		}

		public string OutCue
		{
			get
			{
				if (this.outCue == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.outCue, 0, this.outCue.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.outCue, 0, this.outCue.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.outCue = new byte[64];
					return;
				}
				this.outCue = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.outCue, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.outCue, 0);
			}
		}

		public string StartDate
		{
			get
			{
				if (this.startDate == null)
				{
					return "1900-01-01";
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.startDate, 0, this.startDate.Length).TrimEnd(new char[1]).Replace(' ', '-').Replace(':', '-').Replace('.', '-').Replace('_', '-');
				}
				return Encoding.ASCII.GetString(this.startDate, 0, this.startDate.Length).TrimEnd(new char[1]).Replace(' ', '-').Replace(':', '-').Replace('.', '-').Replace('_', '-');
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.startDate = Encoding.ASCII.GetBytes("1900-01-01");
					return;
				}
				this.startDate = new byte[10];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 10) ? 10 : value.Length, this.startDate, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 10) ? 10 : value.Length, this.startDate, 0);
			}
		}

		public string StartTime
		{
			get
			{
				if (this.startTime == null)
				{
					return "00:00:00";
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.startTime, 0, this.startTime.Length).TrimEnd(new char[1]).Replace(' ', ':').Replace('-', ':').Replace('.', ':').Replace('_', ':');
				}
				return Encoding.ASCII.GetString(this.startTime, 0, this.startTime.Length).TrimEnd(new char[1]).Replace(' ', ':').Replace('-', ':').Replace('.', ':').Replace('_', ':');
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.startTime = Encoding.ASCII.GetBytes("00:00:00");
					return;
				}
				this.startTime = new byte[8];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 8) ? 8 : value.Length, this.startTime, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 8) ? 8 : value.Length, this.startTime, 0);
			}
		}

		public string EndDate
		{
			get
			{
				if (this.endDate == null)
				{
					return "9999-12-31";
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.endDate, 0, this.endDate.Length).TrimEnd(new char[1]).Replace(' ', '-').Replace(':', '-').Replace('.', '-').Replace('_', '-');
				}
				return Encoding.ASCII.GetString(this.endDate, 0, this.endDate.Length).TrimEnd(new char[1]).Replace(' ', '-').Replace(':', '-').Replace('.', '-').Replace('_', '-');
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.endDate = Encoding.ASCII.GetBytes("9999-12-31");
					return;
				}
				this.endDate = new byte[10];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 10) ? 10 : value.Length, this.endDate, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 10) ? 10 : value.Length, this.endDate, 0);
			}
		}

		public string EndTime
		{
			get
			{
				if (this.endTime == null)
				{
					return "23:59:59";
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.endTime, 0, this.startTime.Length).TrimEnd(new char[1]).Replace(' ', ':').Replace('-', ':').Replace('.', ':').Replace('_', ':');
				}
				return Encoding.ASCII.GetString(this.endTime, 0, this.startTime.Length).TrimEnd(new char[1]).Replace(' ', ':').Replace('-', ':').Replace('.', ':').Replace('_', ':');
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					if (BassNet.UseBrokenLatin1Behavior)
					{
						this.endTime = Encoding.Default.GetBytes("23:59:59");
						return;
					}
					this.endTime = Encoding.ASCII.GetBytes("23:59:59");
					return;
				}
				else
				{
					this.endTime = new byte[8];
					if (BassNet.UseBrokenLatin1Behavior)
					{
						Encoding.Default.GetBytes(value, 0, (value.Length > 8) ? 8 : value.Length, this.endTime, 0);
						return;
					}
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 8) ? 8 : value.Length, this.endTime, 0);
					return;
				}
			}
		}

		public string ProducerAppID
		{
			get
			{
				if (this.producerAppID == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.producerAppID, 0, this.producerAppID.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.producerAppID, 0, this.producerAppID.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.producerAppID = new byte[64];
					return;
				}
				this.producerAppID = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.producerAppID, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.producerAppID, 0);
			}
		}

		public string ProducerAppVersion
		{
			get
			{
				if (this.producerAppVersion == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.producerAppVersion, 0, this.producerAppVersion.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.producerAppVersion, 0, this.producerAppVersion.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.producerAppVersion = new byte[64];
					return;
				}
				this.producerAppVersion = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.producerAppVersion, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.producerAppVersion, 0);
			}
		}

		public string UserDef
		{
			get
			{
				if (this.userDef == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.userDef, 0, this.userDef.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.userDef, 0, this.userDef.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.userDef = new byte[64];
					return;
				}
				this.userDef = new byte[64];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.userDef, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 64) ? 64 : value.Length, this.userDef, 0);
			}
		}

		public int LevelReference
		{
			get
			{
				return this.dwLevelReference;
			}
			set
			{
				this.dwLevelReference = value;
			}
		}

		public string Timer1Usage
		{
			get
			{
				if (this.timer1Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.timer1Usage, 0, this.timer1Usage.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.timer1Usage, 0, this.timer1Usage.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.timer1Usage = new byte[4];
					return;
				}
				this.timer1Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer1Usage, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer1Usage, 0);
			}
		}

		public int Timer1Value
		{
			get
			{
				return this.timer1Value;
			}
			set
			{
				this.timer1Value = value;
			}
		}

		public string Timer2Usage
		{
			get
			{
				if (this.timer2Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.timer2Usage, 0, this.timer2Usage.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.timer2Usage, 0, this.timer2Usage.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.timer2Usage = new byte[4];
					return;
				}
				this.timer2Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer2Usage, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer2Usage, 0);
			}
		}

		public int Timer2Value
		{
			get
			{
				return this.timer2Value;
			}
			set
			{
				this.timer2Value = value;
			}
		}

		public string Timer3Usage
		{
			get
			{
				if (this.timer3Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.timer3Usage, 0, this.timer3Usage.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.timer3Usage, 0, this.timer3Usage.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.timer3Usage = new byte[4];
					return;
				}
				this.timer3Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer3Usage, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer3Usage, 0);
			}
		}

		public int Timer3Value
		{
			get
			{
				return this.timer3Value;
			}
			set
			{
				this.timer3Value = value;
			}
		}

		public string Timer4Usage
		{
			get
			{
				if (this.timer4Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.timer4Usage, 0, this.timer4Usage.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.timer4Usage, 0, this.timer4Usage.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.timer4Usage = new byte[4];
					return;
				}
				this.timer4Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer4Usage, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer4Usage, 0);
			}
		}

		public int Timer4Value
		{
			get
			{
				return this.timer4Value;
			}
			set
			{
				this.timer4Value = value;
			}
		}

		public string Timer5Usage
		{
			get
			{
				if (this.timer5Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.timer5Usage, 0, this.timer5Usage.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.timer5Usage, 0, this.timer5Usage.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.timer5Usage = new byte[4];
					return;
				}
				this.timer5Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer5Usage, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer5Usage, 0);
			}
		}

		public int Timer5Value
		{
			get
			{
				return this.timer5Value;
			}
			set
			{
				this.timer5Value = value;
			}
		}

		public string Timer6Usage
		{
			get
			{
				if (this.timer6Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.timer6Usage, 0, this.timer6Usage.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.timer6Usage, 0, this.timer6Usage.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.timer6Usage = new byte[4];
					return;
				}
				this.timer6Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer6Usage, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer6Usage, 0);
			}
		}

		public int Timer6Value
		{
			get
			{
				return this.timer6Value;
			}
			set
			{
				this.timer6Value = value;
			}
		}

		public string Timer7Usage
		{
			get
			{
				if (this.timer7Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.timer7Usage, 0, this.timer7Usage.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.timer7Usage, 0, this.timer7Usage.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.timer7Usage = new byte[4];
					return;
				}
				this.timer7Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer7Usage, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer7Usage, 0);
			}
		}

		public int Timer7Value
		{
			get
			{
				return this.timer7Value;
			}
			set
			{
				this.timer7Value = value;
			}
		}

		public string Timer8Usage
		{
			get
			{
				if (this.timer8Usage == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.timer8Usage, 0, this.timer8Usage.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.timer8Usage, 0, this.timer8Usage.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.timer8Usage = new byte[4];
					return;
				}
				this.timer8Usage = new byte[4];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer8Usage, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.timer8Usage, 0);
			}
		}

		public int Timer8Value
		{
			get
			{
				return this.timer8Value;
			}
			set
			{
				this.timer8Value = value;
			}
		}

		public string URL
		{
			get
			{
				if (this.url == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.url, 0, this.url.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.url, 0, this.url.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.url = new byte[1024];
					return;
				}
				this.url = new byte[1024];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 1024) ? 1024 : value.Length, this.url, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 1024) ? 1024 : value.Length, this.url, 0);
			}
		}

		public override string ToString()
		{
			return this.Artist + " - " + this.Title;
		}

		public unsafe string GetTagText(IntPtr tag)
		{
			if (tag == IntPtr.Zero)
			{
				return null;
			}
			int num;
			return Utils.IntPtrAsStringLatin1(new IntPtr((void*)((byte*)tag.ToPointer() + 2048)), out num);
		}

		public byte[] AsByteArray(string tagText)
		{
			if (string.IsNullOrEmpty(tagText))
			{
				tagText = new string('\0', 256);
			}
			else
			{
				if (!tagText.EndsWith("\0"))
				{
					tagText += "\0";
				}
				if (tagText.Length % 2 == 1)
				{
					tagText += "\0";
				}
				int num = tagText.Length % 256;
				if (num > 0)
				{
					tagText += new string('\0', num);
				}
			}
			byte[] array = BassNet.UseBrokenLatin1Behavior ? Encoding.Default.GetBytes(tagText) : Encoding.ASCII.GetBytes(tagText);
			int num2 = Marshal.SizeOf(typeof(BASS_TAG_CART));
			byte[] array2 = new byte[num2];
			GCHandle gchandle = GCHandle.Alloc(array2, GCHandleType.Pinned);
			Marshal.StructureToPtr(this, gchandle.AddrOfPinnedObject(), false);
			gchandle.Free();
			byte[] array3 = new byte[num2 + array.Length];
			Array.Copy(array2, 0, array3, 0, num2);
			Array.Copy(array, 0, array3, num2, array.Length);
			return array3;
		}

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] version;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] title;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] artist;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] cutID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] clientID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] category;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] classification;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] outCue;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		private byte[] startDate;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		private byte[] startTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		private byte[] endDate;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		private byte[] endTime;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] producerAppID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] producerAppVersion;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] userDef;

		private int dwLevelReference;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer1Usage;

		private int timer1Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer2Usage;

		private int timer2Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer3Usage;

		private int timer3Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer4Usage;

		private int timer4Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer5Usage;

		private int timer5Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer6Usage;

		private int timer6Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer7Usage;

		private int timer7Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] timer8Usage;

		private int timer8Value;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 276)]
		public byte[] Reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
		private byte[] url;
	}
}
