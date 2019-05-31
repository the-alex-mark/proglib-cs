using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ProgLib.Diagnostics
{
    public class SystemInfo
    {
        [DllImport("kernel32.dll")][return: MarshalAs(UnmanagedType.Bool)]
        static extern Boolean GetPhysicallyInstalledSystemMemory(out Int64 TotalMemoryInKilobytes);
        
        /// <summary>
        /// Возвращает имя пользователя
        /// </summary>
        /// <returns></returns>
        public String UserName()
            {
                return Environment.UserName;
            }

        /// <summary>
        /// Возвращает имя локального компьютера.
        /// </summary>
        public String MachineName()
        {
            return Environment.MachineName;
        }

        /// <summary>
        /// Возвращает количество оперативной памяти (в КБ).
        /// </summary>
        public Int64 Ram()
        {
            GetPhysicallyInstalledSystemMemory(out Int64 Memory);
            return Memory;
        }

        /// <summary>
        /// Получает размеры экрана
        /// </summary>
        /// <returns></returns>
        public static Size Screen()
        {
            return new Size(
                System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,
                System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
        }

        /// <summary>
        /// Получает информацию об операционной системе
        /// </summary>
        /// <returns></returns>
        public static OperatingSystem OperatingSystem()
        {
            OperatingSystem _operatingSystem = new OperatingSystem();

            try
            {
                ManagementObjectSearcher Searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject Request in Searcher.Get())
                {
                    _operatingSystem.Caption = Request["Caption"].ToString();
                    _operatingSystem.Version = Request["Version"].ToString();
                    _operatingSystem.Bit = (Environment.Is64BitOperatingSystem) ? "x64" : "x86";
                    _operatingSystem.Description = Request["Description"].ToString();
                    _operatingSystem.Manufacturer = Request["Manufacturer"].ToString();
                    _operatingSystem.SerialNumber = Request["SerialNumber"].ToString();
                    _operatingSystem.InstallDate = Convert.ToDateTime(Request["InstallDate"]);
                    _operatingSystem.Directory = Request["WindowsDirectory"].ToString();
                    _operatingSystem.Drive = Request["SystemDrive"].ToString();
                    _operatingSystem.MaxNumberOfProcesses = (Int32)Request["MaxNumberOfProcesses"];
                    _operatingSystem.MaxProcessMemorySize = (Int32)Request["MaxProcessMemorySize"];
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("Произошла ошибка при запросе данных WMI: \n" + e.Message, "WMI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return _operatingSystem;
        }

        /// <summary>
        /// Получает информацию о доступных приводах
        /// </summary>
        /// <returns></returns>
        public static Drive[] Drives()
        {
            List<Drive> _drives = new List<Drive>();

            try
            {
                for (int i = 0; i < DriveInfo.GetDrives().Length; i++)
                {
                    Drive _drive = new Drive();
                    DriveInfo Drive = DriveInfo.GetDrives()[i];

                    ManagementObjectSearcher Searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive WHERE Index = " + i.ToString());
                    foreach (ManagementObject Request in Searcher.Get())
                    {
                        _drive.Name = Request["Caption"].ToString();
                        _drive.Description = Request["Description"].ToString();
                        _drive.Manufacturer = Request["Manufacturer"].ToString();
                        _drive.SerialNumber = Request["SerialNumber"].ToString();
                    }

                    _drive.VolumeLabel = (Drive.IsReady) ? Drive.Name : "";
                    _drive.Type = Drive.DriveType.ToString();
                    _drive.LogicalName = (Drive.IsReady) ? Drive.VolumeLabel : "";
                    _drive.DriveFormat = (Drive.IsReady) ? Drive.DriveFormat : "";
                    _drive.AvailableFreeSpace = (Drive.IsReady) ? Drive.AvailableFreeSpace : 0;
                    _drive.TotalFreeSpace = (Drive.IsReady) ? Drive.TotalFreeSpace : 0;
                    _drive.TotalSize = (Drive.IsReady) ? Drive.TotalSize : 0;

                    _drives.Add(_drive);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("Произошла ошибка при запросе данных WMI: \n" + e.Message, "WMI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return _drives.ToArray();
        }

        /// <summary>
        /// Получает информацию о всех активных процессах
        /// </summary>
        /// <returns></returns>
        public static Process[] Processes()
        {
            return Process.GetProcesses();
        }

        /// <summary>
        /// Получает информацию об установленном программной обеспечении
        /// </summary>
        /// <returns></returns>
        public static Software[] Softwares()
        {
            List<Software> _softwares = new List<Software>();

            try
            {
                ManagementObjectSearcher Searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Product");
                foreach (ManagementObject Request in Searcher.Get())
                {
                    Software _software = new Software
                    {
                        Name = Request["Caption"].ToString(),
                        Description = Request["Description"].ToString(),
                        InstallDate = Convert.ToDateTime(Request["Description"]),
                        Version = Request["Version"].ToString()
                    };

                    _softwares.Add(_software);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("Произошла ошибка при запросе данных WMI: \n" + e.Message, "WMI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return _softwares.ToArray();
        }

        /// <summary>
        /// Получает информацию об установленном BIOS'е
        /// </summary>
        /// <returns></returns>
        public static BIOS BIOS()
        {
            BIOS _bios = new BIOS();

            try
            {
                ManagementObjectSearcher Searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");
                foreach (ManagementObject Request in Searcher.Get())
                {
                    _bios.Name = Request["Caption"].ToString();
                    _bios.Description = Request["Description"].ToString();
                    _bios.Manufacturer = Request["Manufacturer"].ToString();
                    _bios.SerialNumber = Request["SerialNumber"].ToString();
                    _bios.Version = Request["Version"].ToString();
                    
                    if (Request["BIOSVersion"] == null)
                        _bios.BIOSVersion = Request["BIOSVersion"].ToString();
                    else
                    {
                        _bios.BIOSVersion = "";
                        foreach (String Value in (String[])(Request["BIOSVersion"]))
                            _bios.BIOSVersion += Value + Environment.NewLine;
                    }

                    _bios.SMBIOSBIOSVersion = Request["SMBIOSBIOSVersion"].ToString();
                    _bios.SMBIOSMajorVersion = (int)Request["SMBIOSMajorVersion"];
                    _bios.SMBIOSMinorVersion = (int)Request["SMBIOSMinorVersion"];
                    _bios.SystemBiosMajorVersion = (int)Request["SystemBiosMajorVersion"];
                    _bios.SystemBiosMinorVersion = (int)Request["SystemBiosMinorVersion"];
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("Произошла ошибка при запросе данных WMI: \n" + e.Message, "WMI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return _bios;
        }

        /// <summary>
        /// Получает информацию о доступных устройствах
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Device"></param>
        /// <returns></returns>
        public static T Device<T>(T Device) where T : Device
        {
            T Result = (T)Activator.CreateInstance(Device.GetType());

            try
            {
                ManagementObjectSearcher Searcher;

                switch (Device.ToString())
                {
                    case "ProgLib.Diagnostics.Processor":
                        Searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                        foreach (ManagementObject Request in Searcher.Get())
                        {
                            (Result as Processor).Name += Request["Name"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as Processor).Description += Request["Description"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as Processor).Manufacturer += Request["Manufacturer"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as Processor).SerialNumber += Request["SerialNumber"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");

                            (Result as Processor).NumberOfCores += Convert.ToInt32(Request["NumberOfCores"]);
                            (Result as Processor).CurrentClockSpeed += Convert.ToDouble(Request["CurrentClockSpeed"]) * 0.001;
                        }
                        break;

                    case "ProgLib.Diagnostics.BaseBoard":
                        Searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
                        foreach (ManagementObject Request in Searcher.Get())
                        {
                            (Result as BaseBoard).Name += Request["Product"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as BaseBoard).Description += Request["Description"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as BaseBoard).Manufacturer += Request["Manufacturer"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as BaseBoard).SerialNumber += Request["SerialNumber"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                        }
                        break;

                    case "ProgLib.Diagnostics.VideoController":
                        Searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
                        foreach (ManagementObject Request in Searcher.Get())
                        {
                            (Result as VideoController).Name += Request["Name"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as VideoController).Description += Request["Description"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as VideoController).Manufacturer += "" + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as VideoController).SerialNumber += "" + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");

                            (Result as VideoController).VideoModeDescription = Request["VideoModeDescription"].ToString();
                        }
                        break;

                    case "ProgLib.Diagnostics.SoundDevice":
                        Searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
                        foreach (ManagementObject Request in Searcher.Get())
                        {
                            (Result as SoundDevice).Name += Request["Name"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as SoundDevice).Description += Request["Description"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as SoundDevice).Manufacturer += Request["Manufacturer"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as SoundDevice).SerialNumber += "" + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                        }
                        break;

                    case "ProgLib.Diagnostics.KeyBoard":
                        Searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Keyboard");
                        foreach (ManagementObject Request in Searcher.Get())
                        {
                            (Result as SoundDevice).Name += Request["Name"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as SoundDevice).Description += Request["Description"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as SoundDevice).Manufacturer += "" + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as SoundDevice).SerialNumber += "" + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                        }
                        break;

                    case "ProgLib.Diagnostics.PointingDevice":
                        Searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PointingDevice");
                        foreach (ManagementObject Request in Searcher.Get())
                        {
                            (Result as SoundDevice).Name += Request["Name"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as SoundDevice).Description += Request["Description"].ToString() + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as SoundDevice).Manufacturer += "" + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                            (Result as SoundDevice).SerialNumber += "" + ((Searcher.Get().Count > 1) ? Environment.NewLine : "");
                        }
                        break;
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("Произошла ошибка при запросе данных WMI: \n" + e.Message, "WMI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            return Result;
        }
    }
}
