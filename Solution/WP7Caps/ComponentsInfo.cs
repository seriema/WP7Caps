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

        public Size CurrentCameraResolution { get; private set; }
        public Location LocationData { get; private set; }
        public bool IsAccelerometerSupported { get; private set; }
        public bool IsCompassSupported { get; private set; }
        public bool IsFocusAtPointSupported { get; private set; }
        public bool IsFocusSupported { get; private set; }
        public bool IsFrontFacingCameraSupported { get; private set; }
        public bool IsGyroSupported { get; private set; }
        public bool IsMotionSupported { get; private set; }
        public bool IsMultiResolutionVideoSupported { get; private set; }
        public IEnumerable<Size> SupportedResolutions { get; private set; }
        public YCbCrPixelLayout PhotoPixelLayout { get; private set; }

        GeoCoordinateWatcher watcher;
        PhotoCamera camera;
        Dispatcher uiThread;

        public void Load(Dispatcher uiThread)
        {
            this.uiThread = uiThread;

            IsAccelerometerSupported = Accelerometer.IsSupported;
            IsCompassSupported = Compass.IsSupported;
            IsFrontFacingCameraSupported = Camera.IsCameraTypeSupported(CameraType.FrontFacing);
            IsGyroSupported = Gyroscope.IsSupported;
            IsMotionSupported = Motion.IsSupported;
            IsMultiResolutionVideoSupported = MediaCapabilities.IsMultiResolutionVideoSupported;

            RaisePropertyChanged("IsAccelerometerSupported");
            RaisePropertyChanged("IsCompassSupported");
            RaisePropertyChanged("IsFrontFacingCameraSupported");
            RaisePropertyChanged("IsGyroSupported");
            RaisePropertyChanged("IsMotionSupported");
            RaisePropertyChanged("IsMultiResolutionVideoSupported");

            SetCameraData();
            SetLocationData();
        }

        void CollectCameraCaps(object sender, CameraOperationCompletedEventArgs e)
        {
            SupportedResolutions = camera.AvailableResolutions;
            IsFocusAtPointSupported = camera.IsFocusAtPointSupported;
            IsFocusSupported = camera.IsFocusSupported;
            CurrentCameraResolution = camera.Resolution;
            PhotoPixelLayout = camera.YCbCrPixelLayout;

            uiThread.BeginInvoke(delegate()
            {
                RaisePropertyChanged("SupportedResolutions");
                RaisePropertyChanged("IsFocusAtPointSupported");
                RaisePropertyChanged("IsFocusSupported");
                RaisePropertyChanged("CurrentCameraResolution");
                RaisePropertyChanged("PhotoPixelLayout");
            });

            UninitializeCamera();
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

        void SetCameraData()
        {
            // Camera resolution data gathering requires the camera to be initialized
            camera = new PhotoCamera(CameraType.Primary);
            camera.Initialized += CollectCameraCaps;
            var dummyBrush = new VideoBrush();
            dummyBrush.SetSource(camera); // Needed for the camera.Initialized event to fire.
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

        void UninitializeCamera()
        {
            if (camera != null)
            {
                camera.Initialized -= CollectCameraCaps;
                camera.Dispose();
                camera = null;
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
