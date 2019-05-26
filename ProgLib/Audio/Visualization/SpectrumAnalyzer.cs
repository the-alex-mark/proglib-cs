using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Un4seen.Bass;
using Un4seen.BassWasapi;

namespace ProgLib.Audio.Visualization
{
    /// <summary>
    /// Предоставляет методы для работы с визуализацией звуковых сигналов.
    /// </summary>
    public class SpectrumAnalyzer
    {
        public SpectrumAnalyzer()
        {
            Plugins.Bass.Start();
            
            _timer = new Timer { Interval = 1 };
            _timer.Tick += Getter;

            this.Count = 10;
            this.Interval = 1;

            _ftt = new Single[8192];
            _spectrumData = new List<Byte>();
            _process = new WASAPIPROC(Process);
            _lastLevel = 0;
            _hanctr = 0;
            _initialized = false;

            this.Init();
        }

        #region Variables

        private List<String> _devace = new List<String>();

        private Timer _timer;             // Таймер, который обновляет данные

        private Single[] _ftt;            // Буфер для данных FTT
        private List<Byte> _spectrumData; // Буфер данных спектра
        private WASAPIPROC _process;      // Функция обратного вызова для получения данных
        private Int32 _lastLevel;         // Последний выходной уровень
        private Int32 _hanctr;            // Последний счетчик уровня выходного сигнала
        private Boolean _initialized;     // Инициализированный флаг
        private Int32 _deviceIndex;       // Индекс используемого устройства

        // Событие при получении данных
        public event HandledEventArgs Leveling;
        public delegate void HandledEventArgs(Object sender, SpectrumAnalyzerEventArgs e);

        #endregion

        #region Properties

        /// <summary>
        /// Количество данных
        /// </summary>
        public Int32 Count
        {
            get;
            set;
        }

        /// <summary>
        /// Интервал обновления данных
        /// </summary>
        public Int32 Interval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        #endregion
        
        private void Init()
        {
            Boolean Result = false;
            for (int i = 0; i < BassWasapi.BASS_WASAPI_GetDeviceCount(); i++)
            {
                BASS_WASAPI_DEVICEINFO device = BassWasapi.BASS_WASAPI_GetDeviceInfo(i);
                if (device.IsEnabled && device.IsLoopback)
                {
                    _devace.Add(String.Format("{0} - {1}", i, device.name));
                }
            }

            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATETHREADS, false);
            Result = Bass.BASS_Init(0, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);

            if (!Result)
                throw new Exception("Ошибка инициализации");
        }

        private Int32 Process(IntPtr Buffer, Int32 Length, IntPtr User)
        {
            return Length;
        }

        /// <summary>
        /// Запускает процесс получения данных.
        /// </summary>
        public void Start()
        {
            this.Stop();

            if (!_initialized)
            {
                String[] Array = (_devace[0] as String).Split(' ');
                _deviceIndex = Convert.ToInt32(Array[0]);
                Boolean result = BassWasapi.BASS_WASAPI_Init(_deviceIndex, 0, 0, BASSWASAPIInit.BASS_WASAPI_BUFFER, 1f, 0.05f, _process, IntPtr.Zero);
                if (!result)
                {
                    BASSError Error = Bass.BASS_ErrorGetCode();
                    throw new Exception(Error.ToString());
                }
                else { _initialized = true; }
            }

            BassWasapi.BASS_WASAPI_Start();
            _timer.Start();
        }

        /// <summary>
        /// Метод получения данных с устройсва.
        /// </summary>
        /// <param name="_object"></param>
        /// <param name="_elapsedEventArgs"></param>
        private void Getter(Object _object, EventArgs _elapsedEventArgs)
        {
            Int32 Ret = BassWasapi.BASS_WASAPI_GetData(_ftt, (int)BASSData.BASS_DATA_FFT8192); // Получение данных ch.annel fft
            if (Ret < -1) return;
            Int32 X, Y;
            Int32 B0 = 0;

            // Вычисление данных спектра (код берется из образца bass_wasapi).
            for (X = 0; X < Count; X++)
            {
                float Peak = 0;
                Int32 B1 = (int)Math.Pow(2, X * 10.0 / (Count - 1));
                if (B1 > 1023) B1 = 1023;
                if (B1 <= B0) B1 = B0 + 1;
                for (; B0 < B1; B0++)
                {
                    if (Peak < _ftt[1 + B0]) Peak = _ftt[1 + B0];
                }
                Y = (int)(Math.Sqrt(Peak) * 3 * 255 - 4);
                if (Y > 255) Y = 255;
                if (Y < 0) Y = 0;
                _spectrumData.Add((byte)Y);
            }

            Leveling?.Invoke(this, new SpectrumAnalyzerEventArgs(_spectrumData));
            _spectrumData.Clear();

            Int32 level = BassWasapi.BASS_WASAPI_GetLevel();
            if (level == _lastLevel && level != 0) _hanctr++;
            _lastLevel = level;

            if (_hanctr > 3)
            {
                _hanctr = 0;
                this.Free();
                Bass.BASS_Init(0, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
                _initialized = false;
            }
        }

        /// <summary>
        /// Останавливает процесс получения данных.
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
            BassWasapi.BASS_WASAPI_Stop(true);
        }

        /// <summary>
        /// Освобождает все ресурсы, используемые текущим экземпляром класса <see cref="SpectrumAnalyzer"/>.
        /// </summary>
        public void Free()
        {
            BassWasapi.BASS_WASAPI_Free();
            Bass.BASS_Free();
        }

        /// <summary>
        /// Освобождает все ресурсы, используемые текущим экземпляром класса <see cref="SpectrumAnalyzer"/>.
        /// </summary>
        public void Dispose()
        {
            Stop();
            Free();

            BassWasapi.BASS_WASAPI_Free();
            Bass.BASS_Free();
        }
    }
}
