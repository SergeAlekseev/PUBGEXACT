using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using ClassLibrary;
using Action = ClassLibrary.Action;
using System.Collections.Concurrent;

using Newtonsoft.Json.Linq;
using ClassLibrary.ProcessingsServer;
using ClassLibrary.ProcessingsClient;

namespace ClassLibrary
{
	public class ControllerServer
	{
		bool workingThread;
		bool workingServer;
		short number; //Model

		public ModelServer model;

		TcpListener PublicHost, PublicHost2, PublicHost3;
		System.Timers.Timer timerZone, timerUsersInZone;

		public delegate void StartServerD(string text);
		public event StartServerD StartServerEvent;

		public delegate void StartGameD(string text);
		public event StartGameD StartGameEvent;

		public delegate void StopServerD(string text);
		public event StopServerD StopServerEvent;

		public delegate void ErrorD(string Error);
		public event ErrorD ErrorEvent;

		Thread ConsumerThread;
		ManualResetEvent manualResetEvent;
		public ConcurrentQueue<ProcessingServer> SecureQueue = new ConcurrentQueue<ProcessingServer>(); //___________________________________

		public void StartGame()
		{
			if (!model.workingGame && workingServer)
			{
				model.workingGame = true;

				createdZone();

				for (int i = 0; i < model.ListUsers.Count; i++)
				{
					if (model.ListUsers[i] != null)
					{
						GetZoneStartInfo nextZoneInfo = new GetZoneStartInfo();
						nextZoneInfo.nextZone = model.Map.NextZone;
						GetPrevZoneInfo prevZoneInfo = new GetPrevZoneInfo();
						prevZoneInfo.prevZone = model.Map.PrevZone;

						CTransfers.Writing(nextZoneInfo, model.ListNs[i]); // Инфа  о стартовой зоне
						CTransfers.Writing(prevZoneInfo, model.ListNs[i]);
					}
				}

				timerZone = new System.Timers.Timer();
				timerZone.Interval = 1000;
				timerZone.Elapsed += (x, y) => { timerZone_Tick(); };
				timerZone.Start();

				timerUsersInZone = new System.Timers.Timer();
				timerUsersInZone.Interval = 1000;
				timerUsersInZone.Elapsed += (x, y) => { timerUsersInZone_Tick(); };
				timerUsersInZone.Start();
				StartGameEvent("Игра идёт");
			}
		}

		public ControllerServer(ModelServer model)
		{
			this.model = model;
			CTransfers.jss.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
			CTransfers.jss.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
			CTransfers.jss.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
			CTransfers.jss.Formatting = Newtonsoft.Json.Formatting.Indented;
		}

