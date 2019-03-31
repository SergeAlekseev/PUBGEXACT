using Newtonsoft.Json;
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
				if (PingWatch.ElapsedMilliseconds > 2002)
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

		private void Reading()// Controller
		{
			int countReadingBytes = 0;
			byte[] countRead = new byte[2];

			while (threadStart)
			{

				byte[] typeCommand = new byte[1];
				nStream.Read(typeCommand, 0, 1);

				switch (typeCommand[0])
				{
					case 1:
						{
							countReadingBytes = 0;
							while (countReadingBytes != 2)
								countReadingBytes += nStream.Read(countRead, countReadingBytes, countRead.Count() - countReadingBytes);

							countReadingBytes = 0;

							short lengthBytesRaed = BitConverter.ToInt16(countRead, 0);

							byte[] readBytes = new byte[lengthBytesRaed];


							while (countReadingBytes != lengthBytesRaed)
								countReadingBytes += nStream.Read(readBytes, countReadingBytes, readBytes.Count() - countReadingBytes);

							string tmpString = System.Text.Encoding.UTF8.GetString(readBytes);
							model.ListUsers = JsonConvert.DeserializeObject<List<UserInfo>>(tmpString);

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

							break;
						}
				}


			}

		}

		public void PressKey() // Controller
		{
			if (threadStart)
			{
				string serialized = JsonConvert.SerializeObject(model.Action);
				byte[] massByts = Encoding.UTF8.GetBytes(serialized);
				byte[] countRead = BitConverter.GetBytes((short)massByts.Count());
				byte[] typeComand = new byte[1];
				typeComand[0] = 1;

				nStream.Write(typeComand, 0, 1);//Отпраляет тип команды
				nStream.Write(countRead, 0, 2);//Отпраляет кол-во байт, которое сервер должен будет читать
				nStream.Write(massByts, 0, massByts.Count());
			}
		}

		public void Disconnect()// Controller
		{

			if (threadStart)
			{

				threadReading.Abort();
				nStream.Close();
				client.Close();
				threadStart = false;
				CloseEvent();
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

					threadReading = new Thread(Reading);
					threadReading.Start();
					threadStart = true;
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
