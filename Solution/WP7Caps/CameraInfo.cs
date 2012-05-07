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

		public static bool HasPrimaryCamera { get { return Camera.IsCameraTypeSupported(CameraType.Primary); } }
		public static bool HasFrontFacingCamera { get { return Camera.IsCameraTypeSupported(CameraType.FrontFacing); } }

		public Size CurrentCameraResolution { get; private set; }
		public bool HasFocusAtPoint { get; private set; }
		public bool HasFocus { get; private set; }
		public YCbCrPixelLayout PhotoPixelLayout { get; private set; }
		public IEnumerable<Size> SupportedResolutions { get; private set; }

		// TODO: Do a IsStandardCameraSupported

		public void Load(Dispatcher uiThread)
		{
			this.uiThread = uiThread;

			SetCameraData();
		}

		void CollectCameraCaps(object sender, CameraOperationCompletedEventArgs e)
		{
			CurrentCameraResolution = camera.Resolution;
			HasFocusAtPoint = camera.IsFocusAtPointSupported;
			HasFocus = camera.IsFocusSupported;
			PhotoPixelLayout = camera.YCbCrPixelLayout;
			SupportedResolutions = camera.AvailableResolutions;

			uiThread.BeginInvoke( () =>
			{
				RaisePropertyChanged("CurrentCameraResolution");
				RaisePropertyChanged("HasFocusAtPoint");
				RaisePropertyChanged("HasFocus");
				RaisePropertyChanged("PhotoPixelLayout");
				RaisePropertyChanged("SupportedResolutions");
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
			if (camera == null)
				return;

			camera.Initialized -= CollectCameraCaps;
			camera.Dispose();
			camera = null;
		}
	}
}
