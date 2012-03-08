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
        public ComponentsInfo()
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
            SupportedResolutions = GetSupportedResolution(camera.AvailableResolutions);
            IsFocusAtPointSupported = camera.IsFocusAtPointSupported;
            IsFocusSupported = camera.IsFocusSupported;
            CurrentCameraResolution = SizeAsString(camera.Resolution);
            PhotoPixelLayout = GetPixelLayoutString(camera.YCbCrPixelLayout);

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
            var locationData = new List<string>();

            if (e.Position.Location.IsUnknown)
            {
                locationData.Add("Location unknown");
            }
            else
            {
                locationData.Add("Latitude: " + DoubleAsFriendlyString(e.Position.Location.Latitude));
                locationData.Add("Longitude: " + DoubleAsFriendlyString(e.Position.Location.Longitude));
                locationData.Add("Altitude: " + DoubleAsFriendlyString(e.Position.Location.Altitude));
                locationData.Add("Course: " + DoubleAsFriendlyString(e.Position.Location.Course));
                locationData.Add("Speed: " + DoubleAsFriendlyString(e.Position.Location.Speed));
            }

            LocationData = locationData;
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
                LocationData = new[] { "Permission denied by user" };
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

        static string DoubleAsFriendlyString(double d)
        {
            return double.IsNaN(d) ? "Not available" : d.ToString();
        }

        static IEnumerable<string> GetPixelLayoutString(YCbCrPixelLayout yCbCrPixelLayout)
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

        static IEnumerable<string> GetSupportedResolution(IEnumerable<Size> availableResolutions)
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
