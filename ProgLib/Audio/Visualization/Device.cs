using System;
using CoreAudioApi;

namespace ProgLib.Audio.Visualization
{
    public class Speaker
    {
        /// <summary>
        /// Значение громкости левого динамика
        /// </summary>
        public static Int32 Left
        {
            get
            {
                MMDevice Device = new MMDeviceEnumerator().GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
                return (int)(Device.AudioMeterInformation.PeakValues[1] * 100f);
            }
        }

        /// <summary>
        /// Значение громкости правого динамика
        /// </summary>
        public static Int32 Right
        {
            get
            {
                MMDevice Device = new MMDeviceEnumerator().GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
                return (int)(Device.AudioMeterInformation.PeakValues[0] * 100f);
            }
        }
    }
}