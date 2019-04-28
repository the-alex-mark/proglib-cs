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

namespace Test
{
    public partial class Form1 : ProgLib.Windows.Forms.Metro.MetroForm
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
        }

        private void metroButton2_Click(Object sender, EventArgs e)
        {
            //String _dataBase       = @"C:\Users\Александр Макаров\Desktop\Extramural-master\БД\Заочное.mdb";
            //String _systemDataBase = @"C:\Users\Александр Макаров\AppData\Roaming\Microsoft\Access\System.mdw";
            //String _user           = "Admin";
            //String _password       = "";

            //AccessDataBase DB = new AccessDataBase(_dataBase);
            //AccessResult _result = DB.Request("Select * from [Личные данные]");
            ////AccessResult _result = DB.ShowTables();
            //DB.Dispose();

            //dataGridView1.DataSource = _result.Table;
            //MessageBox.Show(_result.Status);
        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            //V.Stop();
            //V.Free();
            //V.Dispose();
        }
    }
}
