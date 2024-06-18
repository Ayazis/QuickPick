using NAudio.CoreAudioApi;

namespace QuickPick.Utilities
{
    public class AudioControl : IPercentageValueHandler
    {
        public double CurrentVolume => GetCurrentVolume();
        private MMDevice _audioDevice;
        DateTime _timeOfAudioCheck;

        public bool IsAudioPlaying => Is_AudioPlaying();

        public AudioControl()
        {
            MMDeviceEnumerator devEnum = new MMDeviceEnumerator();

            _audioDevice = IsAudioPlaying ? _audioDevice : devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        }

        public bool Is_AudioPlaying()
        {
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
            bool timeToRecheckAudioDevice = (DateTime.UtcNow - _timeOfAudioCheck).TotalMilliseconds > 500;
            if (timeToRecheckAudioDevice)
            {
                _timeOfAudioCheck = DateTime.UtcNow;
                Is_AudioPlaying();
            }


            if (_audioDevice == null)
                return;

            _audioDevice.AudioEndpointVolume.MasterVolumeLevelScalar = (float)value / 100;
        }
        private double GetCurrentVolume()
        {
            if (_audioDevice == null)
                return double.NaN;

            float currentVolume = _audioDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100;
            return (double)currentVolume;
        }

    }
}
