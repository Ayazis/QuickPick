using NAudio.CoreAudioApi;
using QuickPick.Logic;

namespace QuickPick.Utilities
{
    public class PlayBackControl
    {
        MMDevice _soundDevice;

        public PlayBackControl()
        {
            MMDeviceEnumerator devEnum = new MMDeviceEnumerator();
            _soundDevice = devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        }
        public bool IsAudioPlaying => DeviceIsAudioPlaying();

        private bool DeviceIsAudioPlaying()
        {
            var count = _soundDevice.AudioMeterInformation.PeakValues.Count;
            for (int i = 0; i < count; i++)
            {
                float peakValue = _soundDevice.AudioMeterInformation.PeakValues[i];
                if (peakValue > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
