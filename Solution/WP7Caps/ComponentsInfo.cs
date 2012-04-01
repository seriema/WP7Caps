using System;
using System.Device.Location;
using System.Windows.Threading;
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

        Dispatcher uiThread;
        GeoCoordinateWatcher watcher;

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

        bool InitializeGeoCoordinateWatcher()
        {
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);

            if (watcher.Permission == GeoPositionPermission.Denied)
            {
                LocationData = new Location { HasPermission = false };
                RaisePropertyChanged("LocationData");
                return false;
            };

            watcher.MovementThreshold = 0;
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));

            return true;
        }

        void SetLocationData()
        {
            if (!InitializeGeoCoordinateWatcher())
                return;

            LocationData = new Location
            {
                Altitude = watcher.Position.Location.Altitude,
                Course = watcher.Position.Location.Course,
                HasPermission = true,
                IsUnknown = watcher.Position.Location.IsUnknown,
                Latitude = watcher.Position.Location.Latitude,
                Longitude = watcher.Position.Location.Longitude,
                Speed = watcher.Position.Location.Speed
            };
            uiThread.BeginInvoke(delegate
            {
                RaisePropertyChanged("LocationData");
            });

            UninitializeGeoCoordinateWatcher();
        }

        void UninitializeGeoCoordinateWatcher()
        {
            if (watcher != null)
            {
                watcher.Stop();
                watcher.Dispose();
                watcher = null;
            }
        }
    }
}
