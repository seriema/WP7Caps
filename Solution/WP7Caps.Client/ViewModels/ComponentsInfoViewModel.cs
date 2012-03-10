using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Devices;

namespace WinPhoneCaps.Client.ViewModels
{
    public class ComponentsInfoViewModel
    {
        public ComponentsInfoViewModel()
        {
        }

        public string CurrentCameraResolution { get; private set; }
        public IEnumerable<string> LocationData { get; private set; }
        public bool IsAccelerometerSupported { get; private set; }
        public bool IsCompassSupported { get; private set; }
        public bool IsFocusAtPointSupported { get; private set; }
        public bool IsFocusSupported { get; private set; }
        public bool IsFrontFacingCameraSupported { get; private set; }
        public bool IsGyroSupported { get; private set; }
        public bool IsMotionSupported { get; private set; }
        public bool IsMultiResolutionVideoSupported { get; private set; }
        public IEnumerable<string> SupportedResolutions { get; private set; }
        public IEnumerable<string> PhotoPixelLayout { get; private set; }

        public void Load(Dispatcher uiThread)
        {
            var comp = new ComponentsInfo();
            comp.Load(uiThread);

            IsAccelerometerSupported = comp.IsAccelerometerSupported;
            IsCompassSupported = comp.IsCompassSupported;
            IsFrontFacingCameraSupported = comp.IsFrontFacingCameraSupported;
            IsGyroSupported = comp.IsGyroSupported;
            IsMotionSupported = comp.IsMotionSupported;
            IsMultiResolutionVideoSupported = comp.IsMultiResolutionVideoSupported;

            // Have to wait for event to kick off
            LocationData = GetLocationStrings(comp.LocationData);
            SupportedResolutions = GetSupportedResolutions(comp.SupportedResolutions);
            IsFocusAtPointSupported = comp.IsFocusAtPointSupported;
            IsFocusSupported = comp.IsFocusSupported;
            CurrentCameraResolution = SizeAsString(comp.CurrentCameraResolution);
            PhotoPixelLayout = GetPixelLayoutStrings(comp.PhotoPixelLayout);
        }

        static string DoubleAsFriendlyString(double d)
        {
            return double.IsNaN(d) ? "Not available" : d.ToString();
        }

        static IEnumerable<string> GetLocationStrings(ComponentsInfo.Location location)
        {
            // HACK: Temporary copout :)
            if (location == null)
                return null;

            var locationData = new List<string>();

            if (location.HasPermission)
                locationData.Add("Permission denied");
            else if (location.IsUnknown)
                locationData.Add("Location unknown");
            else
            {
                locationData.Add("Latitude: " + DoubleAsFriendlyString(location.Latitude));
                locationData.Add("Longitude: " + DoubleAsFriendlyString(location.Longitude));
                locationData.Add("Altitude: " + DoubleAsFriendlyString(location.Altitude));
                locationData.Add("Course: " + DoubleAsFriendlyString(location.Course));
                locationData.Add("Speed: " + DoubleAsFriendlyString(location.Speed));
            }

            return locationData;
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