		private void timerMove_Tick(bool moveUp, bool moveDown, bool moveLeft, bool moveRight, bool shift, int num) //Здесь будет выполняться перемещение игрока
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
				ErrorEvent("Ложный вызов движения");
			}

		}

		private void timerZone_Tick()
		{
			if (model.Map.NextZone.TimeTocompression > 0)
			{
				GetZoneCompression Compress = new GetZoneCompression();
				Compress.Count = model.Map.NextZone.TimeTocompression;

				foreach (NetworkStream n in model.ListNs)
				{
					CTransfers.Writing(Compress, n);
				}
				
				model.Map.NextZone.TimeTocompression -= 1;				
			}
			else
			{
				GetZoneCompression Compress = new GetZoneCompression();
				Compress.Count = model.Map.NextZone.TimeTocompression;

				foreach (NetworkStream n in model.ListNs)
				{
					CTransfers.Writing(Compress, n);
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
						if (model.ListUsers[i] != null && model.ListNs[i].CanWrite)
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
					timerZone.Start();
				}
			}
		}

		private void timerUsersInZone_Tick()
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
						model.ListUsers[i].hp -= 2;
						if (model.ListUsers[i].hp <= 0)
						{
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
						}
					}

				}
			}
		}

		public void start()
		{
			if (PlayerRead(null) != null)
			{
				model.ListGInfo = PlayerRead(null);
			}
			Thread startThread = new Thread(new ParameterizedThreadStart(StartServer));
			startThread.Start();
		}

		public void StartServer(object tmpObject)//Controller
		{
			SecureQueue = new ConcurrentQueue<ProcessingServer>();

			if (!workingServer && !model.workingGame)
			{
				workingServer = true;
				model.ListMove = new List<MMove>();
				Random random = new Random();
				number = -1;
				TcpListener host = new TcpListener(IPAddress.Any, 1337);

				TcpListener host2 = new TcpListener(IPAddress.Any, 2337);
				host2.Start();
				PublicHost2 = host2;
				Thread menuStarting = new Thread(new ParameterizedThreadStart(MenuStarting));
				menuStarting.Start(host2);

				TcpListener host3 = new TcpListener(IPAddress.Any, 3337);
				host3.Start();
				PublicHost3 = host3;
				Thread menuConnecting = new Thread(new ParameterizedThreadStart(MenuConnecting));
				menuConnecting.Start(host3);

				RandomBushs();
				RandomBox();
				RandomTree();
				GenerateItems();

				PublicHost = host;
				host.Start();
				model.ListUsers = new List<UserInfo>();

				manualResetEvent = new ManualResetEvent(true);
				ConsumerThread = new Thread(new ParameterizedThreadStart(Consumer));
				ConsumerThread.Start(manualResetEvent);

				StartServerEvent("Сервер запущен");
				while (workingServer)
				{
					TcpClient tc = null;
					workingThread = true;//возможно сделать красивее
					try
					{
						tc = host.AcceptTcpClient();

						number++;

						UserInfo userInfoTmp;
						do
							userInfoTmp = new UserInfo(new Point(random.Next(2, model.Map.MapBorders.Width - 2), random.Next(2, model.Map.MapBorders.Height - 2)));
						while (model.Map.bordersForUsers[userInfoTmp.userLocation.X, userInfoTmp.userLocation.Y]);
						userInfoTmp.userNumber = number;

						lock (model.ListUsers)
						{
							model.ListUsers.Add(userInfoTmp);
						}



						model.ListNs.Add(tc.GetStream());
						model.ListMove.Add(new MMove());
						model.ListShoting.Add(new Thread(new ParameterizedThreadStart(ShotUser)));
						Thread thread = new Thread(new ParameterizedThreadStart(PlayUser));
						thread.Start(tc);



						Thread thread2 = new Thread(new ParameterizedThreadStart(InfoUsers));
						thread2.Priority = ThreadPriority.Highest;
						thread2.Start(tc);
					}
					catch
					{
						if (tc != null)
							tc.Close();
						break;
					}
				}
				Thread.Sleep(1000);
				model.Remove();
			}
		}



		public void StopServer()
		{
			if (workingServer)
			{
				PlayerSave(model.ListGInfo);
				number = -1;
				PublicHost.Stop();
				PublicHost2.Stop();
				PublicHost3.Stop();
				if (model.workingGame)
				{
					timerZone.Stop();
					timerUsersInZone.Stop();
				}
				foreach (NetworkStream ns in model.ListNs)
				{
					if (ns != null)
					{
						Disconnect d = new Disconnect();
						try
						{
							CTransfers.Writing(d, ns);
						}
						catch (Exception err) { ErrorEvent(err.Message + " |Ошибка в ControllerServer, методе StopServer"); }
					}
				}
				foreach (System.Timers.Timer t in model.ListTimers)
				{
					t.Stop();
				}

				workingServer = false;
				workingThread = false;
				model.Remove();
				model.workingGame = false;
				manualResetEvent.Set();
				Thread.Sleep(100);
				StopServerEvent("Сервер отключен");
			}
		}

		public void RandomBushs()
		{
			Random random = new Random();
			for (int i = 0; i < model.Map.MapBorders.Height * model.Map.MapBorders.Width / 10000; i++)
			{
				model.Map.ListBush.Add(new Bush(random.Next(model.Map.MapBorders.Width), random.Next(model.Map.MapBorders.Height)));
			}
		}

		public void RandomBox()
		{
			bool flag = true;
			Random random = new Random();
			for (int i = 0; i < model.Map.MapBorders.Height * model.Map.MapBorders.Width / 50000;)
			{
				Box box = new Box(random.Next(13, model.Map.MapBorders.Width - 13), random.Next(13, model.Map.MapBorders.Height - 13));
				foreach (Box b in model.Map.ListBox)//Проверка, заспавнился ли ящик в ящике
				{
					if (Math.Abs(b.Location.X - box.Location.X) < Box.size && Math.Abs(b.Location.Y - box.Location.Y) < Box.size) 
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					model.Map.ListBox.Add(box);
					for (int k = box.Location.X - 10; k < box.Location.X + 10; k++)
					{
						for (int j = box.Location.Y - 10; j < box.Location.Y + 10; j++)
						{
							model.Map.bordersForBullets[k, j] = true;
						}
					}
					for (int k = box.Location.X - 13; k < box.Location.X + 13; k++)
					{
						for (int j = box.Location.Y - 13; j < box.Location.Y + 13; j++)
						{
							model.Map.bordersForUsers[k, j] = true;
						}
					}
					i++;
				}
				flag = true;
			}
		}

		public bool RandomTree()
		{
			try
			{
				bool flag = true;
				Random random = new Random();
				for (int i = 0; i < model.Map.MapBorders.Height * model.Map.MapBorders.Width / 68000;)
				{
					Tree tree = new Tree(random.Next(13, model.Map.MapBorders.Width - 13), random.Next(13, model.Map.MapBorders.Height - 13));
					foreach (Box b in model.Map.ListBox)//Проверка, заспавнился ли ящик в ящике
					{
						if (Math.Abs(b.Location.X - tree.Location.X) < Box.size || Math.Abs(b.Location.Y - tree.Location.Y) < Box.size)
						{
							flag = false;
							break;
						}
					}

					//foreach (Box b in model.Map.ListBox)//Проверка, заспавнился ли ящик в ящике
					//{
					//	if (Math.Abs(b.Location.X - tree.Location.X) < Box.size || Math.Abs(b.Location.Y - tree.Location.Y) < Box.size)
					//	{
					//		flag = false;
					//		break;
					//	}
					//}

					if (flag)
					{
						model.Map.ListTrees.Add(tree);
						for (int k = tree.Location.X - 3; k < tree.Location.X + 11; k++)
						{
							for (int j = tree.Location.Y - 3; j < tree.Location.Y + 11; j++)
							{
								model.Map.bordersForBullets[k, j] = true;
							}
						}
						for (int k = tree.Location.X - 3; k < tree.Location.X + 13; k++)
						{
							for (int j = tree.Location.Y - 3; j < tree.Location.Y + 13; j++)
							{
								model.Map.bordersForUsers[k, j] = true;
							}
						}
						i++;
					}
					flag = true;
				}
				return true;
			}
			catch { return false; }
		}

		public void MenuConnecting(object tc)//Controller
		{
			while (workingServer)
			{
				try
				{
					(tc as TcpListener).AcceptTcpClient();
				}
				catch (Exception err)
				{
					ErrorEvent(err.Message + " |Ошибка в ControllerServer, методе MenuConnecting");
					break;
				}
			}

		}

		public void MenuStarting(object tl)//Controller
		{
			TcpClient tc = null;
			NetworkStream nStream = null;
			while (workingServer)
			{

				try
				{
					tc = (tl as TcpListener).AcceptTcpClient();
				}
				catch
				{
					break;
				}

				nStream = tc.GetStream();

				byte[] typeCommand = new byte[1];
				nStream.Read(typeCommand, 0, 1);

				switch (typeCommand[0])
				{
					case 10:
						{

							string tmpString = CTransfers.Reading(nStream);
							GeneralInfo newUser = JsonConvert.DeserializeObject<GeneralInfo>(tmpString);

							GeneralInfo tmpUser = PlayerCheck(PlayerRead(newUser), newUser);
							if (!model.workingGame)
							{
								if (tmpUser == null)
								{
									model.ListGInfo.Add(new GeneralInfo());
									model.ListGInfo[model.ListGInfo.Count - 1].Name = newUser.Name;
									model.ListGInfo[model.ListGInfo.Count - 1].Password = newUser.Password;

									PlayerSave(model.ListGInfo);


									CTransfers.WritingInMenu(model.ListGInfo[model.ListGInfo.Count - 1], 10, nStream);
								}
								else
								{
									if (CheckData(model.ListGInfo, newUser))
									{
										model.ListGInfo = PlayerRead(newUser);
										CTransfers.WritingInMenu(tmpUser, 10, nStream);
									}
									else
									{
										CTransfers.WritingInMenu("1", 11, nStream);
									}
									//Если такой игрок уже есть , то при правильном пароле выдать всю инфу об игроке
								}
							}
							else
							{
								if (tmpUser == null && model.ListGInfo.Count > 0)
								{
									model.ListGInfo.Add(new GeneralInfo());
									model.ListGInfo[model.ListGInfo.Count - 1].Name = newUser.Name;
									model.ListGInfo[model.ListGInfo.Count - 1].Password = newUser.Password;

									PlayerSave(model.ListGInfo);
									CTransfers.WritingInMenu(model.ListGInfo[model.ListGInfo.Count - 1], 12, nStream);
								}
								else
								{
									if (CheckData(model.ListGInfo, newUser))
									{
										model.ListGInfo = PlayerRead(newUser);
										CTransfers.WritingInMenu(tmpUser, 12, nStream);
									}
									else
									{
										CTransfers.WritingInMenu("1", 12, nStream);
									}
								}
							}
							break;
						}
				}
				tc.Close();
			}
		}

		public void ShotUser(object ui)//Controller
		{
			UserInfo userInfo = (UserInfo)ui;
			Object obj = null;
			while (userInfo.flagShoting)
			{
				obj = (userInfo.Items[userInfo.thisItem] as Item).Use(userInfo);
				if (obj != null)
				{
					if (obj is BulletInfo)
					{
						BulletInfo bi = (BulletInfo)obj;
						Thread thread = new Thread(new ParameterizedThreadStart(Bullet));
						thread.Start(bi);
						lock (model.ListBullet)
						{
							model.ListBullet.Add(bi);
						}
						Thread.Sleep(userInfo.Items[userInfo.thisItem].Time);
					}
					else if (obj is List<BulletInfo>)
					{
						List<BulletInfo> bis = (List<BulletInfo>)obj;
						foreach (BulletInfo bi in bis)
						{
							Thread thread = new Thread(new ParameterizedThreadStart(Bullet));
							thread.Start(bi);
							lock (model.ListBullet)
							{
								model.ListBullet.Add(bi);
							}
						}
						Thread.Sleep(userInfo.Items[userInfo.thisItem].Time);
					}
				}
			}


		}

		public void Bullet(object tmpObject)
		{
			bool flagBreak = false;
			BulletInfo bulletInfo = (BulletInfo)tmpObject;
			double X = bulletInfo.location.X, Y = bulletInfo.location.Y;
			X += (13.0 / bulletInfo.speed) * bulletInfo.speedX;
			bulletInfo.location.X = (int)X;
			Y += (13.0 / bulletInfo.speed) * bulletInfo.speedY;
			bulletInfo.location.Y = (int)Y;
			for (int i = 0; i < bulletInfo.timeLife; i++)
			{
				X += bulletInfo.speedX;
				bulletInfo.location.X = (int)X;
				Y += bulletInfo.speedY;
				bulletInfo.location.Y = (int)Y;
				for (int j = 0; j < model.ListUsers.Count; j++)
				{
					if (model.ListUsers[j] != null && Math.Abs(model.ListUsers[j].userLocation.X - X) <= 9 && Math.Abs(model.ListUsers[j].userLocation.Y - Y) <= 9)
					{
						byte[] popad = new byte[1];
						popad[0] = 6;
						model.ListUsers[j].hp -= bulletInfo.damage;
						flagBreak = true;
						if (model.ListUsers[j].hp <= 0)
						{

		
							PlayerDeath death = new PlayerDeath();
							death.Killer = bulletInfo.owner;

							CTransfers.Writing(death, model.ListNs[j]);

							foreach (GeneralInfo g in model.ListGInfo)
							{
								if (g.Name == model.ListUsers[j].Name)
									g.Dies += 1;
								if (g.Name == bulletInfo.owner)
									g.Kills += 1;
							}

							Kill kill = new Kill();
							kill.killer = bulletInfo.owner;
							kill.dead = model.ListUsers[j].Name;

							for (int k = 0; k < model.ListUsers.Count; k++)
							{
								if (model.ListUsers[k] != null)
								{
									if (model.ListUsers[k].Name == bulletInfo.owner)
										model.ListUsers[k].kills += 1;
									GetKillsInfo killsInfo = new GetKillsInfo();
									killsInfo.kill = kill;
									CTransfers.Writing(killsInfo, model.ListNs[k]);
								}
							}

						}
						break;
					}
				}
				try
				{
					if (model.Map.bordersForBullets[bulletInfo.location.X, bulletInfo.location.Y])
					{
						flagBreak = true;
					}
				}
				catch { break; }
				if (flagBreak) break;
				Thread.Sleep(20);
			}
			lock (model.ListBullet)
			{
				model.ListBullet.TryTake(out bulletInfo);
			}
		}



		public void PlayUser(object tc)//Controller
		{
			int num = number;   //шанс ошибки при одновременном подключении

			SendingInformationAboutObjects(num); //Отправка инфы обо всех объектах карты

			model.CountGamers += 1;
			writingCountGames();

			model.ListUsers[num].Items[1] = new Grenade();
			model.ListUsers[num].Items[2] = new Item();
			model.ListUsers[num].Items[3] = new Item();
			model.ListUsers[num].Items[4] = new Item();
			model.ListUsers[num].Items[5] = new Item();
			model.ListUsers[num].Items[6] = new Item();

			Thread Producerthread = new Thread(new ParameterizedThreadStart(Producer));
			Producerthread.Start(num);

		}

		public bool SendingInformationAboutObjects(int num)
		{
			try
			{
				GetNumber gNumber = new GetNumber();
				gNumber.num = number;
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
			catch (Exception) { return false; }

		}

		public void InfoUsers(object tc)
		{
			TcpClient tcp = (TcpClient)tc;
			Thread.Sleep(200);
			NetworkStream nStream = tcp.GetStream();
			bool PrivateWorkingThread = true;
			while (workingThread && PrivateWorkingThread)
			{
				try
				{
					AngelToZone angel = new AngelToZone();
					angel.listUserInfo = model.ListUsers;
					GetBulletsInfo buletsInfo = new GetBulletsInfo();
					lock (model.ListBullet)
						buletsInfo.listBulets = model.ListBullet.ToList<BulletInfo>();
					GetGrenadesInfo grenadesInfo = new GetGrenadesInfo();
					lock (model.ListGrenade)
						grenadesInfo.grenadesInfo = model.ListGrenade.ToList<GrenadeInfo>();

					CTransfers.Writing(angel, nStream);
					CTransfers.Writing(buletsInfo, nStream);
					CTransfers.Writing(grenadesInfo, nStream);
					Thread.Sleep(10);
				}
				catch (System.IO.IOException err)
				{
					ErrorEvent(err.Message + " |Ошибка в ControllerServer, методе InfoUsers");
					PrivateWorkingThread = false;
				}
			}


		}

		private void Server_FormClosing(object sender, FormClosingEventArgs e)
		{
			StopServer();
		}

		public void createdZone()
		{
			model.Map.NextZone.startCenterZone(model.Map.MapBorders); //Создаст зону внутри игровой области
			model.Map.NextZone.TimeTocompression = 10;
			model.Map.NextZone.ZoneRadius = (int)model.Map.MapBorders.Height / 2;

			model.Map.PrevZone.ZoneCenterCoordinates = new Point(model.Map.MapBorders.Width / 2, model.Map.MapBorders.Height / 2);//Создаст зону внутри игровой области
			model.Map.PrevZone.ZoneRadius = (int)model.Map.MapBorders.Height / 4 * 3;
		}

		public void writingCountGames()
		{
			for (int i = 0; i < model.ListUsers.Count; i++)
			{
				if (model.ListUsers[i] != null)
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
					if (model.ListUsers[i] != null)
					{

						foreach (GeneralInfo g in model.ListGInfo)
						{
							if (g.Name == model.ListUsers[i].Name)
								g.Wins += 1;
						}
						CTransfers.Writing(new GetCountWinsInfo(), model.ListNs[i]);
						break;
					}
				}
				StopServer();
			}
		}

		public GeneralInfo PlayerCheck(List<GeneralInfo> listUser, GeneralInfo newUser)
		{
			try
			{
				if (listUser != null)
					foreach (GeneralInfo user in listUser)
					{

						if (user != null && user.Name == newUser.Name) return user;

					}
				return null;
			}
			catch { return null; }
		}

		public bool CheckData(List<GeneralInfo> listUser, GeneralInfo newUser)
		{
			try
			{
				if (listUser != null)
					foreach (GeneralInfo user in listUser)
					{
						if (user != null && user.Name == newUser.Name && user.Password == newUser.Password) return true;
					}
				return false;
			}
			catch { return false; }
		}

		public List<GeneralInfo> PlayerRead(GeneralInfo newUser)// Читает данные из файла
		{
			BinaryFormatter formatter = new BinaryFormatter();
			try
			{
				using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "usersInfo.dat", FileMode.Open))
				{
					return (List<GeneralInfo>)formatter.Deserialize(fs);
				}
			}
			catch (Exception)
			{
				ErrorEvent("Произошло создание нового списка для зарегестрированных пользователей");
				List<GeneralInfo> newList = new List<GeneralInfo>();
				if (newUser != null)
					newList.Add(newUser);
				else
				{
					GeneralInfo g = new GeneralInfo();
					g.Name = "admin";
					g.Password = "admin";
					newList.Add(g);
				}
				PlayerSave(newList);
				return newList;
			}
		}

		public bool PlayerSave(List<GeneralInfo> listUsers)
		{
			try
			{
				BinaryFormatter formatter = new BinaryFormatter();
				using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "usersInfo.dat", FileMode.OpenOrCreate))
				{
					formatter.Serialize(fs, listUsers);
				}
				return true;
			}
			catch { return false; }
		}

		public void LoadMap()
		{		
				BinaryFormatter formatter = new BinaryFormatter();
				OpenFileDialog sn = new OpenFileDialog();
				sn.DefaultExt = ".dat";
				sn.Filter = "Text files(*.dat)|*.dat|All files(*.*)|*.*";
				sn.InitialDirectory = @"C:\Users\Василий\Desktop\Exaxt\PUBGEXACT\MapEdit\MapEdit\Maps";
				DialogResult res = sn.ShowDialog();
				if (res == DialogResult.Cancel)
					return ;
				if (res == DialogResult.OK)
				{
					string NameFile = sn.FileName;
					try
					{
						using (FileStream fs = new FileStream(NameFile, FileMode.Open))
						{
							Map m = (Map)formatter.Deserialize(fs);
							model.Map = m;
						}
					}
					catch (Exception err)
					{
						ErrorEvent(err.Message + " |Ошибка в ControllerServer, методе LoadMap");
					}
				}
				Random random = new Random();
				for (int i = 0; i < model.ListUsers.Count; i++)
				{
					if (model.ListUsers[i] != null)
					{
						SendingInformationAboutObjects(i);//Отправляется инфа обо всех объектах новоый карты

						do
							model.ListUsers[i] = new UserInfo(new Point(random.Next(2, model.Map.MapBorders.Width - 2), random.Next(2, model.Map.MapBorders.Height - 2)));
						while (model.Map.bordersForUsers[model.ListUsers[i].userLocation.X, model.ListUsers[i].userLocation.Y]);
					}
				}
		}

		public void Producer(object obj)
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
					while (workingServer && workingThread)
					{
						string tmpString = CTransfers.Reading(model.ListNs[num]);

						SecureQueue.Enqueue(JsonConvert.DeserializeObject<ProcessingServer>(tmpString, CTransfers.jss));


						manualResetEvent.Set();

					}
					timerMove.Stop();

				}
				catch (System.IO.IOException)
				{
					ErrorEvent("Отключение игрока в Producer");
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
					writingCountGames();


				}
			}
			catch
			{
				ErrorEvent("Ложный вызов продюсера");
				timerMove.Stop();
			}

		}

		public void Consumer(object obj)
		{
			Thread.Sleep(100);
			ProcessingServer processing;
			while (workingServer && workingThread)
			{
				manualResetEvent.WaitOne();
				if (SecureQueue.Count > 0)
				{

					SecureQueue.TryDequeue(out processing);
					if (processing != null)
					{
						try
						{
							processing.Process(model);
						}
						catch
						{
							ErrorEvent("Не прошла команда от отключенного игрока " + processing.num);
						}
					}

					Thread.Yield();
				}
				else { manualResetEvent.Reset(); }

			}
		}

		public bool GenerateItems()
		{
			try
			{
				Random n = new Random();
				List<Item> ListItems = new List<Item>();
				int Count = model.Map.MapBorders.Height * model.Map.MapBorders.Width / 450000;

				for (int i = 0; i < Count; i++)
				{
					NormalShotgun gun = new NormalShotgun();
					Thread.Sleep(7);
					gun.Location = new Point(n.Next(0, model.Map.MapBorders.Width), n.Next(0, model.Map.MapBorders.Height));
					gun.IdItem = ListItems.Count;
					ListItems.Add(gun);
				}

				for (int i = 0; i < Count; i++)
				{
					NormalGun gun = new NormalGun();
					Thread.Sleep(8);
					gun.Location = new Point(n.Next(0, model.Map.MapBorders.Width), n.Next(0, model.Map.MapBorders.Height));
					gun.IdItem = ListItems.Count;
					ListItems.Add(gun);
				}

				for (int i = 0; i < Count; i++)
				{
					Grenade grenade = new Grenade();
					Thread.Sleep(8);
					grenade.Location = new Point(n.Next(0, model.Map.MapBorders.Width), n.Next(0, model.Map.MapBorders.Height));
					grenade.IdItem = ListItems.Count;
					ListItems.Add(grenade);
				}

				for (int i = 0; i < Count; i++)
				{
					NormalPistol pistol = new NormalPistol();
					Thread.Sleep(8);
					pistol.Location = new Point(n.Next(0, model.Map.MapBorders.Width), n.Next(0, model.Map.MapBorders.Height));
					pistol.IdItem = ListItems.Count;
					ListItems.Add(pistol);
				}

				model.Map.ListItems = ListItems;
				return true;
			}
			catch { return false; }
		}

		public void SendingItemsInfo()
		{
			foreach (NetworkStream nStream in model.ListNs)
			{
				GetInfoItems items = new GetInfoItems();
				items.listItems = model.Map.ListItems;
				CTransfers.Writing(items, nStream);
			}
		}
	}
}
