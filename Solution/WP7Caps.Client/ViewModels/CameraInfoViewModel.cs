using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Devices;

namespace WinPhoneCaps.Client.ViewModels
{
	public class CameraInfoViewModel
	{
		public CameraInfoViewModel()
		{
		}

		public string CurrentCameraResolution { get; private set; }
		public bool IsFocusAtPointSupported { get; private set; }
		public bool IsFocusSupported { get; private set; }
		public bool IsFrontFacingCameraSupported { get; private set; }
		public IEnumerable<string> SupportedResolutions { get; private set; }
		public IEnumerable<string> PhotoPixelLayout { get; private set; }

		public void Load(Dispatcher uiThread)
		{
			var cam = new CameraInfo();
			cam.Load(uiThread);

			IsFrontFacingCameraSupported = cam.IsFrontFacingCameraSupported;

			// Have to wait for event to kick off
			SupportedResolutions = GetSupportedResolutions(cam.SupportedResolutions);
			IsFocusAtPointSupported = cam.IsFocusAtPointSupported;
			IsFocusSupported = cam.IsFocusSupported;
			CurrentCameraResolution = SizeAsString(cam.CurrentCameraResolution);
			PhotoPixelLayout = GetPixelLayoutStrings(cam.PhotoPixelLayout);
		}

		private static IEnumerable<string> GetPixelLayoutStrings(YCbCrPixelLayout pixelLayout)
		{
			var strings = new List<string>();

			strings.Add("Cb Offset: " + pixelLayout.CbOffset);
			strings.Add("Cb Pitch: " + pixelLayout.CbPitch);
			strings.Add("Cb X Pitch: " + pixelLayout.CbXPitch);
			strings.Add("Cr Offset: " + pixelLayout.CrOffset);
			strings.Add("Cr Pitch: " + pixelLayout.CrPitch);
			strings.Add("Cr X Pitch: " + pixelLayout.CrXPitch);

			return strings;
		}

		private static IEnumerable<string> GetSupportedResolutions(IEnumerable<Size> availableResolutions)
		{
			var resolutions = new List<string>();

			foreach (var resolution in availableResolutions)
				resolutions.Add(SizeAsString(resolution));

			return resolutions;
		}

		private static string SizeAsString(Size resolution)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}x{1}", resolution.Width, resolution.Height);
		}
	}
}
