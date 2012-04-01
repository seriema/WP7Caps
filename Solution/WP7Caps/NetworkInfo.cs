using Microsoft.Phone.Net.NetworkInformation;

namespace WinPhoneCaps
{
	public class NetworkInfo : NotifyPropertyChangedBase
	{
		public NetworkInfo()
		{
		}

		public NetworkInterfaceType ConnectionType { get; private set; }
		public bool IsCellularDataEnabled { get; private set; }
		public bool IsCellularDataRoamingEnabled { get; private set; }
		public bool IsConnected { get; private set; }
		public bool IsWifiEnabled { get; private set; }
		public string MobileOperator { get; private set; }

		public void Load()
		{
			ConnectionType = NetworkInterface.NetworkInterfaceType;
			IsCellularDataEnabled = DeviceNetworkInformation.IsCellularDataEnabled;
			IsCellularDataRoamingEnabled = DeviceNetworkInformation.IsCellularDataRoamingEnabled;
			IsConnected = DeviceNetworkInformation.IsNetworkAvailable;
			IsWifiEnabled = DeviceNetworkInformation.IsWiFiEnabled;
			MobileOperator = DeviceNetworkInformation.CellularMobileOperator;

			RaisePropertyChanged("ConnectionType");
			RaisePropertyChanged("IsCellularDataEnabled");
			RaisePropertyChanged("IsCellularDataRoamingEnabled");
			RaisePropertyChanged("IsConnected");
			RaisePropertyChanged("IsWifiEnabled");
			RaisePropertyChanged("MobileOperator");
		}
	}
}
