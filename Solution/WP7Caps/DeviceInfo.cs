using System;
using System.Windows;
using Microsoft.Phone.Info;

namespace WinPhoneCaps
{
	/// <summary>
	/// Class to get device information about the current device.
	/// </summary>
	public class DeviceInfo : NotifyPropertyChangedBase
	{
		/// <summary>Basic constructor added so the class can be used as ViewModel</summary>
		public DeviceInfo()
		{
		}

		/// <summary>Returns the firmware version running on the device.</summary>
		public static string FirmwareVersion { get { return DeviceStatus.DeviceFirmwareVersion; } }

		/// <summary>Returns the hardware version running on the device.</summary>
		public static string HardwareVersion { get { return DeviceStatus.DeviceHardwareVersion; } }

		/// <summary>Indicates whether the device contains a physical hardware keyboard.</summary>
		/// <returns>true if the device contains a physical hardware keyboard; otherwise, false.</returns>
		public static bool HasKeyboard { get { return DeviceStatus.IsKeyboardPresent; } }

		/// <summary>Returns the device manufacturer name.</summary>
		/// <remarks>A standard format is not enforced, and this value may differ from device to device. This value may be empty.</remarks>
		public static string Manufacturer { get { return DeviceStatus.DeviceManufacturer; } }

		/// <summary>Returns the device name.</summary>
		/// <remarks>There is no standard format for this string. This value may be empty.</remarks>
		public static string Name { get { return DeviceStatus.DeviceName; } }

		/// <summary>Indicates whether the device is currently running on battery power or is plugged in to an external power supply.</summary>
		public static PowerSource PowerSource { get { return DeviceStatus.PowerSource; } }

		/// <summary>Represents information about an operating system, such as the version and platform identifier.</summary>
		public static OperatingSystem OsVersion { get { return Environment.OSVersion; } }

		/// <summary>Returns the physical RAM size of the device in bytes.</summary>
		/// <remarks>The value returned is less than the actual amount of device memory, but can be used to help determine memory consumption requirements.</remarks>
		public static long TotalMemory { get { return DeviceStatus.DeviceTotalMemory; } }

		/// <summary>Gets the browser-determined width and height of the Silverlight plug-in content area.</summary>
		/// <returns>The browser-determined width and height, in pixels, of the Silverlight plug-in content area.
		/// The default value is the width and height of the Silverlight plug-in, as specified by the HTML object element that instantiated it.</returns>
		public Size ScreenResolution { get; private set; }

		// TODO: Add System.Environment.OsVersion

		/// <summary>Screen resolution can only be found after Resized-event on app.</summary>
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
