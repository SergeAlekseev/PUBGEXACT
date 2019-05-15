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
using ClassLibrary;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using ClassLibrary.ProcessingsClient;
using ClassLibrary.ProcessingsServer;

namespace ClassLibrary
{

	public class ControllerClient
	{

		ConcurrentQueue<ProcessingClient> SecureQueue = new ConcurrentQueue<ProcessingClient>();
		public ManualResetEvent manualResetEvent;

		public delegate void CloseFormD(object sender, FormClosingEventArgs e);
		public event CloseFormD CloseFormEvent;

		public delegate void CloseD();
		public event CloseD CloseEvent;

		public delegate void ErrorConnectD();
		public event ErrorConnectD ErrorConnect;

		public ModelClient model;

		public TcpClient client;// 25.46.244.0 
		public NetworkStream nStream;
		public Thread threadReading;

		public Stopwatch PingWatch = new Stopwatch();
		public bool threadStart = false;
		public bool serverStart;

		public System.Timers.Timer timerPing = new System.Timers.Timer();
		public System.Timers.Timer timerMouseOverItem = new System.Timers.Timer();

		public void JoinUser(string Name, string Password)
		{
			model.GInfo.Name = Name;
			model.GInfo.Password = Password;
		}

		public void ShotDown()
		{
			if (threadStart)
			{
				ShotDown sd = new ShotDown();
				sd.num = model.number;
				Writing(sd);
			}
		}

		public void ShotUp()
		{
			if (threadStart)
			{
				ShotUp su = new ShotUp();
				su.num = model.number;
				Writing(su);
			}
		}

		public void Mouse_Click()
		{
			Point mousLoc = new Point(model.MouseCoord.X-300 + model.ThisUser.userLocation.X, model.MouseCoord.Y-300 + model.ThisUser.userLocation.Y);
			foreach (Item item in model.Map.ListItems)
			{
				if ((mousLoc.X >= item.Location.X - 15 && mousLoc.X <= item.Location.X + 15) && (mousLoc.Y >= item.Location.Y - 15 && mousLoc.Y <= item.Location.Y + 15))
				{
					if (Math.Abs(model.ThisUser.userLocation.X - item.Location.X) < 50 && Math.Abs(model.ThisUser.userLocation.Y - item.Location.Y) < 50)
					{
						ItemTaken itemT = new ItemTaken();
						itemT.item = item;

						CTransfers.Writing(itemT, nStream);
					}
					else
					{
						// Сообщение о том, что игрок слишком далеко от желаемого предмета
					}

				}
			}
		}

		private void Writing(object obj)
		{
			try
			{
				string serialized = JsonConvert.SerializeObject(obj, CTransfers.jss);
				byte[] massByts = Encoding.UTF8.GetBytes(serialized);
				byte[] countRead = BitConverter.GetBytes(massByts.Count());

				nStream.Write(countRead, 0, 4);//Отпраляет кол-во байт, которое сервер должен будет читать
				nStream.Write(massByts, 0, massByts.Count());
			}
			catch { Disconnect(); }
		}

		public void WriteMouseLocation(Point mouseLocation)
		{
			if (threadStart)
			{
				GetPlayersMousesLocation gpml = new GetPlayersMousesLocation();
				gpml.num = model.number;
				gpml.mouse = mouseLocation;
				Writing(gpml);

			}
		}

		public ControllerClient(ModelClient model) // Конструктор
		{
			this.model = model;
			timerPing.Elapsed += timerPing_Tick;
			timerPing.Interval = 2000;
			timerPing.Start();

			//timerMouseOverItem.Elapsed += timerMouseOverItem_Tick;
			//timerMouseOverItem.Interval = 1000;
			//timerMouseOverItem.Start();
		}

		public void timerPing_Tick(object sender, EventArgs e)
		{
			if (threadStart)
			{
				//if (PingWatch.ElapsedMilliseconds > 4000)
				//{
				//	PingWatch.Stop();
				//	CloseFormEvent(null, null);
				//}
				//else
				//{
				//	PingWatch = new Stopwatch();
				//	ClassLibrary.ProcessingsServer.PingInfoServer pi = new PingInfoServer();
				//	pi.num = model.ThisUser.userNumber;
				//	Writing(pi);
				//	PingWatch.Start();
				//}
			}

		}

		public void ChangeItem(byte num)
		{
			if (threadStart)
			{
				ChangeWeapons cw = new ChangeWeapons();
				cw.num = model.number;
				cw.numItems = num;
				Writing(cw);
			}
		}

		public void Recharge()
		{
			if (threadStart)
			{
				Reload r = new Reload();
				r.num = model.number;
				Writing(r);
			}
		}


