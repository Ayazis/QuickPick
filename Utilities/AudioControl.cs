using NAudio.CoreAudioApi;

namespace QuickPick.Utilities
{
    public class AudioControl : IPercentageValueHandler
    {
        public double CurrentVolume => GetCurrentVolume();
        private MMDevice _audioDevice;
        static DateTime _timeOfAudioCheck;

        public bool IsAudioPlaying => UpdateAudioDeviceStatus();

        public AudioControl()
        {
            MMDeviceEnumerator devEnum = new MMDeviceEnumerator();

            _audioDevice = IsAudioPlaying ? _audioDevice : devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        }

        public bool UpdateAudioDeviceStatus()
        {
            _timeOfAudioCheck = DateTime.UtcNow;

            if (DeviceIsAudioPlaying(_audioDevice))
                return true;

            MMDeviceEnumerator devEnum = new MMDeviceEnumerator();
            MMDeviceCollection soundDevices = devEnum.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            foreach (MMDevice soundDevice in soundDevices)
            {
                bool isPlaying = DeviceIsAudioPlaying(soundDevice);
                if (isPlaying)
                {
                    _audioDevice = soundDevice;
                    return true;
                }
            }
            return false;
        }

        private bool DeviceIsAudioPlaying(MMDevice soundDevice)
        {
            if (_audioDevice == null)
                return false;

            try
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
            catch (Exception)
            {

                return false;
            }
        }

        public void HandleNewValue(double value)
        {
            if (_audioDevice == null)
                return;

            _audioDevice.AudioEndpointVolume.MasterVolumeLevelScalar = (float)value / 100;

            _timeOfAudioCheck = DateTime.UtcNow;
        }
        private double GetCurrentVolume()
        {
            if (_audioDevice == null)
                return double.NaN;

            float currentVolume = _audioDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100;
            return (double)currentVolume;
        }

        public bool IsTimeToUpdate => (DateTime.UtcNow - _timeOfAudioCheck).TotalMilliseconds > 500;
    }
}
