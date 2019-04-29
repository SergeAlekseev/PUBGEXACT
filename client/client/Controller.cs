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

		public delegate void ErrorConnectD();
		public event ErrorConnectD ErrorConnect;

		Model model;

		TcpClient client;// 25.46.244.0 
		NetworkStream nStream;
		Thread threadReading;

		Stopwatch PingWatch = new Stopwatch();
		bool threadStart = false;

		System.Timers.Timer timerPing = new System.Timers.Timer();
		public void JoinUser(string Name, string Password)
		{
			model.GInfo.Name = Name;
			model.GInfo.Password = Password;
		}

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
			if (threadStart)
				Writing(mouseLocation, 5);
		}

		public Controller(Model model) // Конструктор
		{
			this.model = model;
			timerPing.Elapsed += timerPing_Tick;
			timerPing.Interval = 2000;
			timerPing.Start();
		}

		private void timerPing_Tick(object sender, EventArgs e)
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

		private void ReadingStream()
		{
			while (threadStart)
			{

				byte[] typeCommand = new byte[1];
				try
				{
					nStream.Read(typeCommand, 0, 1);
				}
				catch
				{
					break;
				}

				switch (typeCommand[0])
				{
					case 1:
						{
							string tmpString = Reading(nStream);
							GetUserInfo(tmpString);
							model.AngelToZone = defineAngleZone(model.Map.NextZone.ZoneCenterCoordinates, model.ThisUser.userLocation);
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
							GetBulletsInfo();
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
					case 6:
						{
							GetBushesInfo();
							break;
						}
					case 7:
						{
							PlayerDeath();
							break;
						}
					case 8:
						{
							GetMapBordersInfo();
							break;
						}
					case 9:
						{
							GetZoneStartInfo();
							break;
						}
					case 10:
						{
							GetPrevZoneInfo();
							break;
						}
					case 12:
						{
							GetBoxesInfo();
							break;
						}
					case 13:
						{

							break;
						}
					case 20:
						{
							GetKillsInfo();
							break;
						}
					case 21:
						{
							GetCountGamesInfo();
							break;
						}
					case 33:
						{
							GetCountWinsInfo();
							break;
						}
						//case 10 и 11 	уже зарезервированы	
				}


			}

		}

		private void GetCountWinsInfo()
		{
			model.Win = true;
			string tmpString = Reading(nStream);
			nStream.Close();
			threadStart = false;
			client.Close();
			timerPing.Stop();
			threadReading.Abort();
		}

		private void GetCountGamesInfo()
		{
			string tmpString = Reading(nStream);
			model.CountGamers = JsonConvert.DeserializeObject<int>(tmpString);
		}

		private void GetKillsInfo()
		{
			string tmpString = Reading(nStream);
			Kill[] arrayKills = new Kill[3];
			arrayKills[2] = model.ArrayKills[1];
			arrayKills[1] = model.ArrayKills[0];
			arrayKills[0] = JsonConvert.DeserializeObject<Kill>(tmpString);
			model.ArrayKills = arrayKills;
		}

		private void GetBoxesInfo()
		{
			string tmpString = Reading(nStream);
			model.Map.ListBox = JsonConvert.DeserializeObject<List<Box>>(tmpString);
		}

		private void GetPrevZoneInfo()
		{
			string tmpString = Reading(nStream);
			model.Map.PrevZone = JsonConvert.DeserializeObject<Zone>(tmpString);
		}

		private void GetZoneStartInfo()
		{
			string tmpString = Reading(nStream);
			model.Map.PrevZone = model.Map.NextZone;
			model.Map.NextZone = JsonConvert.DeserializeObject<Zone>(tmpString);
		}

		private void GetMapBordersInfo()
		{
			string tmpString = Reading(nStream);
			model.Map.MapBorders = JsonConvert.DeserializeObject<Rectangle>(tmpString);
		}

		private void PlayerDeath()
		{
			model.Die = true;
			string tmpString = Reading(nStream);
			model.Killer = JsonConvert.DeserializeObject<string>(tmpString);
			nStream.Close();
			threadStart = false;
			client.Close();
			timerPing.Stop();
			threadReading.Abort();
		}

		private void GetBushesInfo()
		{
			string tmpString = Reading(nStream);
			model.Map.ListBush = JsonConvert.DeserializeObject<List<Bush>>(tmpString);
		}

		private void GetBulletsInfo()
		{
			string tmpString = Reading(nStream);
			model.ListBullet = JsonConvert.DeserializeObject<List<BulletInfo>>(tmpString);
		}

		private void GetUserInfo(string tmpString)
		{
			model.ListUsers = JsonConvert.DeserializeObject<List<UserInfo>>(tmpString);
			model.ListUsers[model.ThisUser.userNumber].userNumber = model.ThisUser.userNumber;
			model.ThisUser = model.ListUsers[model.ThisUser.userNumber];
		}

		public void PressKey()
		{
			if (threadStart)
			{
				Writing(model.Action, 1);
			}
		}

		public void Disconnect()
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

		public bool Connect(string ip)// Controller
		{
			if (!threadStart)
			{
				try
				{
					try
					{
						client = new TcpClient(ip, 1337);
						nStream = client.GetStream();
						threadStart = true;
						threadReading = new Thread(ReadingStream);
						threadReading.Start();
						byte[] number = new byte[1];
						nStream.Read(number, 0, 1);
						model.ThisUser.userNumber = number[0];
						return true;
					}
					catch
					{
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
			double angleDegree = defineAngle(model.MouseCoord,new Point(300,600));
			model.ThisUser.Rotate = angleDegree;

			if (threadStart)
			Writing(model.ThisUser.Rotate,13);

			return angleDegree;
		}

		public void setName(string Name)
		{
			Writing(Name,14);
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
			Point end2 = new Point { X = onePoint.X, Y = onePoint.Y+200 };
			Vector vector2 = Vector.FromPoints(start2, end2);

			double angleRad = Vector.CalculateAngleBetween(vector1, vector2);

			double angleDegree = angleRad / Math.PI * 180;
			if (onePoint.X < twoPoint.X) angleDegree = 360 - angleDegree;

			return angleDegree;
		}	
	}
}
