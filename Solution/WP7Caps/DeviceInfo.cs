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
        public PowerSource PowerSource { get; private set; }
        public Size ScreenResolution { get; private set; }
        public long TotalMemory { get; private set; }

        public void Load()
        {
            FirmwareVersion = DeviceStatus.DeviceFirmwareVersion;
            HardwareVersion = DeviceStatus.DeviceHardwareVersion;
            HasKeyboard = DeviceStatus.IsKeyboardPresent;
            Manufacturer = DeviceStatus.DeviceManufacturer;
            Name = DeviceStatus.DeviceName;
            PowerSource = DeviceStatus.PowerSource;
            ScreenResolution = new Size(Application.Current.Host.Content.ActualWidth, Application.Current.Host.Content.ActualHeight);
            TotalMemory = DeviceStatus.DeviceTotalMemory;

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
