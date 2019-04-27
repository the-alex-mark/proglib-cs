using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Un4seen.Bass.AddOn.Wma;

namespace Un4seen.Bass.AddOn.Tags
{
	[SuppressUnmanagedCodeSecurity]
	[Serializable]
	public sealed class BassTags
	{
		private BassTags()
		{
		}

		public static TAG_INFO BASS_TAG_GetFromFile(string file)
		{
			return BassTags.BASS_TAG_GetFromFile(file, true, true);
		}

		public static TAG_INFO BASS_TAG_GetFromFile(string file, bool setDefaultTitle, bool prescan)
		{
			if (string.IsNullOrEmpty(file))
			{
				return null;
			}
			TAG_INFO tag_INFO = new TAG_INFO(file, setDefaultTitle);
			if (BassTags.BASS_TAG_GetFromFile(file, prescan, tag_INFO))
			{
				return tag_INFO;
			}
			return null;
		}

		public static bool BASS_TAG_GetFromFile(string file, bool prescan, TAG_INFO tags)
		{
			if (tags == null)
			{
				return false;
			}
			int num = Bass.BASS_StreamCreateFile(file, 0L, 0L, BASSFlag.BASS_STREAM_DECODE | (prescan ? BASSFlag.BASS_STREAM_PRESCAN : BASSFlag.BASS_DEFAULT));
			if (num != 0)
			{
				BassTags.BASS_TAG_GetFromFile(num, tags);
				Bass.BASS_StreamFree(num);
				return true;
			}
			string a = Path.GetExtension(file).ToLower();
			if (!(a == ".wma") && !(a == ".wmv"))
			{
				return false;
			}
			IntPtr intPtr = BassWma.BASS_WMA_GetTags(file);
			if (intPtr != IntPtr.Zero)
			{
				tags.tagType = BASSTag.BASS_TAG_WMA;
				tags.UpdateFromMETA(intPtr, true, false);
				if (BassTags.ReadPictureTAGs)
				{
					IWMMetadataEditor iwmmetadataEditor = null;
					try
					{
						WMFMetadataEditor.WMCreateEditor(out iwmmetadataEditor);
						iwmmetadataEditor.Open(file);
						List<TagPicture> allPictures = new WMFMetadataEditor((IWMHeaderInfo3)iwmmetadataEditor).GetAllPictures();
						if (allPictures != null)
						{
							foreach (TagPicture tagPicture in allPictures)
							{
								tags.AddPicture(tagPicture);
							}
						}
					}
					catch
					{
					}
					finally
					{
						if (iwmmetadataEditor != null)
						{
							iwmmetadataEditor.Close();
						}
						iwmmetadataEditor = null;
					}
				}
				return true;
			}
			return false;
		}

