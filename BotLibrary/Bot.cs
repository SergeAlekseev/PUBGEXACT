﻿using ClassLibrary;
using System;
using System.Drawing;
using Action = ClassLibrary.Action;
using System.Net.Sockets;
using System.Threading;
using Timer = System.Timers.Timer;
using System.Threading.Tasks;

namespace BotLibrary
{
	abstract public class Bot
	{

		private delegate bool ConnectD(string ip);
		private event ConnectD ConnectEvent;

		private delegate void DisconnectD();
		private event DisconnectD DisconnectEvent;

		private delegate void ActionD();
		private event ActionD ActionEvent;

		private delegate void MouseLocatinD(Point MouseLocatin);
		private event MouseLocatinD MouseLocatinEvent;

		private delegate void ShotUpD();
		private event ShotUpD ShotUpEvent;

		private delegate void ShotDownD();
		private event ShotDownD ShotDownEvent;

		private delegate double RotatateD();
		private event RotatateD RotatateEvent;

		private delegate void RechargeD();
		private event RechargeD RechargeEvent;

		private delegate bool СhangeItemD(byte num);
		private event СhangeItemD СhangeItemEvent;

		private delegate void MouseClickD();
		private event MouseClickD MouseClickEvent;

		static ModelClient model = new ModelClient();
		ControllerClient controller = new ControllerClient(model);


		private bool start = false;
		private bool MouseDown = false;
		private Timer defaultTimer;

		public enum kompas
		{
			Right, Left , Up, Down
		}

		TcpClient client;
		LoginTransfer transfer;
		private CMyMenu controllerMenu;

		public Bot()
		{
			creatData();
		}

		public Bot(string ip, string name, string pass)
		{
			creatData();

			join(ip, name, pass);
		}

		private void creatData()
		{
			transfer = new LoginTransfer();
			controllerMenu = new CMyMenu(model);
			controller = new ControllerClient(model);
		}
		abstract protected void doBot(ModelClient model);
		private void connectToServer(string Name, string Pass, string ip)
		{

			model.ThisUser = new UserInfo(new Point(300, 300));
			controller.JoinUser(Name, Pass);

			model.ThisUser.Name = model.GInfo.Name; //Предаю имя игрока для будущей проверки

			ActionEvent += controller.PressKey;
			createDefaultTimer();

			ConnectEvent += controller.Connect;
			DisconnectEvent += controller.Disconnect;

			ShotUpEvent += controller.ShotUp;
			ShotDownEvent += controller.ShotDown;

			MouseLocatinEvent += controller.WriteMouseLocation;
			RotatateEvent += controller.mouseMove;
			СhangeItemEvent += controller.ChangeItem;
			RechargeEvent += controller.Recharge;

			controller.ErrorConnect += ErrorConnect;

			ConnectEvent(ip); //Подключается к тому же, к чему была подключена форма меню
			start = true;
		}

		private void createDefaultTimer()
		{
			defaultTimer = new Timer(100);
			defaultTimer.Elapsed += /*async*/(sender, e) => /*await Task.Run(() =>*/ doBot(model)/*)*/;
			defaultTimer.Start();
		}

		private void ErrorConnect()
		{
			//"Неверно введен IP"; 
		}

		public void moveKompasStart(kompas kompas) //Обработчик нажатия на кнопку 
		{
			switch (kompas)
			{
				case kompas.Up: model.Action.actionThishUser = Action.action.moveUp; ActionEvent(); break;
				case kompas.Down: model.Action.actionThishUser = Action.action.moveDown; ActionEvent(); break;
				case kompas.Right: model.Action.actionThishUser = Action.action.moveRight; ActionEvent(); break;
				case kompas.Left: model.Action.actionThishUser = Action.action.moveLeft; ActionEvent(); break;
			}
		}
		/// <summary>
		/// При вызове этого метода ваш персонаж совершит перезарядку оружия
		/// </summary>
		public void rechange()
		{
			RechargeEvent();
		}

		public void changeItem(byte num)
		{
			СhangeItemEvent(num); 
		}

