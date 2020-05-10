using ClassLibrary;
using System;
using System.Net.Sockets;

namespace BotLibrary1
{
	public class Bot
	{
		TcpClient client;
		LoginTransfer transfer;
		private CMyMenu controllerMenu;

		public Bot()
		{
			transfer = new LoginTransfer();
			controllerMenu = new CMyMenu(model);
			controller = new ControllerClient(model);
		}

		public bool join(string ip, string name, string pass)
		{
			controllerMenu.setNameAndPass(name, pass);

			//1. Авторизация бота
			if (!login(ip, 2337, name, pass)) return false;

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

		public bool login(String ip, int port, String name, String pass)
		{
			try
			{
				TcpClient client = new TcpClient(ip, port);
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
