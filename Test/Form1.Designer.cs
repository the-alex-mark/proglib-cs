namespace Test
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.metroButton1 = new ProgLib.Windows.Metro.MetroButton();
            this.metroButton2 = new ProgLib.Windows.Metro.MetroButton();
            this.pictureBoxQRCode = new System.Windows.Forms.PictureBox();
            this.textBoxQRCode = new System.Windows.Forms.TextBox();
            this.iSpectrum1 = new ProgLib.Audio.Visualization.iSpectrum();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRCode)).BeginInit();
            this.SuspendLayout();
            // 
            // metroButton1
            // 
            this.metroButton1.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.metroButton1.Highlighted = false;
            this.metroButton1.Location = new System.Drawing.Point(77, 151);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(181, 37);
            this.metroButton1.StyleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            this.metroButton1.TabIndex = 0;
            this.metroButton1.Text = "Администратор";
            this.metroButton1.Theme = ProgLib.Windows.Metro.Theme.Dark;
            this.metroButton1.UseVisualStyleBackColor = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // metroButton2
            // 
            this.metroButton2.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.metroButton2.Highlighted = false;
            this.metroButton2.Location = new System.Drawing.Point(77, 194);
            this.metroButton2.Name = "metroButton2";
            this.metroButton2.Size = new System.Drawing.Size(181, 37);
            this.metroButton2.StyleColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            this.metroButton2.TabIndex = 1;
            this.metroButton2.Text = "Врач";
            this.metroButton2.Theme = ProgLib.Windows.Metro.Theme.Dark;
            this.metroButton2.UseVisualStyleBackColor = true;
            this.metroButton2.Click += new System.EventHandler(this.metroButton2_Click);
            // 
            // pictureBoxQRCode
            // 
            this.pictureBoxQRCode.Location = new System.Drawing.Point(377, 110);
            this.pictureBoxQRCode.Name = "pictureBoxQRCode";
            this.pictureBoxQRCode.Size = new System.Drawing.Size(150, 150);
            this.pictureBoxQRCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxQRCode.TabIndex = 2;
            this.pictureBoxQRCode.TabStop = false;
            // 
            // textBoxQRCode
            // 
            this.textBoxQRCode.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxQRCode.Location = new System.Drawing.Point(377, 266);
            this.textBoxQRCode.Name = "textBoxQRCode";
            this.textBoxQRCode.Size = new System.Drawing.Size(150, 21);
            this.textBoxQRCode.TabIndex = 3;
            // 
            // iSpectrum1
            // 
            this.iSpectrum1.BackProgressColor = System.Drawing.Color.Transparent;
            this.iSpectrum1.Count = 39;
            this.iSpectrum1.Location = new System.Drawing.Point(77, 266);
            this.iSpectrum1.Name = "iSpectrum1";
            this.iSpectrum1.ProgressColor = System.Drawing.Color.Green;
            this.iSpectrum1.Size = new System.Drawing.Size(181, 78);
            this.iSpectrum1.TabIndex = 4;
            this.iSpectrum1.Text = "iSpectrum1";
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(677, 396);
            this.Controls.Add(this.iSpectrum1);
            this.Controls.Add(this.textBoxQRCode);
            this.Controls.Add(this.pictureBoxQRCode);
            this.Controls.Add(this.metroButton2);
            this.Controls.Add(this.metroButton1);
            this.Font = new System.Drawing.Font("Century Gothic", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "Form1";
            this.Text = "Поликлиника";
            this.Theme = ProgLib.Windows.Metro.Theme.Dark;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ProgLib.Windows.Metro.MetroButton metroButton1;
        private ProgLib.Windows.Metro.MetroButton metroButton2;
        private System.Windows.Forms.PictureBox pictureBoxQRCode;
        private System.Windows.Forms.TextBox textBoxQRCode;
        private ProgLib.Audio.Visualization.iSpectrum iSpectrum1;
    }
}

