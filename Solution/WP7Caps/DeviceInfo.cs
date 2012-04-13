using System.Windows;
using Microsoft.Phone.Info;

namespace WinPhoneCaps
{
	public class DeviceInfo : NotifyPropertyChangedBase
	{
		public DeviceInfo()
		{
		}

		public static string FirmwareVersion { get { return DeviceStatus.DeviceFirmwareVersion; } }
		public static string HardwareVersion { get { return DeviceStatus.DeviceHardwareVersion; } }
		public static bool HasKeyboard { get { return DeviceStatus.IsKeyboardPresent; } }
		public static string Manufacturer { get { return DeviceStatus.DeviceManufacturer; } }
		public static string Name { get { return DeviceStatus.DeviceName; } }
		public static PowerSource PowerSource { get { return DeviceStatus.PowerSource; } }
		public static long TotalMemory { get { return DeviceStatus.DeviceTotalMemory; } }

		public Size ScreenResolution { get; private set; }

		// TODO: Add System.Environment.OSVersion

		public void Load()
		{
			// In the early stages of the object lifetime of a Silverlight 
			// plug-in instance, ActualHeight and ActualWidth do not contain 
			// usable values. In particular, the plug-in DOM-level OnLoad event 
			// does not yet guarantee a correct value for ActualHeight and ActualWidth.
			// In general, you should check these values in the handler for the Resized 
			// event, which occurs just after OnLoad.
			ScreenResolution = new Size(Application.Current.Host.Content.ActualWidth, Application.Current.Host.Content.ActualHeight);
			RaisePropertyChanged("ScreenResolution");
		}
	}
}
