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
		int Ping;
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
				if (PingWatch.ElapsedMilliseconds > 2000)
				{
					PingWatch.Stop();
					CloseFormEvent(null,null);
				}
				else
				{
					PingWatch = new Stopwatch();
					byte[] Ping = BitConverter.GetBytes((short)-1);
					nStream.Write(Ping, 0, 2);
					PingWatch.Start();
				}
			}

		}

		private void Reading()// Controller
		{
			int count = 0;
			byte[] countRead = new byte[2];
			while (threadStart)//Поставить условие, если флаг дисконет равен истине
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
					model.ListUsers = JsonConvert.DeserializeObject<List<UserInfo>>(tmpString);
				}
				else //ping
				{
					PingWatch.Stop();
					Ping = (int)PingWatch.ElapsedMilliseconds;
				}

			}
		}

		public void PressKey() // Controller
		{
			string serialized = JsonConvert.SerializeObject(model.Action);
			byte[] massByts = Encoding.UTF8.GetBytes(serialized);
			byte[] countRead = BitConverter.GetBytes((short)massByts.Count());

			nStream.Write(countRead, 0, 2);//Отпраляет кол-во байт, которое сервер должен будет читать
			nStream.Write(massByts, 0, massByts.Count());
		}

		public void Disconnect()// Controller
		{

			if (threadStart)
			{

				threadReading.Abort();
				nStream.Close();
				client.Close();
				threadStart = false;
			}
		}

		public void Connect()// Controller
		{
			if (!threadStart)
			{
				try
				{
					client = new TcpClient("25.53.91.50", 1337);

					nStream = client.GetStream();

					threadReading = new Thread(Reading);
					threadReading.Start();
					threadStart = true;
				}
				catch (System.Net.Sockets.SocketException) //не удалось подключится по заданным параметрам
				{
					CloseEvent();
				}
			}

		}

	}
}
