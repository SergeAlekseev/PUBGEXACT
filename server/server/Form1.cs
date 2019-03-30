using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using client;
using System.Threading;
using Action = client.Action;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;

namespace server
{
	public partial class Server : Form
	{

		public delegate void StartServerD();
		public event StartServerD StartServerEvent;

		public delegate void StopServerD();
		public event StopServerD StopServerEvent;

		static Model model = new Model();
		Controller controller = new Controller(model);

		public Server()
		{
			InitializeComponent();

			StopServerEvent += controller.StopServer;
			StartServerEvent += controller.start;
		}

		private void start_Click(object sender, EventArgs e)
		{
			StartServerEvent();

			status.Text = "Сервер включен";
		}

		private void stop_Click(object sender, EventArgs e)
		{
			StopServerEvent();
			
			status.Text = "Сервер отключен";
		}
		private void Server_FormClosing(object sender, FormClosingEventArgs e)
		{
			StopServerEvent();
		}
	}
}
