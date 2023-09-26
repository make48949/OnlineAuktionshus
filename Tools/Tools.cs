using System.Net.Sockets;
using System.Net;

public class Tools {

	public enum State 
	{
		CLOSESERVER,
		EXIT,
		RUNNING,
		OK
	}
	static public IPAddress GetIPAddress() 
	{
		Dictionary<String, IPAddress> menuContent = new Dictionary<string, IPAddress>();
		IPAddress? ipAddr = null;

		try {
			foreach (IPAddress ipa in Dns.GetHostEntry(Dns.GetHostName()).AddressList) 
			{
				if (ipa.AddressFamily == AddressFamily.InterNetwork) 
				{ //aka IPv4 address
					menuContent.Add(ipa.ToString(), ipa);
				}
			}
		}
		catch (Exception ex) 
		{
			Console.WriteLine(ex.ToString());
		}


		int elemCount = menuContent.Count;
		if (elemCount == 1) {//if only 1 ip-address: simply choose it!
			ipAddr = menuContent.Values.ElementAt(0);
		}
		else 
		{
			if (elemCount >= 9) 
			{
				throw new Exception("Maximum 9 ip-adresses allowed");
			}
			else {
				if (elemCount == 0) 
				{
					throw new Exception("At least 1 ip-adresses must be present");
				}
			}

			do {
				Console.WriteLine("\tMENU\n");
				int idx = 1;
				foreach (String label in menuContent.Keys) 
				{
					Console.WriteLine($"[{idx++}] \t {label}");
				}
				Console.Write("\nEnter choice : ");

				if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int choice)) {
					ipAddr = menuContent.Values.ElementAt(choice - 1);
				}
				else 
				{
					Console.WriteLine("Input not valid!\n");
				}
			} while (ipAddr == null);
		}
		Console.Clear();

		return ipAddr;
	}
}
