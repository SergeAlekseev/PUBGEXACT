﻿using ClassLibrary.ProcessingsClient;
using ClassLibrary.ProcessingsServer;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary.ControllersServer
{
	public class CPlay
	{
		public System.Timers.Timer timerZone, timerUsersInZone;
		public ConcurrentQueue<ProcessingServer> SecureQueue = new ConcurrentQueue<ProcessingServer>(); //___________________________________
		ModelServer model;
		public Thread ConsumerThread;
		public ManualResetEvent manualResetEvent;
		public CPlay(ModelServer model)
		{
			this.model = model;
		}
		private void timerMove_Tick(bool moveUp, bool moveDown, bool moveLeft, bool moveRight, bool shift, int num) //игра
		{
			try
			{
				if (model.workingGame)
				{
					byte speed = 1;
					if (shift)
					{
						speed *= 2;
					}

					if (model.workingGame && model.ListUsers[num] != null)
					{
						if ((moveUp) && model.ListUsers[num].userLocation.Y - model.Map.MapBorders.Y > 2 && !model.Map.bordersForUsers[model.ListUsers[num].userLocation.X, model.ListUsers[num].userLocation.Y - speed]) model.ListUsers[num].userLocation.Y -= speed; //Вниз
						if ((moveDown) && model.ListUsers[num].userLocation.Y - model.Map.MapBorders.Width < -2 && !model.Map.bordersForUsers[model.ListUsers[num].userLocation.X, model.ListUsers[num].userLocation.Y + speed]) model.ListUsers[num].userLocation.Y += speed; //Вверх
						if ((moveLeft) && model.ListUsers[num].userLocation.X - model.Map.MapBorders.X > 2 && !model.Map.bordersForUsers[model.ListUsers[num].userLocation.X - speed, model.ListUsers[num].userLocation.Y]) model.ListUsers[num].userLocation.X -= speed; //Влево
						if ((moveRight) && model.ListUsers[num].userLocation.X - model.Map.MapBorders.Height < -2 && !model.Map.bordersForUsers[model.ListUsers[num].userLocation.X + speed, model.ListUsers[num].userLocation.Y]) model.ListUsers[num].userLocation.X += speed;// Вправо	
					}
				}

			}
			catch
			{
				//ErrorEvent("Ложный вызов движения");
			}

		}
	public void timerZone_Tick()//Кароче таймер посылает мёртвым игрокам сообщения о зоне. Потом пофиксить
		{
			if (model.Map.NextZone.TimeTocompression > 0)
			{
				GetZoneCompression Compress = new GetZoneCompression();
				Compress.Count = model.Map.NextZone.TimeTocompression;

				foreach (NetworkStream n in model.ListNs)
				{
					CTransfers.Writing(Compress, n); //FIXME здесь мб отправляется мёртвым челам сообщения
				}

				model.Map.NextZone.TimeTocompression -= 1;
			}
			else
			{
				GetZoneCompression Compress = new GetZoneCompression();
				Compress.Count = model.Map.NextZone.TimeTocompression;

				foreach (NetworkStream n in model.ListNs)
				{
					CTransfers.Writing(Compress, n); //FIXME здесь мб отправляется мёртвым челам сообщения
				}

				timerZone.Stop();
				double x = model.Map.PrevZone.ZoneCenterCoordinates.X, y = model.Map.PrevZone.ZoneCenterCoordinates.Y, radius = model.Map.PrevZone.ZoneRadius; ;
				double koef = Math.Sqrt(Math.Pow(model.Map.PrevZone.ZoneCenterCoordinates.X - model.Map.NextZone.ZoneCenterCoordinates.X, 2)
										+ Math.Pow(model.Map.PrevZone.ZoneCenterCoordinates.Y - model.Map.NextZone.ZoneCenterCoordinates.Y, 2)) / 750;
				double k = Math.Sqrt(Math.Pow(model.Map.PrevZone.ZoneCenterCoordinates.X - model.Map.NextZone.ZoneCenterCoordinates.X, 2)
										+ Math.Pow(model.Map.PrevZone.ZoneCenterCoordinates.Y - model.Map.NextZone.ZoneCenterCoordinates.Y, 2)) / koef;
				double speedX = (model.Map.PrevZone.ZoneCenterCoordinates.X - model.Map.NextZone.ZoneCenterCoordinates.X) / k;
				double speedY = (model.Map.PrevZone.ZoneCenterCoordinates.Y - model.Map.NextZone.ZoneCenterCoordinates.Y) / k;

				double speedRadius = (double)(model.Map.PrevZone.ZoneRadius - model.Map.NextZone.ZoneRadius) / 750;
				while (model.Map.PrevZone.ZoneRadius > model.Map.NextZone.ZoneRadius && model.workingGame)
				{
					x -= speedX;
					model.Map.PrevZone.ZoneCenterCoordinateX = (int)x;
					y -= speedY;
					model.Map.PrevZone.ZoneCenterCoordinateY = (int)y;
					radius -= speedRadius;
					model.Map.PrevZone.ZoneRadius = (int)radius;
					for (int i = 0; i < model.ListUsers.Count; i++)
					{
						if (model.ListUsers[i] != null)
						{
							GetPrevZoneInfo prevZoneInfo = new GetPrevZoneInfo();
							prevZoneInfo.prevZone = model.Map.PrevZone;

							CTransfers.Writing(prevZoneInfo, model.ListNs[i]);
						}
					}
					Thread.Sleep(40);
				}
				if (model.workingGame)
				{
					model.Map.PrevZone = model.Map.NextZone;
					model.Map.NextZone = new Zone();
					model.Map.NextZone.ZoneRadius = (int)model.Map.PrevZone.ZoneRadius / 2;
					model.Map.NextZone.NewCenterZone(model.Map.MapBorders, model.Map.PrevZone.ZoneCenterCoordinates, model.Map.PrevZone.ZoneRadius);//не страдает ли тут MVC?
					for (int i = 0; i < model.ListUsers.Count; i++)
					{
						if (model.ListUsers[i] != null)
						{
							GetZoneStartInfo nextZoneInfo = new GetZoneStartInfo();
							nextZoneInfo.nextZone = model.Map.NextZone;

							CTransfers.Writing(nextZoneInfo, model.ListNs[i]);
						}
					}
					model.Map.NextZone.TimeTocompression = 60;
					model.DamageZone += 2;
					timerZone.Start();
				}
			}
		}
		public void timerUsersInZone_Tick()//игра
		{
			for (int i = 0; i < model.ListUsers.Count; i++)
			{
				if (model.ListUsers[i] != null)
				{
					if (Math.Sqrt(Math.Pow(model.ListUsers[i].userLocation.X - model.Map.NextZone.ZoneCenterCoordinates.X, 2) + Math.Pow(model.ListUsers[i].userLocation.Y - model.Map.NextZone.ZoneCenterCoordinates.Y, 2)) > model.Map.NextZone.ZoneRadius)
					{
						model.ListUsers[i].flagZone = true;
					}
					else model.ListUsers[i].flagZone = false;

					if (model.Map.PrevZone != null && Math.Sqrt(Math.Pow(model.ListUsers[i].userLocation.X - model.Map.PrevZone.ZoneCenterCoordinates.X, 2) + Math.Pow(model.ListUsers[i].userLocation.Y - model.Map.PrevZone.ZoneCenterCoordinates.Y, 2)) > model.Map.PrevZone.ZoneRadius)
					{
						model.ListUsers[i].hp -= model.DamageZone;
						if (model.ListUsers[i].hp <= 0)
						{

							Trace.WriteLine(Environment.NewLine + "============================================================================" + Environment.NewLine +
							"Player " + model.ListUsers[i].Name + " died from zone" + Environment.NewLine +
							"Kills - " + model.ListUsers[i].kills + Environment.NewLine +
							"============================================================================");

							SingalForDroping Signal = new SingalForDroping();
							CTransfers.Writing(Signal, model.ListNs[i]);
							Thread.Sleep(500);//Чтобы вещи успевали дропнуться до удаления игрока

							PlayerDeath death = new PlayerDeath();
							death.Killer = "ZONA";

							CTransfers.Writing(death, model.ListNs[i]);

							foreach (GeneralInfo g in model.ListGInfo)
							{
								if (g.Name == model.ListUsers[i].Name)
									g.Dies += 1;
							}

							GetKillsInfo kill = new GetKillsInfo();
							kill.kill.killer = "ZONA";
							kill.kill.dead = model.ListUsers[i].Name;

							for (int k = 0; k < model.ListUsers.Count; k++)
							{
								if (model.ListUsers[k] != null)
								{
									CTransfers.Writing(kill, model.ListNs[k]);
								}
							}
							//model.ListNs.RemoveAt(i); //////////////////////////////////////////////////////
						}
					}

				}
			}
		}

		public void PlayUser(object tc)//игра
		{
			int num = model.number;   //шанс ошибки при одновременном подключении

			sendMapObjectsInfo(num); //Отправка инфы обо всех объектах карты

			model.CountGamers += 1;
			writingCountGamers();

			model.ListUsers[num].Items[1] = new Item();
			model.ListUsers[num].Items[2] = new Item();
			model.ListUsers[num].Items[3] = new Item();
			model.ListUsers[num].Items[4] = new Item();
			model.ListUsers[num].Items[5] = new Item();
			model.ListUsers[num].Items[6] = new Item();

			Thread Producerthread = new Thread(new ParameterizedThreadStart(Producer));
			Producerthread.Start(num);
		}

		public bool sendMapObjectsInfo(int num)//игра
		{

			GetNumber gNumber = new GetNumber();
			gNumber.num = model.number;
			GetBushesInfo bushesInfo = new GetBushesInfo();
			bushesInfo.listBush = model.Map.ListBush;
			GetMapBordersInfo bordersInfo = new GetMapBordersInfo();
			bordersInfo.rectangle = model.Map.MapBorders;
			GetBoxesInfo boxesInfo = new GetBoxesInfo();
			boxesInfo.listBox = model.Map.ListBox;
			GetInfoItems itemsInfo = new GetInfoItems();
			itemsInfo.listItems = model.Map.ListItems;
			GetTreesInfo treesInfo = new GetTreesInfo();
			treesInfo.listTree = model.Map.ListTrees;
			try
			{
				CTransfers.Writing(gNumber, model.ListNs[num]);
				Thread.Sleep(100);
				CTransfers.Writing(bushesInfo, model.ListNs[num]); // Отправка инфы о кустах
				Thread.Sleep(100);
				CTransfers.Writing(bordersInfo, model.ListNs[num]); //Инфа о границах карты
				Thread.Sleep(100);
				CTransfers.Writing(boxesInfo, model.ListNs[num]); // Отправка инфы о коробках
				Thread.Sleep(100);
				CTransfers.Writing(treesInfo, model.ListNs[num]); // Отправка инфы о деревьях
				Thread.Sleep(100);
				CTransfers.Writing(itemsInfo, model.ListNs[num]); // Отправка инфы о вещах
				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine("Error in sending info about objects of map | " + e.ToString());
				return false;
			}

		}
		public void sendUserInfo(object tc)//игра
		{
			KeyValuePair<int, TcpClient> pair = (KeyValuePair<int, TcpClient>)tc;

			TcpClient tcp = pair.Value;
			Thread.Sleep(200);
			NetworkStream nStream = tcp.GetStream();
			bool PrivateWorkingThread = true;

			while (model.workingThread && PrivateWorkingThread)
			{
				AngelToZone angel = new AngelToZone();
				angel.listUserInfo = model.ListUsers;
				GetBulletsInfo buletsInfo = new GetBulletsInfo();
				lock (model.ListBullet)
					buletsInfo.listBulets = model.ListBullet.ToList<BulletInfo>();
				GetGrenadesInfo grenadesInfo = new GetGrenadesInfo();
				lock (model.ListGrenade)
					grenadesInfo.grenadesInfo = model.ListGrenade.ToList<GrenadeInfo>();

				if (!CTransfers.Writing(angel, nStream)) break;
				if (!CTransfers.Writing(buletsInfo, nStream)) break;
				if (!CTransfers.Writing(grenadesInfo, nStream)) break;
				Thread.Sleep(10);
			}
		}

		public void writingCountGamers()//игра
		{
			for (int i = 0; i < model.ListUsers.Count; i++)
			{
				if (model.ListUsers[i] != null && model.ListUsers[i].hp > 0)
				{
					GetCountGamesInfo gamesInfo = new GetCountGamesInfo();
					gamesInfo.count = model.CountGamers;

					CTransfers.Writing(gamesInfo, model.ListNs[i]);
				}
			}
			if (model.CountGamers == 1 && model.workingGame)
			{

				for (int i = 0; i < model.ListUsers.Count; i++)
				{
					if (model.ListUsers[i] != null && model.ListUsers[i].hp > 0)
					{

						foreach (GeneralInfo g in model.ListGInfo)
						{
							if (g.Name == model.ListUsers[i].Name)
							{
								g.Wins += 1;
								Trace.WriteLine(Environment.NewLine + "============================================================================" + Environment.NewLine +
									"Player " + model.ListUsers[i].Name + " win!" + Environment.NewLine +
									"Kills - " + model.ListUsers[i].kills + Environment.NewLine +
									"============================================================================");
							}

						}
						CTransfers.Writing(new GetCountWinsInfo(), model.ListNs[i]);
						break;
					}
				}
				ControllersS.cServer.StopServer();
			}
		}
		public void Producer(object obj)//игра
		{
			int num = (int)obj;
			System.Timers.Timer timerMove = new System.Timers.Timer();
			model.ListTimers.Add(timerMove);

			try
			{
				timerMove.Interval = 15;
				timerMove.Elapsed += (x, y) => { timerMove_Tick(model.ListMove[num].moveUp, model.ListMove[num].moveDown, model.ListMove[num].moveLeft, model.ListMove[num].moveRight, model.ListMove[num].shift, num); };
				timerMove.Start();

				try
				{
					while (model.workingServer && model.workingThread)
					{
						string tmpString = CTransfers.Reading(model.ListNs[num]);

						try
						{
							ProcessingServer processingServer = JsonConvert.DeserializeObject<ProcessingServer>(tmpString, CTransfers.jss);
							SecureQueue.Enqueue(processingServer);
							Debug.WriteLine("Processing - [" + processingServer.GetType().Name + "], player [" + model.ListUsers[processingServer.num].Name + "]");
						}
						catch
						{
							model.ListNs[num].Read(new Byte[2], 0, 2); //Что здесь происходит 
							Debug.WriteLine("What is going on");
						}

						manualResetEvent.Set();

					}
					timerMove.Stop();

				}
				catch (System.IO.IOException ie)
				{
					if (model.ListUsers.Count != 0 && model.ListUsers[num] != null)
					{
						model.ListUsers[num].flagShoting = false;
						lock (model.ListUsers)
						{
							model.ListUsers.RemoveAt(num);
							model.ListUsers.Insert(num, null);
						}
					}
					model.CountGamers -= 1;
					writingCountGamers();

					Debug.WriteLine("Player disconnect _____");
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString() + " | Ложный вызов продюсера");
				timerMove.Stop();
			}

		}

		public void Consumer(object obj)//игра Здесь тоже может произойти ошибка, если ссылка в модуле указывает на null
		{
			Thread.Sleep(100);
			ProcessingServer processing;
			while (model.workingServer && model.workingThread)
			{
				manualResetEvent.WaitOne();
				if (SecureQueue.Count > 0)
				{
					SecureQueue.TryDequeue(out processing);
					if (processing != null)
						processing.Process(model);

					Thread.Yield();
				}
				else { manualResetEvent.Reset(); }

			}
		}
	}
}
