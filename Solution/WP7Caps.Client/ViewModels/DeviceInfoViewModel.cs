namespace WinPhoneCaps.Client.ViewModels
{
    public class DeviceInfoViewModel
    {
        public DeviceInfoViewModel()
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
            var device = new DeviceInfo();
            device.Load();

            FirmwareVersion = device.FirmwareVersion;
            HardwareVersion = device.HardwareVersion;
            HasKeyboard = device.HasKeyboard;
            Manufacturer = device.Manufacturer;
            Name = device.Name;
            PowerSource = device.PowerSource.ToString();
            ScreenResolution = string.Format("{0}x{1}", device.ScreenResolution.Width, device.ScreenResolution.Height); ;
            TotalMemory = string.Format("{0} MB", device.TotalMemory / 1048576);
        }
    }
}
