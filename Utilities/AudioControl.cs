using NAudio.CoreAudioApi;

namespace QuickPick.Utilities
{
    public class AudioControl : IPercentageValueHandler
    {
        public double CurrentVolume => GetCurrentVolume();
        private MMDevice _latestDevicePlayingAudio;

        public bool IsAudioPlaying => Is_AudioPlaying();

        public AudioControl()
        {
            MMDeviceEnumerator devEnum = new MMDeviceEnumerator();
            _latestDevicePlayingAudio = devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        }

        public bool Is_AudioPlaying()
        {
            MMDeviceEnumerator devEnum = new MMDeviceEnumerator();
            MMDeviceCollection soundDevices = devEnum.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            foreach (MMDevice soundDevice in soundDevices)
            {
                bool isPlaying = DeviceIsAudioPlaying(soundDevice);
                if (isPlaying)
                {
                    _latestDevicePlayingAudio = soundDevice;
                    return true;
                }
            }
            return false;
        }

        private bool DeviceIsAudioPlaying(MMDevice soundDevice)
        {
            var count = soundDevice.AudioMeterInformation.PeakValues.Count;
            for (int i = 0; i < count; i++)
            {
                float peakValue = soundDevice.AudioMeterInformation.PeakValues[i];
                if (peakValue > 0.001)
                {
                    return true;
                }
            }
            return false;
        }

        public void HandleNewValue(double value)
        {
            if (_latestDevicePlayingAudio == null)
                return;

            _latestDevicePlayingAudio.AudioEndpointVolume.MasterVolumeLevelScalar = (float)value / 100;
        }
        private double GetCurrentVolume()
        {
            if (_latestDevicePlayingAudio == null)
                return double.NaN;

            float currentVolume = _latestDevicePlayingAudio.AudioEndpointVolume.MasterVolumeLevelScalar * 100;
            return (double)currentVolume;
        }

    }
}
