namespace Un4seen.Bass
{
	internal partial class SplashScreen : global::System.Windows.Forms.Form
	{
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::Un4seen.Bass.SplashScreen));
			this.pictureBoxLogo = new global::System.Windows.Forms.PictureBox();
			this.labelCopyrightNote = new global::System.Windows.Forms.Label();
			this.labelCopyright = new global::System.Windows.Forms.Label();
			this.panelAll = new global::System.Windows.Forms.Panel();
			this.labelVersion = new global::System.Windows.Forms.Label();
			this.labelMessage = new global::System.Windows.Forms.Label();
			this.toolTip = new global::System.Windows.Forms.ToolTip(this.components);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxLogo).BeginInit();
			this.panelAll.SuspendLayout();
			base.SuspendLayout();
			this.pictureBoxLogo.Cursor = global::System.Windows.Forms.Cursors.Hand;
			this.pictureBoxLogo.Dock = global::System.Windows.Forms.DockStyle.Top;
			this.pictureBoxLogo.ErrorImage = null;
			this.pictureBoxLogo.Image = (global::System.Drawing.Image)componentResourceManager.GetObject("pictureBoxLogo.Image");
			this.pictureBoxLogo.InitialImage = null;
			this.pictureBoxLogo.Location = new global::System.Drawing.Point(0, 0);
			this.pictureBoxLogo.Name = "pictureBoxLogo";
			this.pictureBoxLogo.Size = new global::System.Drawing.Size(288, 115);
			this.pictureBoxLogo.SizeMode = global::System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBoxLogo.TabIndex = 0;
			this.pictureBoxLogo.TabStop = false;
			this.pictureBoxLogo.Click += new global::System.EventHandler(this.pictureBoxLogo_Click);
			this.pictureBoxLogo.MouseDown += new global::System.Windows.Forms.MouseEventHandler(this.pictureBoxLogo_MouseDown);
			this.pictureBoxLogo.MouseMove += new global::System.Windows.Forms.MouseEventHandler(this.pictureBoxLogo_MouseMove);
			this.pictureBoxLogo.MouseUp += new global::System.Windows.Forms.MouseEventHandler(this.pictureBoxLogo_MouseUp);
			this.labelCopyrightNote.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.labelCopyrightNote.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this.labelCopyrightNote.Font = new global::System.Drawing.Font("Tahoma", 6.75f);
			this.labelCopyrightNote.ImageAlign = global::System.Drawing.ContentAlignment.MiddleRight;
			this.labelCopyrightNote.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.labelCopyrightNote.Location = new global::System.Drawing.Point(4, 132);
			this.labelCopyrightNote.Name = "labelCopyrightNote";
			this.labelCopyrightNote.Size = new global::System.Drawing.Size(280, 12);
			this.labelCopyrightNote.TabIndex = 11;
			this.labelCopyrightNote.Text = "This software is protected by international law. All rights reserved.";
			this.labelCopyrightNote.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.labelCopyrightNote.MouseDown += new global::System.Windows.Forms.MouseEventHandler(this.pictureBoxLogo_MouseDown);
			this.labelCopyrightNote.MouseMove += new global::System.Windows.Forms.MouseEventHandler(this.pictureBoxLogo_MouseMove);
			this.labelCopyrightNote.MouseUp += new global::System.Windows.Forms.MouseEventHandler(this.pictureBoxLogo_MouseUp);
			this.labelCopyright.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.labelCopyright.Cursor = global::System.Windows.Forms.Cursors.Default;
			this.labelCopyright.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this.labelCopyright.Font = new global::System.Drawing.Font("Tahoma", 6.75f);
			this.labelCopyright.ImageAlign = global::System.Drawing.ContentAlignment.MiddleRight;
			this.labelCopyright.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.labelCopyright.Location = new global::System.Drawing.Point(4, 118);
			this.labelCopyright.Name = "labelCopyright";
			this.labelCopyright.Size = new global::System.Drawing.Size(280, 12);
			this.labelCopyright.TabIndex = 10;
			this.labelCopyright.Text = "© 2016 www.bass.radio42.com  :  BASS by www.un4seen.com";
			this.labelCopyright.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip.SetToolTip(this.labelCopyright, "radio42 : www.bass.radio42.com");
			this.labelCopyright.MouseDown += new global::System.Windows.Forms.MouseEventHandler(this.pictureBoxLogo_MouseDown);
			this.labelCopyright.MouseMove += new global::System.Windows.Forms.MouseEventHandler(this.pictureBoxLogo_MouseMove);
			this.labelCopyright.MouseUp += new global::System.Windows.Forms.MouseEventHandler(this.pictureBoxLogo_MouseUp);
			this.panelAll.BackColor = global::System.Drawing.Color.Transparent;
			this.panelAll.BorderStyle = global::System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelAll.Controls.Add(this.labelVersion);
			this.panelAll.Controls.Add(this.labelMessage);
			this.panelAll.Controls.Add(this.labelCopyrightNote);
			this.panelAll.Controls.Add(this.labelCopyright);
			this.panelAll.Controls.Add(this.pictureBoxLogo);
			this.panelAll.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.panelAll.Location = new global::System.Drawing.Point(0, 0);
			this.panelAll.Name = "panelAll";
			this.panelAll.Size = new global::System.Drawing.Size(290, 190);
			this.panelAll.TabIndex = 12;
			this.labelVersion.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.labelVersion.Cursor = global::System.Windows.Forms.Cursors.Hand;
			this.labelVersion.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this.labelVersion.Font = new global::System.Drawing.Font("Tahoma", 6.75f);
			this.labelVersion.ImageAlign = global::System.Drawing.ContentAlignment.MiddleRight;
			this.labelVersion.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.labelVersion.Location = new global::System.Drawing.Point(4, 172);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new global::System.Drawing.Size(280, 12);
			this.labelVersion.TabIndex = 13;
			this.labelVersion.Text = "BASS.NET API  by Bernd Niedergesaess - Version ";
			this.labelVersion.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip.SetToolTip(this.labelVersion, "radio42 : www.bass.radio42.com");
			this.labelVersion.Click += new global::System.EventHandler(this.labelVersion_Click);
			this.labelMessage.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.labelMessage.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this.labelMessage.Font = new global::System.Drawing.Font("Tahoma", 8.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.labelMessage.ImeMode = global::System.Windows.Forms.ImeMode.NoControl;
			this.labelMessage.Location = new global::System.Drawing.Point(4, 150);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new global::System.Drawing.Size(280, 16);
			this.labelMessage.TabIndex = 12;
			this.labelMessage.Text = "Freeware version - For personal use only!";
			this.labelMessage.TextAlign = global::System.Drawing.ContentAlignment.MiddleCenter;
			this.labelMessage.MouseDown += new global::System.Windows.Forms.MouseEventHandler(this.pictureBoxLogo_MouseDown);
			this.labelMessage.MouseMove += new global::System.Windows.Forms.MouseEventHandler(this.pictureBoxLogo_MouseMove);
			this.labelMessage.MouseUp += new global::System.Windows.Forms.MouseEventHandler(this.pictureBoxLogo_MouseUp);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(290, 190);
			base.ControlBox = false;
			base.Controls.Add(this.panelAll);
			this.Font = new global::System.Drawing.Font("Tahoma", 8.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.None;
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SplashScreen";
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "BASS.NET API";
			base.TopMost = true;
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.SplashScreen_KeyDown);
			base.MouseDown += new global::System.Windows.Forms.MouseEventHandler(this.pictureBoxLogo_MouseDown);
			base.MouseMove += new global::System.Windows.Forms.MouseEventHandler(this.pictureBoxLogo_MouseMove);
			base.MouseUp += new global::System.Windows.Forms.MouseEventHandler(this.pictureBoxLogo_MouseUp);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxLogo).EndInit();
			this.panelAll.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private global::System.Windows.Forms.PictureBox pictureBoxLogo;

		private global::System.Windows.Forms.Label labelCopyrightNote;

		private global::System.Windows.Forms.Label labelCopyright;

		private global::System.Windows.Forms.Panel panelAll;

		private global::System.Windows.Forms.Label labelVersion;

		private global::System.Windows.Forms.ToolTip toolTip;

		private global::System.ComponentModel.IContainer components;

		private global::System.Windows.Forms.Label labelMessage;
	}
}
