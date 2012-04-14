using System;
using System.Device.Location;
using System.Windows.Threading;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Info;

namespace WinPhoneCaps
{
	public class ComponentsInfo : NotifyPropertyChangedBase
	{
		Dispatcher uiThread;
		GeoCoordinateWatcher watcher;

		public ComponentsInfo()
		{
		}

		public static bool IsAccelerometerSupported { get { return Accelerometer.IsSupported; } }
		public static bool IsCompassSupported { get { return Compass.IsSupported; } }
		public static bool IsGyroSupported { get { return Gyroscope.IsSupported; } }
		public static bool IsMotionSupported { get { return Motion.IsSupported; } }
		public static bool IsMultiResolutionVideoSupported { get { return MediaCapabilities.IsMultiResolutionVideoSupported; } }

		public Location LocationData { get; private set; }

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

		public void Load(Dispatcher uiThread)
		{
			this.uiThread = uiThread;

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
			}

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
