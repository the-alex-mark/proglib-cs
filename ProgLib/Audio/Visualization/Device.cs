using System;
using CoreAudioApi;

namespace ProgLib.Audio.Visualization
{
    public class Device
    {
        /// <summary>
        /// Возвращает значение громкости левого динамика
        /// </summary>
        /// <returns></returns>
        public static Int32 Left()
        {
            MMDevice Device = new MMDeviceEnumerator().GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            return (int)(Device.AudioMeterInformation.PeakValues[1] * 100f);
        }

        /// <summary>
        /// Возвращает значение громкости правого динамика
        /// </summary>
        /// <returns></returns>
        public static Int32 Right()
        {
            MMDevice Device = new MMDeviceEnumerator().GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            return (int)(Device.AudioMeterInformation.PeakValues[0] * 100f);
        }
    }
}