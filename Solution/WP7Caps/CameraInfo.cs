using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Devices;

namespace WinPhoneCaps
{
	public class CameraInfo : NotifyPropertyChangedBase
	{
		PhotoCamera camera;
		Dispatcher uiThread;
		
		public Size CurrentCameraResolution { get; private set; }
		public bool IsFocusAtPointSupported { get; private set; }
		public bool IsFocusSupported { get; private set; }
		public bool IsFrontFacingCameraSupported { get; private set; }
		public IEnumerable<Size> SupportedResolutions { get; private set; }
		public YCbCrPixelLayout PhotoPixelLayout { get; private set; }

		public void Load(Dispatcher uiThread)
		{
			this.uiThread = uiThread;

			IsFrontFacingCameraSupported = Camera.IsCameraTypeSupported(CameraType.FrontFacing);
			RaisePropertyChanged("IsFrontFacingCameraSupported");

			SetCameraData();
		}

		void CollectCameraCaps(object sender, CameraOperationCompletedEventArgs e)
		{
			SupportedResolutions = camera.AvailableResolutions;
			IsFocusAtPointSupported = camera.IsFocusAtPointSupported;
			IsFocusSupported = camera.IsFocusSupported;
			CurrentCameraResolution = camera.Resolution;
			PhotoPixelLayout = camera.YCbCrPixelLayout;

			uiThread.BeginInvoke(delegate
			{
				RaisePropertyChanged("SupportedResolutions");
				RaisePropertyChanged("IsFocusAtPointSupported");
				RaisePropertyChanged("IsFocusSupported");
				RaisePropertyChanged("CurrentCameraResolution");
				RaisePropertyChanged("PhotoPixelLayout");
			});

			UninitializeCamera();
		}

		void InitializeCamera()
		{
			// Camera resolution data gathering requires the camera to be initialized
			camera = new PhotoCamera(CameraType.Primary);
			camera.Initialized += CollectCameraCaps;
			var dummyBrush = new VideoBrush();
			dummyBrush.SetSource(camera); // Needed for the camera.Initialized event to fire.
		}

		void SetCameraData()
		{
			InitializeCamera();
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
	}
}
