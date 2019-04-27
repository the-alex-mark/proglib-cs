using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Un4seen.Bass.AddOn.Tags
{
	[Serializable]
	public class TagPicture
	{
		public TagPicture(int attribIndex, string mimeType, TagPicture.PICTURE_TYPE pictureType, string description, byte[] data)
		{
			this.AttributeIndex = attribIndex;
			this.MIMEType = mimeType;
			this.PictureType = pictureType;
			this.Description = description;
			this.Data = data;
			this.PictureStorage = TagPicture.PICTURE_STORAGE.Internal;
		}

		public TagPicture(Image image, TagPicture.PICTURE_TYPE pictureType, string description)
		{
			this.PictureType = pictureType;
			this.Description = description;
			ImageConverter imageConverter = new ImageConverter();
			this.Data = (byte[])imageConverter.ConvertTo(image, typeof(byte[]));
			this.MIMEType = TagPicture.GetMimeTypeFromImage(image);
			this.PictureStorage = TagPicture.PICTURE_STORAGE.Internal;
		}

		public TagPicture(string file)
		{
			UTF8Encoding utf8Encoding = new UTF8Encoding();
			this.Data = utf8Encoding.GetBytes(file);
			this.MIMEType = TagPicture.GetMimeTypeFromFile(file);
			this.PictureType = TagPicture.PICTURE_TYPE.Unknown;
			this.Description = file;
			this.PictureStorage = TagPicture.PICTURE_STORAGE.External;
		}

		public TagPicture(string file, TagPicture.PICTURE_TYPE type, string description)
		{
			UTF8Encoding utf8Encoding = new UTF8Encoding();
			this.Data = utf8Encoding.GetBytes(file);
			this.MIMEType = TagPicture.GetMimeTypeFromFile(file);
			this.PictureType = type;
			this.Description = description;
			this.PictureStorage = TagPicture.PICTURE_STORAGE.External;
		}

		internal TagPicture(Tag pTag)
		{
			this.AttributeIndex = pTag.Index;
			this.MIMEType = "Unknown";
			this.PictureType = TagPicture.PICTURE_TYPE.Unknown;
			this.Description = "";
			MemoryStream memoryStream = null;
			BinaryReader binaryReader = null;
			if (pTag.Name == "WM/Picture")
			{
				try
				{
					memoryStream = new MemoryStream((byte[])pTag);
					binaryReader = new BinaryReader(memoryStream);
					if (Utils.Is64Bit)
					{
						this.MIMEType = Marshal.PtrToStringUni(new IntPtr(binaryReader.ReadInt64()));
					}
					else
					{
						this.MIMEType = Marshal.PtrToStringUni(new IntPtr(binaryReader.ReadInt32()));
					}
					byte pictureType = binaryReader.ReadByte();
					try
					{
						this.PictureType = (TagPicture.PICTURE_TYPE)pictureType;
					}
					catch
					{
						this.PictureType = TagPicture.PICTURE_TYPE.Unknown;
					}
					if (Utils.Is64Bit)
					{
						this.Description = Marshal.PtrToStringUni(new IntPtr(binaryReader.ReadInt64()));
					}
					else
					{
						this.Description = Marshal.PtrToStringUni(new IntPtr(binaryReader.ReadInt32()));
					}
					int num = binaryReader.ReadInt32();
					this.Data = new byte[num];
					if (Utils.Is64Bit)
					{
						Marshal.Copy(new IntPtr(binaryReader.ReadInt64()), this.Data, 0, num);
					}
					else
					{
						Marshal.Copy(new IntPtr(binaryReader.ReadInt32()), this.Data, 0, num);
					}
				}
				catch
				{
				}
				finally
				{
					try
					{
						if (binaryReader != null)
						{
							binaryReader.Close();
						}
						if (memoryStream != null)
						{
							memoryStream.Close();
							memoryStream.Dispose();
						}
						if (binaryReader != null)
						{
							binaryReader.Dispose();
						}
					}
					catch
					{
					}
				}
			}
		}

		internal TagPicture(byte[] pData, int pType)
		{
			this.PictureStorage = TagPicture.PICTURE_STORAGE.Internal;
			if (pType == 1)
			{
				this.AttributeIndex = -1;
				this.MIMEType = "Unknown";
				this.PictureType = TagPicture.PICTURE_TYPE.Unknown;
				this.Description = "";
				MemoryStream memoryStream = null;
				BinaryReader binaryReader = null;
				if (pData == null || pData.Length <= 32)
				{
					return;
				}
				try
				{
					memoryStream = new MemoryStream(pData);
					binaryReader = new BinaryReader(memoryStream);
					this.PictureType = (TagPicture.PICTURE_TYPE)TagPicture.ReadInt32(binaryReader);
					int count = TagPicture.ReadInt32(binaryReader);
					this.MIMEType = Encoding.UTF8.GetString(binaryReader.ReadBytes(count));
					int count2 = TagPicture.ReadInt32(binaryReader);
					this.Description = Encoding.UTF8.GetString(binaryReader.ReadBytes(count2));
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					int count3 = TagPicture.ReadInt32(binaryReader);
					this.Data = binaryReader.ReadBytes(count3);
					return;
				}
				catch
				{
					return;
				}
				finally
				{
					try
					{
						if (binaryReader != null)
						{
							binaryReader.Close();
						}
						if (memoryStream != null)
						{
							memoryStream.Close();
							memoryStream.Dispose();
						}
						if (binaryReader != null)
						{
							binaryReader.Dispose();
						}
					}
					catch
					{
					}
				}
			}
			if (pType == 2)
			{
				this.AttributeIndex = -1;
				this.MIMEType = "Unknown";
				this.PictureType = TagPicture.PICTURE_TYPE.Unknown;
				this.Description = "";
				if (pData == null || pData.Length == 0)
				{
					return;
				}
				try
				{
					int num = 0;
					while (pData[num] != 0)
					{
						num++;
					}
					this.Description = Encoding.UTF8.GetString(pData, 0, num);
					num++;
					this.Data = new byte[pData.Length - num];
					Array.Copy(pData, num, this.Data, 0, this.Data.Length);
					if (this.PictureImage != null)
					{
						this.MIMEType = TagPicture.GetMimeTypeFromImage(this.PictureImage);
					}
					return;
				}
				catch
				{
					return;
				}
			}
			this.AttributeIndex = -1;
			this.MIMEType = "Unknown";
			this.PictureType = TagPicture.PICTURE_TYPE.FrontAlbumCover;
			this.Description = "CoverArt";
			this.Data = pData;
			if (this.PictureImage != null)
			{
				this.MIMEType = TagPicture.GetMimeTypeFromImage(this.PictureImage);
			}
		}

		public TagPicture(TagPicture pic, int size)
		{
			Image pictureImage = pic.PictureImage;
			int num = size;
			int num2 = size;
			if (pictureImage.Width > pictureImage.Height)
			{
				num2 = (int)((double)num2 / ((double)pictureImage.Width / (double)pictureImage.Height));
			}
			else
			{
				num = (int)((double)num / ((double)pictureImage.Height / (double)pictureImage.Width));
			}
			try
			{
				using (Bitmap bitmap = new Bitmap(num, num2))
				{
					using (Graphics graphics = Graphics.FromImage(bitmap))
					{
						graphics.DrawImage(pictureImage, 0, 0, num, num2);
						using (MemoryStream memoryStream = new MemoryStream())
						{
							bitmap.Save(memoryStream, ImageFormat.Jpeg);
							this.Data = memoryStream.ToArray();
							memoryStream.Close();
						}
					}
				}
			}
			catch
			{
			}
			this.PictureType = pic.PictureType;
			this.Description = pic.Description;
			this.MIMEType = "image/jpeg";
			this.PictureStorage = TagPicture.PICTURE_STORAGE.Internal;
		}

		private static int ReadInt32(BinaryReader stream)
		{
			byte[] array = new byte[4];
			stream.Read(array, 0, 4);
			return (int)array[0] << 24 | (int)array[1] << 16 | (int)array[2] << 8 | (int)array[3];
		}

		public override string ToString()
		{
			return string.Format("{0} [{1}, {2}]", this.Description, this.PictureType, this.MIMEType);
		}

		public static string GetMimeTypeFromImage(Image pImage)
		{
			string result = "Unknown";
			if (pImage == null)
			{
				return result;
			}
			ImageFormat rawFormat = pImage.RawFormat;
			if (rawFormat.Guid == ImageFormat.Jpeg.Guid)
			{
				result = "image/jpeg";
			}
			else if (rawFormat.Guid == ImageFormat.Gif.Guid)
			{
				result = "image/gif";
			}
			else if (rawFormat.Guid == ImageFormat.MemoryBmp.Guid)
			{
				result = "image/bmp";
			}
			else if (rawFormat.Guid == ImageFormat.Bmp.Guid)
			{
				result = "image/bmp";
			}
			else if (rawFormat.Guid == ImageFormat.Png.Guid)
			{
				result = "image/png";
			}
			else if (rawFormat.Guid == ImageFormat.Icon.Guid)
			{
				result = "image/x-icon";
			}
			else if (rawFormat.Guid == ImageFormat.Tiff.Guid)
			{
				result = "image/tiff";
			}
			else if (rawFormat.Guid == ImageFormat.Emf.Guid)
			{
				result = "image/x-emf";
			}
			else if (rawFormat.Guid == ImageFormat.Wmf.Guid)
			{
				result = "image/x-wmf";
			}
			else
			{
				result = "image/jpeg";
			}
			return result;
		}

		public static string GetMimeTypeFromFile(string pFile)
		{
			string result = "image/jpeg";
			string text = Path.GetExtension(pFile).ToLower();
			uint num = PrivateImplementationDetails.ComputeStringHash(text);
			if (num <= 1388056268u)
			{
				if (num <= 850093434u)
				{
					if (num != 175576948u)
					{
						if (num != 850093434u)
						{
							return result;
						}
						if (!(text == ".ico"))
						{
							return result;
						}
						return "image/x-icon";
					}
					else
					{
						if (!(text == ".bmp"))
						{
							return result;
						}
						return "image/bmp";
					}
				}
				else if (num != 1128223456u)
				{
					if (num != 1384894805u)
					{
						if (num != 1388056268u)
						{
							return result;
						}
						if (!(text == ".jpg"))
						{
							return result;
						}
					}
					else
					{
						if (!(text == ".gif"))
						{
							return result;
						}
						return "image/gif";
					}
				}
				else
				{
					if (!(text == ".png"))
					{
						return result;
					}
					return "image/png";
				}
			}
			else
			{
				if (num > 1928100785u)
				{
					if (num != 3560597182u)
					{
						if (num != 4100894060u)
						{
							if (num != 4178554255u)
							{
								return result;
							}
							if (!(text == ".jpeg"))
							{
								return result;
							}
							goto IL_152;
						}
						else if (!(text == ".tif"))
						{
							return result;
						}
					}
					else if (!(text == ".tiff"))
					{
						return result;
					}
					return "image/tiff";
				}
				if (num != 1464784447u)
				{
					if (num != 1928100785u)
					{
						return result;
					}
					if (!(text == ".wmf"))
					{
						return result;
					}
					return "image/x-wmf";
				}
				else
				{
					if (!(text == ".emf"))
					{
						return result;
					}
					return "image/x-emf";
				}
			}
			IL_152:
			result = "image/jpeg";
			return result;
		}

		public static Image LoadImageFromFile(string filename)
		{
			Image result = null;
			try
			{
				using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
				{
					byte[] array = new byte[stream.Length];
					stream.Read(array, 0, (int)stream.Length);
					result = (new ImageConverter().ConvertFrom(array) as Image);
					stream.Close();
				}
			}
			catch
			{
			}
			return result;
		}

		public static bool SaveImageToFile(Image img, string filename, ImageFormat format)
		{
			bool result = false;
			bool flag = false;
			try
			{
				byte[] array = null;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (Image image = (Image)img.Clone())
					{
						image.Save(memoryStream, format);
					}
					array = memoryStream.ToArray();
					memoryStream.Close();
				}
				using (FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
				{
					flag = true;
					try
					{
						fileStream.Write(array, 0, array.Length);
						result = true;
					}
					finally
					{
						fileStream.Close();
					}
				}
			}
			catch
			{
				if (flag && File.Exists(filename))
				{
					File.Delete(filename);
				}
				throw;
			}
			return result;
		}

		public Image PictureImage
		{
			get
			{
				if (this.PictureStorage == TagPicture.PICTURE_STORAGE.Internal)
				{
					try
					{
						return new ImageConverter().ConvertFrom(this.Data) as Image;
					}
					catch
					{
						return null;
					}
				}
				Image result;
				try
				{
					string @string = Encoding.UTF8.GetString(this.Data);
					if (!string.IsNullOrEmpty(@string))
					{
						result = TagPicture.LoadImageFromFile(@string);
					}
					else
					{
						result = null;
					}
				}
				catch
				{
					result = null;
				}
				return result;
			}
		}

		private static bool _ThumbnailCallback()
		{
			return true;
		}

		public string MIMEType;

		public TagPicture.PICTURE_TYPE PictureType;

		public string Description;

		public byte[] Data;

		public int AttributeIndex = -1;

		public TagPicture.PICTURE_STORAGE PictureStorage;

		public enum PICTURE_TYPE : byte
		{
			Unknown,
			Icon32,
			OtherIcon,
			FrontAlbumCover,
			BackAlbumCover,
			LeafletPage,
			Media,
			LeadArtist,
			Artists,
			Conductor,
			Orchestra,
			Composer,
			Writer,
			Location,
			RecordingSession,
			Performance,
			VideoCapture,
			ColoredFish,
			Illustration,
			BandLogo,
			PublisherLogo
		}

		public enum PICTURE_STORAGE : byte
		{
			Internal,
			External
		}
	}
}
