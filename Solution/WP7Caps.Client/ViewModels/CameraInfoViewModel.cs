using System.Collections.Generic;
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

        static IEnumerable<string> GetPixelLayoutStrings(YCbCrPixelLayout yCbCrPixelLayout)
        {
            var pixelLayout = new List<string>();

            pixelLayout.Add("Cb Offset: " + yCbCrPixelLayout.CbOffset);
            pixelLayout.Add("Cb Pitch: " + yCbCrPixelLayout.CbPitch);
            pixelLayout.Add("Cb X Pitch: " + yCbCrPixelLayout.CbXPitch);
            pixelLayout.Add("Cr Offset: " + yCbCrPixelLayout.CrOffset);
            pixelLayout.Add("Cr Pitch: " + yCbCrPixelLayout.CrPitch);
            pixelLayout.Add("Cr X Pitch: " + yCbCrPixelLayout.CrXPitch);

            return pixelLayout;
        }

        static IEnumerable<string> GetSupportedResolutions(IEnumerable<Size> availableResolutions)
        {
            var resolutions = new List<string>();

            foreach (var resolution in availableResolutions)
                resolutions.Add(SizeAsString(resolution));

            return resolutions;
        }

        static string SizeAsString(Size resolution)
        {
            return string.Format("{0}x{1}", resolution.Width, resolution.Height);
        }
    }
}
