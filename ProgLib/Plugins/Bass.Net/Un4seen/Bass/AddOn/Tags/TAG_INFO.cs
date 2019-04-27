using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using Un4seen.Bass.AddOn.Midi;

namespace Un4seen.Bass.AddOn.Tags
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public class TAG_INFO
	{
		public TAG_INFO()
		{
		}

		public TAG_INFO(TAG_INFO clone)
		{
			this.title = clone.title;
			this.artist = clone.artist;
			this.album = clone.album;
			this.albumartist = clone.albumartist;
			this.year = clone.year;
			this.comment = clone.comment;
			this.genre = clone.genre;
			this.track = clone.track;
			this.disc = clone.disc;
			this.copyright = clone.copyright;
			this.encodedby = clone.encodedby;
			this.composer = clone.composer;
			this.conductor = clone.conductor;
			this.publisher = clone.publisher;
			this.lyricist = clone.lyricist;
			this.remixer = clone.remixer;
			this.producer = clone.producer;
			this.bpm = clone.bpm;
			this.mood = clone.mood;
			this.grouping = clone.grouping;
			this.rating = clone.rating;
			this.isrc = clone.isrc;
			this.replaygain_track_peak = clone.replaygain_track_peak;
			this.replaygain_track_gain = clone.replaygain_track_gain;
			this.filename = clone.filename;
			this.pictures = new List<TagPicture>(clone.PictureCount);
			foreach (TagPicture tagPicture in clone.pictures)
			{
				TagPicture tagPicture2 = new TagPicture(tagPicture.AttributeIndex, tagPicture.MIMEType, tagPicture.PictureType, tagPicture.Description, tagPicture.Data);
				tagPicture2.PictureStorage = tagPicture.PictureStorage;
				this.pictures.Add(tagPicture2);
			}
			this.nativetags = new List<string>(clone.nativetags.Count);
			foreach (string item in clone.nativetags)
			{
				this.nativetags.Add(item);
			}
			this.channelinfo.chans = clone.channelinfo.chans;
			this.channelinfo.ctype = clone.channelinfo.ctype;
			this.channelinfo.filename = clone.channelinfo.filename;
			this.channelinfo.flags = clone.channelinfo.flags;
			this.channelinfo.freq = clone.channelinfo.freq;
			this.channelinfo.origres = clone.channelinfo.origres;
			this.channelinfo.plugin = clone.channelinfo.plugin;
			this.channelinfo.sample = clone.channelinfo.sample;
			this.tagType = clone.tagType;
			this.duration = clone.duration;
			this.bitrate = clone.bitrate;
			this._multiCounter = clone._multiCounter;
			this._commentCounter = clone._commentCounter;
		}

		public TAG_INFO(string FileName)
		{
			this.filename = FileName;
			this.title = Path.GetFileNameWithoutExtension(FileName);
		}

		public TAG_INFO(string FileName, bool setDefaultTitle)
		{
			this.filename = FileName;
			if (setDefaultTitle)
			{
				this.title = Path.GetFileNameWithoutExtension(FileName);
			}
		}

		public TAG_INFO Clone()
		{
			return new TAG_INFO(this);
		}

		public override string ToString()
		{
			string text = this.artist;
			if (string.IsNullOrEmpty(text))
			{
				text = this.albumartist;
			}
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(this.title))
			{
				return string.Format("{0} - {1}", text, this.title);
			}
			if (string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(this.title))
			{
				return this.title;
			}
			if (!string.IsNullOrEmpty(text) && string.IsNullOrEmpty(this.title))
			{
				return text;
			}
			return Path.GetFileNameWithoutExtension(this.filename);
		}

		public int NativeTagsCount
		{
			get
			{
				if (this.nativetags != null)
				{
					return this.nativetags.Count;
				}
				return 0;
			}
		}

		public string[] NativeTags
		{
			get
			{
				return this.nativetags.ToArray();
			}
		}

		public void ClearAllNativeTags()
		{
			this.nativetags.Clear();
		}

		public string NativeTag(string tagname)
		{
			if (tagname == null)
			{
				return null;
			}
			try
			{
				string result = null;
				foreach (string text in this.nativetags)
				{
					if (text.StartsWith(tagname))
					{
						string[] array = text.Split(new char[]
						{
							'=',
							':'
						}, 2);
						if (array.Length == 2)
						{
							result = array[1].TrimWithBOM();
							break;
						}
					}
				}
				return result;
			}
			catch
			{
			}
			return null;
		}

		public bool UpdateFromMETA(IntPtr data, bool utf8, bool multiple)
		{
			return this.UpdateFromMETA(data, utf8 ? TAGINFOEncoding.Utf8 : TAGINFOEncoding.Latin1, multiple);
		}

		public unsafe bool UpdateFromMETA(IntPtr data, TAGINFOEncoding encoding, bool multiple)
		{
			if (data == IntPtr.Zero)
			{
				return false;
			}
			bool flag = false;
			this.ResetTags();
			bool flag2 = true;
			int num = 0;
			IntPtr intPtr = data;
			while (flag2)
			{
				string text;
				switch (encoding)
				{
				case TAGINFOEncoding.Ansi:
					text = Utils.IntPtrAsStringAnsi(new IntPtr((void*)((byte*)intPtr.ToPointer() + num)));
					if (text != null)
					{
						num += text.Length + 1;
					}
					break;
				case TAGINFOEncoding.Latin1:
					goto IL_A5;
				case TAGINFOEncoding.Utf8:
				{
					int num2;
					text = Utils.IntPtrAsStringUtf8(new IntPtr((void*)((byte*)intPtr.ToPointer() + num)), out num2);
					if (text != null)
					{
						num += num2 + 1;
					}
					break;
				}
				case TAGINFOEncoding.Utf8OrLatin1:
				{
					int num3;
					text = Utils.IntPtrAsStringUtf8orLatin1(new IntPtr((void*)((byte*)intPtr.ToPointer() + num)), out num3);
					if (text != null)
					{
						num += num3 + 1;
					}
					break;
				}
				default:
					goto IL_A5;
				}
				IL_C5:
				if (text == null || text.Length == 0)
				{
					flag2 = false;
					continue;
				}
				if (!multiple)
				{
					flag |= this.EvalTagEntry(text.TrimWithBOM());
					continue;
				}
				string[] array = text.Split(new char[]
				{
					';',
					','
				});
				if (array.Length != 0)
				{
					foreach (string str in array)
					{
						flag |= this.EvalTagEntry(str.TrimWithBOM());
					}
					continue;
				}
				continue;
				IL_A5:
				int num4;
				text = Utils.IntPtrAsStringLatin1(new IntPtr((void*)((byte*)intPtr.ToPointer() + num)), out num4);
				if (text != null)
				{
					num += num4 + 1;
					goto IL_C5;
				}
				goto IL_C5;
			}
			return flag;
		}

		public bool UpdateFromMIDILyric(BASS_MIDI_MARK midiMark)
		{
			if (midiMark == null)
			{
				return false;
			}
			if (!this.track.Equals(midiMark.track.ToString()))
			{
				this.track = midiMark.track.ToString();
			}
			bool result;
			if (midiMark.text == null || midiMark.text == string.Empty)
			{
				result = false;
			}
			else if (midiMark.text.StartsWith("/"))
			{
				this.comment += "\n";
				if (midiMark.text.Length > 1)
				{
					this.comment += midiMark.text.Substring(1);
				}
				result = true;
			}
			else if (midiMark.text.StartsWith("\\"))
			{
				this.comment = string.Empty;
				if (midiMark.text.Length > 1)
				{
					this.comment += midiMark.text.Substring(1);
				}
				result = true;
			}
			else
			{
				this.comment += midiMark.text;
				result = true;
			}
			return result;
		}

		public void AddNativeTag(string key, object value)
		{
			string item = string.Format("{0}={1}", key, value);
			if (!this.nativetags.Contains(item))
			{
				this.nativetags.Add(item);
			}
		}

		public void AddOrReplaceNativeTag(string key, object value)
		{
			bool flag = false;
			string value2 = key + "=";
			for (int i = 0; i < this.nativetags.Count; i++)
			{
				if (this.nativetags[i].StartsWith(value2))
				{
					this.nativetags[i] = string.Format("{0}={1}", key, value);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.nativetags.Add(string.Format("{0}={1}", key, value));
			}
		}

		public void RemoveNativeTag(string key)
		{
			string value = key + "=";
			for (int i = 0; i < this.nativetags.Count; i++)
			{
				if (this.nativetags[i].StartsWith(value))
				{
					this.nativetags.RemoveAt(i);
					return;
				}
			}
		}

		internal void ResetTags()
		{
			this._multiCounter = 0;
			this._commentCounter = 0;
			this.nativetags.Clear();
		}

		internal void ResetTags2()
		{
			this._commentCounter = 0;
		}

		internal bool EvalTagEntry(string tagEntry)
		{
			if (string.IsNullOrEmpty(tagEntry))
			{
				return false;
			}
			bool result = false;
			string text = string.Empty;
			string[] array = tagEntry.TrimWithBOM().Split(new char[]
			{
				'=',
				':'
			}, 2);
			if (array.Length == 2)
			{
				if (BassTags.EvalNativeTAGs)
				{
					if (this.NativeTag(array[0].TrimWithBOM()) != null)
					{
						this._multiCounter++;
						array[0] = array[0].TrimWithBOM() + this._multiCounter.ToString();
					}
					try
					{
						this.nativetags.Add(array[0].TrimWithBOM() + "=" + array[1].TrimWithBOM());
					}
					catch
					{
					}
				}
				if (array[0].ToLower().TrimWithBOM().StartsWith("txx"))
				{
					string[] array2 = array[1].TrimWithBOM().Split(new char[]
					{
						'=',
						':'
					}, 2);
					if (array2.Length == 2)
					{
						array[0] = array2[0].TrimWithBOM();
						array[1] = array2[1].TrimWithBOM();
					}
				}
				string text2 = array[0].ToLower().TrimWithBOM();
				uint num = PrivateImplementationDetails.ComputeStringHash(text2);
				if (num <= 2472024882u)
				{
					if (num <= 1234871453u)
					{
						if (num <= 772060872u)
						{
							if (num <= 369949246u)
							{
								if (num <= 231034112u)
								{
									if (num <= 84022231u)
									{
										if (num != 27276696u)
										{
											if (num != 84022231u)
											{
												return result;
											}
											if (!(text2 == "album artist"))
											{
												return result;
											}
											goto IL_1CD9;
										}
										else
										{
											if (!(text2 == "iaar"))
											{
												return result;
											}
											goto IL_1CD9;
										}
									}
									else if (num != 192097854u)
									{
										if (num != 219569605u)
										{
											if (num != 231034112u)
											{
												return result;
											}
											if (!(text2 == "inam"))
											{
												return result;
											}
											goto IL_1BCC;
										}
										else
										{
											if (!(text2 == "wm/isrc"))
											{
												return result;
											}
											goto IL_2926;
										}
									}
									else
									{
										if (!(text2 == "streamurl"))
										{
											return result;
										}
										goto IL_2C0E;
									}
								}
								else if (num <= 345615029u)
								{
									if (num != 330216824u)
									{
										if (num != 345615029u)
										{
											return result;
										}
										if (!(text2 == "performer"))
										{
											return result;
										}
										goto IL_1D41;
									}
									else
									{
										if (!(text2 == "tempo"))
										{
											return result;
										}
										goto IL_29AB;
									}
								}
								else if (num != 346853036u)
								{
									if (num != 355560749u)
									{
										if (num != 369949246u)
										{
											return result;
										}
										if (!(text2 == "release date"))
										{
											return result;
										}
										goto IL_2130;
									}
									else
									{
										if (!(text2 == "beatsperminute"))
										{
											return result;
										}
										goto IL_29AB;
									}
								}
								else
								{
									if (!(text2 == "discnum"))
									{
										return result;
									}
									text = array[1].TrimWithBOM();
									if (text != string.Empty && string.IsNullOrEmpty(this.disc))
									{
										this.disc = text;
										return true;
									}
									return result;
								}
							}
							else if (num <= 572804318u)
							{
								if (num <= 395168604u)
								{
									if (num != 395114040u)
									{
										if (num != 395168604u)
										{
											return result;
										}
										if (!(text2 == "wm/contentgroupdescription"))
										{
											return result;
										}
										goto IL_2566;
									}
									else
									{
										if (!(text2 == "rating wmp"))
										{
											return result;
										}
										goto IL_27AB;
									}
								}
								else if (num != 455929684u)
								{
									if (num != 532403852u)
									{
										if (num != 572804318u)
										{
											return result;
										}
										if (!(text2 == "artist"))
										{
											return result;
										}
										goto IL_1C03;
									}
									else
									{
										if (!(text2 == "icy-url"))
										{
											return result;
										}
										goto IL_2C0E;
									}
								}
								else
								{
									if (!(text2 == "talb"))
									{
										return result;
									}
									goto IL_1CB2;
								}
							}
							else if (num <= 730173168u)
							{
								if (num != 577722853u)
								{
									if (num != 600132050u)
									{
										if (num != 730173168u)
										{
											return result;
										}
										if (!(text2 == "tmoo"))
										{
											return result;
										}
										goto IL_28FF;
									}
									else
									{
										if (!(text2 == "irtd"))
										{
											return result;
										}
										goto IL_2784;
									}
								}
								else
								{
									if (!(text2 == "ignr"))
									{
										return result;
									}
									goto IL_2167;
								}
							}
							else if (num != 737799129u)
							{
								if (num != 762324290u)
								{
									if (num != 772060872u)
									{
										return result;
									}
									if (!(text2 == "encodedby"))
									{
										return result;
									}
									goto IL_1E92;
								}
								else
								{
									if (!(text2 == "tdrc"))
									{
										return result;
									}
									goto IL_2130;
								}
							}
							else
							{
								if (!(text2 == "tyer"))
								{
									return result;
								}
								goto IL_20D6;
							}
						}
						else if (num <= 890800816u)
						{
							if (num <= 840467959u)
							{
								if (num <= 815399983u)
								{
									if (num != 815078726u)
									{
										if (num != 815399983u)
										{
											return result;
										}
										if (!(text2 == "encoder"))
										{
											return result;
										}
									}
									else
									{
										if (!(text2 == "band"))
										{
											return result;
										}
										goto IL_1D41;
									}
								}
								else if (num != 816487251u)
								{
									if (num != 837046592u)
									{
										if (num != 840467959u)
										{
											return result;
										}
										if (!(text2 == "tcop"))
										{
											return result;
										}
										goto IL_1E34;
									}
									else
									{
										if (!(text2 == "ieng"))
										{
											return result;
										}
										goto IL_1FAA;
									}
								}
								else
								{
									if (!(text2 == "tpos"))
									{
										return result;
									}
									goto IL_1DD6;
								}
							}
							else if (num <= 852633064u)
							{
								if (num != 841388516u)
								{
									if (num != 848267271u)
									{
										if (num != 852633064u)
										{
											return result;
										}
										if (!(text2 == "ultravoxtitle"))
										{
											return result;
										}
										goto IL_2B89;
									}
									else
									{
										if (!(text2 == "metadata_block_picture"))
										{
											return result;
										}
										goto IL_2B45;
									}
								}
								else
								{
									if (!(text2 == "tit2"))
									{
										return result;
									}
									goto IL_1BCC;
								}
							}
							else if (num != 879019937u)
							{
								if (num != 879704937u)
								{
									if (num != 890800816u)
									{
										return result;
									}
									if (!(text2 == "tcom"))
									{
										return result;
									}
									goto IL_1FD1;
								}
								else
								{
									if (!(text2 == "description"))
									{
										return result;
									}
									goto IL_2049;
								}
							}
							else
							{
								if (!(text2 == "remixer"))
								{
									return result;
								}
								goto IL_2663;
							}
						}
						else if (num <= 1029714956u)
						{
							if (num <= 941133673u)
							{
								if (num != 891721373u)
								{
									if (num != 941133673u)
									{
										return result;
									}
									if (!(text2 == "tcon"))
									{
										return result;
									}
									goto IL_21E6;
								}
								else
								{
									if (!(text2 == "tit1"))
									{
										return result;
									}
									goto IL_2566;
								}
							}
							else if (num != 945596323u)
							{
								if (num != 992392888u)
								{
									if (num != 1029714956u)
									{
										return result;
									}
									if (!(text2 == "disc"))
									{
										return result;
									}
									goto IL_1DD6;
								}
								else
								{
									if (!(text2 == "lyricist"))
									{
										return result;
									}
									goto IL_25C4;
								}
							}
							else
							{
								if (!(text2 == "ikey"))
								{
									return result;
								}
								goto IL_28FF;
							}
						}
						else if (num <= 1109888656u)
						{
							if (num != 1065574956u)
							{
								if (num != 1087725139u)
								{
									if (num != 1109888656u)
									{
										return result;
									}
									if (!(text2 == "software"))
									{
										return result;
									}
								}
								else
								{
									if (!(text2 == "iedt"))
									{
										return result;
									}
									goto IL_2663;
								}
							}
							else
							{
								if (!(text2 == "grouping"))
								{
									return result;
								}
								goto IL_2566;
							}
						}
						else if (num != 1181855383u)
						{
							if (num != 1205918082u)
							{
								if (num != 1234871453u)
								{
									return result;
								}
								if (!(text2 == "irgp"))
								{
									return result;
								}
								goto IL_2A8D;
							}
							else
							{
								if (!(text2 == "tpub"))
								{
									return result;
								}
								goto IL_1F4C;
							}
						}
						else if (!(text2 == "version"))
						{
							return result;
						}
					}
					else if (num <= 1806413385u)
					{
						if (num <= 1507129445u)
						{
							if (num <= 1366625991u)
							{
								if (num <= 1333443158u)
								{
									if (num != 1332083167u)
									{
										if (num != 1333443158u)
										{
											return result;
										}
										if (!(text2 == "author"))
										{
											return result;
										}
										goto IL_1C7B;
									}
									else
									{
										if (!(text2 == "ipro"))
										{
											return result;
										}
										goto IL_26CB;
									}
								}
								else if (num != 1333525265u)
								{
									if (num != 1357396007u)
									{
										if (num != 1366625991u)
										{
											return result;
										}
										if (!(text2 == "tool"))
										{
											return result;
										}
										text = array[1].TrimWithBOM();
										if (!(text != string.Empty) || !string.IsNullOrEmpty(this.encodedby))
										{
											return result;
										}
										this.encodedby = text;
										result = true;
										if (text[0] < ' ')
										{
											this.encodedby = ((byte)text[0]).ToString();
											return result;
										}
										return result;
									}
									else
									{
										if (!(text2 == "producer"))
										{
											return result;
										}
										goto IL_26CB;
									}
								}
								else
								{
									if (!(text2 == "trck"))
									{
										return result;
									}
									goto IL_1D78;
								}
							}
							else if (num <= 1415613031u)
							{
								if (num != 1385870024u)
								{
									if (num != 1393387308u)
									{
										if (num != 1415613031u)
										{
											return result;
										}
										if (!(text2 == "album1"))
										{
											return result;
										}
										goto IL_1CB2;
									}
									else
									{
										if (!(text2 == "wm/description"))
										{
											return result;
										}
										goto IL_2049;
									}
								}
								else
								{
									if (!(text2 == "irgg"))
									{
										return result;
									}
									goto IL_2A19;
								}
							}
							else if (num != 1479629586u)
							{
								if (num != 1483081738u)
								{
									if (num != 1507129445u)
									{
										return result;
									}
									if (!(text2 == "wm/writer"))
									{
										return result;
									}
									goto IL_25C4;
								}
								else
								{
									if (!(text2 == "iprd"))
									{
										return result;
									}
									goto IL_1CB2;
								}
							}
							else
							{
								if (!(text2 == "tipl"))
								{
									return result;
								}
								goto IL_26F2;
							}
						}
						else if (num <= 1694181484u)
						{
							if (num <= 1556378865u)
							{
								if (num != 1511499486u)
								{
									if (num != 1556378865u)
									{
										return result;
									}
									if (!(text2 == "ibpm"))
									{
										return result;
									}
									goto IL_294D;
								}
								else
								{
									if (!(text2 == "rating mm"))
									{
										return result;
									}
									goto IL_27AB;
								}
							}
							else if (num != 1575106246u)
							{
								if (num != 1605967500u)
								{
									if (num != 1694181484u)
									{
										return result;
									}
									if (!(text2 == "album"))
									{
										return result;
									}
									goto IL_1CB2;
								}
								else
								{
									if (!(text2 == "group"))
									{
										return result;
									}
									text = array[1].TrimWithBOM();
									if (text != string.Empty && string.IsNullOrEmpty(this.grouping))
									{
										this.grouping = text;
										return true;
									}
									return result;
								}
							}
							else
							{
								if (!(text2 == "bpm"))
								{
									return result;
								}
								goto IL_294D;
							}
						}
						else if (num <= 1738982494u)
						{
							if (num != 1733853966u)
							{
								if (num != 1735464473u)
								{
									if (num != 1738982494u)
									{
										return result;
									}
									if (!(text2 == "comment"))
									{
										return result;
									}
									goto IL_2049;
								}
								else if (!(text2 == "encoded-by"))
								{
									return result;
								}
							}
							else
							{
								if (!(text2 == "icy-genre"))
								{
									return result;
								}
								goto IL_2167;
							}
						}
						else if (num != 1751523642u)
						{
							if (num != 1762760298u)
							{
								if (num != 1806413385u)
								{
									return result;
								}
								if (!(text2 == "replaygain_track_gain"))
								{
									return result;
								}
								goto IL_2A19;
							}
							else
							{
								if (!(text2 == "releasedate"))
								{
									return result;
								}
								goto IL_2130;
							}
						}
						else
						{
							if (!(text2 == "iprt"))
							{
								return result;
							}
							goto IL_1D78;
						}
					}
					else if (num <= 2233903452u)
					{
						if (num <= 1986820544u)
						{
							if (num <= 1914819054u)
							{
								if (num != 1881263816u)
								{
									if (num != 1914819054u)
									{
										return result;
									}
									if (!(text2 == "tp3"))
									{
										return result;
									}
									goto IL_2525;
								}
								else
								{
									if (!(text2 == "tp1"))
									{
										return result;
									}
									goto IL_1C3A;
								}
							}
							else if (num != 1931596673u)
							{
								if (num != 1965151911u)
								{
									if (num != 1986820544u)
									{
										return result;
									}
									if (!(text2 == "genre"))
									{
										return result;
									}
									goto IL_2167;
								}
								else
								{
									if (!(text2 == "tp4"))
									{
										return result;
									}
									goto IL_268A;
								}
							}
							else
							{
								if (!(text2 == "tp2"))
								{
									return result;
								}
								goto IL_1D00;
							}
						}
						else if (num <= 2067622972u)
						{
							if (num != 2000177693u)
							{
								if (num != 2033652838u)
								{
									if (num != 2067622972u)
									{
										return result;
									}
									if (!(text2 == "track"))
									{
										return result;
									}
									goto IL_1D78;
								}
								else
								{
									if (!(text2 == "wm/partofset"))
									{
										return result;
									}
									goto IL_1DD6;
								}
							}
							else
							{
								if (!(text2 == "discnumber"))
								{
									return result;
								}
								goto IL_1DD6;
							}
						}
						else if (num != 2183777184u)
						{
							if (num != 2228000106u)
							{
								if (num != 2233903452u)
								{
									return result;
								}
								if (!(text2 == "tracknum"))
								{
									return result;
								}
								text = array[1].TrimWithBOM();
								if (text != string.Empty && string.IsNullOrEmpty(this.track))
								{
									this.track = text;
									return true;
								}
								return result;
							}
							else
							{
								if (!(text2 == "writer"))
								{
									return result;
								}
								goto IL_1FAA;
							}
						}
						else
						{
							if (!(text2 == "icy-name"))
							{
								return result;
							}
							goto IL_1CB2;
						}
					}
					else if (num <= 2390354663u)
					{
						if (num <= 2296462230u)
						{
							if (num != 2248989446u)
							{
								if (num != 2296462230u)
								{
									return result;
								}
								if (!(text2 == "ultravox-bitrate"))
								{
									return result;
								}
								goto IL_2C4A;
							}
							else if (!(text2 == "encoded by"))
							{
								return result;
							}
						}
						else if (num != 2318775059u)
						{
							if (num != 2349015378u)
							{
								if (num != 2390354663u)
								{
									return result;
								}
								if (!(text2 == "wm/provider"))
								{
									return result;
								}
								goto IL_1E5B;
							}
							else
							{
								if (!(text2 == "wm/albumartist"))
								{
									return result;
								}
								goto IL_1CD9;
							}
						}
						else
						{
							if (!(text2 == "composer"))
							{
								return result;
							}
							goto IL_1FAA;
						}
					}
					else if (num <= 2434060901u)
					{
						if (num != 2421692025u)
						{
							if (num != 2430580359u)
							{
								if (num != 2434060901u)
								{
									return result;
								}
								if (!(text2 == "wm/albumtitle"))
								{
									return result;
								}
								goto IL_1CB2;
							}
							else
							{
								if (!(text2 == "popm"))
								{
									return result;
								}
								goto IL_27AB;
							}
						}
						else
						{
							if (!(text2 == "isrf"))
							{
								return result;
							}
							goto IL_2566;
						}
					}
					else if (num != 2451408672u)
					{
						if (num != 2453864966u)
						{
							if (num != 2472024882u)
							{
								return result;
							}
							if (!(text2 == "isrc"))
							{
								return result;
							}
							goto IL_2926;
						}
						else
						{
							if (!(text2 == "wm/beatsperminute"))
							{
								return result;
							}
							goto IL_294D;
						}
					}
					else
					{
						if (!(text2 == "trc"))
						{
							return result;
						}
						goto IL_2926;
					}
					text = array[1].TrimWithBOM();
					if (text != string.Empty && string.IsNullOrEmpty(this.encodedby))
					{
						this.encodedby = text;
						return true;
					}
					return result;
					IL_2130:
					text = array[1].TrimWithBOM();
					if (text != string.Empty && string.IsNullOrEmpty(this.year))
					{
						this.year = text;
						return true;
					}
					return result;
					IL_27AB:
					text = array[1].TrimWithBOM();
					if (text != string.Empty)
					{
						this.rating = text;
						int num2 = 0;
						if (int.TryParse(text, out num2))
						{
							if (num2 == 255)
							{
								num2 = 100;
							}
							else if (num2 == 196 || num2 == 204)
							{
								num2 = 80;
							}
							else if (num2 == 128 || num2 == 153)
							{
								num2 = 60;
							}
							else if (num2 == 64 || num2 == 102)
							{
								num2 = 40;
							}
							else if (num2 == 1 || num2 == 51)
							{
								num2 = 20;
							}
							else if (num2 > 2 && num2 < 255)
							{
								num2 = (int)((float)num2 / 255f * 100f);
							}
							else if (num2 > 0 && num2 <= 255)
							{
								num2 = 1;
							}
							else
							{
								num2 = 0;
							}
							this.rating = num2.ToString();
						}
						return true;
					}
					return result;
				}
				else
				{
					if (num <= 3171021908u)
					{
						if (num <= 2767520409u)
						{
							if (num <= 2622037715u)
							{
								if (num <= 2556802313u)
								{
									if (num <= 2544944211u)
									{
										if (num != 2482856090u)
										{
											if (num != 2544944211u)
											{
												return result;
											}
											if (!(text2 == "vendor"))
											{
												return result;
											}
										}
										else
										{
											if (!(text2 == "wm/publisher"))
											{
												return result;
											}
											goto IL_1F4C;
										}
									}
									else if (num != 2550397100u)
									{
										if (num != 2553523123u)
										{
											if (num != 2556802313u)
											{
												return result;
											}
											if (!(text2 == "title"))
											{
												return result;
											}
											goto IL_1BCC;
										}
										else
										{
											if (!(text2 == "itch"))
											{
												return result;
											}
											goto IL_24FE;
										}
									}
									else
									{
										if (!(text2 == "tda"))
										{
											return result;
										}
										goto IL_20D6;
									}
								}
								else if (num <= 2585629624u)
								{
									if (num != 2583792016u)
									{
										if (num != 2585629624u)
										{
											return result;
										}
										if (!(text2 == "trk"))
										{
											return result;
										}
										goto IL_1D78;
									}
									else
									{
										if (!(text2 == "ipl"))
										{
											return result;
										}
										goto IL_26F2;
									}
								}
								else if (num != 2592838589u)
								{
									if (num != 2616374743u)
									{
										if (num != 2622037715u)
										{
											return result;
										}
										if (!(text2 == "istr"))
										{
											return result;
										}
										goto IL_1C7B;
									}
									else
									{
										if (!(text2 == "tcm"))
										{
											return result;
										}
										goto IL_1FD1;
									}
								}
								else
								{
									if (!(text2 == "wm/author"))
									{
										return result;
									}
									goto IL_1C03;
								}
							}
							else if (num <= 2678536424u)
							{
								if (num <= 2649929981u)
								{
									if (num != 2649722891u)
									{
										if (num != 2649929981u)
										{
											return result;
										}
										if (!(text2 == "tco"))
										{
											return result;
										}
										goto IL_21E6;
									}
									else
									{
										if (!(text2 == "modifiedby"))
										{
											return result;
										}
										goto IL_2663;
									}
								}
								else if (num != 2652195647u)
								{
									if (num != 2667001790u)
									{
										if (num != 2678536424u)
										{
											return result;
										}
										if (!(text2 == "orchestra"))
										{
											return result;
										}
										goto IL_1D41;
									}
									else
									{
										if (!(text2 == "tal"))
										{
											return result;
										}
										goto IL_1CB2;
									}
								}
								else
								{
									if (!(text2 == "tmo"))
									{
										return result;
									}
									goto IL_28FF;
								}
							}
							else if (num <= 2701711575u)
							{
								if (num != 2686589528u)
								{
									if (num != 2700262838u)
									{
										if (num != 2701711575u)
										{
											return result;
										}
										if (!(text2 == "itrk"))
										{
											return result;
										}
										goto IL_1D78;
									}
									else
									{
										if (!(text2 == "tcr"))
										{
											return result;
										}
										goto IL_1E34;
									}
								}
								else
								{
									if (!(text2 == "tpa"))
									{
										return result;
									}
									goto IL_1DD6;
								}
							}
							else if (num != 2712905244u)
							{
								if (num != 2736922385u)
								{
									if (num != 2767520409u)
									{
										return result;
									}
									if (!(text2 == "tbp"))
									{
										return result;
									}
									goto IL_294D;
								}
								else
								{
									if (!(text2 == "tpb"))
									{
										return result;
									}
									goto IL_1F4C;
								}
							}
							else
							{
								if (!(text2 == "tbpm"))
								{
									return result;
								}
								goto IL_294D;
							}
						}
						else if (num <= 2987208944u)
						{
							if (num <= 2833475869u)
							{
								if (num <= 2794345237u)
								{
									if (num != 2771757551u)
									{
										if (num != 2794345237u)
										{
											return result;
										}
										if (!(text2 == "ishp"))
										{
											return result;
										}
										goto IL_2784;
									}
									else
									{
										if (!(text2 == "txt"))
										{
											return result;
										}
										goto IL_25EB;
									}
								}
								else if (num != 2821943313u)
								{
									if (num != 2826717717u)
									{
										if (num != 2833475869u)
										{
											return result;
										}
										if (!(text2 == "h2_bpm"))
										{
											return result;
										}
										goto IL_29AB;
									}
									else
									{
										if (!(text2 == "replaygain_track_peak"))
										{
											return result;
										}
										goto IL_2A8D;
									}
								}
								else
								{
									if (!(text2 == "tye"))
									{
										return result;
									}
									goto IL_20D6;
								}
							}
							else if (num <= 2926992071u)
							{
								if (num != 2848777048u)
								{
									if (num != 2923983231u)
									{
										if (num != 2926992071u)
										{
											return result;
										}
										if (!(text2 == "isft"))
										{
											return result;
										}
										goto IL_1E92;
									}
									else if (!(text2 == "publisher"))
									{
										return result;
									}
								}
								else
								{
									if (!(text2 == "h2_albumartist"))
									{
										return result;
									}
									goto IL_1D41;
								}
							}
							else if (num != 2927578396u)
							{
								if (num != 2971952389u)
								{
									if (num != 2987208944u)
									{
										return result;
									}
									if (!(text2 == "wm/modifiedby"))
									{
										return result;
									}
									goto IL_2663;
								}
								else
								{
									if (!(text2 == "wm/mood"))
									{
										return result;
									}
									goto IL_28FF;
								}
							}
							else
							{
								if (!(text2 == "year"))
								{
									return result;
								}
								goto IL_20D6;
							}
						}
						else if (num <= 3082861500u)
						{
							if (num <= 3060726762u)
							{
								if (num != 3045288777u)
								{
									if (num != 3060726762u)
									{
										return result;
									}
									if (!(text2 == "organization"))
									{
										return result;
									}
									text = array[1].TrimWithBOM();
									if (text != string.Empty && string.IsNullOrEmpty(this.composer))
									{
										this.composer = text;
										return true;
									}
									return result;
								}
								else
								{
									if (!(text2 == "texter"))
									{
										return result;
									}
									goto IL_262C;
								}
							}
							else if (num != 3070989195u)
							{
								if (num != 3075725865u)
								{
									if (num != 3082861500u)
									{
										return result;
									}
									if (!(text2 == "provider"))
									{
										return result;
									}
									goto IL_1E5B;
								}
								else
								{
									if (!(text2 == "wm/encodedby"))
									{
										return result;
									}
									goto IL_1E92;
								}
							}
							else
							{
								if (!(text2 == "imus"))
								{
									return result;
								}
								goto IL_1FAA;
							}
						}
						else if (num <= 3158105787u)
						{
							if (num != 3104697662u)
							{
								if (num != 3112721564u)
								{
									if (num != 3158105787u)
									{
										return result;
									}
									if (!(text2 == "itgl"))
									{
										return result;
									}
									goto IL_2A19;
								}
								else
								{
									if (!(text2 == "wm/producer"))
									{
										return result;
									}
									goto IL_26CB;
								}
							}
							else
							{
								if (!(text2 == "copyright"))
								{
									return result;
								}
								goto IL_1E34;
							}
						}
						else if (num != 3161448022u)
						{
							if (num != 3162467117u)
							{
								if (num != 3171021908u)
								{
									return result;
								}
								if (!(text2 == "ten"))
								{
									return result;
								}
								goto IL_1E92;
							}
							else
							{
								if (!(text2 == "isbj"))
								{
									return result;
								}
								goto IL_1CD9;
							}
						}
						else
						{
							if (!(text2 == "conductor"))
							{
								return result;
							}
							goto IL_24FE;
						}
					}
					else if (num <= 3768460191u)
					{
						if (num <= 3522494505u)
						{
							if (num <= 3333495651u)
							{
								if (num <= 3223598364u)
								{
									if (num != 3185987134u)
									{
										if (num != 3223598364u)
										{
											return result;
										}
										if (!(text2 == "wm/title"))
										{
											return result;
										}
										goto IL_1BCC;
									}
									else
									{
										if (!(text2 == "text"))
										{
											return result;
										}
										goto IL_25EB;
									}
								}
								else if (num != 3238484687u)
								{
									if (num != 3239277205u)
									{
										if (num != 3333495651u)
										{
											return result;
										}
										if (!(text2 == "ifrm"))
										{
											return result;
										}
										goto IL_1DD6;
									}
									else
									{
										if (!(text2 == "tenc"))
										{
											return result;
										}
										goto IL_1E92;
									}
								}
								else
								{
									if (!(text2 == "albumartist"))
									{
										return result;
									}
									goto IL_1CD9;
								}
							}
							else if (num <= 3437799904u)
							{
								if (num != 3347325063u)
								{
									if (num != 3357160338u)
									{
										if (num != 3437799904u)
										{
											return result;
										}
										if (!(text2 == "wm/composer"))
										{
											return result;
										}
										goto IL_1FAA;
									}
									else
									{
										if (!(text2 == "ultravox-name"))
										{
											return result;
										}
										goto IL_1CB2;
									}
								}
								else
								{
									if (!(text2 == "currentbitrate"))
									{
										return result;
									}
									goto IL_2C88;
								}
							}
							else if (num != 3445080013u)
							{
								if (num != 3483336282u)
								{
									if (num != 3522494505u)
									{
										return result;
									}
									if (!(text2 == "streamtitle"))
									{
										return result;
									}
									goto IL_2B89;
								}
								else
								{
									if (!(text2 == "ensemble"))
									{
										return result;
									}
									goto IL_1D41;
								}
							}
							else
							{
								if (!(text2 == "tracknumber"))
								{
									return result;
								}
								goto IL_1D78;
							}
						}
						else if (num <= 3678111601u)
						{
							if (num <= 3564297305u)
							{
								if (num != 3543308881u)
								{
									if (num != 3564297305u)
									{
										return result;
									}
									if (!(text2 == "date"))
									{
										return result;
									}
									text = array[1].TrimWithBOM();
									if (text != string.Empty && this.tagType != BASSTag.BASS_TAG_ID3V2)
									{
										this.year = text;
										return true;
									}
									return result;
								}
								else
								{
									if (!(text2 == "wm/genre"))
									{
										return result;
									}
									goto IL_2167;
								}
							}
							else if (num != 3603129877u)
							{
								if (num != 3609456715u)
								{
									if (num != 3678111601u)
									{
										return result;
									}
									if (!(text2 == "icms"))
									{
										return result;
									}
									goto IL_1F4C;
								}
								else
								{
									if (!(text2 == "icy-br"))
									{
										return result;
									}
									goto IL_2C4A;
								}
							}
							else
							{
								if (!(text2 == "ultravox-title"))
								{
									return result;
								}
								goto IL_2B89;
							}
						}
						else if (num <= 3704530584u)
						{
							if (num != 3694889220u)
							{
								if (num != 3703713125u)
								{
									if (num != 3704530584u)
									{
										return result;
									}
									if (!(text2 == "iwri"))
									{
										return result;
									}
									goto IL_25C4;
								}
								else
								{
									if (!(text2 == "coverart"))
									{
										return result;
									}
									goto IL_2B01;
								}
							}
							else
							{
								if (!(text2 == "icmt"))
								{
									return result;
								}
								goto IL_2049;
							}
						}
						else if (num != 3708646915u)
						{
							if (num != 3730857219u)
							{
								if (num != 3768460191u)
								{
									return result;
								}
								if (!(text2 == "tsrc"))
								{
									return result;
								}
								goto IL_2926;
							}
							else
							{
								if (!(text2 == "icrd"))
								{
									return result;
								}
								goto IL_20D6;
							}
						}
						else
						{
							if (!(text2 == "wm/conductor"))
							{
								return result;
							}
							goto IL_24FE;
						}
					}
					else if (num <= 4055450288u)
					{
						if (num <= 3945499333u)
						{
							if (num <= 3876361627u)
							{
								if (num != 3829404362u)
								{
									if (num != 3876361627u)
									{
										return result;
									}
									if (!(text2 == "idpi"))
									{
										return result;
									}
									goto IL_29AB;
								}
								else
								{
									if (!(text2 == "icop"))
									{
										return result;
									}
									goto IL_1E34;
								}
							}
							else if (num != 3886329039u)
							{
								if (num != 3895166476u)
								{
									if (num != 3945499333u)
									{
										return result;
									}
									if (!(text2 == "tt2"))
									{
										return result;
									}
									goto IL_1BCC;
								}
								else
								{
									if (!(text2 == "tt1"))
									{
										return result;
									}
									goto IL_2566;
								}
							}
							else if (!(text2 == "originalsource"))
							{
								return result;
							}
						}
						else if (num <= 4002128452u)
						{
							if (num != 3969640727u)
							{
								if (num != 3982560995u)
								{
									if (num != 4002128452u)
									{
										return result;
									}
									if (!(text2 == "wm/tracknumber"))
									{
										return result;
									}
									goto IL_1D78;
								}
								else
								{
									if (!(text2 == "songwriter"))
									{
										return result;
									}
									goto IL_262C;
								}
							}
							else
							{
								if (!(text2 == "wm/year"))
								{
									return result;
								}
								goto IL_20D6;
							}
						}
						else if (num != 4011007077u)
						{
							if (num != 4052603614u)
							{
								if (num != 4055450288u)
								{
									return result;
								}
								if (!(text2 == "tpe4"))
								{
									return result;
								}
								goto IL_268A;
							}
							else
							{
								if (!(text2 == "com"))
								{
									return result;
								}
								goto IL_2049;
							}
						}
						else
						{
							if (!(text2 == "caption"))
							{
								return result;
							}
							goto IL_2B89;
						}
					}
					else if (num <= 4139338383u)
					{
						if (num <= 4099267849u)
						{
							if (num != 4069068880u)
							{
								if (num != 4099267849u)
								{
									return result;
								}
								if (!(text2 == "wm/shareduserrating"))
								{
									return result;
								}
								goto IL_2784;
							}
							else
							{
								if (!(text2 == "rating"))
								{
									return result;
								}
								text = array[1].TrimWithBOM();
								if (!(text != string.Empty) || !string.IsNullOrEmpty(this.rating))
								{
									return result;
								}
								this.rating = text;
								result = true;
								if (text.Length == 1 && (text[0] < '0' || text[0] > '9'))
								{
									this.rating = ((byte)text[0]).ToString();
									return result;
								}
								return result;
							}
						}
						else if (num != 4114788297u)
						{
							if (num != 4137097213u)
							{
								if (num != 4139338383u)
								{
									return result;
								}
								if (!(text2 == "tpe1"))
								{
									return result;
								}
								goto IL_1C3A;
							}
							else
							{
								if (!(text2 == "label"))
								{
									return result;
								}
								goto IL_1F4C;
							}
						}
						else
						{
							if (!(text2 == "comm"))
							{
								return result;
							}
							goto IL_2049;
						}
					}
					else if (num <= 4170207818u)
					{
						if (num != 4144498492u)
						{
							if (num != 4156116002u)
							{
								if (num != 4170207818u)
								{
									return result;
								}
								if (!(text2 == "ultravox-url"))
								{
									return result;
								}
								goto IL_2C0E;
							}
							else
							{
								if (!(text2 == "tpe2"))
								{
									return result;
								}
								goto IL_1D00;
							}
						}
						else
						{
							if (!(text2 == "ultravox-genre"))
							{
								return result;
							}
							goto IL_2167;
						}
					}
					else if (num != 4172893621u)
					{
						if (num != 4185316089u)
						{
							if (num != 4219780606u)
							{
								return result;
							}
							if (!(text2 == "mood"))
							{
								return result;
							}
							goto IL_28FF;
						}
						else
						{
							if (!(text2 == "iart"))
							{
								return result;
							}
							goto IL_1C03;
						}
					}
					else
					{
						if (!(text2 == "tpe3"))
						{
							return result;
						}
						goto IL_2525;
					}
					text = array[1].TrimWithBOM();
					if (text != string.Empty && string.IsNullOrEmpty(this.publisher))
					{
						this.publisher = text;
						return true;
					}
					return result;
					IL_24FE:
					text = array[1].TrimWithBOM();
					if (text != string.Empty)
					{
						this.conductor = text;
						return true;
					}
					return result;
					IL_25EB:
					text = array[1].TrimWithBOM();
					if (text != string.Empty)
					{
						this.lyricist = string.Join("; ", text.Split(new char[]
						{
							'\0',
							'/'
						}));
						return true;
					}
					return result;
					IL_262C:
					text = array[1].TrimWithBOM();
					if (text != string.Empty && string.IsNullOrEmpty(this.lyricist))
					{
						this.lyricist = text;
						return true;
					}
					return result;
				}
				IL_1BCC:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.title = text.Trim(new char[]
					{
						'"'
					});
					return true;
				}
				return result;
				IL_1C03:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.artist = text.Trim(new char[]
					{
						'"'
					});
					return true;
				}
				return result;
				IL_1C3A:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.artist = string.Join("; ", text.Split(new char[]
					{
						'\0',
						'/'
					}));
					return true;
				}
				return result;
				IL_1C7B:
				text = array[1].TrimWithBOM();
				if (text != string.Empty && string.IsNullOrEmpty(this.artist))
				{
					this.artist = text;
					return true;
				}
				return result;
				IL_1CB2:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.album = text;
					return true;
				}
				return result;
				IL_1CD9:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.albumartist = text;
					return true;
				}
				return result;
				IL_1D00:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.albumartist = string.Join("; ", text.Split(new char[]
					{
						'\0',
						'/'
					}));
					return true;
				}
				return result;
				IL_1D41:
				text = array[1].TrimWithBOM();
				if (text != string.Empty && string.IsNullOrEmpty(this.albumartist))
				{
					this.albumartist = text;
					return true;
				}
				return result;
				IL_1D78:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.track = text;
					return true;
				}
				return result;
				IL_1DD6:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.disc = text;
					return true;
				}
				return result;
				IL_1E34:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.copyright = text;
					return true;
				}
				return result;
				IL_1E5B:
				text = array[1].TrimWithBOM();
				if (text != string.Empty && string.IsNullOrEmpty(this.copyright))
				{
					this.copyright = text;
					return true;
				}
				return result;
				IL_1E92:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.encodedby = text;
					return true;
				}
				return result;
				IL_1F4C:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.publisher = text;
					return true;
				}
				return result;
				IL_1FAA:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.composer = text;
					return true;
				}
				return result;
				IL_1FD1:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.composer = string.Join("; ", text.Split(new char[]
					{
						'\0',
						'/'
					}));
					return true;
				}
				return result;
				IL_2049:
				text = array[1].TrimWithBOM();
				if (text != string.Empty && this._commentCounter == 0)
				{
					string[] array3 = text.TrimWithBOM().Split(new char[]
					{
						'(',
						')',
						':'
					});
					if (array3 != null && array3.Length == 4 && array3[1].Length > 0 && array3[3].Length > 0)
					{
						this.comment = array3[3];
					}
					else
					{
						this.comment = text;
					}
					result = true;
					this._commentCounter++;
					return result;
				}
				return result;
				IL_20D6:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.year = text;
					return true;
				}
				return result;
				IL_2167:
				text = array[1].Trim(new char[]
				{
					'\'',
					'"'
				}).TrimWithBOM();
				if (!(text != string.Empty))
				{
					return result;
				}
				this.genre = text;
				result = true;
				if (this.channelinfo == null || this.channelinfo.ctype != BASSChannelType.BASS_CTYPE_STREAM_MP4)
				{
					return result;
				}
				try
				{
					int num3 = int.Parse(text);
					try
					{
						this.genre = BassTags.ID3v1Genre[num3 - 1];
					}
					catch
					{
					}
					return result;
				}
				catch
				{
					return result;
				}
				IL_21E6:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					string[] array4 = text.Split(new char[]
					{
						'\0',
						'/'
					});
					if (array4 != null && array4.Length != 0)
					{
						for (int i = 0; i < array4.Length; i++)
						{
							string text3 = array4[i].TrimWithBOM();
							string text4 = string.Empty;
							while (text3.Length > 1 && text3[0] == '(')
							{
								int num4 = text3.IndexOf(')');
								if (num4 < 0)
								{
									break;
								}
								string text5 = text3.Substring(1, num4 - 1).TrimWithBOM();
								int num5 = -1;
								if (int.TryParse(text5, out num5))
								{
									try
									{
										text5 = BassTags.ID3v1Genre[num5];
										goto IL_22B0;
									}
									catch
									{
										goto IL_22B0;
									}
									goto IL_2284;
								}
								goto IL_2284;
								IL_22B0:
								text3 = text3.Substring(num4 + 1).TrimStart(new char[]
								{
									'/',
									' '
								});
								if (text5 != null && text3.StartsWith(text5))
								{
									text3 = text3.Substring(text5.Length).TrimStart(new char[]
									{
										'/',
										' '
									});
								}
								else if (text3.StartsWith("(("))
								{
									num4 = text3.IndexOf(')');
									text5 = text3.Substring(2, num4 - 2).TrimWithBOM();
									text3 = text3.Substring(num4 + 1).TrimStart(new char[]
									{
										'/',
										' '
									});
								}
								else if (text3.Length > 0 && text3[0] != '(' && text3[0] != ' ' && text3[0] != '/')
								{
									num4 = text3.IndexOfAny(new char[]
									{
										'(',
										'/'
									});
									if (num4 > 0)
									{
										text5 = text3.Substring(0, num4).TrimWithBOM();
										text3 = text3.Substring(num4).TrimStart(new char[]
										{
											'/',
											' '
										});
									}
									else
									{
										text5 = text3;
										text3 = string.Empty;
									}
								}
								if (text4 == string.Empty)
								{
									text4 = text5;
									continue;
								}
								text4 = text4 + "; " + text5;
								continue;
								IL_2284:
								if (text5 == "RX")
								{
									text5 = "Remix";
									goto IL_22B0;
								}
								if (text5 == "CR")
								{
									text5 = "Cover";
									goto IL_22B0;
								}
								goto IL_22B0;
							}
							if (text3.Length > 0)
							{
								bool flag = true;
								string text6 = text3;
								text2 = text3;
								for (int j = 0; j < text2.Length; j++)
								{
									if (!char.IsNumber(text2[j]))
									{
										flag = false;
										break;
									}
								}
								if (flag)
								{
									int num6 = -1;
									if (int.TryParse(text3, out num6))
									{
										try
										{
											text6 = BassTags.ID3v1Genre[num6];
										}
										catch
										{
										}
									}
								}
								if (text4 == string.Empty)
								{
									text4 = text6;
								}
								else
								{
									text4 = text4 + "; " + text6;
								}
							}
							if (text4 == string.Empty)
							{
								array4[i] = text3;
							}
							else
							{
								array4[i] = text4;
							}
						}
						this.genre = string.Join("; ", array4);
					}
					else
					{
						this.genre = text;
					}
					return true;
				}
				return result;
				IL_2525:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.conductor = string.Join("; ", text.Split(new char[]
					{
						'\0',
						'/'
					}));
					return true;
				}
				return result;
				IL_2566:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.grouping = text;
					return true;
				}
				return result;
				IL_25C4:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.lyricist = text;
					return true;
				}
				return result;
				IL_2663:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.remixer = text;
					return true;
				}
				return result;
				IL_268A:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.remixer = string.Join("; ", text.Split(new char[]
					{
						'\0',
						'/'
					}));
					return true;
				}
				return result;
				IL_26CB:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.producer = text;
					return true;
				}
				return result;
				IL_26F2:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					string[] array5 = text.Split(new char[]
					{
						'\0',
						'/',
						';',
						':'
					});
					if (array5 != null && array5.Length > 1)
					{
						for (int k = 0; k < array5.Length; k += 2)
						{
							if (array5[k].TrimWithBOM().ToLower() == "producer" && k + 1 < array5.Length)
							{
								text = array5[k + 1].TrimWithBOM();
								break;
							}
						}
					}
					this.producer = text;
					return true;
				}
				return result;
				IL_2784:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.rating = text;
					return true;
				}
				return result;
				IL_28FF:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.mood = text;
					return true;
				}
				return result;
				IL_2926:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					this.isrc = text;
					return true;
				}
				return result;
				IL_294D:
				text = array[1].TrimWithBOM();
				if (text != string.Empty)
				{
					if (text.ToUpper().EndsWith("BPM"))
					{
						text = text.Substring(0, text.Length - 3).TrimWithBOM().TrimStart(new char[]
						{
							'0'
						});
					}
					this.bpm = text;
					return true;
				}
				return result;
				IL_29AB:
				text = array[1].TrimWithBOM();
				if (text != string.Empty && string.IsNullOrEmpty(this.bpm))
				{
					if (text.ToUpper().EndsWith("BPM"))
					{
						text = text.Substring(0, text.Length - 3).TrimWithBOM().TrimStart(new char[]
						{
							'0'
						});
					}
					this.bpm = text;
					return true;
				}
				return result;
				IL_2A19:
				text = array[1].TrimWithBOM();
				if (!(text != string.Empty))
				{
					return result;
				}
				if (text.ToUpper().EndsWith("DB"))
				{
					text = text.Substring(0, text.Length - 2).TrimWithBOM().TrimStart(new char[]
					{
						'0'
					});
				}
				try
				{
					this.replaygain_track_gain = float.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);
					return true;
				}
				catch
				{
					return result;
				}
				IL_2A8D:
				text = array[1].TrimWithBOM();
				if (!(text != string.Empty))
				{
					return result;
				}
				if (text.ToUpper().EndsWith("DB"))
				{
					text = text.Substring(0, text.Length - 2).TrimWithBOM().TrimStart(new char[]
					{
						'0'
					});
				}
				try
				{
					this.replaygain_track_peak = float.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);
					return true;
				}
				catch
				{
					return result;
				}
				IL_2B01:
				if (!BassTags.ReadPictureTAGs)
				{
					return result;
				}
				try
				{
					byte[] array6 = Convert.FromBase64String(array[1]);
					if (array6 != null && array6.Length != 0)
					{
						TagPicture tagPicture = new TagPicture(array6, 0);
						if (tagPicture.PictureImage != null)
						{
							this.AddPicture(tagPicture);
						}
					}
					return result;
				}
				catch
				{
					return result;
				}
				IL_2B45:
				if (!BassTags.ReadPictureTAGs)
				{
					return result;
				}
				try
				{
					byte[] array7 = Convert.FromBase64String(array[1]);
					if (array7 != null && array7.Length != 0)
					{
						TagPicture tagPicture2 = new TagPicture(array7, 1);
						if (tagPicture2.PictureImage != null)
						{
							this.AddPicture(tagPicture2);
						}
					}
					return result;
				}
				catch
				{
					return result;
				}
				IL_2B89:
				text = array[1].Trim(new char[]
				{
					'\'',
					'"'
				}).TrimWithBOM();
				if (text != string.Empty)
				{
					int num7 = text.IndexOf(" - ");
					if (num7 > 0 && num7 + 3 < text.Length)
					{
						this.artist = text.Substring(0, num7).TrimWithBOM();
						this.title = text.Substring(num7 + 3).TrimWithBOM();
					}
					else
					{
						this.title = text;
					}
					return true;
				}
				return result;
				IL_2C0E:
				text = array[1].Trim(new char[]
				{
					'\'',
					'"'
				}).TrimWithBOM();
				if (text != string.Empty)
				{
					this.comment = text;
					return true;
				}
				return result;
				IL_2C4A:
				text = array[1].Trim(new char[]
				{
					'\'',
					'"'
				}).TrimWithBOM();
				if (!(text != string.Empty))
				{
					return result;
				}
				try
				{
					this.bitrate = int.Parse(text);
					return true;
				}
				catch
				{
					return result;
				}
				IL_2C88:
				text = array[1].TrimWithBOM();
				if (!(text != string.Empty))
				{
					return result;
				}
				try
				{
					this.bitrate = int.Parse(text) / 1000;
					return result;
				}
				catch
				{
					return result;
				}
			}
			if (BassTags.EvalNativeTAGs && !this.nativetags.Contains(tagEntry) && tagEntry != string.Empty)
			{
				this.nativetags.Add(tagEntry);
			}
			return result;
		}

		public bool AddPicture(TagPicture tagPicture)
		{
			if (tagPicture == null)
			{
				return false;
			}
			bool result = false;
			try
			{
				this.pictures.Add(tagPicture);
				result = true;
			}
			catch
			{
			}
			return result;
		}

		public void RemovePicture(int i)
		{
			this.pictures.RemoveAt(i);
		}

		public void RemoveAllPictures()
		{
			this.pictures.Clear();
		}

		public int PictureCount
		{
			get
			{
				return this.pictures.Count;
			}
		}

		public TagPicture PictureGet(int i)
		{
			if (i < 0 || i > this.PictureCount - 1)
			{
				return null;
			}
			return this.pictures[i];
		}

		public Image PictureGetImage(int i)
		{
			if (i < 0 || i > this.PictureCount - 1)
			{
				return null;
			}
			try
			{
				return this.pictures[i].PictureImage;
			}
			catch
			{
			}
			return null;
		}

		public string PictureGetDescription(int i)
		{
			if (i < 0 || i > this.PictureCount - 1)
			{
				return null;
			}
			try
			{
				return this.pictures[i].Description;
			}
			catch
			{
			}
			return null;
		}

		public string PictureGetType(int i)
		{
			if (i < 0 || i > this.PictureCount - 1)
			{
				return null;
			}
			try
			{
				return this.pictures[i].PictureType.ToString();
			}
			catch
			{
			}
			return null;
		}

		public void ReadPicturesFromDirectory(string searchPattern, bool all)
		{
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(this.filename));
				FileInfo[] files;
				if (string.IsNullOrEmpty(searchPattern))
				{
					files = directoryInfo.GetFiles(Path.GetFileName(Path.ChangeExtension(this.filename, ".jpg")));
					if (files == null || files.Length < 1)
					{
						files = directoryInfo.GetFiles(Path.GetFileName(Path.ChangeExtension(this.filename, ".gif")));
					}
					if (files == null || files.Length < 1)
					{
						files = directoryInfo.GetFiles(Path.GetFileName(Path.ChangeExtension(this.filename, ".png")));
					}
					if (files == null || files.Length < 1)
					{
						files = directoryInfo.GetFiles(Path.GetFileName(Path.ChangeExtension(this.filename, ".bmp")));
					}
					if (files == null || files.Length < 1)
					{
						files = directoryInfo.GetFiles("Folder*.jpg");
					}
					if (files == null || files.Length < 1)
					{
						files = directoryInfo.GetFiles("Album*.jpg");
					}
					if (files == null || (files.Length < 1 && !string.IsNullOrEmpty(this.album)))
					{
						string str = this.album.Replace('?', '_').Replace('*', '_').Replace('|', '_').Replace('\\', '-').Replace('/', '-').Replace(':', '-').Replace('"', '\'').Replace('<', '(').Replace('>', ')');
						files = directoryInfo.GetFiles(str + "*.jpg");
						if (files == null || files.Length < 1)
						{
							files = directoryInfo.GetFiles(str + "*.gif");
						}
						if (files == null || files.Length < 1)
						{
							files = directoryInfo.GetFiles(str + "*.png");
						}
						if (files == null || files.Length < 1)
						{
							files = directoryInfo.GetFiles(str + "*.bmp");
						}
					}
				}
				else
				{
					files = directoryInfo.GetFiles(searchPattern);
				}
				if (files != null && files.Length != 0)
				{
					foreach (FileInfo fileInfo in files)
					{
						this.AddPicture(new TagPicture(fileInfo.FullName));
						if (!all)
						{
							break;
						}
					}
				}
			}
			catch
			{
			}
		}

		public static List<TagPicture> ReadPicturesFromDirectory(string filename, string album, string searchPattern, bool all)
		{
			List<TagPicture> list = new List<TagPicture>();
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(filename));
				FileInfo[] files;
				if (string.IsNullOrEmpty(searchPattern))
				{
					files = directoryInfo.GetFiles(Path.GetFileName(Path.ChangeExtension(filename, ".jpg")));
					if (files == null || files.Length < 1)
					{
						files = directoryInfo.GetFiles(Path.GetFileName(Path.ChangeExtension(filename, ".gif")));
					}
					if (files == null || files.Length < 1)
					{
						files = directoryInfo.GetFiles(Path.GetFileName(Path.ChangeExtension(filename, ".png")));
					}
					if (files == null || files.Length < 1)
					{
						files = directoryInfo.GetFiles(Path.GetFileName(Path.ChangeExtension(filename, ".bmp")));
					}
					if (files == null || files.Length < 1)
					{
						files = directoryInfo.GetFiles("Folder*.jpg");
					}
					if (files == null || files.Length < 1)
					{
						files = directoryInfo.GetFiles("Album*.jpg");
					}
					if (files == null || (files.Length < 1 && !string.IsNullOrEmpty(album)))
					{
						string str = album.Replace('?', '_').Replace('*', '_').Replace('|', '_').Replace('\\', '-').Replace('/', '-').Replace(':', '-').Replace('"', '\'').Replace('<', '(').Replace('>', ')');
						files = directoryInfo.GetFiles(str + "*.jpg");
						if (files == null || files.Length < 1)
						{
							files = directoryInfo.GetFiles(str + "*.gif");
						}
						if (files == null || files.Length < 1)
						{
							files = directoryInfo.GetFiles(str + "*.png");
						}
						if (files == null || files.Length < 1)
						{
							files = directoryInfo.GetFiles(str + "*.bmp");
						}
					}
				}
				else
				{
					files = directoryInfo.GetFiles(searchPattern);
				}
				if (files != null && files.Length != 0)
				{
					foreach (FileInfo fileInfo in files)
					{
						list.Add(new TagPicture(fileInfo.FullName));
						if (!all)
						{
							break;
						}
					}
				}
			}
			catch
			{
			}
			return list;
		}

		public byte[] ConvertToRiffINFO(bool fromNativeTags)
		{
			byte[] result;
			try
			{
				ASCIIEncoding asciiencoding = new ASCIIEncoding();
				List<byte> list = new List<byte>();
				list.AddRange(asciiencoding.GetBytes("INFO"));
				string text = fromNativeTags ? this.NativeTag("IART") : this.artist;
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("IART"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("INAM") : this.title);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("INAM"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("IPRD") : this.album);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("IPRD"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("ISBJ") : this.albumartist);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("ISBJ"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("IPRT") : this.track);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("IPRT"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("IFRM") : this.disc);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("IFRM"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("ICRD") : this.year);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("ICRD"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("IGNR") : this.genre);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("IGNR"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("ICOP") : this.copyright);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("ICOP"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("ISFT") : this.encodedby);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("ISFT"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("ICMT") : this.comment);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("ICMT"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("IENG") : this.composer);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("IENG"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("ICMS") : this.publisher);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("ICMS"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("ITCH") : this.conductor);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("ITCH"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("IWRI") : this.lyricist);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("IWRI"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("IEDT") : this.remixer);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("IEDT"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("IPRO") : this.producer);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("IPRO"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("ISRF") : this.grouping);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("ISRF"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("IKEY") : this.mood);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("IKEY"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("ISHP") : this.rating);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("ISHP"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("ISRC") : this.isrc);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("ISRC"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("IBPM") : this.bpm);
				if (!string.IsNullOrEmpty(text))
				{
					list.AddRange(asciiencoding.GetBytes("IBPM"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("IRGP") : this.replaygain_track_peak.ToString("R", CultureInfo.InvariantCulture));
				if ((fromNativeTags && !string.IsNullOrEmpty(text)) || (!fromNativeTags && this.replaygain_track_peak >= 0f))
				{
					list.AddRange(asciiencoding.GetBytes("IRGP"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				text = (fromNativeTags ? this.NativeTag("IRGG") : (this.replaygain_track_gain.ToString("R", CultureInfo.InvariantCulture) + " dB"));
				if ((fromNativeTags && !string.IsNullOrEmpty(text)) || (!fromNativeTags && this.replaygain_track_gain >= -60f && this.replaygain_track_gain <= 60f))
				{
					list.AddRange(asciiencoding.GetBytes("IRGG"));
					list.AddRange(BitConverter.GetBytes(text.Length));
					byte[] bytes = asciiencoding.GetBytes(text);
					list.AddRange(bytes);
					if (bytes.Length % 2 != 0)
					{
						list.Add(0);
					}
				}
				result = list.ToArray();
			}
			catch
			{
				result = null;
			}
			return result;
		}

		public byte[] ConvertToRiffBEXT(bool fromNativeTags)
		{
			byte[] result;
			try
			{
				BASS_TAG_BEXT bass_TAG_BEXT = default(BASS_TAG_BEXT);
				bass_TAG_BEXT.Description = (fromNativeTags ? this.NativeTag("BWFDescription") : this.title);
				bass_TAG_BEXT.Originator = (fromNativeTags ? this.NativeTag("BWFOriginator") : this.artist);
				bass_TAG_BEXT.OriginatorReference = (fromNativeTags ? this.NativeTag("BWFOriginatorReference") : this.encodedby);
				if (fromNativeTags)
				{
					bass_TAG_BEXT.OriginationDate = this.NativeTag("BWFOriginationDate");
					bass_TAG_BEXT.OriginationTime = this.NativeTag("BWFOriginationTime");
					try
					{
						string value = this.NativeTag("BWFTimeReference");
						if (string.IsNullOrEmpty(value))
						{
							value = "0";
						}
						bass_TAG_BEXT.TimeReference = Convert.ToInt64(value);
						goto IL_1FA;
					}
					catch
					{
						bass_TAG_BEXT.TimeReference = 0L;
						goto IL_1FA;
					}
				}
				DateTime now = DateTime.Now;
				try
				{
					if (DateTime.TryParseExact(this.year, new string[]
					{
						"yyyy-MM-dd HH:mm:ss",
						"yyyy-MM",
						"yyyy",
						"yyyy-MM-dd",
						"dd.MM.yyyy",
						"MM/dd/yyyy",
						"HH:mm:ss"
					}, CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out now))
					{
						bass_TAG_BEXT.OriginationDate = now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
						bass_TAG_BEXT.OriginationTime = now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
					}
					else if (this.year.Length >= 19)
					{
						bass_TAG_BEXT.OriginationDate = this.year.Substring(0, 10);
						bass_TAG_BEXT.OriginationTime = this.year.Substring(11, 8);
					}
					else if (this.year.Length >= 10)
					{
						bass_TAG_BEXT.OriginationDate = this.year.Substring(0, 10);
						bass_TAG_BEXT.OriginationTime = "00:00:00";
					}
					else
					{
						bass_TAG_BEXT.OriginationDate = now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
						bass_TAG_BEXT.OriginationTime = now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
					}
				}
				catch
				{
				}
				bass_TAG_BEXT.TimeReference = 0L;
				IL_1FA:
				bass_TAG_BEXT.UMID = (fromNativeTags ? this.NativeTag("BWFUMID") : string.Empty);
				if (fromNativeTags)
				{
					try
					{
						string value2 = this.NativeTag("BWFVersion");
						if (string.IsNullOrEmpty(value2))
						{
							value2 = "1";
						}
						bass_TAG_BEXT.Version = Convert.ToInt16(value2);
						goto IL_255;
					}
					catch
					{
						bass_TAG_BEXT.Version = 1;
						goto IL_255;
					}
				}
				bass_TAG_BEXT.Version = 1;
				IL_255:
				bass_TAG_BEXT.Reserved = new byte[190];
				result = bass_TAG_BEXT.AsByteArray(fromNativeTags ? this.NativeTag("BWFCodingHistory") : string.Empty);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		public byte[] ConvertToRiffCART(bool fromNativeTags)
		{
			byte[] result;
			try
			{
				BASS_TAG_CART bass_TAG_CART = default(BASS_TAG_CART);
				bass_TAG_CART.Title = (fromNativeTags ? this.NativeTag("CARTTitle") : this.title);
				bass_TAG_CART.Artist = (fromNativeTags ? this.NativeTag("CARTArtist") : this.artist);
				bass_TAG_CART.CutID = (fromNativeTags ? this.NativeTag("CARTCutID") : this.album);
				bass_TAG_CART.ClientID = (fromNativeTags ? this.NativeTag("CARTClientID") : this.copyright);
				bass_TAG_CART.Category = (fromNativeTags ? this.NativeTag("CARTCategory") : this.genre);
				bass_TAG_CART.Classification = (fromNativeTags ? this.NativeTag("CARTClassification") : this.grouping);
				bass_TAG_CART.ProducerAppID = (fromNativeTags ? this.NativeTag("CARTProducerAppID") : this.encodedby);
				if (fromNativeTags)
				{
					bass_TAG_CART.Version = this.NativeTag("CARTVersion");
					bass_TAG_CART.OutCue = this.NativeTag("CARTOutCue");
					bass_TAG_CART.StartDate = this.NativeTag("CARTStartDate");
					bass_TAG_CART.StartTime = this.NativeTag("CARTStartTime");
					bass_TAG_CART.EndDate = this.NativeTag("CARTEndDate");
					bass_TAG_CART.EndTime = this.NativeTag("CARTEndTime");
					bass_TAG_CART.ProducerAppVersion = this.NativeTag("CARTProducerAppVersion");
					bass_TAG_CART.UserDef = this.NativeTag("CARTUserDef");
					try
					{
						string value = this.NativeTag("CARTLevelReference");
						if (string.IsNullOrEmpty(value))
						{
							value = "32768";
						}
						bass_TAG_CART.LevelReference = (int)Convert.ToInt16(value);
					}
					catch
					{
						bass_TAG_CART.LevelReference = 32768;
					}
					bass_TAG_CART.Timer1Usage = this.NativeTag("CARTTimer1Usage");
					bass_TAG_CART.Timer2Usage = this.NativeTag("CARTTimer2Usage");
					bass_TAG_CART.Timer3Usage = this.NativeTag("CARTTimer3Usage");
					bass_TAG_CART.Timer4Usage = this.NativeTag("CARTTimer4Usage");
					bass_TAG_CART.Timer5Usage = this.NativeTag("CARTTimer5Usage");
					bass_TAG_CART.Timer6Usage = this.NativeTag("CARTTimer6Usage");
					bass_TAG_CART.Timer7Usage = this.NativeTag("CARTTimer7Usage");
					bass_TAG_CART.Timer8Usage = this.NativeTag("CARTTimer8Usage");
					try
					{
						string value2 = this.NativeTag("CARTTimer1Value");
						if (string.IsNullOrEmpty(value2))
						{
							value2 = "0";
						}
						bass_TAG_CART.Timer1Value = (int)Convert.ToInt16(value2);
					}
					catch
					{
						bass_TAG_CART.Timer1Value = 0;
					}
					try
					{
						string value3 = this.NativeTag("CARTTimer2Value");
						if (string.IsNullOrEmpty(value3))
						{
							value3 = "0";
						}
						bass_TAG_CART.Timer2Value = (int)Convert.ToInt16(value3);
					}
					catch
					{
						bass_TAG_CART.Timer2Value = 0;
					}
					try
					{
						string value4 = this.NativeTag("CARTTimer3Value");
						if (string.IsNullOrEmpty(value4))
						{
							value4 = "0";
						}
						bass_TAG_CART.Timer3Value = (int)Convert.ToInt16(value4);
					}
					catch
					{
						bass_TAG_CART.Timer3Value = 0;
					}
					try
					{
						string value5 = this.NativeTag("CARTTimer4Value");
						if (string.IsNullOrEmpty(value5))
						{
							value5 = "0";
						}
						bass_TAG_CART.Timer4Value = (int)Convert.ToInt16(value5);
					}
					catch
					{
						bass_TAG_CART.Timer4Value = 0;
					}
					try
					{
						string value6 = this.NativeTag("CARTTimer5Value");
						if (string.IsNullOrEmpty(value6))
						{
							value6 = "0";
						}
						bass_TAG_CART.Timer5Value = (int)Convert.ToInt16(value6);
					}
					catch
					{
						bass_TAG_CART.Timer5Value = 0;
					}
					try
					{
						string value7 = this.NativeTag("CARTTimer6Value");
						if (string.IsNullOrEmpty(value7))
						{
							value7 = "0";
						}
						bass_TAG_CART.Timer6Value = (int)Convert.ToInt16(value7);
					}
					catch
					{
						bass_TAG_CART.Timer6Value = 0;
					}
					try
					{
						string value8 = this.NativeTag("CARTTimer7Value");
						if (string.IsNullOrEmpty(value8))
						{
							value8 = "0";
						}
						bass_TAG_CART.Timer7Value = (int)Convert.ToInt16(value8);
					}
					catch
					{
						bass_TAG_CART.Timer7Value = 0;
					}
					try
					{
						string value9 = this.NativeTag("CARTTimer8Value");
						if (string.IsNullOrEmpty(value9))
						{
							value9 = "0";
						}
						bass_TAG_CART.Timer8Value = (int)Convert.ToInt16(value9);
					}
					catch
					{
						bass_TAG_CART.Timer8Value = 0;
					}
					bass_TAG_CART.URL = this.NativeTag("CARTURL");
				}
				else
				{
					bass_TAG_CART.Version = "0100";
					bass_TAG_CART.OutCue = string.Empty;
					bass_TAG_CART.StartDate = null;
					bass_TAG_CART.StartTime = null;
					bass_TAG_CART.EndDate = null;
					bass_TAG_CART.EndTime = null;
					bass_TAG_CART.ProducerAppVersion = string.Empty;
					bass_TAG_CART.UserDef = string.Empty;
					bass_TAG_CART.LevelReference = 32768;
					bass_TAG_CART.Timer1Usage = null;
					bass_TAG_CART.Timer1Value = 0;
					bass_TAG_CART.Timer2Usage = null;
					bass_TAG_CART.Timer2Value = 0;
					bass_TAG_CART.Timer3Usage = null;
					bass_TAG_CART.Timer3Value = 0;
					bass_TAG_CART.Timer4Usage = null;
					bass_TAG_CART.Timer4Value = 0;
					bass_TAG_CART.Timer5Usage = null;
					bass_TAG_CART.Timer5Value = 0;
					bass_TAG_CART.Timer6Usage = null;
					bass_TAG_CART.Timer6Value = 0;
					bass_TAG_CART.Timer7Usage = null;
					bass_TAG_CART.Timer7Value = 0;
					bass_TAG_CART.Timer8Usage = null;
					bass_TAG_CART.Timer8Value = 0;
					bass_TAG_CART.URL = string.Empty;
				}
				bass_TAG_CART.Reserved = new byte[276];
				result = bass_TAG_CART.AsByteArray(fromNativeTags ? this.NativeTag("CARTTagText") : this.comment);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		public string title = string.Empty;

		public string artist = string.Empty;

		public string album = string.Empty;

		public string albumartist = string.Empty;

		public string year = string.Empty;

		public string comment = string.Empty;

		public string genre = string.Empty;

		public string track = string.Empty;

		public string disc = string.Empty;

		public string copyright = string.Empty;

		public string encodedby = string.Empty;

		public string composer = string.Empty;

		public string publisher = string.Empty;

		public string lyricist = string.Empty;

		public string remixer = string.Empty;

		public string producer = string.Empty;

		public string bpm = string.Empty;

		public string filename = string.Empty;

		private List<TagPicture> pictures = new List<TagPicture>();

		private List<string> nativetags = new List<string>();

		public BASS_CHANNELINFO channelinfo = new BASS_CHANNELINFO();

		public BASSTag tagType = BASSTag.BASS_TAG_UNKNOWN;

		public double duration;

		public int bitrate;

		public float replaygain_track_gain = -100f;

		public float replaygain_track_peak = -1f;

		public string conductor = string.Empty;

		public string grouping = string.Empty;

		public string mood = string.Empty;

		public string rating = string.Empty;

		public string isrc = string.Empty;

		private int _multiCounter;

		private int _commentCounter;
	}
}
