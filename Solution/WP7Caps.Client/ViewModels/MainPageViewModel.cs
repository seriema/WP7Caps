﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Microsoft.Phone.Tasks;

namespace WinPhoneCaps.Client.ViewModels
{
    public class MainPageViewModel
    {
        public MainPageViewModel()
        {
            ComponentsInfo = new ComponentsInfo();
            DeviceInfo = new DeviceInfo();
            NetworkInfo = new NetworkInfo();
        }

        public ComponentsInfo ComponentsInfo { get; private set; }
        public DeviceInfo DeviceInfo { get; private set; }
        public NetworkInfo NetworkInfo { get; private set; }

        // TODO: Is there a way to get rid of the Dispatcher?
        public void Load(Dispatcher uiThread)
        {
            ComponentsInfo.Load(uiThread);
            DeviceInfo.Load();
            NetworkInfo.Load();
        }
		
		public void EmailData()
		{
			var email = new EmailComposeTask();
            email.Subject = "Device data about my WP7 phone";
            email.Body = PhoneDataToString();
            email.Show();
		}

        string PhoneDataToString()
        {
            var data = new StringBuilder();

            var device = DeviceInfo;
            data.AppendLine("[Information]");
            data.AppendLine("Device name: " + device.Name);
            data.AppendLine("Manufacturer: " + device.Manufacturer);
            data.AppendLine("Hardware version: " + device.HardwareVersion);
            data.AppendLine("Firmware version: " + device.FirmwareVersion);
            data.AppendLine("Total memory: " + device.TotalMemory);
            data.AppendLine("Screen resolution: " + device.ScreenResolution);
            data.AppendLine("Has keyboard: " + device.HasKeyboard);
            data.AppendLine("Power source: " + device.PowerSource);
            data.AppendLine();

            var net = NetworkInfo;
            data.AppendLine("[Network]");
            data.AppendLine("Connected to network: " + net.IsConnected);
            data.AppendLine("Connection type: " + net.ConnectionType);
            data.AppendLine("Mobile operator: " + net.MobileOperator);
            data.AppendLine("Cellular data connection enabled: " + net.IsCellularDataEnabled);
            data.AppendLine("Cellular data roaming enabled: " + net.IsCellularDataRoamingEnabled);
            data.AppendLine("Wifi enabled: " + net.IsWifiEnabled);
            data.AppendLine();

            var comp = ComponentsInfo;
            data.AppendLine("[Capabilities]");
            data.AppendLine("Supports gyroscope: " + comp.IsGyroSupported);
            data.AppendLine("Supports accelerometer: " + comp.IsAccelerometerSupported);
            data.AppendLine("Supports compass: " + comp.IsCompassSupported);
            data.AppendLine("Supports motion: " + comp.IsMotionSupported);
            data.AppendLine("Supports multi-resolution video: " + comp.IsMultiResolutionVideoSupported);
            data.AppendLine("Supported camera resolutions: " + StringCollectionToString(comp.SupportedResolutions));
            data.AppendLine("Supports front facing camera: " + comp.IsFrontFacingCameraSupported);
            data.AppendLine("Location data: " + StringCollectionToString(comp.LocationData));
            data.AppendLine();

            return data.ToString();
        }

        static string StringCollectionToString(IEnumerable<string> collection)
        {
            if (collection == null || collection.Count() == 0)
                return "No data";

            return string.Join(", ", collection);
        }
    }
}
