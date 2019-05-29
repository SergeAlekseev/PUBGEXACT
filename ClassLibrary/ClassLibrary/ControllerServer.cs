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
		
		//Model

		public ModelServer model;

		TcpListener PublicHost, PublicHost2, PublicHost3;
		

		public delegate void StartServerD(string text);
		public event StartServerD StartServerEvent;

		public delegate void StartGameD(string text);
		public event StartGameD StartGameEvent;

		public delegate void StopServerD(string text);
		public event StopServerD StopServerEvent;

		public delegate void ErrorD(string Error);
		public event ErrorD ErrorEvent;

		
		

		public void StartGame() //основа
		{
			if (!model.workingGame && model.workingServer)
			{
				model.workingGame = true;

				ControllersS.cMap.createdZone();

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

				ControllersS.cPlay.timerZone = new System.Timers.Timer();
				ControllersS.cPlay.timerZone.Interval = 1000;
				ControllersS.cPlay.timerZone.Elapsed += (x, y) => { ControllersS.cPlay.timerZone_Tick(); };
				ControllersS.cPlay.timerZone.Start();

				ControllersS.cPlay.timerUsersInZone = new System.Timers.Timer();
				ControllersS.cPlay.timerUsersInZone.Interval = 1000;
				ControllersS.cPlay.timerUsersInZone.Elapsed += (x, y) => { ControllersS.cPlay.timerUsersInZone_Tick(); };
				ControllersS.cPlay.timerUsersInZone.Start();
				StartGameEvent("Игра идёт");
			}
		}

		public ControllerServer(ModelServer model)//основа
		{
			this.model = model;
			CTransfers.jss.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
			CTransfers.jss.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
			CTransfers.jss.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
			CTransfers.jss.Formatting = Newtonsoft.Json.Formatting.Indented;
			new ControllersS(model);
		}

		

		

		

		public void start()//основа
		{
			if (ControllersS.cInfoUsers.PlayerRead(null) != null)
			{
				model.ListGInfo = ControllersS.cInfoUsers.PlayerRead(null);
			}
			Thread startThread = new Thread(new ParameterizedThreadStart(StartServer));
			startThread.Start();
		}

		public void StartServer(object tmpObject)//основа
		{
			ControllersS.cPlay.SecureQueue = new ConcurrentQueue<ProcessingServer>();

			if (!model.workingServer && !model.workingGame)
			{
				model.workingServer = true;
				model.ListMove = new List<MMove>();
				Random random = new Random();
				model.number = -1;
				TcpListener host = new TcpListener(IPAddress.Any, 1337);

				TcpListener host2 = new TcpListener(IPAddress.Any, 2337);
				host2.Start();
				PublicHost2 = host2;
				Thread menuStarting = new Thread(new ParameterizedThreadStart(ControllersS.cStart.MenuStarting));
				menuStarting.Start(host2);

				TcpListener host3 = new TcpListener(IPAddress.Any, 3337);
				host3.Start();
				PublicHost3 = host3;
				Thread menuConnecting = new Thread(new ParameterizedThreadStart(ControllersS.cStart.MenuConnecting));
				menuConnecting.Start(host3);

				ControllersS.cMap.RandomBushs();
				ControllersS.cMap.RandomBox();
				ControllersS.cMap.RandomTree();
				ControllersS.cMap.GenerateItems();

				PublicHost = host;
				host.Start();
				model.ListUsers = new List<UserInfo>();

				ControllersS.cPlay.manualResetEvent = new ManualResetEvent(true);
				ControllersS.cPlay.ConsumerThread = new Thread(new ParameterizedThreadStart(ControllersS.cPlay.Consumer));
				ControllersS.cPlay.ConsumerThread.Start(ControllersS.cPlay.manualResetEvent);

				StartServerEvent("Сервер запущен");
				while (model.workingServer)
				{
					TcpClient tc = null;
					model.workingThread = true;//возможно сделать красивее
					try
					{
						tc = host.AcceptTcpClient();

						model.number++;

						UserInfo userInfoTmp;
						do
							userInfoTmp = new UserInfo(new Point(random.Next(2, model.Map.MapBorders.Width - 2), random.Next(2, model.Map.MapBorders.Height - 2)));
						while (model.Map.bordersForUsers[userInfoTmp.userLocation.X, userInfoTmp.userLocation.Y]);
						userInfoTmp.userNumber = model.number;

						lock (model.ListUsers)
						{
							model.ListUsers.Add(userInfoTmp);
						}



						model.ListNs.Add(tc.GetStream());
						model.ListMove.Add(new MMove());
						model.ListShoting.Add(new Thread(ControllersS.cPlay.InfoUsers));
						Thread thread = new Thread(new ParameterizedThreadStart(ControllersS.cPlay.PlayUser));
						thread.Start(tc);



						Thread thread2 = new Thread(new ParameterizedThreadStart(ControllersS.cPlay.InfoUsers));
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



		public void StopServer()//основа
		{
			if (model.workingServer)
			{
				ControllersS.cInfoUsers.PlayerSave(model.ListGInfo);
				model.number = -1;
				PublicHost.Stop();
				PublicHost2.Stop();
				PublicHost3.Stop();
				if (model.workingGame)
				{
					ControllersS.cPlay.timerZone.Stop();
					ControllersS.cPlay.timerUsersInZone.Stop();
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

				model.workingServer = false;
				model.workingThread = false;
				model.Remove();
				model.workingGame = false;
				ControllersS.cPlay.manualResetEvent.Set();
				Thread.Sleep(100);
				StopServerEvent("Сервер отключен");
			}
		}



		private void Server_FormClosing(object sender, FormClosingEventArgs e)//основа
		{
			StopServer();
		}

		
	}
}
