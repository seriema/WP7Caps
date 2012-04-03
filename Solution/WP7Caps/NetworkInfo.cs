using Microsoft.Phone.Net.NetworkInformation;

namespace WinPhoneCaps
{
	public class NetworkInfo
	{
		public NetworkInterfaceType ConnectionType { get { return NetworkInterface.NetworkInterfaceType; } }
		public bool IsCellularDataEnabled          { get { return DeviceNetworkInformation.IsCellularDataEnabled; } }
		public bool IsCellularDataRoamingEnabled   { get { return DeviceNetworkInformation.IsNetworkAvailable; } }
		public bool IsConnected                    { get { return DeviceNetworkInformation.IsNetworkAvailable; } }
		public bool IsWifiEnabled                  { get { return DeviceNetworkInformation.IsWiFiEnabled; } }
		public string MobileOperator               { get { return DeviceNetworkInformation.CellularMobileOperator; } }
	}
}
