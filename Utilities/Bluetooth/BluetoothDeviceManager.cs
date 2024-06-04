using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

public class BlueToothAudioDeviceConnector
{
    private BluetoothClient _bluetoothClient = new BluetoothClient();

    public List<string> GetPairedDevices()
    {
        return _bluetoothClient.PairedDevices
            .Where(w => w.ClassOfDevice.Service.HasFlag(ServiceClass.LEAudio) || w.ClassOfDevice.Service.HasFlag(ServiceClass.Audio))
            .Select(s => s.DeviceName)
            .ToList();
    }

    public bool ConnectToDevice(string deviceName)
    {
        try
        {
            var device = _bluetoothClient.PairedDevices.FirstOrDefault(s => s.DeviceName == deviceName);


            if (device == null)
                return false;

            _bluetoothClient.Connect(device.DeviceAddress, BluetoothService.Headset);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
