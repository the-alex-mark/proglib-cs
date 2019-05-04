using ProgLib.Windows.Forms.Minimal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProgLib.Windows;
using ProgLib.Windows.Forms.Metro;
using ProgLib.Audio;
using System.IO;
using ProgLib.Text.Encoding.QRCode;
using ProgLib.Diagnostics;
using ProgLib;
using ProgLib.Network;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using ProgLib.Audio.Visualization;
using ProgLib.Windows.Forms;
using ProgLib.Data.Access;
using ProgLib.Windows.Forms.VSCode;

namespace Test
{
    public partial class Form1 : Form// ProgLib.Windows.Forms.Metro.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void metroButton1_Click(Object sender, EventArgs e)
        {
            String Data = textBoxQRCode.Text.Trim();
            if (Data.Length == 0)
            {
                MessageBox.Show("Данные не должны быть пустыми.");
                return;
            }

            QREncoder QRCodeEncoder = new QREncoder();
            QRCodeEncoder.Encode(ErrorCorrection.H, Data);
            pictureBoxQRCode.Image = QRCodeToBitmap.CreateBitmap(QRCodeEncoder, 50, 50);
        }

        //Spectrum V;
        private void Form1_Load(Object sender, EventArgs e)
        {
            //MessageBox.Show(LocalNetwork.GetMachines().Aggregate("", (S, I) => S += I + "\n"));
            //MessageBox.Show(LocalNetwork.GetServers(TypeServer.Workstation).Aggregate("", (S, I) => S += I + "\n"));

            //V = new Spectrum
            //{
            //    Count = iSpectrum1.Count,
            //    Interval = 1
            //};
            //V.Leveling += delegate (Object _object, SpectrumEventArgs _visualizationEventArgs)
            //{
            //    iSpectrum1.Set(_visualizationEventArgs.Data);
            //};
            //V.Start();

            //adobeComboBox1.SelectedIndex = 0;
            
            menuStrip1.Renderer = new VSCodeToolStripRenderer(VSCodeTheme.QuietLight, true);
            MainMenu.Renderer = new VSCodeToolStripRenderer(VSCodeTheme.QuietLight, true);
        }

        private void metroButton2_Click(Object sender, EventArgs e)
        {
            //FileInfo _dataBase = new FileInfo(@"C:\Users\Александр Макаров\Desktop\Extramural-master\БД\Заочное.mdb");
            //FileInfo _dataBase = new FileInfo(@"D:\Files\Учёба\Курс №4\!Экзамены\Разработка и администрирование баз данных\Туристическое агенство (Пароль - admin).accdb");
            //FileInfo _systemDataBase = new FileInfo(@"D:\Files\Учёба\Курс №4\!Экзамены\Разработка и администрирование баз данных\Туристическое агенство (Пароль - admin).accdb");

            //AccessDataBase DB = new AccessDataBase(_dataBase, "admin");
            //AccessResult _result = DB.Request("Select * from [Сотрудник]");
            //DB.Dispose();

            //dataGridView1.DataSource = _result.Table;
            //MessageBox.Show(_result.Status);

            //adobeComboBox1.Font = new Font(adobeComboBox1.Font.FontFamily.Name, adobeComboBox1.Font.Size + 2);

            MinimalMessageBoxRenderer _minimalMessageBoxRenderer = new MinimalMessageBoxRenderer
            {
                Animation = true,
                BackColor = SystemColors.Control,
                Border = true,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Regular),
                HeaderColor = Color.Black,
                StatusBarColor = SystemColors.ControlDark,
                StyleColor = ProgLib.Drawing.MetroColors.Blue,
                TextColor = Color.Black
            };
            MinimalMessageBox.Show("Сообщение", "Заголовок", _minimalMessageBoxRenderer, ProgLib.Windows.Forms.Minimal.MessageType.Information);
        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            //V.Stop();
            //V.Free();
            //V.Dispose();
        }
    }
}
