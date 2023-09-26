using System.Net.Sockets;
using System.Net;
using System.Text;

namespace AuktionsClient {
	public class Client 
	{
		private const int port = 30000;
		static public void Main(string[] args) {
			Client client = new Client();
			client.Run();
		}

		private void Run() 
		{
			Socket server;

			Console.WriteLine("Starting client...");
			IPAddress ipa = Tools.GetIPAddress();
			try {
				server = new Socket(ipa.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint localEndPoint = new IPEndPoint(ipa, port);
				server.Connect(localEndPoint);

				Communicate(server);

				server.Shutdown(SocketShutdown.Both);
				server.Close();
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
		}

		private void Communicate(Socket server) 
		{
			bool hot = true;

			do {
				Console.Write("Enter message to server (Exit to end, Quit to close server): ");
				String? input = Console.ReadLine();

				if (input == null) 
				{
					Console.WriteLine("No input - try harder...");
				} else 
				{
					input = input.ToUpper();
					if (input == "EXIT") {
						input = "C@EXIT#";
						hot = false;
					} else if (input == "QUIT") 
					{
						input = "C@CLOSESERVER#";
						hot = false;
					}
					byte[] messageSent = Encoding.ASCII.GetBytes(input);
					int byteSent = server.Send(messageSent);

					byte[] messageReceived = new byte[1024];

					int byteRecv = server.Receive(messageReceived);
					string answer = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);

					switch (answer) 
					{
						case "OK":
							Console.WriteLine("Server says: Ok");
							break;

						case "BID IS TOO LOW":
							Console.WriteLine("Server says: Bid was too low");
							break;

						default: Console.WriteLine($"{answer} \n"); break;
					}
				}
			} while (hot);
		}
	}
}