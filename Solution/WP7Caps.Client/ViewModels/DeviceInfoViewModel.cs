using System.Globalization;

namespace WinPhoneCaps.Client.ViewModels
{
	public class DeviceInfoViewModel : NotifyPropertyChangedBase
	{
		public DeviceInfoViewModel()
		{
			FirmwareVersion = DeviceInfo.FirmwareVersion;
			HardwareVersion = DeviceInfo.HardwareVersion;
			HasKeyboard = DeviceInfo.HasKeyboard;
			Manufacturer = DeviceInfo.Manufacturer;
			Name = DeviceInfo.Name;
			OsVersion = DeviceInfo.OsVersion.ToString();
			PowerSource = DeviceInfo.PowerSource.ToString();
			TotalMemory = string.Format(CultureInfo.InvariantCulture, "{0} MB", BytesAsMegaBytes(DeviceInfo.TotalMemory));
		}

		public string FirmwareVersion { get; private set; }
		public string HardwareVersion { get; private set; }
		public bool HasKeyboard { get; private set; }
		public string Manufacturer { get; private set; }
		public string Name { get; private set; }
		public string OsVersion { get; private set; }
		public string PowerSource { get; private set; }
		public string ScreenResolution { get; private set; }
		public string TotalMemory { get; private set; }

		public void Load()
		{
			var device = new DeviceInfo();
			device.Load();

			ScreenResolution = string.Format(CultureInfo.InvariantCulture, "{0}x{1}", device.ScreenResolution.Width, device.ScreenResolution.Height);

			RaisePropertyChanged("ScreenResolution");
		}

		private static long BytesAsMegaBytes(long bytes)
		{
			return bytes / 1048576;
		}
	}
}
