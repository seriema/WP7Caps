using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Devices;
using System.Globalization;

namespace WinPhoneCaps.Client.ViewModels
{
    public class ComponentsInfoViewModel
    {
        public ComponentsInfoViewModel()
        {
        }

        public IEnumerable<string> LocationData { get; private set; }
        public bool IsAccelerometerSupported { get; private set; }
        public bool IsCompassSupported { get; private set; }
        public bool IsGyroSupported { get; private set; }
        public bool IsMotionSupported { get; private set; }
        public bool IsMultiResolutionVideoSupported { get; private set; }

        public void Load(Dispatcher uiThread)
        {
            var comp = new ComponentsInfo();
            comp.Load(uiThread);

            IsAccelerometerSupported = comp.IsAccelerometerSupported;
            IsCompassSupported = comp.IsCompassSupported;
            IsGyroSupported = comp.IsGyroSupported;
            IsMotionSupported = comp.IsMotionSupported;
            IsMultiResolutionVideoSupported = comp.IsMultiResolutionVideoSupported;

            // Have to wait for event to kick off
            LocationData = GetLocationStrings(comp.LocationData);
        }

        private static string DoubleAsFriendlyString(double d)
        {
            return double.IsNaN(d) ? "Not available" : d.ToString(CultureInfo.InvariantCulture);
        }

        private static IEnumerable<string> GetLocationStrings(ComponentsInfo.Location location)
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
    }
}
