namespace ProgLib.Windows.Forms.Cyotek
{
    partial class ColorDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.materialTabSelector1 = new ProgLib.Windows.Forms.Material.MaterialTabSelector();
            this.materialTabControl1 = new ProgLib.Windows.Forms.Material.MaterialTabControl();
            this.PageSystemColors = new System.Windows.Forms.TabPage();
            this.ListSystemColors = new System.Windows.Forms.ListBox();
            this.PageWebColors = new System.Windows.Forms.TabPage();
            this.ListWebColors = new System.Windows.Forms.ListBox();
            this.PageCustomColors = new System.Windows.Forms.TabPage();
            this.hsbSlider2 = new ProgLib.Windows.Forms.Cyotek.HSBSlider();
            this.adobeLabel1 = new ProgLib.Windows.Forms.Adobe.AdobeLabel();
            this.adobeNumericUpDown7 = new ProgLib.Windows.Forms.Adobe.AdobeNumericUpDown();
            this.adobeNumericUpDown4 = new ProgLib.Windows.Forms.Adobe.AdobeNumericUpDown();
            this.adobeNumericUpDown5 = new ProgLib.Windows.Forms.Adobe.AdobeNumericUpDown();
            this.adobeNumericUpDown6 = new ProgLib.Windows.Forms.Adobe.AdobeNumericUpDown();
            this.adobeNumericUpDown3 = new ProgLib.Windows.Forms.Adobe.AdobeNumericUpDown();
            this.adobeNumericUpDown2 = new ProgLib.Windows.Forms.Adobe.AdobeNumericUpDown();
            this.materialButton2 = new ProgLib.Windows.Forms.Material.MaterialButton();
            this.materialButton1 = new ProgLib.Windows.Forms.Material.MaterialButton();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.materialTabControl1.SuspendLayout();
            this.PageSystemColors.SuspendLayout();
            this.PageWebColors.SuspendLayout();
            this.PageCustomColors.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(109)))), ((int)(((byte)(186)))));
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.materialTabSelector1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(482, 95);
            this.panel1.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Light", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(20, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Dialog";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(347, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(130, 80);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(16, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 30);
            this.label1.TabIndex = 2;
            this.label1.Text = "Color Palette";
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.AnimateColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(109)))), ((int)(((byte)(186)))));
            this.materialTabSelector1.BaseTabControl = this.materialTabControl1;
            this.materialTabSelector1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.materialTabSelector1.Font = new System.Drawing.Font("Segoe UI", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.materialTabSelector1.ForeColor = System.Drawing.Color.White;
            this.materialTabSelector1.IndicatorColor = System.Drawing.SystemColors.Control;
            this.materialTabSelector1.Location = new System.Drawing.Point(0, 62);
            this.materialTabSelector1.MouseState = ProgLib.Windows.Forms.Material.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(482, 33);
            this.materialTabSelector1.TabIndex = 6;
            this.materialTabSelector1.Text = "materialTabSelector1";
            // 
            // materialTabControl1
            // 
            this.materialTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialTabControl1.Controls.Add(this.PageSystemColors);
            this.materialTabControl1.Controls.Add(this.PageWebColors);
            this.materialTabControl1.Controls.Add(this.PageCustomColors);
            this.materialTabControl1.Location = new System.Drawing.Point(12, 103);
            this.materialTabControl1.MouseState = ProgLib.Windows.Forms.Material.MouseState.HOVER;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.Size = new System.Drawing.Size(258, 183);
            this.materialTabControl1.TabIndex = 7;
            // 
            // PageSystemColors
            // 
            this.PageSystemColors.BackColor = System.Drawing.SystemColors.Control;
            this.PageSystemColors.Controls.Add(this.ListSystemColors);
            this.PageSystemColors.Location = new System.Drawing.Point(4, 21);
            this.PageSystemColors.Name = "PageSystemColors";
            this.PageSystemColors.Padding = new System.Windows.Forms.Padding(3);
            this.PageSystemColors.Size = new System.Drawing.Size(250, 158);
            this.PageSystemColors.TabIndex = 2;
            this.PageSystemColors.Text = "SYSTEM";
            // 
            // ListSystemColors
            // 
            this.ListSystemColors.BackColor = System.Drawing.SystemColors.Control;
            this.ListSystemColors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ListSystemColors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListSystemColors.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.ListSystemColors.FormattingEnabled = true;
            this.ListSystemColors.ItemHeight = 32;
            this.ListSystemColors.Location = new System.Drawing.Point(3, 3);
            this.ListSystemColors.Name = "ListSystemColors";
            this.ListSystemColors.Size = new System.Drawing.Size(244, 152);
            this.ListSystemColors.TabIndex = 1;
            // 
            // PageWebColors
            // 
            this.PageWebColors.BackColor = System.Drawing.SystemColors.Control;
            this.PageWebColors.Controls.Add(this.ListWebColors);
            this.PageWebColors.Location = new System.Drawing.Point(4, 22);
            this.PageWebColors.Name = "PageWebColors";
            this.PageWebColors.Padding = new System.Windows.Forms.Padding(3);
            this.PageWebColors.Size = new System.Drawing.Size(250, 157);
            this.PageWebColors.TabIndex = 3;
            this.PageWebColors.Text = "WEB";
            // 
            // ListWebColors
            // 
            this.ListWebColors.BackColor = System.Drawing.SystemColors.Control;
            this.ListWebColors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ListWebColors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListWebColors.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.ListWebColors.FormattingEnabled = true;
            this.ListWebColors.ItemHeight = 32;
            this.ListWebColors.Location = new System.Drawing.Point(3, 3);
            this.ListWebColors.Name = "ListWebColors";
            this.ListWebColors.Size = new System.Drawing.Size(244, 151);
            this.ListWebColors.TabIndex = 0;
            // 
            // PageCustomColors
            // 
            this.PageCustomColors.BackColor = System.Drawing.SystemColors.Control;
            this.PageCustomColors.Controls.Add(this.hsbSlider2);
            this.PageCustomColors.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PageCustomColors.Location = new System.Drawing.Point(4, 22);
            this.PageCustomColors.Name = "PageCustomColors";
            this.PageCustomColors.Padding = new System.Windows.Forms.Padding(3);
            this.PageCustomColors.Size = new System.Drawing.Size(250, 157);
            this.PageCustomColors.TabIndex = 0;
            this.PageCustomColors.Text = "CUSTOM";
            // 
            // hsbSlider2
            // 
            this.hsbSlider2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.hsbSlider2.Location = new System.Drawing.Point(6, 6);
            this.hsbSlider2.MinimumSize = new System.Drawing.Size(115, 80);
            this.hsbSlider2.Name = "hsbSlider2";
            this.hsbSlider2.Size = new System.Drawing.Size(210, 173);
            this.hsbSlider2.TabIndex = 9;
            // 
            // adobeLabel1
            // 
            this.adobeLabel1.Alignment = ProgLib.Windows.Forms.Adobe.Alignment.Right;
            this.adobeLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.adobeLabel1.BorderColor = System.Drawing.Color.Gray;
            this.adobeLabel1.BorderRadius = 5;
            this.adobeLabel1.Caption = "#";
            this.adobeLabel1.CaptionBackColor = System.Drawing.Color.LightGray;
            this.adobeLabel1.CaptionColor = System.Drawing.SystemColors.ControlText;
            this.adobeLabel1.CaptionWidth = 25;
            this.adobeLabel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.adobeLabel1.Location = new System.Drawing.Point(276, 212);
            this.adobeLabel1.Name = "adobeLabel1";
            this.adobeLabel1.ShowIcon = false;
            this.adobeLabel1.Size = new System.Drawing.Size(194, 24);
            this.adobeLabel1.TabIndex = 20;
            this.adobeLabel1.Text = "FF0000";
            this.adobeLabel1.TextBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            // 
            // adobeNumericUpDown7
            // 
            this.adobeNumericUpDown7.Alignment = ProgLib.Windows.Forms.Adobe.Alignment.Right;
            this.adobeNumericUpDown7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.adobeNumericUpDown7.BorderColor = System.Drawing.Color.Gray;
            this.adobeNumericUpDown7.BorderRadius = 5;
            this.adobeNumericUpDown7.Caption = "B";
            this.adobeNumericUpDown7.CaptionBackColor = System.Drawing.Color.LightGray;
            this.adobeNumericUpDown7.CaptionColor = System.Drawing.SystemColors.ControlText;
            this.adobeNumericUpDown7.CaptionWidth = 25;
            this.adobeNumericUpDown7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.adobeNumericUpDown7.Location = new System.Drawing.Point(276, 182);
            this.adobeNumericUpDown7.Maximum = 255;
            this.adobeNumericUpDown7.Minimum = 0;
            this.adobeNumericUpDown7.Name = "adobeNumericUpDown7";
            this.adobeNumericUpDown7.Size = new System.Drawing.Size(94, 24);
            this.adobeNumericUpDown7.TabIndex = 19;
            this.adobeNumericUpDown7.Text = "255";
            this.adobeNumericUpDown7.TextBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.adobeNumericUpDown7.Value = 255;
            // 
            // adobeNumericUpDown4
            // 
            this.adobeNumericUpDown4.Alignment = ProgLib.Windows.Forms.Adobe.Alignment.Right;
            this.adobeNumericUpDown4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.adobeNumericUpDown4.BorderColor = System.Drawing.Color.Gray;
            this.adobeNumericUpDown4.BorderRadius = 5;
            this.adobeNumericUpDown4.Caption = "H";
            this.adobeNumericUpDown4.CaptionBackColor = System.Drawing.Color.LightGray;
            this.adobeNumericUpDown4.CaptionColor = System.Drawing.SystemColors.ControlText;
            this.adobeNumericUpDown4.CaptionWidth = 25;
            this.adobeNumericUpDown4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.adobeNumericUpDown4.Location = new System.Drawing.Point(376, 122);
            this.adobeNumericUpDown4.Maximum = 255;
            this.adobeNumericUpDown4.Minimum = 0;
            this.adobeNumericUpDown4.Name = "adobeNumericUpDown4";
            this.adobeNumericUpDown4.Size = new System.Drawing.Size(94, 24);
            this.adobeNumericUpDown4.TabIndex = 18;
            this.adobeNumericUpDown4.Text = "255";
            this.adobeNumericUpDown4.TextBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.adobeNumericUpDown4.Value = 255;
            // 
            // adobeNumericUpDown5
            // 
            this.adobeNumericUpDown5.Alignment = ProgLib.Windows.Forms.Adobe.Alignment.Right;
            this.adobeNumericUpDown5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.adobeNumericUpDown5.BorderColor = System.Drawing.Color.Gray;
            this.adobeNumericUpDown5.BorderRadius = 5;
            this.adobeNumericUpDown5.Caption = "S";
            this.adobeNumericUpDown5.CaptionBackColor = System.Drawing.Color.LightGray;
            this.adobeNumericUpDown5.CaptionColor = System.Drawing.SystemColors.ControlText;
            this.adobeNumericUpDown5.CaptionWidth = 25;
            this.adobeNumericUpDown5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.adobeNumericUpDown5.Location = new System.Drawing.Point(376, 152);
            this.adobeNumericUpDown5.Maximum = 255;
            this.adobeNumericUpDown5.Minimum = 0;
            this.adobeNumericUpDown5.Name = "adobeNumericUpDown5";
            this.adobeNumericUpDown5.Size = new System.Drawing.Size(94, 24);
            this.adobeNumericUpDown5.TabIndex = 17;
            this.adobeNumericUpDown5.Text = "255";
            this.adobeNumericUpDown5.TextBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.adobeNumericUpDown5.Value = 255;
            // 
            // adobeNumericUpDown6
            // 
            this.adobeNumericUpDown6.Alignment = ProgLib.Windows.Forms.Adobe.Alignment.Right;
            this.adobeNumericUpDown6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.adobeNumericUpDown6.BorderColor = System.Drawing.Color.Gray;
            this.adobeNumericUpDown6.BorderRadius = 5;
            this.adobeNumericUpDown6.Caption = "B";
            this.adobeNumericUpDown6.CaptionBackColor = System.Drawing.Color.LightGray;
            this.adobeNumericUpDown6.CaptionColor = System.Drawing.SystemColors.ControlText;
            this.adobeNumericUpDown6.CaptionWidth = 25;
            this.adobeNumericUpDown6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.adobeNumericUpDown6.Location = new System.Drawing.Point(376, 182);
            this.adobeNumericUpDown6.Maximum = 255;
            this.adobeNumericUpDown6.Minimum = 0;
            this.adobeNumericUpDown6.Name = "adobeNumericUpDown6";
            this.adobeNumericUpDown6.Size = new System.Drawing.Size(94, 24);
            this.adobeNumericUpDown6.TabIndex = 16;
            this.adobeNumericUpDown6.Text = "255";
            this.adobeNumericUpDown6.TextBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.adobeNumericUpDown6.Value = 255;
            // 
            // adobeNumericUpDown3
            // 
            this.adobeNumericUpDown3.Alignment = ProgLib.Windows.Forms.Adobe.Alignment.Right;
            this.adobeNumericUpDown3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.adobeNumericUpDown3.BorderColor = System.Drawing.Color.Gray;
            this.adobeNumericUpDown3.BorderRadius = 5;
            this.adobeNumericUpDown3.Caption = "R";
            this.adobeNumericUpDown3.CaptionBackColor = System.Drawing.Color.LightGray;
            this.adobeNumericUpDown3.CaptionColor = System.Drawing.SystemColors.ControlText;
            this.adobeNumericUpDown3.CaptionWidth = 25;
            this.adobeNumericUpDown3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.adobeNumericUpDown3.Location = new System.Drawing.Point(276, 122);
            this.adobeNumericUpDown3.Maximum = 255;
            this.adobeNumericUpDown3.Minimum = 0;
            this.adobeNumericUpDown3.Name = "adobeNumericUpDown3";
            this.adobeNumericUpDown3.Size = new System.Drawing.Size(94, 24);
            this.adobeNumericUpDown3.TabIndex = 15;
            this.adobeNumericUpDown3.Text = "255";
            this.adobeNumericUpDown3.TextBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.adobeNumericUpDown3.Value = 255;
            // 
            // adobeNumericUpDown2
            // 
            this.adobeNumericUpDown2.Alignment = ProgLib.Windows.Forms.Adobe.Alignment.Right;
            this.adobeNumericUpDown2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.adobeNumericUpDown2.BorderColor = System.Drawing.Color.Gray;
            this.adobeNumericUpDown2.BorderRadius = 5;
            this.adobeNumericUpDown2.Caption = "G";
            this.adobeNumericUpDown2.CaptionBackColor = System.Drawing.Color.LightGray;
            this.adobeNumericUpDown2.CaptionColor = System.Drawing.SystemColors.ControlText;
            this.adobeNumericUpDown2.CaptionWidth = 25;
            this.adobeNumericUpDown2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.adobeNumericUpDown2.Location = new System.Drawing.Point(276, 152);
            this.adobeNumericUpDown2.Maximum = 255;
            this.adobeNumericUpDown2.Minimum = 0;
            this.adobeNumericUpDown2.Name = "adobeNumericUpDown2";
            this.adobeNumericUpDown2.Size = new System.Drawing.Size(94, 24);
            this.adobeNumericUpDown2.TabIndex = 14;
            this.adobeNumericUpDown2.Text = "255";
            this.adobeNumericUpDown2.TextBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.adobeNumericUpDown2.Value = 255;
            // 
            // materialButton2
            // 
            this.materialButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.materialButton2.Animation = true;
            this.materialButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(109)))), ((int)(((byte)(186)))));
            this.materialButton2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(109)))), ((int)(((byte)(186)))));
            this.materialButton2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.materialButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.materialButton2.ForeColor = System.Drawing.Color.White;
            this.materialButton2.Location = new System.Drawing.Point(376, 259);
            this.materialButton2.Name = "materialButton2";
            this.materialButton2.Size = new System.Drawing.Size(94, 25);
            this.materialButton2.TabIndex = 11;
            this.materialButton2.Text = "Отмена";
            this.materialButton2.UseVisualStyleBackColor = false;
            // 
            // materialButton1
            // 
            this.materialButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.materialButton1.Animation = true;
            this.materialButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(109)))), ((int)(((byte)(186)))));
            this.materialButton1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(109)))), ((int)(((byte)(186)))));
            this.materialButton1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.materialButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.materialButton1.ForeColor = System.Drawing.Color.White;
            this.materialButton1.Location = new System.Drawing.Point(276, 259);
            this.materialButton1.Name = "materialButton1";
            this.materialButton1.Size = new System.Drawing.Size(94, 25);
            this.materialButton1.TabIndex = 10;
            this.materialButton1.Text = "ОК";
            this.materialButton1.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(232, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ColorDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(482, 296);
            this.Controls.Add(this.adobeLabel1);
            this.Controls.Add(this.adobeNumericUpDown7);
            this.Controls.Add(this.adobeNumericUpDown4);
            this.Controls.Add(this.adobeNumericUpDown5);
            this.Controls.Add(this.adobeNumericUpDown6);
            this.Controls.Add(this.adobeNumericUpDown3);
            this.Controls.Add(this.adobeNumericUpDown2);
            this.Controls.Add(this.materialButton2);
            this.Controls.Add(this.materialButton1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.materialTabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ColorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ColorDialog";
            this.Load += new System.EventHandler(this.ColorDialog_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.materialTabControl1.ResumeLayout(false);
            this.PageSystemColors.ResumeLayout(false);
            this.PageWebColors.ResumeLayout(false);
            this.PageCustomColors.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Material.MaterialTabSelector materialTabSelector1;
        private Material.MaterialTabControl materialTabControl1;
        private System.Windows.Forms.TabPage PageSystemColors;
        private System.Windows.Forms.TabPage PageWebColors;
        private System.Windows.Forms.TabPage PageCustomColors;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private HSBSlider hsbSlider2;
        private System.Windows.Forms.ListBox ListWebColors;
        private System.Windows.Forms.ListBox ListSystemColors;
        private Material.MaterialButton materialButton1;
        private Material.MaterialButton materialButton2;
        private Adobe.AdobeNumericUpDown adobeNumericUpDown2;
        private Adobe.AdobeNumericUpDown adobeNumericUpDown3;
        private Adobe.AdobeNumericUpDown adobeNumericUpDown4;
        private Adobe.AdobeNumericUpDown adobeNumericUpDown5;
        private Adobe.AdobeNumericUpDown adobeNumericUpDown6;
        private Adobe.AdobeNumericUpDown adobeNumericUpDown7;
        private Adobe.AdobeLabel adobeLabel1;
        private System.Windows.Forms.Button button1;
    }
}