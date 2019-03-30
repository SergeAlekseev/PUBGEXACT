﻿using client;
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
		bool workingThread;
		bool workingServer;
		int number; //Model

		Model model;
		TcpListener PublicHost; // Это тоже в модел,наверное. И вообще в клиенте тоже много данных из контроллера можно перенести в модел  и обращаться к ним через модел _!__!__!__!_!_!_!__!_!__

		public Controller(Model model)
		{
			this.model = model;
		}

		public void start()
		{
			Thread startThread = new Thread(new ParameterizedThreadStart(StartServer)); 
			startThread.Start();
		}

		public void StartServer(object tmpObject)//Controller
		{
			if (!workingServer)
			{
				number = -1;
				TcpListener host = new TcpListener(IPAddress.Any, 1337);
				PublicHost = host;
				host.Start();
				model.ListUsers = new List<UserInfo>();
				workingServer = true;

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
						if (tc != null)
							tc.Close();
						break;
					}
					number++;

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

		public void StopServer()//Controller
		{
			if (workingServer)
			{
				workingServer = false;
				workingThread = false;
				PublicHost.Stop();
			}
		}

		public void PlayUser(object tc)//Controller
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
							case Action.action.moveUp: if (model.ListUsers[num].userLocation.Y != 0) model.ListUsers[num].userLocation.Y -= 1; break;
							case Action.action.moveDown: if (model.ListUsers[num].userLocation.Y != 600) model.ListUsers[num].userLocation.Y += 1; break;
							case Action.action.noveLeft: if (model.ListUsers[num].userLocation.X != 0) model.ListUsers[num].userLocation.X -= 1; break;
							case Action.action.moveRight: if (model.ListUsers[num].userLocation.X != 600) model.ListUsers[num].userLocation.X += 1; break;
						}
					}
					else //ping пока будет работать так, потом следует после количества посылать вид команды или наоборот
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
					PrivateWorkingThread = false;
				}
			}
		}

		private void Server_FormClosing(object sender, FormClosingEventArgs e)
		{
			StopServer();
		}
	}
}
