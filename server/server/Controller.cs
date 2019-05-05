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
using System.Runtime.Serialization.Json;
using ClassLibrary;
using Action = ClassLibrary.Action;
using System.Collections.Concurrent;
using server.Processings;
using Newtonsoft.Json.Linq;

namespace server
{
	class Controller
	{


		bool workingThread;
		bool workingServer;
		short number; //Model

		TcpListener PublicHost, PublicHost2, PublicHost3;
		System.Timers.Timer timerZone, timerUsersInZone;

		public delegate void StartServerD(string text);
		public event StartServerD StartServerEvent;

		public delegate void StartGameD(string text);
		public event StartGameD StartGameEvent;

		public delegate void StopServerD(string text);
		public event StopServerD StopServerEvent;
		Thread ConsumerThread;
		ManualResetEvent manualResetEvent;
		ConcurrentQueue<Processing> SecureQueue = new ConcurrentQueue<Processing>(); //___________________________________

		public void StartGame()
		{
			if (!Model.workingGame && workingServer)
			{
				Model.workingGame = true;

				createdZone();

				for (int i = 0; i < Model.ListUsers.Count; i++)
				{
					if (Model.ListUsers[i] != null)
					{
						CTransfers.Writing(Model.Map.NextZone, 9, Model.ListNs[i]); // Инфа  о стартовой зоне
						CTransfers.Writing(Model.Map.PrevZone, 10, Model.ListNs[i]);
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

		public Controller()
		{
			CTransfers.jss.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
			CTransfers.jss.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
			CTransfers.jss.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
			CTransfers.jss.Formatting = Newtonsoft.Json.Formatting.Indented;
		}

		private void timerMove_Tick(bool moveUp, bool moveDown, bool moveLeft, bool moveRight, bool shift, int num) //Здесь будет выполняться перемещение игрока
		{
			byte speed = 1;
			if (shift)
			{
				speed *= 2;
			}
			try
			{
				if (Model.workingGame && Model.ListUsers[num] != null)
				{
					if ((moveUp) && Model.ListUsers[num].userLocation.Y - Model.Map.MapBorders.Y > 2 && !Model.Map.bordersForUsers[Model.ListUsers[num].userLocation.X, Model.ListUsers[num].userLocation.Y - speed]) Model.ListUsers[num].userLocation.Y -= speed; //Вниз
					if ((moveDown) && Model.ListUsers[num].userLocation.Y - Model.Map.MapBorders.Width < -2 && !Model.Map.bordersForUsers[Model.ListUsers[num].userLocation.X, Model.ListUsers[num].userLocation.Y + speed]) Model.ListUsers[num].userLocation.Y += speed; //Вверх
					if ((moveLeft) && Model.ListUsers[num].userLocation.X - Model.Map.MapBorders.X > 2 && !Model.Map.bordersForUsers[Model.ListUsers[num].userLocation.X - speed, Model.ListUsers[num].userLocation.Y]) Model.ListUsers[num].userLocation.X -= speed; //Влево
					if ((moveRight) && Model.ListUsers[num].userLocation.X - Model.Map.MapBorders.Height < -2 && !Model.Map.bordersForUsers[Model.ListUsers[num].userLocation.X + speed, Model.ListUsers[num].userLocation.Y]) Model.ListUsers[num].userLocation.X += speed;// Вправо	
				}
			}
			catch
			{

			}
		}

		private void timerZone_Tick()
		{
			if (Model.Map.NextZone.TimeTocompression > 0)
			{
				Model.Map.NextZone.TimeTocompression -= 1;
			}
			else
			{
				timerZone.Stop();
				double x = Model.Map.PrevZone.ZoneCenterCoordinates.X, y = Model.Map.PrevZone.ZoneCenterCoordinates.Y, radius = Model.Map.PrevZone.ZoneRadius; ;
				double koef = Math.Sqrt(Math.Pow(Model.Map.PrevZone.ZoneCenterCoordinates.X - Model.Map.NextZone.ZoneCenterCoordinates.X, 2)
										+ Math.Pow(Model.Map.PrevZone.ZoneCenterCoordinates.Y - Model.Map.NextZone.ZoneCenterCoordinates.Y, 2)) / 750;
				double k = Math.Sqrt(Math.Pow(Model.Map.PrevZone.ZoneCenterCoordinates.X - Model.Map.NextZone.ZoneCenterCoordinates.X, 2)
										+ Math.Pow(Model.Map.PrevZone.ZoneCenterCoordinates.Y - Model.Map.NextZone.ZoneCenterCoordinates.Y, 2)) / koef;
				double speedX = (Model.Map.PrevZone.ZoneCenterCoordinates.X - Model.Map.NextZone.ZoneCenterCoordinates.X) / k;
				double speedY = (Model.Map.PrevZone.ZoneCenterCoordinates.Y - Model.Map.NextZone.ZoneCenterCoordinates.Y) / k;

				double speedRadius = (double)(Model.Map.PrevZone.ZoneRadius - Model.Map.NextZone.ZoneRadius) / 750;
				while (Model.Map.PrevZone.ZoneRadius > Model.Map.NextZone.ZoneRadius && Model.workingGame)
				{
					x -= speedX;
					Model.Map.PrevZone.ZoneCenterCoordinateX = (int)x;
					y -= speedY;
					Model.Map.PrevZone.ZoneCenterCoordinateY = (int)y;
					radius -= speedRadius;
					Model.Map.PrevZone.ZoneRadius = (int)radius;
					for (int i = 0; i < Model.ListUsers.Count; i++)
					{
						if (Model.ListUsers[i] != null && Model.ListNs[i].CanWrite)
						{
							CTransfers.Writing(Model.Map.PrevZone, 10, Model.ListNs[i]);
						}
					}
					Thread.Sleep(40);
				}
				if (Model.workingGame)
				{
					Model.Map.PrevZone = Model.Map.NextZone;
					Model.Map.NextZone = new Zone();
					Model.Map.NextZone.ZoneRadius = (int)Model.Map.PrevZone.ZoneRadius / 2;
					Model.Map.NextZone.NewCenterZone(Model.Map.MapBorders, Model.Map.PrevZone.ZoneCenterCoordinates, Model.Map.PrevZone.ZoneRadius);//не страдает ли тут MVC?
					for (int i = 0; i < Model.ListUsers.Count; i++)
					{
						if (Model.ListUsers[i] != null)
							CTransfers.Writing(Model.Map.NextZone, 9, Model.ListNs[i]);
					}
					Model.Map.NextZone.TimeTocompression = 60;
					timerZone.Start();
				}
			}
		}

		private void timerUsersInZone_Tick()
		{
			for (int i = 0; i < Model.ListUsers.Count; i++)
			{
				if (Model.ListUsers[i] != null)
				{
					if (Math.Sqrt(Math.Pow(Model.ListUsers[i].userLocation.X - Model.Map.NextZone.ZoneCenterCoordinates.X, 2) + Math.Pow(Model.ListUsers[i].userLocation.Y - Model.Map.NextZone.ZoneCenterCoordinates.Y, 2)) > Model.Map.NextZone.ZoneRadius)
					{
						Model.ListUsers[i].flagZone = true;
					}
					else Model.ListUsers[i].flagZone = false;

					if (Model.Map.PrevZone != null && Math.Sqrt(Math.Pow(Model.ListUsers[i].userLocation.X - Model.Map.PrevZone.ZoneCenterCoordinates.X, 2) + Math.Pow(Model.ListUsers[i].userLocation.Y - Model.Map.PrevZone.ZoneCenterCoordinates.Y, 2)) > Model.Map.PrevZone.ZoneRadius)
					{
						Model.ListUsers[i].hp -= 2;
						if (Model.ListUsers[i].hp <= 0)
						{
							CTransfers.Writing("ZONA", 7, Model.ListNs[i]);

							foreach (GeneralInfo g in Model.ListGInfo)
							{
								if (g.Name == Model.ListUsers[i].Name)
									g.Dies += 1;
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
				Model.ListGInfo = PlayerRead(null);
			}
			Thread startThread = new Thread(new ParameterizedThreadStart(StartServer));
			startThread.Start();
		}

		public void StartServer(object tmpObject)//Controller
		{

			if (!workingServer && !Model.workingGame)
			{
				workingServer = true;
				Random random = new Random();
				RandomBushs();
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


				RandomBox();


				PublicHost = host;
				host.Start();
				Model.ListUsers = new List<UserInfo>();

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
					}
					catch
					{
						if (tc != null)
							tc.Close();
						break;
					}
					number++;

					UserInfo userInfoTmp;
					do
						userInfoTmp = new UserInfo(new Point(random.Next(2, Model.Map.MapBorders.Width - 2), random.Next(2, Model.Map.MapBorders.Height - 2)));
					while (Model.Map.bordersForUsers[userInfoTmp.userLocation.X, userInfoTmp.userLocation.Y]);
					userInfoTmp.userNumber = number;

					lock (Model.ListUsers)
					{
						Model.ListUsers.Add(userInfoTmp);
					}



					Model.ListNs.Add(tc.GetStream());
					Model.ListMove.Add(new MMove());
					Model.ListShoting.Add(new Thread(new ParameterizedThreadStart(ShotUser)));
					Thread thread = new Thread(new ParameterizedThreadStart(PlayUser));
					thread.Start(tc);



					Thread thread2 = new Thread(new ParameterizedThreadStart(InfoUsers));
					thread2.Priority = ThreadPriority.Highest;
					thread2.Start(tc);
				}
				Thread.Sleep(1000);
				Model.Remove();
			}
		}



		public void StopServer()
		{
			if (workingServer)
			{
				PlayerSave(Model.ListGInfo);
				SecureQueue = new ConcurrentQueue<Processing>();
				number = -1;
				PublicHost.Stop();
				PublicHost2.Stop();
				PublicHost3.Stop();
				if (Model.workingGame)
				{

					timerZone.Stop();
					timerUsersInZone.Stop();
				}
				byte[] numberUser = new byte[1];
				foreach (NetworkStream ns in Model.ListNs)
				{
					if (ns != null)
					{
						numberUser[0] = 5;
						try
						{
							ns.Write(numberUser, 0, 1);
							ns.Close();
						}
						catch { }
					}
				}

				workingServer = false;
				workingThread = false;
				Model.Remove();
				Model.workingGame = false;
				Thread.Sleep(1000);
				StopServerEvent("Сервер отключен");
			}
		}

		public void RandomBushs()
		{
			Random random = new Random();
			for (int i = 0; i < Model.Map.MapBorders.Height * Model.Map.MapBorders.Width / 10000; i++)
			{
				Model.Map.ListBush.Add(new Bush(random.Next(Model.Map.MapBorders.Width), random.Next(Model.Map.MapBorders.Height)));
			}
		}

		public void RandomBox()
		{
			bool flag = true;
			Random random = new Random();
			for (int i = 0; i < Model.Map.MapBorders.Height * Model.Map.MapBorders.Width / 50000;)
			{
				Box box = new Box(random.Next(13, Model.Map.MapBorders.Width - 13), random.Next(13, Model.Map.MapBorders.Height - 13));
				foreach (Box b in Model.Map.ListBox)
				{
					if (Math.Abs(b.Location.X - box.Location.X) < Box.size || Math.Abs(b.Location.Y - box.Location.Y) < Box.size)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					Model.Map.ListBox.Add(box);
					for (int k = box.Location.X - 10; k < box.Location.X + 10; k++)
					{
						for (int j = box.Location.Y - 10; j < box.Location.Y + 10; j++)
						{
							Model.Map.bordersForBullets[k, j] = true;
						}
					}
					for (int k = box.Location.X - 10 - 3; k < box.Location.X + 10 + 3; k++)
					{
						for (int j = box.Location.Y - 10 - 3; j < box.Location.Y + 10 + 3; j++)
						{
							Model.Map.bordersForUsers[k, j] = true;
						}
					}
					i++;
				}
				flag = true;
			}
		}

		public void MenuConnecting(object tc)//Controller
		{
			while (workingServer)
			{
				try
				{
					(tc as TcpListener).AcceptTcpClient();
				}
				catch
				{
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
							if (!Model.workingGame)
							{
								if (tmpUser == null && Model.ListGInfo.Count > 0)
								{
									Model.ListGInfo.Add(new GeneralInfo());
									Model.ListGInfo[Model.ListGInfo.Count - 1].Name = newUser.Name;
									Model.ListGInfo[Model.ListGInfo.Count - 1].Password = newUser.Password;

									PlayerSave(Model.ListGInfo);
									CTransfers.Writing(Model.ListGInfo[Model.ListGInfo.Count - 1], 10, nStream);
								}
								else
								{
									if (CheckData(Model.ListGInfo, newUser))
									{
										Model.ListGInfo = PlayerRead(newUser);
										CTransfers.Writing(tmpUser, 10, nStream);
									}
									else
									{
										CTransfers.Writing("1", 11, nStream);
									}
									//Если такой игрок уже есть , то при правильном пароле выдать всю инфу об игроке
								}
							}
							else
							{
								if (tmpUser == null && Model.ListGInfo.Count > 0)
								{
									Model.ListGInfo.Add(new GeneralInfo());
									Model.ListGInfo[Model.ListGInfo.Count - 1].Name = newUser.Name;
									Model.ListGInfo[Model.ListGInfo.Count - 1].Password = newUser.Password;

									PlayerSave(Model.ListGInfo);
									CTransfers.Writing(Model.ListGInfo[Model.ListGInfo.Count - 1], 12, nStream);
								}
								else
								{
									if (CheckData(Model.ListGInfo, newUser))
									{
										Model.ListGInfo = PlayerRead(newUser);
										CTransfers.Writing(tmpUser, 12, nStream);
									}
									else
									{
										CTransfers.Writing("1", 12, nStream);
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
						lock (Model.ListBullet)
						{
							Model.ListBullet.Add(bi);
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
							lock (Model.ListBullet)
							{
								Model.ListBullet.Add(bi);
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
				for (int j = 0; j < Model.ListUsers.Count; j++)
				{
					if (Model.ListUsers[j] != null && Math.Abs(Model.ListUsers[j].userLocation.X - X) <= 9 && Math.Abs(Model.ListUsers[j].userLocation.Y - Y) <= 9)
					{
						byte[] popad = new byte[1];
						popad[0] = 6;
						Model.ListUsers[j].hp -= bulletInfo.damage;
						flagBreak = true;
						if (Model.ListUsers[j].hp <= 0)
						{
							byte[] flagDie = new byte[1];
							flagDie[0] = 7;
							CTransfers.Writing(bulletInfo.owner, 7, Model.ListNs[j]);

							foreach (GeneralInfo g in Model.ListGInfo)
							{
								if (g.Name == Model.ListUsers[j].Name)
									g.Dies += 1;
								if (g.Name == bulletInfo.owner)
									g.Kills += 1;
							}

							Kill kill = new Kill();
							kill.killer = bulletInfo.owner;
							kill.dead = Model.ListUsers[j].Name;

							for (int k = 0; k < Model.ListUsers.Count; k++)
							{
								if (Model.ListUsers[k] != null)
								{
									if (Model.ListUsers[k].Name == bulletInfo.owner)
										Model.ListUsers[k].kills += 1;
									CTransfers.Writing(kill, 20, Model.ListNs[k]);
								}
							}

						}
						break;
					}
				}
				try
				{
					if (Model.Map.bordersForBullets[bulletInfo.location.X, bulletInfo.location.Y])
					{
						flagBreak = true;
					}
				}
				catch { break; }
				if (flagBreak) break;
				Thread.Sleep(20);
			}
			lock (Model.ListBullet)
			{
				Model.ListBullet.Remove(bulletInfo);
			}
		}



		public void PlayUser(object tc)//Controller
		{
			//MMove mmove = new MMove();
			//bool moveUp = false;
			//bool moveDown = false;
			//bool moveLeft = false;
			//bool moveRight = false;
			//bool shift = false;

			//bool PrivateWorkingThread = true;

			//Thread Shoting = new Thread(new ParameterizedThreadStart(ShotUser));



			int num = number;   //шанс ошибки при одновременном подключении


			CTransfers.Writing(number, 44, Model.ListNs[num]);
			Thread.Sleep(1000);
			CTransfers.Writing(Model.Map.ListBush, 6, Model.ListNs[num]); // Отправка инфы о кустах
			Thread.Sleep(100);
			CTransfers.Writing(Model.Map.MapBorders, 8, Model.ListNs[num]); //Инфа о границах карты
			Thread.Sleep(100);
			CTransfers.Writing(Model.Map.ListBox, 12, Model.ListNs[num]); // Отправка инфы о коробках


			Model.CountGamers += 1;
			writingCountGames();

			Model.ListUsers[num].Items[1] = new NormalGun();
			Model.ListUsers[num].Items[2] = new NormalShotgun();

			Thread Producerthread = new Thread(new ParameterizedThreadStart(Producer));
			Producerthread.Start(num);


			//while (workingServer && workingThread && PrivateWorkingThread)
			//{
			//	try
			//	{
			//		byte[] typeCommand = new byte[1];
			//		nStream.Read(typeCommand, 0, 1);

			//		switch (typeCommand[0])
			//		{
			//			case 1:
			//				{
			//					PlayerMovementsInfo(ref mmove.moveUp, ref mmove.moveDown, ref mmove.moveLeft, ref mmove.moveRight, ref mmove.shift, nStream);

			//					break;
			//				}
			//			case 2:
			//				{
			//					PingInfo(nStream);
			//					break;
			//				}
			//			case 3://УХХХХХХ
			//				{
			//					Model.ListUsers[num].flagRecharge = false;
			//					string tmpString = CTransfers.Reading(nStream);
			//					if (!Model.ListUsers[num].flagShoting && !Model.ListUsers[num].flagWaitShoting && Model.workingGame)
			//					{
			//						Model.ListUsers[num].flagShoting = true;
			//						Shoting = new Thread(new ParameterizedThreadStart(ShotUser));
			//						Shoting.Start(Model.ListUsers[num]);
			//					}
			//					break;
			//				}
			//			case 4:// УУ НЕ ПОНЕМАЮ, выноси их сам в методы, чтобы легче читалось ( микро-рефакторинг)
			//				{
			//					string tmpString = CTransfers.Reading(nStream);
			//					if (Model.ListUsers[num].flagShoting && !Model.ListUsers[num].flagWaitShoting)
			//					{
			//						Model.ListUsers[num].flagWaitShoting = true;
			//						Shoting.Abort();
			//						Model.ListUsers[num].flagShoting = false;
			//						Thread t = new Thread(() =>
			//						{
			//							Thread.Sleep(Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Time);
			//							Model.ListUsers[num].flagWaitShoting = false;
			//						});
			//						t.Start();
			//					}
			//					break;
			//				}
			//			case 5:
			//				{
			//					GetPlayersMousesLocation(nStream, num);
			//					break;
			//				}
			//			case 13:
			//				{
			//					GetPlayersAngels(nStream, num);
			//					break;
			//				}
			//			case 14: //Да это уже рофл какой-то
			//				{
			//					GetUserName(nStream, num);
			//					break;
			//				}
			//			case 66:
			//				{
			//					string tmpString = CTransfers.Reading(nStream);
			//					Model.ListUsers[num].flagRecharge = false;
			//					Model.ListUsers[num].flagWaitShoting = true;
			//					Shoting.Abort();
			//					Model.ListUsers[num].flagShoting = false;
			//					Thread t = new Thread(() =>
			//					{
			//						Thread.Sleep(Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Time);
			//						Model.ListUsers[num].flagWaitShoting = false;
			//					});
			//					t.Start();
			//					Model.ListUsers[num].thisItem = JsonConvert.DeserializeObject<byte>(tmpString, CTransfers.jss);
			//					break;
			//				}
			//			case 67:
			//				{
			//					string tmpString = CTransfers.Reading(nStream);
			//					if (Model.ListUsers[num].Items[Model.ListUsers[num].thisItem] is Weapon)
			//					{
			//						Model.ListUsers[num].flagRecharge = true;
			//						Shoting.Abort();
			//						Model.ListUsers[num].flagShoting = false;
			//						Thread t = new Thread(() =>
			//						{
			//							int time = 0;
			//							while (Model.ListUsers[num].flagRecharge)
			//							{
			//								time++;
			//								Thread.Sleep(100);
			//								if (time >= Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].TimeReloading)
			//								{
			//									switch ((Model.ListUsers[num].Items[Model.ListUsers[num].thisItem] as Weapon).TypeBullets)
			//									{
			//										case Weapon.typeBullets.Gun:
			//											{
			//												Model.ListUsers[num].GunBullets += Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count;
			//												Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count = Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].MaxCount;
			//												Model.ListUsers[num].GunBullets -= Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].MaxCount;
			//												if (Model.ListUsers[num].GunBullets < 0)
			//												{
			//													Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count += Model.ListUsers[num].GunBullets;
			//													Model.ListUsers[num].GunBullets = 0;
			//												}
			//												break;
			//											}
			//										case Weapon.typeBullets.Pistol:
			//											{
			//												Model.ListUsers[num].PistolBullets += Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count;
			//												Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count = Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].MaxCount;
			//												Model.ListUsers[num].PistolBullets -= Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].MaxCount;
			//												if (Model.ListUsers[num].PistolBullets < 0)
			//												{
			//													Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count += Model.ListUsers[num].PistolBullets;
			//													Model.ListUsers[num].PistolBullets = 0;
			//												}
			//												break;
			//											}
			//										case Weapon.typeBullets.Shotgun:
			//											{
			//												Model.ListUsers[num].ShotgunBullets += Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count;
			//												Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count = Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].MaxCount;
			//												Model.ListUsers[num].ShotgunBullets -= Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].MaxCount;
			//												if (Model.ListUsers[num].ShotgunBullets < 0)
			//												{
			//													Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count += Model.ListUsers[num].ShotgunBullets;
			//													Model.ListUsers[num].ShotgunBullets = 0;
			//												}
			//												break;
			//											}
			//									}

			//									Model.ListUsers[num].flagRecharge = false;
			//								}
			//							}


			//						});
			//						t.Start();
			//					}
			//					break;
			//				}
			//		}
			//	}
			//	catch (System.IO.IOException)
			//	{
			//		if (Model.ListUsers.Count != 0 && Model.ListUsers[num] != null)
			//		{
			//			Model.ListUsers[num].flagShoting = false;
			//			lock (Model.ListUsers)
			//			{
			//				Model.ListUsers.RemoveAt(num);
			//				Model.ListUsers.Insert(num, null); // <--------- Здесь костыль(вместо каждого удалённого элемента вставляется пустой)
			//			}
			//		}
			//		Model.CountGamers -= 1;
			//		writingCountGames();
			//		PrivateWorkingThread = false;
			//		timerMove.Stop();
			//		Shoting.Abort();
			//	}
			//	catch
			//	{

			//	}
			//}
		}

		private void GetUserName(NetworkStream nStream, int num)
		{
			string tmpString = CTransfers.Reading(nStream);
			Model.ListUsers[num].Name = JsonConvert.DeserializeObject<string>(tmpString, CTransfers.jss);
		}

		private void GetPlayersAngels(NetworkStream nStream, int num)
		{
			string tmpString = CTransfers.Reading(nStream);
			Model.ListUsers[num].Rotate = JsonConvert.DeserializeObject<double>(tmpString, CTransfers.jss);
		}

		private void GetPlayersMousesLocation(NetworkStream nStream, int num)
		{
			string tmpString = CTransfers.Reading(nStream);
			Model.ListUsers[num].mouseLocation = JsonConvert.DeserializeObject<Point>(tmpString, CTransfers.jss);
		}

		private static void PingInfo(NetworkStream nStream)
		{
			byte[] ping = new byte[1];
			ping[0] = 2;
			lock (nStream)
			{
				nStream.Write(ping, 0, 1);
			}
		}

		private void PlayerMovementsInfo(ref bool moveUp, ref bool moveDown, ref bool moveLeft, ref bool moveRight, ref bool shift, NetworkStream nStream)
		{
			string tmpString = CTransfers.Reading(nStream);
			Action act = JsonConvert.DeserializeObject<Action>(tmpString);
			switch (act.act)
			{
				case Action.action.moveUp: moveUp = true; break;
				case Action.action.moveDown: moveDown = true; break;
				case Action.action.noveLeft: moveLeft = true; break;
				case Action.action.moveRight: moveRight = true; break;
				case Action.action.shiftDown: shift = true; break;

				case Action.action.stopUp: moveUp = false; break;
				case Action.action.stopDown: moveDown = false; break;
				case Action.action.stopLeft: moveLeft = false; break;
				case Action.action.stopRight: moveRight = false; break;
				case Action.action.shiftUp: shift = false; break;
			}
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
					CTransfers.Writing(Model.ListUsers, 1, nStream);
					CTransfers.Writing(Model.ListBullet, 3, nStream);
					Thread.Sleep(20);
				}
				catch (System.IO.IOException)
				{
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
			Model.Map.NextZone.startCenterZone(Model.Map.MapBorders); //Создаст зону внутри игровой области
			Model.Map.NextZone.TimeTocompression = 10;
			Model.Map.NextZone.ZoneRadius = (int)Model.Map.MapBorders.Height / 2;

			Model.Map.PrevZone.ZoneCenterCoordinates = new Point(Model.Map.MapBorders.Width / 2, Model.Map.MapBorders.Height / 2);//Создаст зону внутри игровой области
			Model.Map.PrevZone.ZoneRadius = (int)Model.Map.MapBorders.Height / 4 * 3;
		}

		public void writingCountGames()
		{
			for (int i = 0; i < Model.ListUsers.Count; i++)
			{
				if (Model.ListUsers[i] != null)
				{
					CTransfers.Writing(Model.CountGamers, 21, Model.ListNs[i]);
				}
			}
			if (Model.CountGamers == 1 && Model.workingGame)
			{
				for (int i = 0; i < Model.ListUsers.Count; i++)
				{
					if (Model.ListUsers[i] != null)
					{
						CTransfers.Writing("", 33, Model.ListNs[i]);
						foreach (GeneralInfo g in Model.ListGInfo)
						{
							if (g.Name == Model.ListUsers[i].Name)
								g.Wins += 1;
						}
						break;
					}
				}
				Thread.Sleep(2000);
				StopServer();
				Thread.Sleep(2000);
				start();
			}
		}

		public GeneralInfo PlayerCheck(List<GeneralInfo> listUser, GeneralInfo newUser)
		{
			if (listUser != null)
				foreach (GeneralInfo user in listUser)
				{

					if (user != null && user.Name == newUser.Name) return user;

				}
			return null;
		}
		public bool CheckData(List<GeneralInfo> listUser, GeneralInfo newUser)
		{
			foreach (GeneralInfo user in listUser)
			{
				if (user != null && user.Name == newUser.Name && user.Password == newUser.Password) return true;
			}
			return false;
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



		public void PlayerSave(List<GeneralInfo> listUsers)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "usersInfo.dat", FileMode.OpenOrCreate))
			{
				formatter.Serialize(fs, listUsers);
			}
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
				return;
			if (res == DialogResult.OK)
			{
				string NameFile = sn.FileName;
				try
				{
					using (FileStream fs = new FileStream(NameFile, FileMode.Open))
					{
						Map m = (Map)formatter.Deserialize(fs);
						Model.Map = m;
					}
				}
				catch (Exception err)
				{
					MessageBox.Show(err.Message);
				}
			}
			Random random = new Random();
			for (int i = 0; i < Model.ListUsers.Count; i++)
			{
				if (Model.ListUsers[i] != null)
				{
					CTransfers.Writing(Model.Map.ListBush, 6, Model.ListNs[i]); // Отправка инфы о кустах
					Thread.Sleep(100);
					CTransfers.Writing(Model.Map.MapBorders, 8, Model.ListNs[i]); //Инфа о границах карты
					Thread.Sleep(100);
					CTransfers.Writing(Model.Map.ListBox, 12, Model.ListNs[i]); // Отправка инфы о коробках
					do
						Model.ListUsers[i] = new UserInfo(new Point(random.Next(2, Model.Map.MapBorders.Width - 2), random.Next(2, Model.Map.MapBorders.Height - 2)));
					while (Model.Map.bordersForUsers[Model.ListUsers[i].userLocation.X, Model.ListUsers[i].userLocation.Y]);
				}
			}
		}

		public void Producer(object obj)
		{
			int num = (int)obj;


			System.Timers.Timer timerMove = new System.Timers.Timer();
			timerMove.Interval = 15;
			timerMove.Elapsed += (x, y) => { timerMove_Tick(Model.ListMove[num].moveUp, Model.ListMove[num].moveDown, Model.ListMove[num].moveLeft, Model.ListMove[num].moveRight, Model.ListMove[num].shift, num); };
			timerMove.Start();

			try
			{
				while (workingServer && workingThread)
				{
					string tmpString = CTransfers.Reading(Model.ListNs[num]);
					try
					{
						SecureQueue.Enqueue(JsonConvert.DeserializeObject<Processing>(tmpString, CTransfers.jss));
					}
					catch { }
					manualResetEvent.Set();

				}

			}
			catch (System.IO.IOException)
			{

				if (Model.ListUsers.Count != 0 && Model.ListUsers[num] != null)
				{
					Model.ListUsers[num].flagShoting = false;
					lock (Model.ListUsers)
					{
						Model.ListUsers.RemoveAt(num);
						Model.ListUsers.Insert(num, null);
					}
				}
				Model.CountGamers -= 1;
				writingCountGames();
				timerMove.Stop();

			}

		}

		public void Consumer(object obj)
		{
			Thread.Sleep(1000);
			MessageBox.Show("Костыль №1");
			Processing processing;
			while (workingServer && workingThread)
			{

				manualResetEvent.WaitOne();
				if (SecureQueue.Count > 0)
				{
					
					SecureQueue.TryDequeue(out processing);
					if (processing != null)
					{
						
						processing.Process();
					}
					
					Thread.Yield();
				}
				else { manualResetEvent.Reset(); }

			}
		}
	}
}
