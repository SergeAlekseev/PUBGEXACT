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

namespace server
{
	class Controller
	{

		bool workingGame;
		bool workingThread;
		bool workingServer;
		short number; //Model

		Model model = new Model();
		TcpListener PublicHost, PublicHost2, PublicHost3;
		System.Timers.Timer timerZone, timerUsersInZone;

		public delegate void StartServerD(string text);
		public event StartServerD StartServerEvent;

		public delegate void StartGameD(string text);
		public event StartGameD StartGameEvent;

		public delegate void StopServerD(string text);
		public event StopServerD StopServerEvent;

		public void StartGame()
		{
			if (!workingGame && workingServer)
			{
				workingGame = true;

				createdZone();

				for (int i = 0; i < model.ListNs.Count; i++)
				{
					if (model.ListUsers[i] != null)
						Writing(model.Map.NextZone, 9, model.ListNs[i]); // Инфа  о стартовой зоне
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

		public Controller(Model model)
		{
			this.model = model;
		}

		private void timerMove_Tick(bool moveUp, bool moveDown, bool moveLeft, bool moveRight, bool shift, int num) //Здесь будет выполняться перемещение игрока
		{
			byte speed = 1;
			if (shift)
			{
				speed *= 2;
			}
			if (workingGame && model.ListUsers[num] != null)
			{
				if ((moveUp) && !model.Map.bordersForUsers[model.ListUsers[num].userLocation.X, model.ListUsers[num].userLocation.Y - speed] && model.ListUsers[num].userLocation.Y - model.Map.MapBorders.Y > 1) model.ListUsers[num].userLocation.Y -= speed; //Вниз
				if ((moveDown) && !model.Map.bordersForUsers[model.ListUsers[num].userLocation.X, model.ListUsers[num].userLocation.Y + speed] && model.ListUsers[num].userLocation.Y - model.Map.MapBorders.Width < -1) model.ListUsers[num].userLocation.Y += speed; //Вверх
				if ((moveLeft) && !model.Map.bordersForUsers[model.ListUsers[num].userLocation.X - speed, model.ListUsers[num].userLocation.Y] && model.ListUsers[num].userLocation.X - model.Map.MapBorders.X > 1) model.ListUsers[num].userLocation.X -= speed; //Влево
				if ((moveRight) && !model.Map.bordersForUsers[model.ListUsers[num].userLocation.X + speed, model.ListUsers[num].userLocation.Y] && model.ListUsers[num].userLocation.X - model.Map.MapBorders.Height < -1) model.ListUsers[num].userLocation.X += speed;// Вправо	
			}
		}

		private void timerZone_Tick()
		{
			if (model.Map.NextZone.TimeTocompression > 0)
			{
				model.Map.NextZone.TimeTocompression -= 1;
			}
			else
			{
				model.Map.PrevZone = model.Map.NextZone;
				model.Map.NextZone = new Zone();
				model.Map.NextZone.ZoneRadius = (int)model.Map.PrevZone.ZoneRadius / 2;
				model.Map.NextZone.NewCenterZone(model.Map.MapBorders, model.Map.PrevZone.ZoneCenterCoordinates, model.Map.PrevZone.ZoneRadius);//не страдает ли тут MVC?
				for (int i = 0; i < model.ListNs.Count; i++)
				{
					if (model.ListUsers[i] != null)
						Writing(model.Map.NextZone, 9, model.ListNs[i]);
				}
				model.Map.NextZone.TimeTocompression = 60;
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
							byte[] flagDie = new byte[1];
							flagDie[0] = 7;
							model.ListNs[i].Write(flagDie, 0, 1);
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

			if (!workingServer && !workingGame)
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
				model.ListUsers = new List<UserInfo>();

				
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
						userInfoTmp = new UserInfo(new Point(random.Next(2, model.Map.MapBorders.Width - 2), random.Next(2, model.Map.MapBorders.Height - 2)));
					while (model.Map.bordersForUsers[userInfoTmp.userLocation.X, userInfoTmp.userLocation.X]);
					userInfoTmp.userNumber = number;

					lock (model.ListUsers)
					{
						model.ListUsers.Add(userInfoTmp);
					}
					model.ListNs.Add(tc.GetStream());
					Thread thread = new Thread(new ParameterizedThreadStart(PlayUser));
					thread.Start(tc);

					Thread thread2 = new Thread(new ParameterizedThreadStart(InfoUsers));
					thread2.Start(tc);
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
				if (workingGame)
				{
					timerUsersInZone.Close();
					timerZone.Close();
				}
				byte[] numberUser = new byte[1];
				foreach (NetworkStream ns in model.ListNs)
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
				model.Remove();
				workingGame = false;
				Thread.Sleep(1000);
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
			for (int i = 0; i < model.Map.MapBorders.Height * model.Map.MapBorders.Width / 20000;)
			{
				Box box = new Box(random.Next(13, model.Map.MapBorders.Width - 13), random.Next(13, model.Map.MapBorders.Height - 13));
				foreach (Box b in model.Map.ListBox)
				{
					if (Math.Abs(b.Location.X - box.Location.X) < Box.size || Math.Abs(b.Location.Y - box.Location.Y) < Box.size)
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
					for (int k = box.Location.X - 10 - 3; k < box.Location.X + 10 + 3; k++)
					{
						for (int j = box.Location.Y - 10 - 3; j < box.Location.Y + 10 + 3; j++)
						{
							model.Map.bordersForUsers[k, j] = true;
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

							string tmpString = Reading(nStream);
							GeneralInfo newUser = JsonConvert.DeserializeObject<GeneralInfo>(tmpString);

							GeneralInfo tmpUser = PlayerCheck(PlayerRead(newUser), newUser);
							if (!workingGame)
							{
								if (tmpUser == null && model.ListGInfo.Count > 0)
								{
									model.ListGInfo.Add(new GeneralInfo());
									model.ListGInfo[model.ListGInfo.Count - 1].Name = newUser.Name;
									model.ListGInfo[model.ListGInfo.Count - 1].Password = newUser.Password;

									PlayerSave(model.ListGInfo);
									Writing(model.ListGInfo[model.ListGInfo.Count - 1], 10, nStream);
								}
								else
								{
									if (CheckData(model.ListGInfo, newUser))
									{
										model.ListGInfo = PlayerRead(newUser);
										Writing(tmpUser, 10, nStream);
									}
									else
									{
										Writing("1", 11, nStream);
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
									Writing(model.ListGInfo[model.ListGInfo.Count - 1], 12, nStream);
								}
								else
								{
									if (CheckData(model.ListGInfo, newUser))
									{
										model.ListGInfo = PlayerRead(newUser);
										Writing(tmpUser, 12, nStream);
									}
									else
									{
										Writing("1", 12, nStream);
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

			while (userInfo.flagShoting)
			{
				if (userInfo.userLocation != userInfo.mouseLocation)
				{
					BulletInfo bulletInfo = new BulletInfo(userInfo.userLocation);
					double k = Math.Sqrt(Math.Pow(userInfo.mouseLocation.X - userInfo.userLocation.X, 2)
										+ Math.Pow(userInfo.mouseLocation.Y - userInfo.userLocation.Y, 2)) / 4;
					bulletInfo.speedX = (userInfo.mouseLocation.X - userInfo.userLocation.X) / k;
					bulletInfo.speedY = (userInfo.mouseLocation.Y - userInfo.userLocation.Y) / k;


					if (userInfo.flagShoting)
					{
						Thread thread = new Thread(new ParameterizedThreadStart(Bullet));
						thread.Start(bulletInfo);
						lock (model.ListBullet)
						{
							model.ListBullet.Add(bulletInfo);
						}
						Thread.Sleep(200);
					}
					else { break; }
				}
			}


		}

		public void Bullet(object tmpObject)
		{
			bool flagBreak = false;
			BulletInfo bulletInfo = (BulletInfo)tmpObject;
			double X = bulletInfo.location.X, Y = bulletInfo.location.Y;
			X += bulletInfo.speedX;
			bulletInfo.location.X = (int)X;
			Y += bulletInfo.speedY;
			bulletInfo.location.Y = (int)Y;
			for (int i = 0; i < 150; i++)
			{
				X += bulletInfo.speedX;
				bulletInfo.location.X = (int)X;
				Y += bulletInfo.speedY;
				bulletInfo.location.Y = (int)Y;
				for (int j = 0; j < model.ListUsers.Count; j++)
				{
					if (model.ListUsers[j] != null && Math.Abs(model.ListUsers[j].userLocation.X - X) <= 3 && Math.Abs(model.ListUsers[j].userLocation.Y - Y) <= 3)
					{
						byte[] popad = new byte[1];
						popad[0] = 6;
						model.ListUsers[j].hp -= 20;
						flagBreak = true;
						if (model.ListUsers[j].hp <= 0)
						{
							byte[] flagDie = new byte[1];
							flagDie[0] = 7;
							model.ListNs[j].Write(flagDie, 0, 1);
							//model.ListGInfo[j-1].Dies += 1;  _______________пока без глобальной инфы
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
				model.ListBullet.Remove(bulletInfo);
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

		public void PlayUser(object tc)//Controller
		{
			bool moveUp = false;
			bool moveDown = false;
			bool moveLeft = false;
			bool moveRight = false;
			bool shift = false;

			bool PrivateWorkingThread = true;

			Thread Shoting = new Thread(new ParameterizedThreadStart(ShotUser));

			TcpClient tcp = (TcpClient)tc;
			NetworkStream nStream = tcp.GetStream();

			int num = number;   //шанс ошибки при одновременном подключении
			byte[] numberUser = new byte[1];
			numberUser[0] = (byte)num;
			nStream.Write(numberUser, 0, 1);

			Writing(model.Map.ListBush, 6, nStream); // Отправка инфы о кустах
			Writing(model.Map.MapBorders, 8, nStream); //Инфа о границах карты
			Writing(model.Map.ListBox, 12, nStream); // Отправка инфы о коробках

			System.Timers.Timer timerMove = new System.Timers.Timer();
			timerMove.Interval = 15;
			timerMove.Elapsed += (x, y) => { timerMove_Tick(moveUp, moveDown, moveLeft, moveRight, shift, num); };
			timerMove.Start();

			while (workingThread && PrivateWorkingThread)
			{
				try
				{
					byte[] typeCommand = new byte[1];
					nStream.Read(typeCommand, 0, 1);

					switch (typeCommand[0])
					{
						case 1:
							{
								string tmpString = Reading(nStream);
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

								break;
							}
						case 2:
							{
								byte[] ping = new byte[1];
								ping[0] = 2;
								lock (nStream)
								{
									nStream.Write(ping, 0, 1);
								}
								break;
							}
						case 3:
							{
								string tmpString = Reading(nStream);
								if (!model.ListUsers[num].flagShoting && !model.ListUsers[num].flagWaitShoting && workingGame)
								{
									model.ListUsers[num].flagShoting = true;
									Shoting = new Thread(new ParameterizedThreadStart(ShotUser));
									Shoting.Start(model.ListUsers[num]);
								}
								break;
							}
						case 4:
							{
								string tmpString = Reading(nStream);
								if (model.ListUsers[num].flagShoting && !model.ListUsers[num].flagWaitShoting)
								{
									model.ListUsers[num].flagWaitShoting = true;
									Shoting.Abort();
									model.ListUsers[num].flagShoting = false;
									Thread t = new Thread(() =>
									{
										Thread.Sleep(200);
										model.ListUsers[num].flagWaitShoting = false;
									});
									t.Start();
								}
								break;
							}
						case 5:
							{
								string tmpString = Reading(nStream);
								model.ListUsers[num].mouseLocation = JsonConvert.DeserializeObject<Point>(tmpString);
								break;
							}
						case 13:
							{
								string tmpString = Reading(nStream);
								model.ListUsers[num].Rotate = JsonConvert.DeserializeObject<double>(tmpString);					
								break;
							}
					}
				}
				catch (System.IO.IOException)
				{
					if (model.ListUsers.Count != 0 && model.ListUsers[num] != null)
					{
						model.ListUsers[num].flagShoting = false;
						lock (model.ListUsers)
						{
							model.ListUsers.RemoveAt(num);
							model.ListUsers.Insert(num, null); // <--------- Здесь костыль(вместо каждого удалённого элемента вставляется пустой)
						}
					}
					PrivateWorkingThread = false;
					timerMove.Stop();
					Shoting.Abort();
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
					Writing(model.ListUsers, 1, nStream);
					Writing(model.ListBullet, 3, nStream);
					Thread.Sleep(20);
				}
				catch (System.IO.IOException)
				{
					PrivateWorkingThread = false;
				}
			}
		}

		private void Writing(object obj, byte numComand, NetworkStream nStream)
		{
			string serialized = "";
			lock (obj)
			{
				serialized = JsonConvert.SerializeObject(obj);
			}
			byte[] massByts = Encoding.UTF8.GetBytes(serialized);
			byte[] countRead = BitConverter.GetBytes(massByts.Count());
			byte[] typeComand = new byte[1];
			typeComand[0] = numComand;

			lock (nStream)
			{
				nStream.Write(typeComand, 0, 1);//Отпраляет тип команды
				nStream.Write(countRead, 0, 4);//Отпраляет кол-во байт, которое сервер должен будет читать
				nStream.Write(massByts, 0, massByts.Count());
			}
		}

		private void Server_FormClosing(object sender, FormClosingEventArgs e)
		{
			StopServer();
		}

		public void createdZone()
		{
			model.Map.NextZone.startCenterZone(model.Map.MapBorders); //Создаст зону внутри игровой области
			model.Map.NextZone.TimeTocompression = 120;
			model.Map.NextZone.ZoneRadius = (int)model.Map.MapBorders.Height / 2;
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
					g.Name = "kek";
					g.Password = "-1";
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
	}
}
