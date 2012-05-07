using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Devices;

namespace WinPhoneCaps.Client.ViewModels
{
	public class CameraInfoViewModel : NotifyPropertyChangedBase
	{
		private Dispatcher uiThread;

		public CameraInfoViewModel()
		{
			IsFrontFacingCameraSupported = CameraInfo.HasFrontFacingCamera;
			IsPrimaryCameraSupported = CameraInfo.HasFrontFacingCamera;
		}

		public string CurrentCameraResolution { get; private set; }
		public bool IsFocusAtPointSupported { get; private set; }
		public bool IsFocusSupported { get; private set; }
		public bool IsFrontFacingCameraSupported { get; private set; }
		public bool IsPrimaryCameraSupported { get; private set; }
		public IEnumerable<string> SupportedResolutions { get; private set; }
		public IEnumerable<string> PhotoPixelLayout { get; private set; }

		public void Load(Dispatcher uiThread)
		{
			this.uiThread = uiThread;

			var cam = new CameraInfo();
			cam.Load(uiThread);
			CurrentCameraResolution = SizeToString(cam.CurrentCameraResolution);
			IsFocusAtPointSupported = cam.HasFocusAtPoint;
			IsFocusSupported = cam.HasFocus;
			PhotoPixelLayout = GetPixelLayoutStrings(cam.PhotoPixelLayout);
			SupportedResolutions = GetSupportedResolutions(cam.SupportedResolutions);

			RaisePropertyChanged("CurrentCameraResolution");
			RaisePropertyChanged("IsFocusAtPointSupported");
			RaisePropertyChanged("IsFocusSupported");
			RaisePropertyChanged("PhotoPixelLayout");
			RaisePropertyChanged("SupportedResolutions");
		}

		private static IEnumerable<string> GetPixelLayoutStrings(YCbCrPixelLayout pixelLayout)
		{
			var strings = new List<string>
			{
			    "Cb Offset: " + pixelLayout.CbOffset,
			    "Cb Pitch: " + pixelLayout.CbPitch,
			    "Cb X Pitch: " + pixelLayout.CbXPitch,
			    "Cr Offset: " + pixelLayout.CrOffset,
			    "Cr Pitch: " + pixelLayout.CrPitch,
			    "Cr X Pitch: " + pixelLayout.CrXPitch
			};

			return strings;
		}

		private static IEnumerable<string> GetSupportedResolutions(IEnumerable<Size> availableResolutions)
		{
			return availableResolutions.Select(SizeToString).ToList();
		}

		private static string SizeToString(Size resolution)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}x{1}", resolution.Width, resolution.Height);
		}
	}
}
