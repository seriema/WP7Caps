using System.Collections.Generic;
using System.Device.Location;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Devices;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Info;

namespace WinPhoneCaps
{
    public class ComponentsInfo : NotifyPropertyChangedBase
    {
        // Created this class so lib-user doesn't need to include a reference
        // to a whole new DLL just to get this info.
        public class Location
        {
            public double Altitude { get; set; }
            public double Course { get; set; }
            public bool HasPermission { get; set; }
            public bool IsUnknown { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public double Speed { get; set; }
        }

        public ComponentsInfo()
        {
        }

        public Location LocationData { get; private set; }
        public bool IsAccelerometerSupported { get; private set; }
        public bool IsCompassSupported { get; private set; }
        public bool IsGyroSupported { get; private set; }
        public bool IsMotionSupported { get; private set; }
        public bool IsMultiResolutionVideoSupported { get; private set; }

        GeoCoordinateWatcher watcher;
        Dispatcher uiThread;

        public void Load(Dispatcher uiThread)
        {
            this.uiThread = uiThread;

            IsAccelerometerSupported = Accelerometer.IsSupported;
            IsCompassSupported = Compass.IsSupported;
            IsGyroSupported = Gyroscope.IsSupported;
            IsMotionSupported = Motion.IsSupported;
            IsMultiResolutionVideoSupported = MediaCapabilities.IsMultiResolutionVideoSupported;

            RaisePropertyChanged("IsAccelerometerSupported");
            RaisePropertyChanged("IsCompassSupported");
            RaisePropertyChanged("IsGyroSupported");
            RaisePropertyChanged("IsMotionSupported");
            RaisePropertyChanged("IsMultiResolutionVideoSupported");

            SetLocationData();
        }

        void CollectLocationData(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            LocationData = new Location
            {
                Altitude = e.Position.Location.Altitude,
                Course = e.Position.Location.Course,
                HasPermission = true,
                IsUnknown = e.Position.Location.IsUnknown,
                Latitude = e.Position.Location.Latitude,
                Longitude = e.Position.Location.Longitude,
                Speed = e.Position.Location.Speed
            };
            uiThread.BeginInvoke(delegate()
            {
                RaisePropertyChanged("LocationData");
            });

            UninitializeGeoCoordinateWatcher();
        }

        void SetLocationData()
        {
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);

            if (watcher.Permission == GeoPositionPermission.Denied)
            {
                LocationData = new Location { HasPermission = false };
                RaisePropertyChanged("LocationData");
            }
            else
            {
                watcher.MovementThreshold = 1;
                watcher.PositionChanged += CollectLocationData;
                watcher.Start();
            }
        }

        void UninitializeGeoCoordinateWatcher()
        {
            if (watcher != null)
            {
                watcher.Stop();
                watcher.PositionChanged -= CollectLocationData;
                watcher.Dispose();
                watcher = null;
            }
        }
    }
}
