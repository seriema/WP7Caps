using Microsoft.Phone.Net.NetworkInformation;

namespace WinPhoneCaps.Client.ViewModels
{
	public class NetworkInfoViewModel
	{
		public NetworkInfoViewModel()
		{
			var net = new NetworkInfo();

			ConnectionType = GetInterfaceTypeString(net.ConnectionType);
			IsCellularDataEnabled = net.IsCellularDataEnabled;
			IsCellularDataRoamingEnabled = net.IsCellularDataRoamingEnabled;
			IsConnected = net.IsConnected;
			IsWifiEnabled = net.IsWifiEnabled;
			MobileOperator = net.MobileOperator;
			if (string.IsNullOrEmpty(MobileOperator))
				MobileOperator = "N/A";
		}

		public string ConnectionType { get; private set; }
		public bool IsCellularDataEnabled { get; private set; }
		public bool IsCellularDataRoamingEnabled { get; private set; }
		public bool IsConnected { get; private set; }
		public bool IsWifiEnabled { get; private set; }
		public string MobileOperator { get; private set; }

		public void Load()
		{
		}

		private static string GetInterfaceTypeString(NetworkInterfaceType networkInterfaceType)
		{
			switch (networkInterfaceType)
			{
				case NetworkInterfaceType.AsymmetricDsl:
					return "Asymmetric DSL";
				case NetworkInterfaceType.Atm:
					return "Atm";
				case NetworkInterfaceType.BasicIsdn:
					return "Basic ISDN";
				case NetworkInterfaceType.Ethernet:
					return "Ethernet";
				case NetworkInterfaceType.Ethernet3Megabit:
					return "3 Mbit Ethernet";
				case NetworkInterfaceType.FastEthernetFx:
					return "Fast Ethernet";
				case NetworkInterfaceType.FastEthernetT:
					return "Fast Ethernet";
				case NetworkInterfaceType.Fddi:
					return "FDDI";
				case NetworkInterfaceType.GenericModem:
					return "Generic Modem";
				case NetworkInterfaceType.GigabitEthernet:
					return "Gigabit Ethernet";
				case NetworkInterfaceType.HighPerformanceSerialBus:
					return "High Performance Serial Bus";
				case NetworkInterfaceType.IPOverAtm:
					return "IP Over Atm";
				case NetworkInterfaceType.Isdn:
					return "ISDN";
				case NetworkInterfaceType.Loopback:
					return "Loopback";
				case NetworkInterfaceType.MobileBroadbandCdma:
					return "CDMA Broadband Connection";
				case NetworkInterfaceType.MobileBroadbandGsm:
					return "GSM Broadband Connection";
				case NetworkInterfaceType.MultiRateSymmetricDsl:
					return "Multi-Rate Symmetrical DSL";
				case NetworkInterfaceType.None:
					return "None";
				case NetworkInterfaceType.Ppp:
					return "PPP";
				case NetworkInterfaceType.PrimaryIsdn:
					return "Primary ISDN";
				case NetworkInterfaceType.RateAdaptDsl:
					return "Rate Adapt DSL";
				case NetworkInterfaceType.Slip:
					return "Slip";
				case NetworkInterfaceType.SymmetricDsl:
					return "Symmetric DSL";
				case NetworkInterfaceType.TokenRing:
					return "Token Ring";
				case NetworkInterfaceType.Tunnel:
					return "Tunnel";
				case NetworkInterfaceType.Unknown:
					return "Unknown";
				case NetworkInterfaceType.VeryHighSpeedDsl:
					return "Very High Speed DSL";
				case NetworkInterfaceType.Wireless80211:
					return "Wireless";
				default:
					return "Classified";
			}
		}
	}
}
