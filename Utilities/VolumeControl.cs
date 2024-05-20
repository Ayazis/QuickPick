using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;

namespace QuickPick.Utilities
{
    public class VolumeControl : IValueHandler
    {
        public void HandleNewValue(double value)
        {
            MMDeviceEnumerator devEnum = new MMDeviceEnumerator();
            MMDevice defaultDevice = devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar = (float)value;

            //float currentVolume = defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100;

        }
    }
}