		public void GetCountWinsInfo()
		{
			model.Win = true;
			// string tmpString = Reading(nStream);
			nStream.Close();
			serverStart = false;
			client.Close();
			timerPing.Stop();
			threadReading.Abort();
		}



		public void DeathPlayer()
		{
			nStream.Close();
			threadStart = false;
			client.Close();
			timerPing.Stop();
			threadReading.Abort();
		}


		public void PressKey()
		{
			if (threadStart)
			{
				PlayerMovementsInfo pmi = new PlayerMovementsInfo();
				pmi.num = model.number;
				pmi.action = model.Action;
				Writing(pmi);
			}
		}

		public void Disconnect()
		{

			if (threadStart)
			{
				serverStart = false;
				client.Close();
				timerPing.Stop();
				threadReading.Abort();
				manualResetEvent.Set();
				nStream.Close();
				CloseEvent();
			}
		}

		public bool Connect(string ip)// Controller
		{
			if (!threadStart)
			{
				try
				{
					try
					{
						manualResetEvent = new ManualResetEvent(true);
						client = new TcpClient(ip, 1337);
						nStream = client.GetStream();
						serverStart = true;

						//threadReading = new Thread(ReadingStream);
						threadReading = new Thread(Producer);
						Thread threadConsumer = new Thread(Consumer);
						//Вот тут вставить продусюре и консамера
						threadReading.Start();
						threadConsumer.Start();
						return true;
					}
					catch (Exception err)
					{
						MessageBox.Show(err.Message);
						ErrorConnect();
						return false;
					}
				}
				catch (System.Net.Sockets.SocketException) //не удалось подключится по заданным параметрам
				{
					CloseEvent();
				}
			}
			return false;
		}

		public double mouseMove()
		{
			double angleDegree = defineAngle(model.MouseCoord, new Point(300, 600));
			model.ThisUser.Rotate = angleDegree;

			if (threadStart)
			{
				GetPlayersAngels gpa = new GetPlayersAngels();
				gpa.num = model.number;
				gpa.angels = angleDegree;
				Writing(gpa);
			}
			return angleDegree;
		}

		public void setName(string Name)
		{
			if (threadStart)
			{

				GetUserName gun = new GetUserName();
				gun.num = model.number;
				gun.name = Name;
				Writing(gun);
			}
		}
		public double defineAngle(Point onePoint, Point twoPoint)
		{
			Point start1 = new Point { X = 300, Y = 300 };
			Point end1 = new Point { X = twoPoint.X, Y = twoPoint.Y };
			Vector vector1 = Vector.FromPoints(start1, end1);

			Point start2 = new Point { X = 300, Y = 300 };
			Point end2 = new Point { X = onePoint.X, Y = onePoint.Y };
			Vector vector2 = Vector.FromPoints(start2, end2);

			double angleRad = Vector.CalculateAngleBetween(vector1, vector2);

			double angleDegree = angleRad / Math.PI * 180;
			if (onePoint.X >= 300) angleDegree = 360 - angleDegree;

			return angleDegree;
		}

		public double defineAngleZone(Point onePoint, Point twoPoint)
		{
			Point start1 = new Point { X = onePoint.X, Y = onePoint.Y };
			Point end1 = new Point { X = twoPoint.X, Y = twoPoint.Y };
			Vector vector1 = Vector.FromPoints(start1, end1);

			Point start2 = new Point { X = onePoint.X, Y = onePoint.Y };
			Point end2 = new Point { X = onePoint.X, Y = onePoint.Y + 200 };
			Vector vector2 = Vector.FromPoints(start2, end2);

			double angleRad = Vector.CalculateAngleBetween(vector1, vector2);

			double angleDegree = angleRad / Math.PI * 180;
			if (onePoint.X < twoPoint.X) angleDegree = 360 - angleDegree;

			return angleDegree;
		}

		public void Producer()
		{
			while (serverStart )
			{
				try
				{
					string tmpString = CTransfers.Reading(nStream);

					SecureQueue.Enqueue(JsonConvert.DeserializeObject<ProcessingClient>(tmpString, CTransfers.jss));




					manualResetEvent.Set();
				}
				catch { Disconnect(); }

			}

		}
		public void Consumer()
		{
			Thread.Sleep(100);
			//MessageBox.Show("Костыль №1");
			ProcessingClient processing;
			while (serverStart)
			{

				manualResetEvent.WaitOne();
				if (SecureQueue.Count > 0)
				{

					SecureQueue.TryDequeue(out processing);
					if (processing != null)
					{

						processing.Process(this);
					}

					Thread.Yield();
				}
				else { manualResetEvent.Reset(); }
			}
		}

		//public void timerMouseOverItem_Tick(object sender, EventArgs e)
		//{
		
		//}
	}
}
