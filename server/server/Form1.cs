﻿using System;
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
	public partial class Form1 : Form
	{
		static private List<UserInfo> listUsers;
		static public int number;
		bool workingThread = true;
		bool workingServer = true;
		public Form1()
		{
			InitializeComponent();
		}

		private void start_Click(object sender, EventArgs e)
		{
			Thread startThread = new Thread(new ParameterizedThreadStart(Start)); //Добавил всё в отдельный поток, чтобы можно было работать с формой, пока сервер робит(без этого форма зависает)
			startThread.Start();
		}

		public void Start(object tmpObject)
		{ 
			number = -1;
			TcpListener host = new TcpListener(IPAddress.Any, 1337);
			host.Start();
			listUsers = new List<UserInfo>();
			//status.Text = "Сервер включен"; Потом сделаю, чтобы можно было менять интерфейс формы из других потоков..
			while (workingServer)
			{
				TcpClient tc = host.AcceptTcpClient();
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
		private void stop_Click(object sender, EventArgs e)
		{
			workingServer = false;
			workingThread = false;
			//status.Text = "Сервер отключен";
		}

		public void PlayUser(object tc)
		{
			TcpClient tcp = (TcpClient)tc;
			NetworkStream nStream = tcp.GetStream();
			int num = number;
			int count = 0;
			byte[] countRead = new byte[2];

			while (workingThread)
			{
				try
				{
					count = 0;
					while (count != 2)
						count += nStream.Read(countRead, count, countRead.Count() - count);

					count = 0;

					short count2 = BitConverter.ToInt16(countRead, 0);

					byte[] readBytes = new byte[count2];


					while (count != count2)
						count += nStream.Read(readBytes, count, readBytes.Count() - count);

					string tmpString = System.Text.Encoding.UTF8.GetString(readBytes);
					Action act = JsonConvert.DeserializeObject<Action>(tmpString);
					switch (act.act)
					{
						case Action.action.moveUp: if (listUsers[num].userLocation.Y != 0) listUsers[num].userLocation.Y -= 1; break;
						case Action.action.moveDown: if (listUsers[num].userLocation.Y != 600) listUsers[num].userLocation.Y += 1; break;
						case Action.action.noveLeft: if (listUsers[num].userLocation.X != 0) listUsers[num].userLocation.X -= 1; break;
						case Action.action.moveRight: if (listUsers[num].userLocation.X != 600) listUsers[num].userLocation.X += 1; break;
					}
				}
				catch (System.IO.IOException err)
				{
					lock (listUsers)
					{
						listUsers.RemoveAt(num);
						listUsers.Insert(num, null); // <------------------------ Здесь костыль(вместо каждого удалённого элемента вставляется пустой)
					}
					Console.WriteLine(err.Message);
					workingThread = false;
				}
			}
		}


		public void InfoUsers(object tc)
		{
			TcpClient tcp = (TcpClient)tc;
			NetworkStream nStream = tcp.GetStream();
			bool workingThread = true;
			while (workingThread)
			{
				try
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
				catch (System.IO.IOException err)
				{
					Console.WriteLine(err.Message);
					workingThread = false;
				}
				//catch()

			}
		}
		
	}
}
