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
		public Thread threadReading;
		public Thread threadConsumer;

		
		

		public System.Timers.Timer timerPing = new System.Timers.Timer();
		public System.Timers.Timer timerMouseOverItem = new System.Timers.Timer();

		public void JoinUser(string Name, string Password)
		{
			model.GInfo.Name = Name;
			model.GInfo.Password = Password;
		}

		public void exit()
		{
			client.Close();
			timerPing.Stop();
			Disconnect();
			threadReading.Abort();
			threadConsumer.Abort();
			manualResetEvent.Set();
		}
		public void ShotDown()
		{
			if (model.threadStart)
			{
				ShotDown sd = new ShotDown();
				sd.num = model.number;
				Writing(sd);
			}
		}

		public void ShotUp()
		{
			if (model.threadStart)
			{
				ShotUp su = new ShotUp();
				su.num = model.number;
				Writing(su);
			}
		}

		public void Mouse_Click()
		{
			Point mousLoc = new Point(model.MouseCoord.X - 300 + model.ThisUser.userLocation.X, model.MouseCoord.Y - 300 + model.ThisUser.userLocation.Y);
			foreach (Item item in model.Map.ListItems)
			{
				if ((mousLoc.X >= item.Location.X - 15 && mousLoc.X <= item.Location.X + 15) && (mousLoc.Y >= item.Location.Y - 15 && mousLoc.Y <= item.Location.Y + 15))
				{
					if (Math.Abs(model.ThisUser.userLocation.X - item.Location.X) < 50 && Math.Abs(model.ThisUser.userLocation.Y - item.Location.Y) < 50)
					{
						ItemTaken itemTaken = new ItemTaken();
						itemTaken.item = item;
						itemTaken.num = model.number;

						CTransfers.Writing(itemTaken, model.NStream);
						break;
					}
					else
					{
						// Сообщение о том, что игрок слишком далеко от желаемого предмета
					}

				}
			}
		}

		public void ItemDroping()
		{		
				Point mousLoc = new Point(model.MouseCoord.X - 300 + model.ThisUser.userLocation.X, model.MouseCoord.Y - 300 + model.ThisUser.userLocation.Y);

				for (int i = 1; i < model.ThisUser.Items.Length; i++)
				{
					if (i == model.ThisUser.thisItem && model.ThisUser.Items[model.ThisUser.thisItem].Name != null)
					{
						ItemDroping itemDrop = new ItemDroping();
						List<Item> listItems = new List<Item>();
						listItems.Add(model.ThisUser.Items[i]);
						itemDrop.items = listItems;
						itemDrop.num = model.number;
						itemDrop.itemLocation = model.ThisUser.userLocation;

						CTransfers.Writing(itemDrop, model.NStream);
					}

				}			
		}

		public void Writing(object obj)
		{
			try
			{
				string serialized = JsonConvert.SerializeObject(obj, CTransfers.jss);
				byte[] massByts = Encoding.UTF8.GetBytes(serialized);
				byte[] countRead = BitConverter.GetBytes(massByts.Count());

				model.NStream.Write(countRead, 0, 4);//Отпраляет кол-во байт, которое сервер должен будет читать
				model.NStream.Write(massByts, 0, massByts.Count());
			}
			catch { Disconnect(); }
		}

		public void WriteMouseLocation(Point mouseLocation)
		{
			if (model.threadStart)
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

			model.exit = exit;
			model.setName = setName;

			timerPing.Elapsed += timerPing_Tick;
			timerPing.Interval = 2000;
			timerPing.Start();
		}

		public void timerPing_Tick(object sender, EventArgs e)
		{
			//if (model.threadStart)
			//{
			//	if (model.PingWatch.ElapsedMilliseconds > 4000)
			//	{
			//		model.PingWatch.Stop();
			//		CloseFormEvent(null, null);
			//	}
			//	else
			//	{
			//		model.PingWatch = new Stopwatch();
			//		ClassLibrary.ProcessingsServer.PingInfoServer pi = new PingInfoServer();
			//		pi.num = model.ThisUser.userNumber;
			//		Writing(pi);
			//		model.PingWatch.Start();
			//	}
			//}

		}

		public bool ChangeItem(byte num)
		{
			try
			{
				if (model.threadStart)
				{
					ChangeWeapons cw = new ChangeWeapons();
					cw.num = model.number;
					cw.numItems = num;
					Writing(cw);
					return true;
				}
				else return false;
			}
			catch { return false; }
		}

		public void Recharge()
		{
			if (model.threadStart)
			{
				Reload r = new Reload();
				r.num = model.number;
				Writing(r);
			}
		}


		public void GetCountWinsInfo()
		{
			model.Win = true;
			model.NStream.Close();
			model.serverStart = false;
			client.Close();
			timerPing.Stop();
			threadReading.Abort();
			threadConsumer.Abort();
		}



		public void DeathPlayer()
		{
			model.NStream.Close();
			model.threadStart = false;
			client.Close();
			timerPing.Stop();
			threadReading.Abort();
			threadConsumer.Abort();
		}


		public void PressKey()
		{
			if (model.threadStart)
			{
				PlayerMovementsInfo pmi = new PlayerMovementsInfo();
				pmi.num = model.number;
				pmi.action = model.Action;
				Writing(pmi);
			}
		}

		public void Disconnect()
		{

			if (model.threadStart)
			{
				model.serverStart = false;
				client.Close();
				timerPing.Stop();
				threadReading.Abort();
				threadConsumer.Abort();
				manualResetEvent.Set();
				model.NStream.Close();
				CloseEvent();

			}
		}

		public bool Connect(string ip)// Controller
		{
			if (!model.threadStart)
			{
				try
				{
					try
					{
						manualResetEvent = new ManualResetEvent(true);
						client = new TcpClient(ip, 1337);
						model.NStream = client.GetStream();
						model.serverStart = true;

						//threadReading = new Thread(ReadingStream);
						threadReading = new Thread(Producer);
						threadConsumer = new Thread(Consumer);
						//Вот тут вставить продусюре и консамера
						threadReading.Start();
						threadConsumer.Start();
						return true;
					}
					catch (Exception err)
					{
						//MessageBox.Show(err.Message);
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
			double angleDegree = defineAngleBot(model.ThisUser.userLocation ,model.MouseCoord);
			model.ThisUser.Rotate = angleDegree;

			if (model.threadStart)
			{
				GetPlayersAngels gpa = new GetPlayersAngels();
				gpa.num = model.number;
				gpa.angels = angleDegree;
				Writing(gpa);
			}
			return angleDegree;
		}
		public bool setName(string Name)
		{
			if (model.threadStart)
			{
				GetUserName gun = new GetUserName();
				gun.num = model.number;
				gun.name = Name;
				Writing(gun);
				return true;
			}
			else return false;
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

		public double defineAngleBot(Point botLocation, Point target)
		{
			Point start1 = new Point { X = botLocation.X, Y = botLocation.Y };
			Point end1 = new Point { X = botLocation.X, Y = botLocation.Y + 300};
			Vector vector1 = Vector.FromPoints(start1, end1);

			Point start2 = new Point {X = botLocation.X, Y = botLocation.Y};
			Point end2 = new Point { X = target.X, Y = target.Y };
			Vector vector2 = Vector.FromPoints(start2, end2);

			double angleRad = Vector.CalculateAngleBetween(vector1, vector2);

			double angleDegree = angleRad / Math.PI * 180;
			if (target.X >= botLocation.X) angleDegree = 360 - angleDegree;

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
			while (model.serverStart)
			{
				try
				{
					string tmpString = CTransfers.Reading(model.NStream);

					SecureQueue.Enqueue(JsonConvert.DeserializeObject<ProcessingClient>(tmpString, CTransfers.jss));

					manualResetEvent.Set();
				}
				catch { Disconnect(); }

			}

		}
		public void Consumer()
		{
			Thread.Sleep(100);
			ProcessingClient processing;
			while (model.serverStart)
			{

				manualResetEvent.WaitOne();
				if (SecureQueue.Count > 0)
				{

					SecureQueue.TryDequeue(out processing);
					if (processing != null)
					{

						processing.Process(model);
					}

					Thread.Yield();
				}
				else { manualResetEvent.Reset(); }
			}
		}
	}
}
