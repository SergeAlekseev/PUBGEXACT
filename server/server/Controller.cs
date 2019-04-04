using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace server
{
	class Controller
	{


		bool workingThread;
		bool workingServer;
		short number; //Model

		Model model = new Model();
		TcpListener PublicHost; // Это тоже в модел,наверное. И вообще в клиенте тоже много данных из контроллера можно перенести в модел  и обращаться к ним через модел _!__!__!__!_!_!_!__!_!__



		public Controller(Model model)
		{
			this.model = model;
		}

		private void timerMove_Tick(bool moveUp, bool moveDown, bool moveLeft, bool moveRight, int num) //Здесь будет выполняться перемещение игрока
		{
			if (workingThread)
			{
				if (model.ListUsers[num].userLocation.Y != 0 && (moveUp)) model.ListUsers[num].userLocation.Y -= 1; //Вниз
				if (model.ListUsers[num].userLocation.Y != 600 && (moveDown)) model.ListUsers[num].userLocation.Y += 1; //Вверх
				if (model.ListUsers[num].userLocation.X != 0 && (moveLeft)) model.ListUsers[num].userLocation.X -= 1; //Влево
				if (model.ListUsers[num].userLocation.X != 600 && (moveRight)) model.ListUsers[num].userLocation.X += 1;// Вправо	
			}
		}

		public void start()
		{
			Thread startThread = new Thread(new ParameterizedThreadStart(StartServer));
			startThread.Start();
		}

		public void StartServer(object tmpObject)//Controller
		{
			if (!workingServer)
			{
				number = -1;
				TcpListener host = new TcpListener(IPAddress.Any, 1337);
				PublicHost = host;
				host.Start();
				model.ListUsers = new List<UserInfo>();
				workingServer = true;

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

					UserInfo userInfoTmp = new UserInfo(new Point(300, 300));
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

		public void StopServer()//Controller
		{
			if (workingServer)
			{
				workingServer = false;
				workingThread = false;
				PublicHost.Stop();
				byte[] numberUser = new byte[1];
				foreach (NetworkStream ns in model.ListNs)
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
										+ Math.Pow(userInfo.mouseLocation.Y - userInfo.userLocation.Y, 2)) / 2;
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
						Thread.Sleep(5);
					}
					else { break; }
				}
			}


		}

		public void Bullet(object tmpObject)
		{
			bool flagBreak= false;
			BulletInfo bulletInfo = (BulletInfo)tmpObject;
			double X = bulletInfo.location.X, Y = bulletInfo.location.Y;
			X += bulletInfo.speedX;
			bulletInfo.location.X = (int)X;
			Y += bulletInfo.speedY;
			bulletInfo.location.Y = (int)Y;
			for (int i = 0; i < 300; i++)
			{
				X += bulletInfo.speedX;
				bulletInfo.location.X = (int)X;
				Y += bulletInfo.speedY;
				bulletInfo.location.Y = (int)Y;
				for (int j = 0; j < model.ListUsers.Count; j++)
				{
					if (Math.Abs(model.ListUsers[j].userLocation.X - X) <= 3 && Math.Abs(model.ListUsers[j].userLocation.Y - Y) <= 3)
					{
						byte[] popad = new byte[1];
						popad[0] = 6;
						model.ListUsers[j].hp -= 20;
						flagBreak = true;
						break;
					}
				}
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

			TcpClient tcp = (TcpClient)tc;
			NetworkStream nStream = tcp.GetStream();

			int num = number;   //шанс ошибки при одновременном подключении
			byte[] numberUser = new byte[1];
			numberUser[0] = (byte)num;
			nStream.Write(numberUser, 0, 1);

			int countReadingBytes = 0;
			bool PrivateWorkingThread = true;

			System.Timers.Timer timerMove = new System.Timers.Timer();
			timerMove.Interval = 20;
			timerMove.Elapsed += (x, y) => { timerMove_Tick(moveUp, moveDown, moveLeft, moveRight, num); };
			timerMove.Start();

			Thread Shoting = new Thread(new ParameterizedThreadStart(ShotUser));

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

									case Action.action.stopUp: moveUp = false; break;
									case Action.action.stopDown: moveDown = false; break;
									case Action.action.stopLeft: moveLeft = false; break;
									case Action.action.stopRight: moveRight = false; break;

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
								if (!model.ListUsers[num].flagShoting && !model.ListUsers[num].flagWaitShoting)
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
										Thread.Sleep(5);
										model.ListUsers[num].flagWaitShoting = false;
									});
									t.Start();
								}
								break;
							}
						case 5:
							{
								string tmpString = Reading(nStream);
								model.ListUsers[num].mouseLocation = JsonConvert.DeserializeObject<Point>(tmpString); ;
								break;
							}
					}
				}
				catch (System.IO.IOException)
				{
					model.ListUsers[num].flagShoting = false;
					lock (model.ListUsers)
					{
						model.ListUsers.RemoveAt(num);
						model.ListUsers.Insert(num, null); // <--------- Здесь костыль(вместо каждого удалённого элемента вставляется пустой)
					}
					PrivateWorkingThread = false;
					timerMove.Stop();
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
	}
}
