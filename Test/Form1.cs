using ProgLib.Windows.Minimal;
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
using ProgLib.Windows.Metro;
using ProgLib.Audio;
using System.IO;
using ProgLib.Text.Encoding.QRCode;
using ProgLib.Diagnostics;
using ProgLib;
using ProgLib.Network;

namespace Test
{
    public partial class Form1 : MetroForm
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

        private void Form1_Load(Object sender, EventArgs e)
        {
            //Processor _processor = SystemInfo.Device(new Processor());
            //MessageBox.Show(_processor.SerialNumber.ToString());

            //Drive[] _drives = SystemInfo.Drives();
            //MessageBox.Show(_drives.Aggregate("", (S, I) => S += I.Name + "\n" + I.LogicalName + "\n" + I.VolumeLabel + "\n\n"));




            //Playlist _playlist = Playlist.Load(@"D:\Files\Учёба\Курс №3\#Учебная практика\Курсовая работа №1 - Программа\Music\Radios.m3u");
            //String Name = (_playlist.Records[3] as Radio).Name;
            //String URL = (_playlist.Records[3] as Radio).URL;

            //Playlist _playlist = Playlist.Load(@"D:\Files\Учёба\Курс №3\#Учебная практика\Курсовая работа №1 - Программа\Music\Songs.m3u");
            //String URL = (_playlist.Records[3] as Song).Format;

            //MessageBox.Show(/*Name + "\n" + */URL);

            //int x = 881;
            //MessageBox.Show(Convert.ToString(85, 10));
            //MessageBox.Show(Convert.ToString(, 10));

            //MessageBox.Show(Unit.HexadecimalToDecimal("1AD1").ToString());
            //MessageBox.Show(('B' == 'B').ToString());
            //.00000000000000000086736
            //MessageBox.Show(String.Format("{0:0.00}", Unit.BitToMegabyte(1024)));

            //LocalNetwork LN = new LocalNetwork();
            //MessageBox.Show(LN.GetMachinesList().Aggregate("", (S, I) => S += I + "\n"));
            //MessageBox.Show(LN.GetServerList(TypeServer.Workstation).Aggregate("", (S, I) => S += I + "\n"));
        }
    }
}
