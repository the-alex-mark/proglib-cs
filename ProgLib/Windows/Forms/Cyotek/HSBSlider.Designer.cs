namespace ProgLib.Windows.Forms.Cyotek
{
    partial class HSBSlider
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.HueSlider = new ProgLib.Windows.Forms.Cyotek.HueSlider();
            this.SBSlider = new ProgLib.Windows.Forms.Cyotek.SBSlider();
            this.SuspendLayout();
            // 
            // HueSlider
            // 
            this.HueSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.HueSlider.BorderColor = System.Drawing.Color.Black;
            this.HueSlider.Location = new System.Drawing.Point(209, 3);
            this.HueSlider.MinimumSize = new System.Drawing.Size(32, 32);
            this.HueSlider.Name = "HueSlider";
            this.HueSlider.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.HueSlider.Size = new System.Drawing.Size(32, 200);
            this.HueSlider.TabIndex = 1;
            this.HueSlider.Text = "hueSlider1";
            this.HueSlider.Value = 360;
            this.HueSlider.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HueSlider_Scroll);
            this.HueSlider.Resize += new System.EventHandler(this.HueSlider_Resize);
            // 
            // SBSlider
            // 
            this.SBSlider.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.SBSlider.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SBSlider.BorderColor = System.Drawing.Color.Black;
            this.SBSlider.Brightness = 100;
            this.SBSlider.Hue = 360;
            this.SBSlider.Location = new System.Drawing.Point(5, 5);
            this.SBSlider.Name = "SBSlider";
            this.SBSlider.Saturation = 100;
            this.SBSlider.Size = new System.Drawing.Size(195, 195);
            this.SBSlider.TabIndex = 0;
            this.SBSlider.Text = "sbSlider1";
            this.SBSlider.Resize += new System.EventHandler(this.SBSlider_Resize);
            // 
            // HSBSlider
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.HueSlider);
            this.Controls.Add(this.SBSlider);
            this.MinimumSize = new System.Drawing.Size(115, 80);
            this.Name = "HSBSlider";
            this.Size = new System.Drawing.Size(246, 205);
            this.Resize += new System.EventHandler(this.HSBSlider_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private SBSlider SBSlider;
        private HueSlider HueSlider;
    }
}
