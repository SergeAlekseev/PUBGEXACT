using client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Action = client.Action;

namespace server
{
	class Controller
	{
		Model model = new Model();
		TcpListener PublicHost; // Это тоже в модел,наверное. И вообще в клиенте тоже много данных из контроллера можно перенести в модел  и обращаться к ним через модел _!__!__!__!_!_!_!__!_!__

		public void start()//Controller
		{
			Thread startThread = new Thread(new ParameterizedThreadStart(StartServer)); 
			startThread.Start();
		}

		public void StartServer(object tmpObject)//Controller
		{
			if (!model.WorkingServer)
			{
				model.Number = -1;
				TcpListener host = new TcpListener(IPAddress.Any, 1337);
				PublicHost = host;
				host.Start();
				model.ListUsers = new List<UserInfo>();
				model.WorkingServer = true;

				//BeginInvoke(new MethodInvoker(delegate
				//{
				//	status.Text = "Сервер включен";
				//}));

				while (model.WorkingServer)
				{
					TcpClient tc = null;
					model.WorkingThread = true;//возможно сделать красивее
					try
					{
						tc = host.AcceptTcpClient();
					}
					catch
					{
						if (tc != null)
							tc.Close();
						break;
					}
					model.Number++;

					lock (model.ListUsers)
					{
						model.ListUsers.Add(new UserInfo(new Point(300, 300))); 
					}
					Thread thread = new Thread(new ParameterizedThreadStart(PlayUser));
					thread.Start(tc);
					Thread thread2 = new Thread(new ParameterizedThreadStart(InfoUsers));
					thread2.Start(tc);
				}
			}
		}

		private void stop_Click(object sender, EventArgs e)//Controller
		{
			if (model.WorkingServer)
			{
				StopServer();
				//BeginInvoke(new MethodInvoker(delegate
				//{
				//	status.Text = "Сервер отключен";
				//}));
			}
		}

		public void StopServer()//Controller
		{
			if (model.WorkingServer)
			{
				model.WorkingServer = false;
				model.WorkingThread = false;
				PublicHost.Stop();
			}
		}

		public void PlayUser(object tc)//Controller
		{
			TcpClient tcp = (TcpClient)tc;
			NetworkStream nStream = tcp.GetStream();
			int num = model.Number;
			int count = 0;
			byte[] countRead = new byte[2];
			bool PrivateWorkingThread = true;

			while (model.WorkingThread && PrivateWorkingThread)
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
							case Action.action.moveUp: if (model.ListUsers[num].userLocation.Y != 0) model.ListUsers[num].userLocation.Y -= 1; break;
							case Action.action.moveDown: if (model.ListUsers[num].userLocation.Y != 600) model.ListUsers[num].userLocation.Y += 1; break;
							case Action.action.noveLeft: if (model.ListUsers[num].userLocation.X != 0) model.ListUsers[num].userLocation.X -= 1; break;
							case Action.action.moveRight: if (model.ListUsers[num].userLocation.X != 600) model.ListUsers[num].userLocation.X += 1; break;
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
				catch (System.IO.IOException )
				{
					lock (model.ListUsers)
					{
						model.ListUsers.RemoveAt(num);
						model.ListUsers.Insert(num, null); // <------------------------ Здесь костыль(вместо каждого удалённого элемента вставляется пустой)
					}
					//Console.WriteLine(err.Message);
					PrivateWorkingThread = false;
				}
			}
		}


		public void InfoUsers(object tc)//Controller
		{
			TcpClient tcp = (TcpClient)tc;
			NetworkStream nStream = tcp.GetStream();
			bool PrivateWorkingThread = true;
			while (model.WorkingThread && PrivateWorkingThread)
			{
				try
				{
					lock (nStream)
					{
						string serialized = "";
						lock (model.ListUsers)
						{
							serialized = JsonConvert.SerializeObject(model.ListUsers);
						}
						byte[] massByts = Encoding.UTF8.GetBytes(serialized);
						byte[] countRead = BitConverter.GetBytes((short)massByts.Count());
						nStream.Write(countRead, 0, 2);//Отпраляет кол-во байт, которое сервер должен будет читать
						nStream.Write(massByts, 0, massByts.Count());
						Thread.Sleep(20);
					}
				}
				catch (System.IO.IOException )
				{
					//Console.WriteLine(err.Message);
					PrivateWorkingThread = false;
				}
				//catch()

			}
		}

		private void Server_FormClosing(object sender, FormClosingEventArgs e)//Controller
		{
			StopServer();
		}
	}
}
