using NAudio.CoreAudioApi;

namespace QuickPick.Utilities
{
    public class VolumeControl : IValueHandler
    {
        MMDevice _defaultDevice;

        public double CurrentVolume => GetCurrentVolume();
        public VolumeControl()
        {
            MMDeviceEnumerator devEnum = new MMDeviceEnumerator();
            _defaultDevice = devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        }
        public void HandleNewValue(double value)
        {
            _defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar = (float)value / 100;
        }

        private double GetCurrentVolume()
        {
            float currentVolume = _defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100;
            return (double)currentVolume;
        }
    }
}
