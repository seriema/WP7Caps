using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Microsoft.Devices;

namespace WinPhoneCaps
{
	public class CameraInfo
	{
		public static bool HasPrimaryCamera { get { return Camera.IsCameraTypeSupported(CameraType.Primary); } }
		public static bool HasFrontFacingCamera { get { return Camera.IsCameraTypeSupported(CameraType.FrontFacing); } }

		public Size CurrentCameraResolution { get; private set; }
		public bool HasFocusAtPoint { get; private set; }
		public bool HasFocus { get; private set; }
		public YCbCrPixelLayout PhotoPixelLayout { get; private set; }
		public IEnumerable<Size> SupportedResolutions { get; private set; }

		// TODO: Do a IsStandardCameraSupported

		public void Load()
		{
			SetCameraData();
		}

		void CollectCameraCaps(object sender, CameraOperationCompletedEventArgs e)
		{
			var camera = sender as PhotoCamera;
			if (camera == null)
				return;

			if (camera.CameraType == CameraType.Primary)
			{
				CurrentCameraResolution = camera.Resolution;
				HasFocusAtPoint = camera.IsFocusAtPointSupported;
				HasFocus = camera.IsFocusSupported;
				PhotoPixelLayout = camera.YCbCrPixelLayout;
				SupportedResolutions = camera.AvailableResolutions;
			}

			UninitializeCamera(camera);
		}

		void InitializeCamera(CameraType cameraType)
		{
			// Camera resolution data gathering requires the camera to be initialized
			var camera = new PhotoCamera(cameraType);
			using (var mutex = new ManualResetEvent(false))
			{
				camera.Initialized += (sender, e) =>
				{
				    try
				    {
				        CollectCameraCaps(sender, e);
				    }
				    finally
				    {
				        mutex.Set();
				    }
				};

				var dummyBrush = new VideoBrush();
				dummyBrush.SetSource(camera); // Needed for the camera.Initialized event to fire.

				mutex.WaitOne();
			}
		}

		void SetCameraData()
		{
			if(HasPrimaryCamera)
				InitializeCamera(CameraType.Primary);
			//if(HasFrontFacingCamera)
			//    InitializeCamera(CameraType.FrontFacing);
		}

		void UninitializeCamera(PhotoCamera camera)
		{
			if (camera == null)
				return;

			camera.Initialized -= CollectCameraCaps;
			camera.Dispose();
		}
	}
}
