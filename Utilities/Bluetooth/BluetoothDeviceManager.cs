using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.Collections.Generic;

public class BluetoothManager
{
    private BluetoothClient _bluetoothClient = new BluetoothClient();

    public List<string> GetPairedDevices()
    {
        return _bluetoothClient.PairedDevices.Select(s => s.DeviceName).ToList();
    }

    public bool ConnectToDevice(string deviceName)
    {
        try
        {
            var device = _bluetoothClient.PairedDevices.FirstOrDefault(s => s.DeviceName == deviceName);
            if (device == null)
                return false;

            _bluetoothClient.Connect(device.DeviceAddress, BluetoothService.SerialPort);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
