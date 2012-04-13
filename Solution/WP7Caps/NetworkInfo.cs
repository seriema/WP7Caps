using Microsoft.Phone.Net.NetworkInformation;

namespace WinPhoneCaps
{
	public class NetworkInfo
	{
		/// <summary>Gets the type of the network that is servicing Internet requests.</summary>
		/// <remarks>Checking the network type is not instantaneous, so we recommend that you always do it on a background thread.</remarks>
		public NetworkInterfaceType ConnectionType { get { return NetworkInterface.NetworkInterfaceType; } }

		/// <summary>Gets a value that indicates whether the network is cellular data enabled.</summary>
		/// <returns>true if the network is cellular data enabled; otherwise, false.</returns>
		/// <remarks>The cellular network must have a radio connection, be correctly configured, and be connected.</remarks>
		public bool IsCellularDataEnabled { get { return DeviceNetworkInformation.IsCellularDataEnabled; } }

		/// <summary>Gets a value that indicates whether the network allows data roaming.</summary>
		/// <returns>true if the network allows data roaming; otherwise, false.</returns>
		public bool IsCellularDataRoamingEnabled { get { return DeviceNetworkInformation.IsCellularDataRoamingEnabled; } }

		/// <summary>Gets a value that indicates whether the network is available.</summary>
		/// <returns>true if there is at least one network interface available; otherwise, false.</returns>
		/// <remarks>The network interface must be available and connected.</remarks>
		public bool IsConnected { get { return DeviceNetworkInformation.IsNetworkAvailable; } }

		/// <summary>Gets a value that indicates whether the network is Wi-Fi enabled.</summary>
		/// <returns>true if the network is Wi-Fi enabled; otherwise, false.</returns>
		/// <remarks>Wi-Fi must have a radio connection and be correctly configured.</remarks>
		public bool IsWifiEnabled { get { return DeviceNetworkInformation.IsWiFiEnabled; } }

		/// <summary>Gets the name of the cellular mobile operator.</summary>
		/// <returns>The name of the cellular mobile operator, or a Nothing string.</returns>
		/// <remarks>The name is a Unicode string that uses UTF-8 encoding.</remarks>
		public string MobileOperator { get { return DeviceNetworkInformation.CellularMobileOperator; } }
	}
}
