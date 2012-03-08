using System.Windows;
using Microsoft.Phone.Info;

namespace WinPhoneCaps
{
    public class DeviceInfo : NotifyPropertyChangedBase
    {
        public DeviceInfo()
        {
        }

        public string FirmwareVersion { get; private set; }
        public string HardwareVersion { get; private set; }
        public bool HasKeyboard { get; private set; }
        public string Manufacturer { get; private set; }
        public string Name { get; private set; }
        public string PowerSource { get; private set; }
        public string ScreenResolution { get; private set; }
        public string TotalMemory { get; private set; }

        public void Load()
        {
            FirmwareVersion = DeviceStatus.DeviceFirmwareVersion;
            HardwareVersion = DeviceStatus.DeviceHardwareVersion;
            HasKeyboard = DeviceStatus.IsKeyboardPresent;
            Manufacturer = DeviceStatus.DeviceManufacturer;
            Name = DeviceStatus.DeviceName;
            PowerSource = DeviceStatus.PowerSource.ToString();
            ScreenResolution = string.Format("{0}x{1}", Application.Current.Host.Content.ActualWidth, Application.Current.Host.Content.ActualHeight);
            TotalMemory = string.Format("{0} MB", DeviceStatus.DeviceTotalMemory / 1048576);

            RaisePropertyChanged("FirmwareVersion");
            RaisePropertyChanged("HardwareVersion");
            RaisePropertyChanged("HasKeyboard");
            RaisePropertyChanged("Manufacturer");
            RaisePropertyChanged("Name");
            RaisePropertyChanged("PowerSource");
            RaisePropertyChanged("ScreenResolution");
            RaisePropertyChanged("TotalMemory");
        }
    }
}
