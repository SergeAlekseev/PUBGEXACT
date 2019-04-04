﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace client
{
	class Controller
	{

		public delegate void CloseFormD(object sender, FormClosingEventArgs e);
		public event CloseFormD CloseFormEvent;

		public delegate void CloseD();
		public event CloseD CloseEvent;

		Model model;

		TcpClient client;// 25.46.244.0 
		NetworkStream nStream;
		Thread threadReading;

		Stopwatch PingWatch = new Stopwatch();
		bool threadStart = false;

		System.Timers.Timer timerPing = new System.Timers.Timer();

		public void Shot(byte type)
		{
			if (threadStart)
			{
				Writing(null, type);
			}
		}

		private void Writing(object obj, byte numComand)
		{
			string serialized = JsonConvert.SerializeObject(obj);
			byte[] massByts = Encoding.UTF8.GetBytes(serialized);
			byte[] countRead = BitConverter.GetBytes(massByts.Count());
			byte[] typeComand = new byte[1];
			typeComand[0] = numComand;

			nStream.Write(typeComand, 0, 1);//Отпраляет тип команды
			nStream.Write(countRead, 0, 4);//Отпраляет кол-во байт, которое сервер должен будет читать
			nStream.Write(massByts, 0, massByts.Count());
		}

		public void WriteMouseLocation(Point mouseLocation)
		{
			Writing(mouseLocation,5);
		}

		public Controller(Model model) // Конструктор
		{
			this.model = model;
			timerPing.Elapsed += timerPing_Tick;
			timerPing.Interval = 2000;// Controller
			timerPing.Start();// Controller
		}

		private void timerPing_Tick(object sender, EventArgs e) // Controller
		{
			if (threadStart)
			{
				if (PingWatch.ElapsedMilliseconds > 4000)
				{
					PingWatch.Stop();
					CloseFormEvent(null, null);
				}
				else
				{
					PingWatch = new Stopwatch();
					byte[] Ping = BitConverter.GetBytes((short)2);
					nStream.Write(Ping, 0, 2);
					PingWatch.Start();
				}
			}

		}

		public string Reading(NetworkStream nStream)
		{
			byte[] countRead = new byte[4];
			int countReadingBytes = 0;
			while (countReadingBytes != 4)
				countReadingBytes += nStream.Read(countRead, countReadingBytes, countRead.Count() - countReadingBytes);

			countReadingBytes = 0;

			int lengthBytesRaed = BitConverter.ToInt32(countRead, 0);

			byte[] readBytes = new byte[lengthBytesRaed];


			while (countReadingBytes != lengthBytesRaed)
				countReadingBytes += nStream.Read(readBytes, countReadingBytes, readBytes.Count() - countReadingBytes);

			string tmpString = System.Text.Encoding.UTF8.GetString(readBytes);

			return tmpString;
		}

		private void ReadingStream()// Controller
		{
			while (threadStart)
			{

				byte[] typeCommand = new byte[1];
				nStream.Read(typeCommand, 0, 1);

				switch (typeCommand[0])
				{
					case 1:
						{
							string tmpString = Reading(nStream);
							model.ListUsers = JsonConvert.DeserializeObject<List<UserInfo>>(tmpString);
							model.ListUsers[model.ThisUser.userNumber].userNumber = model.ThisUser.userNumber;
							model.ThisUser = model.ListUsers[model.ThisUser.userNumber];
							break;
						}
					case 2:
						{
							PingWatch.Stop();
							model.Ping = (int)PingWatch.ElapsedMilliseconds;
							break;
						}
					case 3:
						{
							string tmpString = Reading(nStream);
							model.ListBullet = JsonConvert.DeserializeObject<List<BulletInfo>>(tmpString);
							break;
						}
					case 4:
						{

							break;
						}
					case 5:
						{
							Disconnect();
							break;
						}
				}


			}

		}

		public void PressKey() // Controller
		{
			if (threadStart)
			{
				Writing(model.Action, 1);
			}
		}

		public void Disconnect()// Controller
		{

			if (threadStart)
			{
				nStream.Close();
				threadStart = false;
				client.Close();
				timerPing.Stop();
				CloseEvent();
				threadReading.Abort();
			}
		}

		public void Connect()// Controller
		{
			if (!threadStart)
			{
				try
				{
					client = new TcpClient("25.46.244.0", 1337);

					nStream = client.GetStream();
					threadStart = true;
					threadReading = new Thread(ReadingStream);
					threadReading.Start();
					byte[] number = new byte[1];
					nStream.Read(number, 0, 1);
					model.ThisUser.userNumber = number[0];
					
				}
				catch (System.Net.Sockets.SocketException) //не удалось подключится по заданным параметрам
				{
					CloseEvent();
				}
			}

		}

	}
}
