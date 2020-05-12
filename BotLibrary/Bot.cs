using ClassLibrary;
using System;
using System.Drawing;
using Action = ClassLibrary.Action;
using System.Net.Sockets;

namespace BotLibrary
{
	public class Bot
	{

		public delegate bool ConnectD(string ip);
		public event ConnectD ConnectEvent;

		public delegate void DisconnectD();
		public event DisconnectD DisconnectEvent;

		public delegate void ActionD();
		public event ActionD ActionEvent;

		public delegate void MouseLocatinD(Point MouseLocatin);
		public event MouseLocatinD MouseLocatinEvent;

		public delegate void ShotUpD();
		public event ShotUpD ShotUpEvent;

		public delegate void ShotDownD();
		public event ShotDownD ShotDownEvent;

		public delegate double RotatateD();
		public event RotatateD RotatateEvent;

		public delegate void RechargeD();
		public event RechargeD RechargeEvent;

		public delegate bool СhangeItemD(byte num);
		public event СhangeItemD СhangeItemEvent;

		public delegate void MouseClickD();
		public event MouseClickD MouseClickEvent;

		static ModelClient model = new ModelClient();
		ControllerClient controller = new ControllerClient(model);

		
		bool start = false;
		bool MouseDown = false;

		enum kompas
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

		public void connectToServer(string Name, string Pass, string ip)
		{

			model.ThisUser = new UserInfo(new Point(300, 300));
			controller.JoinUser(Name, Pass);

			model.ThisUser.Name = model.GInfo.Name; //Предаю имя игрока для будущей проверки

			ActionEvent += controller.PressKey;
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

		public void ErrorConnect()
		{
			//"Неверно введен IP"; 
		}

		private void moveKompasStart(kompas kompas) //Обработчик нажатия на кнопку 
		{
			switch (kompas)
			{
				case kompas.Up: model.Action.actionThishUser = Action.action.moveUp; ActionEvent(); break;
				case kompas.Down: model.Action.actionThishUser = Action.action.moveDown; ActionEvent(); break;
				case kompas.Right: model.Action.actionThishUser = Action.action.moveRight; ActionEvent(); break;
				case kompas.Left: model.Action.actionThishUser = Action.action.noveLeft; ActionEvent(); break;
			}
		}

		private void rechange()
		{
			RechargeEvent();
		}

		private void changeItem(byte num)
		{
			СhangeItemEvent(num); 
		}

		private void moveKompasStop(kompas kompas) //Обработчик  отпускания кнопки
		{
			switch (kompas)
			{
				case kompas.Up: model.Action.actionThishUser = Action.action.stopUp; ActionEvent(); break;
				case kompas.Down: model.Action.actionThishUser = Action.action.stopDown; ActionEvent(); break;
				case kompas.Right: model.Action.actionThishUser = Action.action.stopRight; ActionEvent(); break;
				case kompas.Left: model.Action.actionThishUser = Action.action.stopLeft; ActionEvent(); break;
			}
		}


		private void sprintON()
		{
			model.Action.actionThishUser = Action.action.shiftUp; ActionEvent();
		}

		private void sprintOFF()
		{
			model.Action.actionThishUser = Action.action.shiftDown; ActionEvent();
		}


		private void disconect() //Вызвать событие в Controller
		{
			DisconnectEvent();
		}


		private void shotOn()
		{
			if (!MouseDown)
			{
				ShotDownEvent();
				MouseDown = true;
			}
		}

		private void ShotOff()
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
		/// 
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

		public bool login(String ip, String name, String pass)
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