		public void moveKompasStop(kompas kompas) //Обработчик  отпускания кнопки
		{
			switch (kompas)
			{
				case kompas.Up: model.Action.actionThishUser = Action.action.stopUp; ActionEvent(); break;
				case kompas.Down: model.Action.actionThishUser = Action.action.stopDown; ActionEvent(); break;
				case kompas.Right: model.Action.actionThishUser = Action.action.stopRight; ActionEvent(); break;
				case kompas.Left: model.Action.actionThishUser = Action.action.stopLeft; ActionEvent(); break;
			}
		}

		/// <summary>
		/// Этот метод заставит идти вашего персонажа в указанном направлении заданное время
		/// </summary>
		/// <param name="komp">Направление в которое нужно двигаться</param>
		/// <param name="time">Время в миллисекундах</param>
		public void moveToSomeTime(kompas komp, int time)
		{
			moveKompasStart(komp);
			Thread.Sleep(time);
			moveKompasStop(komp);
		}

		/// <summary>
		/// Этот метод заставит идти вашего персонажа в указанном направлении, пока не достигнет указанной точки
		/// </summary>
		/// /// <param name="currentLocation"></param>
		/// <param name="destination"></param>
		public void moveToPoint(Point currentLocation, Point destination)
		{
			Point offset = new Point(destination.X - currentLocation.X, destination.Y - currentLocation.Y);
			kompas horizontalDirection;
			kompas verticalDirection;

			if (offset.X > 0) horizontalDirection = kompas.Right;
			else horizontalDirection = kompas.Left;

			if (offset.Y < 0) verticalDirection = kompas.Up;
			else verticalDirection = kompas.Down;

			moveKompasStart(horizontalDirection);
			moveKompasStart(verticalDirection);
			Thread.Sleep(75);
			moveKompasStop(horizontalDirection);
			moveKompasStop(verticalDirection);
		}

		public void sprintON()
		{
			model.Action.actionThishUser = Action.action.shiftUp; ActionEvent();
		}

		public void sprintOFF()
		{
			model.Action.actionThishUser = Action.action.shiftDown; ActionEvent();
		}


		public void disconect() //Вызвать событие в Controller
		{
			DisconnectEvent();
		}


		public void shotOn()
		{
			if (!MouseDown)
			{
				ShotDownEvent();
				MouseDown = true;
			}
		}

		public void ShotOff()
		{
			if (MouseDown)
			{
				ShotUpEvent();
				MouseDown = false;
			}
		}

		public bool join(string ip, string name, string pass)
		{
			controllerMenu.setNameAndPass(name, pass);

			//1. Авторизация бота
			if (!login(ip, name, pass)) return false;

			//2. Подключение бота на сервер
			return joinOnServer(ip, name, pass);
		}
		
		/// <summary>
		/// Connect to game on server
		/// </summary>
		private bool joinOnServer(string ip, string name, string pass)
		{
			try
			{
				if (controllerMenu.connect(ip) && !controllerMenu.flagGame)
				{
					TcpClient client = new TcpClient(ip, 3337);
					NetworkStream nStream = client.GetStream();
					connectToServer(name, pass, ip);
					return true;
				}
				else
				{
					//Add log
					//	MessageBox.Show("Подождите, игра идёт");
					return false;
				}
			}
			catch
			{
				//Add log
				// MessageBox.Show("IP недоступен");
				return false;
			}
		}

		private bool login(String ip, String name, String pass)
		{
			try
			{
				TcpClient client = new TcpClient(ip, 3337);
				if (correctData(ip, name, pass))
				{
					NetworkStream nStream = client.GetStream();
					return true;
				}
				else
				{
					return false;
					//MessageBox.Show("Неверный логин или пароль");
				}
			}
			catch (Exception err)
			{
				//MessageBox.Show(err.Message);
				return false;
			}
		}

		private bool correctData(String ip, String name, String pass)
		{
			client = new TcpClient(ip, 2337);
			NetworkStream nStream = client.GetStream();
			GeneralInfo gi = new GeneralInfo();
			gi.Name = name;
			gi.Password = pass;
			transfer.writing(nStream, gi, 10);
			if (transfer.readingStream(nStream, gi)) return true;
			else return false;
		}
	}
	
}