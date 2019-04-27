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
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

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
            //MessageBox.Show(LocalNetwork.GetMachines().Aggregate("", (S, I) => S += I + "\n"));
            //MessageBox.Show(LocalNetwork.GetServers(TypeServer.Workstation).Aggregate("", (S, I) => S += I + "\n"));
        }

        private void metroButton2_Click(Object sender, EventArgs e)
        {
            WaveStream mainOutputStream = new WaveFileReader(@"D:\System\User\Музыка\Old school\Laurent Wolf - No Stress resampled 96kHz.wav");
            WaveChannel32 volumeStream = new WaveChannel32(mainOutputStream);

            WaveOutEvent player = new WaveOutEvent();

            player.Init(volumeStream);
            player.Play();


            Song _song = new Song(@"D:\System\User\Музыка\Old school\Laurent Wolf - No Stress.mp3");
            MessageBox.Show(_song.Tags.All.Title);

            //ProgLib.IO.Archive.AddFile(@"C:\Users\Александр Макаров\Desktop\TestZipArchive.zip", @"D:\System\User\Музыка\Old school\Laurent Wolf - No Stress.mp3");
        }
    }
}
