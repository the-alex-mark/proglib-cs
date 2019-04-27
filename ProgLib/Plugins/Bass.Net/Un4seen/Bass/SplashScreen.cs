using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Un4seen.Bass
{
	internal partial class SplashScreen : Form
	{
		public SplashScreen(bool close, int wait)
		{
			this._close = close;
			if (!close && wait > 0)
			{
				this._waitTime = wait;
			}
			this.InitializeComponent();
			if (close)
			{
				this.toolTip.SetToolTip(this.pictureBoxLogo, null);
				this.pictureBoxLogo.Cursor = Cursors.Default;
			}
			else
			{
				this.toolTip.SetToolTip(this.pictureBoxLogo, "Close");
				this.pictureBoxLogo.Cursor = Cursors.Hand;
			}
			Label label = this.labelVersion;
			label.Text += Assembly.GetExecutingAssembly().GetName().Version.ToString();
			if (string.IsNullOrEmpty(BassNet._eMail))
			{
				this.labelMessage.Text = "Unregistered Freeware Version";
			}
			else
			{
				this.labelMessage.Text = "Version registered for: " + BassNet._eMail;
			}
			if (close)
			{
				Console.WriteLine("**********************************************************");
				Console.WriteLine("* " + this.labelVersion.Text + " *");
				Console.WriteLine("*       Freeware version - For personal use only!        *");
				Console.WriteLine("* " + this.labelCopyright.Text + " *");
				Console.WriteLine("**********************************************************");
			}
		}

		internal void SetClose(bool close)
		{
			this._close = close;
			if (close)
			{
				this.toolTip.SetToolTip(this.pictureBoxLogo, null);
				this.pictureBoxLogo.Cursor = Cursors.Default;
				return;
			}
			this.toolTip.SetToolTip(this.pictureBoxLogo, "Close");
			this.pictureBoxLogo.Cursor = Cursors.Hand;
		}

		internal void SetOpacity(double opacity)
		{
			base.Opacity = opacity;
		}

		internal void SetPosition(int pos)
		{
			if (pos == 1)
			{
				base.StartPosition = FormStartPosition.WindowsDefaultLocation;
				return;
			}
			if (pos != 2)
			{
				base.StartPosition = FormStartPosition.CenterScreen;
				return;
			}
			base.StartPosition = FormStartPosition.CenterParent;
		}

		private void pictureBoxLogo_Click(object sender, EventArgs e)
		{
			if (!this._close)
			{
				base.Close();
			}
		}

		private void SplashScreen_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape && !this._close)
			{
				base.Close();
			}
		}

		private void labelVersion_Click(object sender, EventArgs e)
		{
			try
			{
				new Process
				{
					StartInfo = 
					{
						FileName = "http://www.bass.radio42.com/"
					}
				}.Start();
			}
			catch
			{
			}
		}

		private void pictureBoxLogo_MouseDown(object sender, MouseEventArgs e)
		{
			this._moveX = e.X;
			this._moveY = e.Y;
		}

		private void pictureBoxLogo_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				base.Location = new Point(base.Location.X - (this._moveX - e.X), base.Location.Y - (this._moveY - e.Y));
			}
		}

		private void pictureBoxLogo_MouseUp(object sender, MouseEventArgs e)
		{
			this._moveX = 0;
			this._moveY = 0;
		}

		private bool _close = true;

		internal int _waitTime = 4000;

		private int _moveX;

		private int _moveY;
	}
}