		public static bool BASS_TAG_GetFromFile(int stream, TAG_INFO tags)
		{
			if (stream == 0 || tags == null)
			{
				return false;
			}
			bool flag = false;
			BASS_CHANNELINFO bass_CHANNELINFO = new BASS_CHANNELINFO();
			if (Bass.BASS_ChannelGetInfo(stream, bass_CHANNELINFO))
			{
				tags.channelinfo = bass_CHANNELINFO;
				BASSTag basstag = BASSTag.BASS_TAG_UNKNOWN;
				IntPtr intPtr = BassTags.BASS_TAG_GetIntPtr(stream, bass_CHANNELINFO, out basstag);
				tags.tagType = basstag;
				if (intPtr != IntPtr.Zero)
				{
					if (basstag <= BASSTag.BASS_TAG_MUSIC_NAME)
					{
						if (basstag <= BASSTag.BASS_TAG_MF)
						{
							switch (basstag)
							{
							case BASSTag.BASS_TAG_ID3:
							{
								IntPtr intPtr2 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_APE);
								if (intPtr2 != IntPtr.Zero)
								{
									tags.UpdateFromMETA(intPtr2, true, false);
									if (BassTags.ReadPictureTAGs)
									{
										BassTags.ReadAPEPictures(stream, tags);
									}
									tags.ResetTags2();
								}
								flag = BassTags.ReadID3v1(intPtr, tags);
								goto IL_782;
							}
							case BASSTag.BASS_TAG_ID3V2:
							{
								IntPtr intPtr3 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3);
								if (intPtr3 != IntPtr.Zero)
								{
									BassTags.ReadID3v1(intPtr3, tags);
									tags.ResetTags2();
								}
								IntPtr intPtr4 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_APE);
								if (intPtr4 != IntPtr.Zero)
								{
									tags.UpdateFromMETA(intPtr4, true, false);
									if (BassTags.ReadPictureTAGs)
									{
										BassTags.ReadAPEPictures(stream, tags);
									}
									tags.ResetTags2();
								}
								flag = BassTags.ReadID3v2(intPtr, tags);
								goto IL_782;
							}
							case BASSTag.BASS_TAG_OGG:
							{
								IntPtr intPtr5 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_APE);
								if (intPtr5 != IntPtr.Zero)
								{
									tags.UpdateFromMETA(intPtr5, true, false);
									if (BassTags.ReadPictureTAGs)
									{
										BassTags.ReadAPEPictures(stream, tags);
									}
									tags.ResetTags2();
								}
								if (BassTags.ReadPictureTAGs && (bass_CHANNELINFO.ctype == BASSChannelType.BASS_CTYPE_STREAM_FLAC || bass_CHANNELINFO.ctype == BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG))
								{
									int num = 0;
									BASS_TAG_FLAC_PICTURE tag;
									while ((tag = BASS_TAG_FLAC_PICTURE.GetTag(stream, num)) != null)
									{
										tags.AddPicture(new TagPicture(num, tag.Mime, TagPicture.PICTURE_TYPE.FrontAlbumCover, tag.Desc, tag.Data));
										num++;
									}
								}
								flag = tags.UpdateFromMETA(intPtr, true, false);
								goto IL_782;
							}
							case BASSTag.BASS_TAG_HTTP:
							case BASSTag.BASS_TAG_ICY:
							case BASSTag.BASS_TAG_META:
								goto IL_782;
							case BASSTag.BASS_TAG_APE:
							{
								IntPtr intPtr6 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3);
								if (intPtr6 != IntPtr.Zero)
								{
									BassTags.ReadID3v1(intPtr6, tags);
									tags.ResetTags2();
								}
								IntPtr intPtr7 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3V2);
								if (intPtr7 != IntPtr.Zero)
								{
									BassTags.ReadID3v2(intPtr7, tags);
									tags.ResetTags2();
								}
								if (BassTags.ReadPictureTAGs && (bass_CHANNELINFO.ctype == BASSChannelType.BASS_CTYPE_STREAM_FLAC || bass_CHANNELINFO.ctype == BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG))
								{
									int num2 = 0;
									BASS_TAG_FLAC_PICTURE tag2;
									while ((tag2 = BASS_TAG_FLAC_PICTURE.GetTag(stream, num2)) != null)
									{
										tags.AddPicture(new TagPicture(num2, tag2.Mime, TagPicture.PICTURE_TYPE.FrontAlbumCover, tag2.Desc, tag2.Data));
										num2++;
									}
								}
								flag = tags.UpdateFromMETA(intPtr, true, false);
								if (BassTags.ReadPictureTAGs)
								{
									BassTags.ReadAPEPictures(stream, tags);
									goto IL_782;
								}
								goto IL_782;
							}
							case BASSTag.BASS_TAG_MP4:
							{
								IntPtr intPtr8 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_APE);
								if (intPtr8 != IntPtr.Zero)
								{
									tags.UpdateFromMETA(intPtr8, true, false);
									if (BassTags.ReadPictureTAGs)
									{
										BassTags.ReadAPEPictures(stream, tags);
									}
									tags.ResetTags2();
								}
								IntPtr intPtr9 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3V2);
								if (intPtr9 != IntPtr.Zero)
								{
									BassTags.ReadID3v2(intPtr9, tags);
									tags.ResetTags2();
								}
								flag = tags.UpdateFromMETA(intPtr, true, false);
								goto IL_782;
							}
							case BASSTag.BASS_TAG_WMA:
								flag = tags.UpdateFromMETA(intPtr, true, false);
								if (!BassTags.ReadPictureTAGs)
								{
									goto IL_782;
								}
								try
								{
									IntPtr intPtr10 = BassWma.BASS_WMA_GetWMObject(stream);
									if (intPtr10 != IntPtr.Zero)
									{
										IWMHeaderInfo3 iwmheaderInfo = (IWMHeaderInfo3)Marshal.GetObjectForIUnknown(intPtr10);
										List<TagPicture> allPictures = new WMFMetadataEditor(iwmheaderInfo).GetAllPictures();
										if (allPictures != null)
										{
											foreach (TagPicture tagPicture in allPictures)
											{
												tags.AddPicture(tagPicture);
											}
										}
										Marshal.FinalReleaseComObject(iwmheaderInfo);
									}
									goto IL_782;
								}
								catch
								{
									goto IL_782;
								}
								break;
							default:
								if (basstag != BASSTag.BASS_TAG_MF)
								{
									goto IL_782;
								}
								break;
							}
							flag = tags.UpdateFromMETA(intPtr, true, false);
						}
						else
						{
							switch (basstag)
							{
							case BASSTag.BASS_TAG_RIFF_INFO:
							{
								flag = tags.UpdateFromMETA(intPtr, BassNet.UseRiffInfoUTF8, false);
								IntPtr intPtr11 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_RIFF_BEXT);
								if (intPtr11 != IntPtr.Zero)
								{
									BassTags.ReadRiffBEXT(intPtr11, tags);
								}
								IntPtr intPtr12 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_RIFF_CART);
								if (intPtr12 != IntPtr.Zero)
								{
									BassTags.ReadRiffCART(intPtr12, tags);
								}
								IntPtr intPtr13 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3V2);
								if (intPtr13 != IntPtr.Zero)
								{
									BassTags.ReadID3v2(intPtr13, tags);
								}
								IntPtr intPtr14 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_RIFF_DISP);
								if (intPtr14 != IntPtr.Zero)
								{
									string text;
									if (BassNet.UseRiffInfoUTF8)
									{
										int num3;
										text = Utils.IntPtrAsStringUtf8orLatin1(intPtr14, out num3);
									}
									else
									{
										text = Utils.IntPtrAsStringAnsi(intPtr14);
									}
									if (!string.IsNullOrEmpty(text))
									{
										if (string.IsNullOrEmpty(tags.title))
										{
											tags.title = text;
										}
										else if (string.IsNullOrEmpty(tags.artist))
										{
											tags.artist = text;
										}
										else if (string.IsNullOrEmpty(tags.album))
										{
											tags.album = text;
										}
									}
								}
								break;
							}
							case BASSTag.BASS_TAG_RIFF_BEXT:
							case BASSTag.BASS_TAG_RIFF_CART:
							{
								IntPtr intPtr15 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_RIFF_INFO);
								if (intPtr15 != IntPtr.Zero)
								{
									tags.UpdateFromMETA(intPtr15, BassNet.UseRiffInfoUTF8, false);
								}
								IntPtr intPtr16 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_RIFF_CART);
								if (intPtr16 != IntPtr.Zero)
								{
									BassTags.ReadRiffCART(intPtr16, tags);
								}
								IntPtr intPtr17 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3V2);
								if (intPtr17 != IntPtr.Zero)
								{
									BassTags.ReadID3v2(intPtr17, tags);
									tags.ResetTags2();
								}
								flag = BassTags.ReadRiffBEXT(intPtr, tags);
								break;
							}
							default:
								if (basstag == BASSTag.BASS_TAG_MUSIC_NAME)
								{
									tags.title = Bass.BASS_ChannelGetMusicName(stream);
									if (tags.title == null)
									{
										tags.title = string.Empty;
									}
									tags.artist = Bass.BASS_ChannelGetMusicMessage(stream);
									if (tags.artist == null)
									{
										tags.artist = string.Empty;
									}
									flag = true;
								}
								break;
							}
						}
					}
					else
					{
						if (basstag <= BASSTag.BASS_TAG_DSD_ARTIST)
						{
							if (basstag != BASSTag.BASS_TAG_MIDI_TRACK)
							{
								if (basstag != BASSTag.BASS_TAG_DSD_ARTIST)
								{
									goto IL_782;
								}
							}
							else
							{
								int num4 = 0;
								for (;;)
								{
									IntPtr intPtr18 = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_MIDI_TRACK + num4);
									if (!(intPtr18 != IntPtr.Zero))
									{
										break;
									}
									flag |= tags.UpdateFromMETA(intPtr18, false, false);
									num4++;
								}
								if (flag || tags.NativeTags.Length == 0)
								{
									goto IL_782;
								}
								flag = true;
								if (tags.NativeTags.Length != 0)
								{
									tags.title = tags.NativeTags[0].TrimWithBOM();
								}
								if (tags.NativeTags.Length > 1)
								{
									tags.artist = tags.NativeTags[1].TrimWithBOM();
									goto IL_782;
								}
								goto IL_782;
							}
						}
						else if (basstag != BASSTag.BASS_TAG_DSD_TITLE && basstag != BASSTag.BASS_TAG_DSD_COMMENT)
						{
							goto IL_782;
						}
						tags.title = Bass.BASS_ChannelGetTagsDSDTitle(stream);
						if (tags.title == null)
						{
							tags.title = string.Empty;
						}
						tags.artist = Bass.BASS_ChannelGetTagsDSDArtist(stream);
						if (tags.artist == null)
						{
							tags.artist = string.Empty;
						}
						StringBuilder stringBuilder = new StringBuilder();
						BASS_TAG_DSD_COMMENT[] array = Bass.BASS_ChannelGetTagsDSDComments(stream);
						if (array != null)
						{
							foreach (BASS_TAG_DSD_COMMENT bass_TAG_DSD_COMMENT in array)
							{
								stringBuilder.AppendFormat("{0}.{1}.{2} {3}:{4} ({5}/{6}) - {7}\n", new object[]
								{
									bass_TAG_DSD_COMMENT.TimeStampYear,
									bass_TAG_DSD_COMMENT.TimeStampMonth,
									bass_TAG_DSD_COMMENT.TimeStampDay,
									bass_TAG_DSD_COMMENT.TimeStampHour,
									bass_TAG_DSD_COMMENT.TimeStampMinutes,
									bass_TAG_DSD_COMMENT.CommentType,
									bass_TAG_DSD_COMMENT.CommentRef,
									bass_TAG_DSD_COMMENT.CommentText
								});
							}
						}
						tags.comment = stringBuilder.ToString();
					}
				}
				IL_782:
				tags.duration = Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream));
				if (tags.duration < 0.0)
				{
					tags.duration = 0.0;
				}
				if (tags.bitrate == 0)
				{
					float num5 = 0f;
					if (Bass.BASS_ChannelGetAttribute(stream, BASSAttribute.BASS_ATTRIB_BITRATE, ref num5))
					{
						tags.bitrate = (int)num5;
					}
					else
					{
						long num6 = Bass.BASS_StreamGetFilePosition(stream, BASSStreamFilePosition.BASS_FILEPOS_END);
						tags.bitrate = (int)((double)num6 / (125.0 * tags.duration) + 0.5);
					}
				}
			}
			return flag;
		}

		public static bool BASS_TAG_GetFromURL(int stream, TAG_INFO tags)
		{
			if (stream == 0 || tags == null)
			{
				return false;
			}
			bool result = false;
			BASS_CHANNELINFO bass_CHANNELINFO = new BASS_CHANNELINFO();
			if (Bass.BASS_ChannelGetInfo(stream, bass_CHANNELINFO))
			{
				tags.channelinfo = bass_CHANNELINFO;
			}
			IntPtr intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ICY);
			if (intPtr != IntPtr.Zero)
			{
				tags.tagType = BASSTag.BASS_TAG_ICY;
			}
			else
			{
				intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_HTTP);
				if (intPtr != IntPtr.Zero)
				{
					tags.tagType = BASSTag.BASS_TAG_HTTP;
				}
				else
				{
					intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_HLS_EXTINF);
					if (intPtr != IntPtr.Zero)
					{
						tags.tagType = BASSTag.BASS_TAG_HLS_EXTINF;
					}
				}
			}
			if (intPtr != IntPtr.Zero)
			{
				result = tags.UpdateFromMETA(intPtr, TAGINFOEncoding.Utf8OrLatin1, true);
			}
			intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_META);
			if (intPtr != IntPtr.Zero)
			{
				tags.tagType = BASSTag.BASS_TAG_META;
				result = tags.UpdateFromMETA(intPtr, TAGINFOEncoding.Utf8OrLatin1, true);
			}
			else
			{
				intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_OGG);
				if (intPtr == IntPtr.Zero)
				{
					intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_APE);
					if (intPtr != IntPtr.Zero)
					{
						tags.tagType = BASSTag.BASS_TAG_APE;
					}
				}
				else
				{
					tags.tagType = BASSTag.BASS_TAG_OGG;
				}
				if (intPtr == IntPtr.Zero)
				{
					intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_WMA);
					if (intPtr != IntPtr.Zero)
					{
						tags.tagType = BASSTag.BASS_TAG_WMA;
					}
				}
				if (intPtr == IntPtr.Zero)
				{
					intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3V2);
				}
				if (intPtr != IntPtr.Zero)
				{
					result = tags.UpdateFromMETA(intPtr, TAGINFOEncoding.Utf8, false);
				}
			}
			tags.duration = Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream));
			return result;
		}

		private static IntPtr BASS_TAG_GetIntPtr(int stream, BASS_CHANNELINFO info, out BASSTag tagType)
		{
			IntPtr intPtr = IntPtr.Zero;
			tagType = BASSTag.BASS_TAG_UNKNOWN;
			if (stream == 0 || info == null)
			{
				return intPtr;
			}
			BASSChannelType basschannelType = info.ctype;
			if ((basschannelType & BASSChannelType.BASS_CTYPE_STREAM_WAV) > BASSChannelType.BASS_CTYPE_UNKNOWN)
			{
				basschannelType = BASSChannelType.BASS_CTYPE_STREAM_WAV;
			}
			if (basschannelType <= BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG)
			{
				if (basschannelType <= BASSChannelType.BASS_CTYPE_STREAM_WINAMP)
				{
					if (basschannelType <= BASSChannelType.BASS_CTYPE_STREAM_MF)
					{
						if (basschannelType == BASSChannelType.BASS_CTYPE_MUSIC_MO3)
						{
							goto IL_4F1;
						}
						switch (basschannelType)
						{
						case BASSChannelType.BASS_CTYPE_STREAM_OGG:
							goto IL_239;
						case BASSChannelType.BASS_CTYPE_STREAM_MP1:
						case BASSChannelType.BASS_CTYPE_STREAM_MP2:
						case BASSChannelType.BASS_CTYPE_STREAM_MP3:
							break;
						case BASSChannelType.BASS_CTYPE_STREAM_AIFF:
							goto IL_47E;
						case BASSChannelType.BASS_CTYPE_STREAM_CA:
							goto IL_30A;
						case BASSChannelType.BASS_CTYPE_STREAM_MF:
							intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_MF);
							if (!(intPtr == IntPtr.Zero))
							{
								tagType = BASSTag.BASS_TAG_MF;
								return intPtr;
							}
							intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_MP4);
							if (!(intPtr == IntPtr.Zero))
							{
								tagType = BASSTag.BASS_TAG_MP4;
								return intPtr;
							}
							intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3V2);
							if (!(intPtr == IntPtr.Zero))
							{
								tagType = BASSTag.BASS_TAG_ID3V2;
								return intPtr;
							}
							intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_APE);
							if (intPtr != IntPtr.Zero)
							{
								tagType = BASSTag.BASS_TAG_APE;
								return intPtr;
							}
							tagType = BASSTag.BASS_TAG_MF;
							return intPtr;
						default:
							goto IL_5DF;
						}
					}
					else
					{
						if (basschannelType == BASSChannelType.BASS_CTYPE_STREAM_WMA)
						{
							intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_WMA);
							tagType = BASSTag.BASS_TAG_WMA;
							return intPtr;
						}
						if (basschannelType != BASSChannelType.BASS_CTYPE_STREAM_WMA_MP3)
						{
							if (basschannelType != BASSChannelType.BASS_CTYPE_STREAM_WINAMP)
							{
								goto IL_5DF;
							}
							intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3V2);
							if (!(intPtr == IntPtr.Zero))
							{
								tagType = BASSTag.BASS_TAG_ID3V2;
								return intPtr;
							}
							intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_APE);
							if (!(intPtr == IntPtr.Zero))
							{
								tagType = BASSTag.BASS_TAG_APE;
								return intPtr;
							}
							intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_OGG);
							if (!(intPtr == IntPtr.Zero))
							{
								tagType = BASSTag.BASS_TAG_OGG;
								return intPtr;
							}
							intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3);
							if (intPtr != IntPtr.Zero)
							{
								tagType = BASSTag.BASS_TAG_ID3;
								return intPtr;
							}
							tagType = BASSTag.BASS_TAG_ID3V2;
							return intPtr;
						}
					}
					intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3V2);
					if (!(intPtr == IntPtr.Zero))
					{
						tagType = BASSTag.BASS_TAG_ID3V2;
						return intPtr;
					}
					intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3);
					if (!(intPtr == IntPtr.Zero))
					{
						tagType = BASSTag.BASS_TAG_ID3;
						return intPtr;
					}
					intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_APE);
					if (!(intPtr == IntPtr.Zero))
					{
						tagType = BASSTag.BASS_TAG_APE;
						return intPtr;
					}
					intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_RIFF_BEXT);
					if (intPtr != IntPtr.Zero)
					{
						tagType = BASSTag.BASS_TAG_RIFF_BEXT;
						return intPtr;
					}
					tagType = BASSTag.BASS_TAG_ID3V2;
					return intPtr;
				}
				else if (basschannelType <= BASSChannelType.BASS_CTYPE_STREAM_OFR)
				{
					switch (basschannelType)
					{
					case BASSChannelType.BASS_CTYPE_STREAM_WV:
					case BASSChannelType.BASS_CTYPE_STREAM_WV_H:
					case BASSChannelType.BASS_CTYPE_STREAM_WV_L:
					case BASSChannelType.BASS_CTYPE_STREAM_WV_LH:
						goto IL_386;
					default:
						if (basschannelType != BASSChannelType.BASS_CTYPE_STREAM_OFR)
						{
							goto IL_5DF;
						}
						goto IL_386;
					}
				}
				else
				{
					if (basschannelType == BASSChannelType.BASS_CTYPE_STREAM_APE)
					{
						goto IL_386;
					}
					if (basschannelType != BASSChannelType.BASS_CTYPE_STREAM_FLAC && basschannelType != BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG)
					{
						goto IL_5DF;
					}
				}
			}
			else if (basschannelType <= BASSChannelType.BASS_CTYPE_STREAM_MIDI)
			{
				if (basschannelType <= BASSChannelType.BASS_CTYPE_STREAM_AAC)
				{
					if (basschannelType == BASSChannelType.BASS_CTYPE_STREAM_MPC)
					{
						goto IL_386;
					}
					if (basschannelType != BASSChannelType.BASS_CTYPE_STREAM_AAC)
					{
						goto IL_5DF;
					}
					goto IL_30A;
				}
				else
				{
					if (basschannelType == BASSChannelType.BASS_CTYPE_STREAM_MP4)
					{
						goto IL_30A;
					}
					if (basschannelType == BASSChannelType.BASS_CTYPE_STREAM_SPX)
					{
						goto IL_386;
					}
					if (basschannelType != BASSChannelType.BASS_CTYPE_STREAM_MIDI)
					{
						goto IL_5DF;
					}
					intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_MIDI_TRACK);
					if (!(intPtr == IntPtr.Zero))
					{
						tagType = BASSTag.BASS_TAG_MIDI_TRACK;
						return intPtr;
					}
					intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_RIFF_INFO);
					if (intPtr != IntPtr.Zero)
					{
						tagType = BASSTag.BASS_TAG_RIFF_INFO;
						return intPtr;
					}
					tagType = BASSTag.BASS_TAG_MIDI_TRACK;
					return intPtr;
				}
			}
			else if (basschannelType <= BASSChannelType.BASS_CTYPE_STREAM_OPUS)
			{
				if (basschannelType == BASSChannelType.BASS_CTYPE_STREAM_ALAC)
				{
					goto IL_30A;
				}
				if (basschannelType == BASSChannelType.BASS_CTYPE_STREAM_TTA)
				{
					goto IL_386;
				}
				if (basschannelType != BASSChannelType.BASS_CTYPE_STREAM_OPUS)
				{
					goto IL_5DF;
				}
			}
			else if (basschannelType != BASSChannelType.BASS_CTYPE_STREAM_DSD)
			{
				switch (basschannelType)
				{
				case BASSChannelType.BASS_CTYPE_MUSIC_MOD:
				case BASSChannelType.BASS_CTYPE_MUSIC_MTM:
				case BASSChannelType.BASS_CTYPE_MUSIC_S3M:
				case BASSChannelType.BASS_CTYPE_MUSIC_XM:
				case BASSChannelType.BASS_CTYPE_MUSIC_IT:
					goto IL_4F1;
				default:
					if (basschannelType != BASSChannelType.BASS_CTYPE_STREAM_WAV)
					{
						goto IL_5DF;
					}
					goto IL_47E;
				}
			}
			else
			{
				intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3V2);
				if (!(intPtr == IntPtr.Zero))
				{
					tagType = BASSTag.BASS_TAG_ID3V2;
					return intPtr;
				}
				intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_DSD_COMMENT);
				if (!(intPtr == IntPtr.Zero))
				{
					tagType = BASSTag.BASS_TAG_DSD_COMMENT;
					return intPtr;
				}
				intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_DSD_ARTIST);
				if (!(intPtr == IntPtr.Zero))
				{
					tagType = BASSTag.BASS_TAG_DSD_ARTIST;
					return intPtr;
				}
				intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_DSD_TITLE);
				if (intPtr != IntPtr.Zero)
				{
					tagType = BASSTag.BASS_TAG_DSD_TITLE;
					return intPtr;
				}
				return intPtr;
			}
			IL_239:
			intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_OGG);
			if (!(intPtr == IntPtr.Zero))
			{
				tagType = BASSTag.BASS_TAG_OGG;
				return intPtr;
			}
			intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_APE);
			if (intPtr != IntPtr.Zero)
			{
				tagType = BASSTag.BASS_TAG_APE;
				return intPtr;
			}
			tagType = BASSTag.BASS_TAG_OGG;
			return intPtr;
			IL_30A:
			intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_MP4);
			if (!(intPtr == IntPtr.Zero))
			{
				tagType = BASSTag.BASS_TAG_MP4;
				return intPtr;
			}
			intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3V2);
			if (!(intPtr == IntPtr.Zero))
			{
				tagType = BASSTag.BASS_TAG_ID3V2;
				return intPtr;
			}
			intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_APE);
			if (!(intPtr == IntPtr.Zero))
			{
				tagType = BASSTag.BASS_TAG_APE;
				return intPtr;
			}
			intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_OGG);
			if (intPtr != IntPtr.Zero)
			{
				tagType = BASSTag.BASS_TAG_OGG;
				return intPtr;
			}
			tagType = BASSTag.BASS_TAG_MP4;
			return intPtr;
			IL_386:
			intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_APE);
			if (!(intPtr == IntPtr.Zero))
			{
				tagType = BASSTag.BASS_TAG_APE;
				return intPtr;
			}
			intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_OGG);
			if (!(intPtr == IntPtr.Zero))
			{
				tagType = BASSTag.BASS_TAG_OGG;
				return intPtr;
			}
			intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3V2);
			if (!(intPtr == IntPtr.Zero))
			{
				tagType = BASSTag.BASS_TAG_ID3V2;
				return intPtr;
			}
			intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3);
			if (intPtr != IntPtr.Zero)
			{
				tagType = BASSTag.BASS_TAG_ID3;
				return intPtr;
			}
			tagType = BASSTag.BASS_TAG_APE;
			return intPtr;
			IL_47E:
			intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_RIFF_INFO);
			if (!(intPtr == IntPtr.Zero))
			{
				tagType = BASSTag.BASS_TAG_RIFF_INFO;
				return intPtr;
			}
			intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_RIFF_BEXT);
			if (!(intPtr == IntPtr.Zero))
			{
				tagType = BASSTag.BASS_TAG_RIFF_BEXT;
				return intPtr;
			}
			intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_ID3V2);
			if (intPtr != IntPtr.Zero)
			{
				tagType = BASSTag.BASS_TAG_ID3V2;
				return intPtr;
			}
			tagType = BASSTag.BASS_TAG_RIFF_INFO;
			return intPtr;
			IL_4F1:
			intPtr = Bass.BASS_ChannelGetTags(stream, BASSTag.BASS_TAG_MUSIC_NAME);
			tagType = BASSTag.BASS_TAG_MUSIC_NAME;
			return intPtr;
			IL_5DF:
			intPtr = IntPtr.Zero;
			return intPtr;
		}

		private static bool ReadRiffBEXT(IntPtr p, TAG_INFO tags)
		{
			if (p == IntPtr.Zero || tags == null)
			{
				return false;
			}
			bool result = true;
			try
			{
				BASS_TAG_BEXT bass_TAG_BEXT = (BASS_TAG_BEXT)Marshal.PtrToStructure(p, typeof(BASS_TAG_BEXT));
				if (!string.IsNullOrEmpty(bass_TAG_BEXT.Description) && string.IsNullOrEmpty(tags.title))
				{
					tags.title = bass_TAG_BEXT.Description;
				}
				if (BassTags.EvalNativeTAGsBEXT)
				{
					tags.AddNativeTag("BWFDescription", bass_TAG_BEXT.Description);
				}
				if (!string.IsNullOrEmpty(bass_TAG_BEXT.Originator) && string.IsNullOrEmpty(tags.artist))
				{
					tags.artist = bass_TAG_BEXT.Originator;
				}
				if (BassTags.EvalNativeTAGsBEXT)
				{
					tags.AddNativeTag("BWFOriginator", bass_TAG_BEXT.Originator);
				}
				if (!string.IsNullOrEmpty(bass_TAG_BEXT.OriginatorReference) && string.IsNullOrEmpty(tags.encodedby))
				{
					tags.encodedby = bass_TAG_BEXT.OriginatorReference;
				}
				if (BassTags.EvalNativeTAGsBEXT)
				{
					tags.AddNativeTag("BWFOriginatorReference", bass_TAG_BEXT.OriginatorReference);
				}
				string originationDate = bass_TAG_BEXT.OriginationDate;
				if (!string.IsNullOrEmpty(originationDate) && string.IsNullOrEmpty(tags.year) && originationDate != "0000-01-01" && originationDate != "0001-01-01")
				{
					tags.year = bass_TAG_BEXT.OriginationDate;
				}
				if (BassTags.EvalNativeTAGsBEXT)
				{
					tags.AddNativeTag("BWFOriginationDate", bass_TAG_BEXT.OriginationDate);
					tags.AddNativeTag("BWFOriginationTime", bass_TAG_BEXT.OriginationTime);
					tags.AddNativeTag("BWFTimeReference", bass_TAG_BEXT.TimeReference.ToString());
					tags.AddNativeTag("BWFVersion", bass_TAG_BEXT.Version.ToString());
					tags.AddNativeTag("BWFUMID", bass_TAG_BEXT.UMID);
				}
				string codingHistory = bass_TAG_BEXT.GetCodingHistory(p);
				if (!string.IsNullOrEmpty(codingHistory) && string.IsNullOrEmpty(tags.comment))
				{
					tags.comment = codingHistory;
				}
				if (BassTags.EvalNativeTAGsBEXT)
				{
					tags.AddNativeTag("BWFCodingHistory", codingHistory);
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		private static bool ReadRiffCART(IntPtr p, TAG_INFO tags)
		{
			if (p == IntPtr.Zero || tags == null)
			{
				return false;
			}
			bool result = true;
			try
			{
				BASS_TAG_CART bass_TAG_CART = (BASS_TAG_CART)Marshal.PtrToStructure(p, typeof(BASS_TAG_CART));
				if (BassTags.EvalNativeTAGsCART)
				{
					tags.AddNativeTag("CARTVersion", bass_TAG_CART.Version);
				}
				if (!string.IsNullOrEmpty(bass_TAG_CART.Title) && string.IsNullOrEmpty(tags.title))
				{
					tags.title = bass_TAG_CART.Title;
				}
				if (BassTags.EvalNativeTAGsCART)
				{
					tags.AddNativeTag("CARTTitle", bass_TAG_CART.Title);
				}
				if (!string.IsNullOrEmpty(bass_TAG_CART.Artist) && string.IsNullOrEmpty(tags.artist))
				{
					tags.artist = bass_TAG_CART.Artist;
				}
				if (BassTags.EvalNativeTAGsCART)
				{
					tags.AddNativeTag("CARTArtist", bass_TAG_CART.Artist);
				}
				if (!string.IsNullOrEmpty(bass_TAG_CART.CutID) && string.IsNullOrEmpty(tags.album))
				{
					tags.album = bass_TAG_CART.CutID;
				}
				if (BassTags.EvalNativeTAGsCART)
				{
					tags.AddNativeTag("CARTCutID", bass_TAG_CART.CutID);
				}
				if (!string.IsNullOrEmpty(bass_TAG_CART.ClientID) && string.IsNullOrEmpty(tags.copyright))
				{
					tags.copyright = bass_TAG_CART.ClientID;
				}
				if (BassTags.EvalNativeTAGsCART)
				{
					tags.AddNativeTag("CARTClientID", bass_TAG_CART.ClientID);
				}
				if (!string.IsNullOrEmpty(bass_TAG_CART.Category) && string.IsNullOrEmpty(tags.genre))
				{
					tags.genre = bass_TAG_CART.Category;
				}
				if (BassTags.EvalNativeTAGsCART)
				{
					tags.AddNativeTag("CARTCategory", bass_TAG_CART.Category);
				}
				if (!string.IsNullOrEmpty(bass_TAG_CART.Classification) && string.IsNullOrEmpty(tags.grouping))
				{
					tags.grouping = bass_TAG_CART.Classification;
				}
				if (BassTags.EvalNativeTAGsCART)
				{
					tags.AddNativeTag("CARTClassification", bass_TAG_CART.Classification);
				}
				if (!string.IsNullOrEmpty(bass_TAG_CART.ProducerAppID) && string.IsNullOrEmpty(tags.encodedby))
				{
					tags.encodedby = bass_TAG_CART.ProducerAppID;
				}
				if (BassTags.EvalNativeTAGsCART)
				{
					tags.AddNativeTag("CARTProducerAppID", bass_TAG_CART.ProducerAppID);
				}
				string tagText = bass_TAG_CART.GetTagText(p);
				if (!string.IsNullOrEmpty(tagText) && string.IsNullOrEmpty(tags.comment))
				{
					tags.comment = tagText;
				}
				if (BassTags.EvalNativeTAGsCART)
				{
					tags.AddNativeTag("CARTTagText", tagText);
				}
				if (BassTags.EvalNativeTAGsCART)
				{
					tags.AddNativeTag("CARTOutCue", bass_TAG_CART.OutCue);
					tags.AddNativeTag("CARTStartDate", bass_TAG_CART.StartDate);
					tags.AddNativeTag("CARTStartTime", bass_TAG_CART.StartTime);
					tags.AddNativeTag("CARTEndDate", bass_TAG_CART.EndDate);
					tags.AddNativeTag("CARTEndTime", bass_TAG_CART.EndTime);
					tags.AddNativeTag("CARTProducerAppVersion", bass_TAG_CART.ProducerAppVersion);
					tags.AddNativeTag("CARTUserDef", bass_TAG_CART.UserDef);
					tags.AddNativeTag("CARTLevelReference", bass_TAG_CART.LevelReference.ToString(CultureInfo.InvariantCulture));
					tags.AddNativeTag("CARTTimer1Usage", bass_TAG_CART.Timer1Usage);
					tags.AddNativeTag("CARTTimer1Value", ((uint)bass_TAG_CART.Timer1Value).ToString(CultureInfo.InvariantCulture));
					tags.AddNativeTag("CARTTimer2Usage", bass_TAG_CART.Timer2Usage);
					tags.AddNativeTag("CARTTimer2Value", ((uint)bass_TAG_CART.Timer2Value).ToString(CultureInfo.InvariantCulture));
					tags.AddNativeTag("CARTTimer3Usage", bass_TAG_CART.Timer3Usage);
					tags.AddNativeTag("CARTTimer3Value", ((uint)bass_TAG_CART.Timer3Value).ToString(CultureInfo.InvariantCulture));
					tags.AddNativeTag("CARTTimer4Usage", bass_TAG_CART.Timer4Usage);
					tags.AddNativeTag("CARTTimer4Value", ((uint)bass_TAG_CART.Timer4Value).ToString(CultureInfo.InvariantCulture));
					tags.AddNativeTag("CARTTimer5Usage", bass_TAG_CART.Timer5Usage);
					tags.AddNativeTag("CARTTimer5Value", ((uint)bass_TAG_CART.Timer5Value).ToString(CultureInfo.InvariantCulture));
					tags.AddNativeTag("CARTTimer6Usage", bass_TAG_CART.Timer6Usage);
					tags.AddNativeTag("CARTTimer6Value", ((uint)bass_TAG_CART.Timer6Value).ToString(CultureInfo.InvariantCulture));
					tags.AddNativeTag("CARTTimer7Usage", bass_TAG_CART.Timer7Usage);
					tags.AddNativeTag("CARTTimer7Value", ((uint)bass_TAG_CART.Timer7Value).ToString(CultureInfo.InvariantCulture));
					tags.AddNativeTag("CARTTimer8Usage", bass_TAG_CART.Timer8Usage);
					tags.AddNativeTag("CARTTimer8Value", ((uint)bass_TAG_CART.Timer8Value).ToString(CultureInfo.InvariantCulture));
					tags.AddNativeTag("CARTURL", bass_TAG_CART.URL);
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public static bool ReadID3v1(IntPtr p, TAG_INFO tags)
		{
			if (p == IntPtr.Zero || tags == null)
			{
				return false;
			}
			bool result = true;
			try
			{
				BASS_TAG_ID3 bass_TAG_ID = (BASS_TAG_ID3)Marshal.PtrToStructure(p, typeof(BASS_TAG_ID3));
				if (bass_TAG_ID.ID.Equals("TAG"))
				{
					tags.title = bass_TAG_ID.Title;
					if (BassTags.EvalNativeTAGs)
					{
						tags.AddNativeTag("Title", bass_TAG_ID.Title);
					}
					tags.artist = bass_TAG_ID.Artist;
					if (BassTags.EvalNativeTAGs)
					{
						tags.AddNativeTag("Artist", bass_TAG_ID.Artist);
					}
					tags.album = bass_TAG_ID.Album;
					if (BassTags.EvalNativeTAGs)
					{
						tags.AddNativeTag("Album", bass_TAG_ID.Album);
					}
					tags.year = bass_TAG_ID.Year;
					if (BassTags.EvalNativeTAGs)
					{
						tags.AddNativeTag("Year", bass_TAG_ID.Year);
					}
					tags.comment = bass_TAG_ID.Comment;
					if (BassTags.EvalNativeTAGs)
					{
						tags.AddNativeTag("Comment", bass_TAG_ID.Comment);
					}
					if (bass_TAG_ID.Dummy == 0)
					{
						tags.track = bass_TAG_ID.Track.ToString();
						if (BassTags.EvalNativeTAGs)
						{
							tags.AddNativeTag("Track", bass_TAG_ID.Track.ToString());
						}
					}
					if (BassTags.EvalNativeTAGs)
					{
						tags.AddNativeTag("Genre", bass_TAG_ID.Genre);
					}
					try
					{
						tags.genre = BassTags.ID3v1Genre[(int)bass_TAG_ID.Genre];
					}
					catch
					{
						tags.genre = "Unknown";
					}
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public static bool ReadID3v2(IntPtr p, TAG_INFO tags)
		{
			if (p == IntPtr.Zero || tags == null)
			{
				return false;
			}
			try
			{
				tags.ResetTags();
				int num = 0;
				int num2 = 0;
				ID3v2Reader id3v2Reader = new ID3v2Reader(p);
				while (id3v2Reader.Read())
				{
					string key = id3v2Reader.GetKey();
					short flags = id3v2Reader.GetFlags();
					object value = id3v2Reader.GetValue();
					if (key.Length > 0 && value is string)
					{
						tags.EvalTagEntry(string.Format("{0}={1}", key, value));
					}
					else if ((key == "POPM" || key == "POP") && value is byte)
					{
						if (num2 == 0)
						{
							tags.EvalTagEntry(string.Format("POPM={0}", value));
						}
						num2++;
					}
					else if (BassTags.ReadPictureTAGs && (key == "APIC" || key == "PIC") && value is byte[])
					{
						num++;
						tags.AddPicture(id3v2Reader.GetPicture(value as byte[], flags, tags.PictureCount, key == "PIC"));
					}
				}
				id3v2Reader.Close();
				if (BassTags.ReadPictureTAGs && BassTags.EvalNativeTAGs)
				{
					tags.AddNativeTag("APIC", num);
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		private static void ReadAPEPictures(int stream, TAG_INFO tags)
		{
			TagPicture[] array = Bass.BASS_ChannelGetTagsAPEPictures(stream);
			if (array != null && array.Length != 0)
			{
				foreach (TagPicture tagPicture in array)
				{
					tags.AddPicture(tagPicture);
				}
			}
		}

		public static bool ReadPictureTAGs = true;

		public static bool EvalNativeTAGs = true;

		public static bool EvalNativeTAGsBEXT = true;

		public static bool EvalNativeTAGsCART = true;

		public static readonly string[] ID3v1Genre = new string[]
		{
			"Blues",
			"Classic Rock",
			"Country",
			"Dance",
			"Disco",
			"Funk",
			"Grunge",
			"Hip-Hop",
			"Jazz",
			"Metal",
			"New Age",
			"Oldies",
			"Other",
			"Pop",
			"R&B",
			"Rap",
			"Reggae",
			"Rock",
			"Techno",
			"Industrial",
			"Alternative",
			"Ska",
			"Death Metal",
			"Pranks",
			"Soundtrack",
			"Euro-Techno",
			"Ambient",
			"Trip-Hop",
			"Vocal",
			"Jazz+Funk",
			"Fusion",
			"Trance",
			"Classical",
			"Instrumental",
			"Acid",
			"House",
			"Game",
			"Sound Clip",
			"Gospel",
			"Noise",
			"Alternative Rock",
			"Bass",
			"Soul",
			"Punk",
			"Space",
			"Meditative",
			"Instrumental Pop",
			"Instrumental Rock",
			"Ethnic",
			"Gothic",
			"Darkwave",
			"Techno-Industrial",
			"Electronic",
			"Pop-Folk",
			"Eurodance",
			"Dream",
			"Southern Rock",
			"Comedy",
			"Cult",
			"Gangsta",
			"Top 40",
			"Christian Rap",
			"Pop/Funk",
			"Jungle",
			"Native American",
			"Cabaret",
			"New Wave",
			"Psychedelic",
			"Rave",
			"Showtunes",
			"Trailer",
			"Lo-Fi",
			"Tribal",
			"Acid Punk",
			"Acid Jazz",
			"Polka",
			"Retro",
			"Musical",
			"Rock & Roll",
			"Hard Rock",
			"Folk",
			"Folk/Rock",
			"National Folk",
			"Swing",
			"Fusion",
			"Bebob",
			"Latin",
			"Revival",
			"Celtic",
			"Bluegrass",
			"Avantgarde",
			"Gothic Rock",
			"Progressive Rock",
			"Psychedelic Rock",
			"Symphonic Rock",
			"Slow Rock",
			"Big Band",
			"Chorus",
			"Easy Listening",
			"Acoustic",
			"Humour",
			"Speech",
			"Chanson",
			"Opera",
			"Chamber Music",
			"Sonata",
			"Symphony",
			"Booty Bass",
			"Primus",
			"Porn Groove",
			"Satire",
			"Slow Jam",
			"Club",
			"Tango",
			"Samba",
			"Folklore",
			"Ballad",
			"Power Ballad",
			"Rhythmic Soul",
			"Freestyle",
			"Duet",
			"Punk Rock",
			"Drum Solo",
			"A Cappella",
			"Euro-House",
			"Dance Hall",
			"Goa",
			"Drum & Bass",
			"Club-House",
			"Hardcore",
			"Terror",
			"Indie",
			"BritPop",
			"Negerpunk",
			"Polsk Punk",
			"Beat",
			"Christian Gangsta Rap",
			"Heavy Metal",
			"Black Metal",
			"Crossover",
			"Contemporary Christian",
			"Christian Rock",
			"Merengue",
			"Salsa",
			"Thrash Metal",
			"Anime",
			"Jpop",
			"Synthpop"
		};
	}
}
