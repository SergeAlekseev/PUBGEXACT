using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using client;
using System.Threading;
using Action = client.Action;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;

namespace server
{
	public partial class Server : Form
	{
		static private List<UserInfo> listUsers;
		static public int number;
		bool workingThread;
		bool workingServer;
		TcpListener PublicHost;

		public Server()
		{
			InitializeComponent();
		}

		private void start_Click(object sender, EventArgs e)
		{
			Thread startThread = new Thread(new ParameterizedThreadStart(StartServer)); //Добавил всё в отдельный поток, чтобы можно было работать с формой, пока сервер робит(без этого форма зависает)
			startThread.Start();
		}

		public void StartServer(object tmpObject)
		{
			if (!workingServer)
			{
				number = -1;
				TcpListener host = new TcpListener(IPAddress.Any, 1337);
				PublicHost = host;
				host.Start();
				listUsers = new List<UserInfo>();
				workingServer = true;

				BeginInvoke(new MethodInvoker(delegate
				{
					status.Text = "Сервер включен";
				}));

				while (workingServer)
				{
					TcpClient tc = null;
					workingThread = true;//возможно сделать красивее
					try
					{
						tc = host.AcceptTcpClient();
					}
					catch
					{
						if(tc!=null)
						tc.Close();
						break;
					}
					number++;

					lock (listUsers)
					{
						listUsers.Add(new UserInfo(new Point(300, 300)));
					}
					Thread thread = new Thread(new ParameterizedThreadStart(PlayUser));
					thread.Start(tc);
					Thread thread2 = new Thread(new ParameterizedThreadStart(InfoUsers));
					thread2.Start(tc);
				}
			}
		}
		private void stop_Click(object sender, EventArgs e)
		{
			if (workingServer)
			{
				StopServer();
				BeginInvoke(new MethodInvoker(delegate
				{
					status.Text = "Сервер отключен";
				}));
			}
		}

		public void StopServer()
		{
			if (workingServer)
			{
				workingServer = false;
				workingThread = false;
				PublicHost.Stop();
			}
		}

		public void PlayUser(object tc)
		{
			TcpClient tcp = (TcpClient)tc;
			NetworkStream nStream = tcp.GetStream();
			int num = number;
			int count = 0;
			byte[] countRead = new byte[2];
			bool PrivateWorkingThread = true;

			while (workingThread && PrivateWorkingThread)
			{
				try
				{
					count = 0;
					while (count != 2)
						count += nStream.Read(countRead, count, countRead.Count() - count);

					count = 0;

					short count2 = BitConverter.ToInt16(countRead, 0);

					if (count2 != -1)
					{

						byte[] readBytes = new byte[count2];


						while (count != count2)
							count += nStream.Read(readBytes, count, readBytes.Count() - count);

						string tmpString = System.Text.Encoding.UTF8.GetString(readBytes);
						Action act = JsonConvert.DeserializeObject<Action>(tmpString);
						switch (act.act)
						{
							case Action.action.moveUp: if (listUsers[num].userLocation.Y != 0) listUsers[num].userLocation.Y -= 5; break;
							case Action.action.moveDown: if (listUsers[num].userLocation.Y != 600) listUsers[num].userLocation.Y += 5; break;
							case Action.action.noveLeft: if (listUsers[num].userLocation.X != 0) listUsers[num].userLocation.X -= 5; break;
							case Action.action.moveRight: if (listUsers[num].userLocation.X != 600) listUsers[num].userLocation.X += 5; break;
						}
					}
					else //ping пока будет работать так, потом следует после количества посылать вид команды
					{
						byte[] ping = BitConverter.GetBytes(-1);
						lock (nStream)
						{
							nStream.Write(ping, 0, 2);
						}
					}
				}
				catch (System.IO.IOException err)
				{
					lock (listUsers)
					{
						listUsers.RemoveAt(num);
						listUsers.Insert(num, null); // <------------------------ Здесь костыль(вместо каждого удалённого элемента вставляется пустой)
					}
					//Console.WriteLine(err.Message);
					PrivateWorkingThread = false;
				}
			}
		}


		public void InfoUsers(object tc)
		{
			TcpClient tcp = (TcpClient)tc;
			NetworkStream nStream = tcp.GetStream();
			bool PrivateWorkingThread = true;
			while (workingThread && PrivateWorkingThread)
			{
				try
				{
					lock (nStream)
					{
						string serialized = "";
						lock (listUsers)
						{
							serialized = JsonConvert.SerializeObject(listUsers);
						}
						byte[] massByts = Encoding.UTF8.GetBytes(serialized);
						byte[] countRead = BitConverter.GetBytes((short)massByts.Count());
						nStream.Write(countRead, 0, 2);//Отпраляет кол-во байт, которое сервер должен будет читать
						nStream.Write(massByts, 0, massByts.Count());
						Thread.Sleep(20);
					}
				}
				catch (System.IO.IOException err)
				{
					//Console.WriteLine(err.Message);
					PrivateWorkingThread = false;
				}
				//catch()

			}
		}

		private void Server_FormClosing(object sender, FormClosingEventArgs e)//
		{
			StopServer();
		}
	}
}
