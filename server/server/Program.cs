using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Drawing;
using System.Net.Sockets;
using System.Net;
using client;
using System.Threading;
using Newtonsoft.Json;
using Action = client.Action;

namespace server
{
	class Program
	{
		static public List<UserInfo> listUsers;

		static public int number;

		static void Main(string[] args)
		{
			number = 0;
			TcpListener host = new TcpListener(IPAddress.Any, 8002);
			host.Start();
			
			
			while (true)
			{
				TcpClient tc = host.AcceptTcpClient();
				Thread thread = new Thread(new ParameterizedThreadStart(PlayUser));
				thread.Start(tc);
				Thread thread2 = new Thread(new ParameterizedThreadStart(InfoUsers));
				thread.Start(tc);
				listUsers.Add(new UserInfo(new Point(300, 300)));
			}

		}

		static void PlayUser(object tc)
		{
			NetworkStream nStream = (NetworkStream)tc;
			int num = number;
			int count = 0;
			byte[] countRead = new byte[2];
			while (true)
			{
				count = 0;
				while (count != 2)
					count += nStream.Read(countRead, count, countRead.Count() - count);

				count = 0;

				short count2 = BitConverter.ToInt16(countRead, 0);
				count2 = IPAddress.NetworkToHostOrder(count2);
				byte[] readBytes = new byte[count2];


				while (count != count2)
					count += nStream.Read(readBytes, count, readBytes.Count() - count);

				string tmpString = System.Text.Encoding.UTF8.GetString(readBytes);
				Action act  = JsonConvert.DeserializeObject<Action>(tmpString);
				switch (act.act)
				{
					case Action.action.moveUp: if (listUsers[num].userLocation.Y != 0) listUsers[num].userLocation.Y -= 1;  break;
					case Action.action.moveDown: if (listUsers[num].userLocation.Y != 600) listUsers[num].userLocation.Y += 1; break;
					case Action.action.noveLeft: if (listUsers[num].userLocation.X != 0) listUsers[num].userLocation.X -= 1; break;
					case Action.action.moveRight: if (listUsers[num].userLocation.X != 600) listUsers[num].userLocation.X += 1; break;
				}
			}
		}	
		

		static void InfoUsers(object tc)
		{
			NetworkStream nStream = (NetworkStream)tc;
			string serialized = JsonConvert.SerializeObject(listUsers);
			byte[] massByts = Encoding.UTF8.GetBytes(serialized);
			byte[] countRead = BitConverter.GetBytes((short)massByts.Count());
			nStream.Write(countRead, 0, 2);//Отпраляет кол-во байт, которое сервер должен будет читать
			nStream.Write(massByts, 0, massByts.Count());
		}


	}
}
