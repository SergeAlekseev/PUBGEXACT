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
		Model model = new Model();
		Controller controller = new Controller();

		public Server()
		{
			InitializeComponent();
		}

		private void start_Click(object sender, EventArgs e)//Controller и событие
		{
			controller.start();
		}

		
		private void stop_Click(object sender, EventArgs e)//Controller и событие
		{
			if (model.WorkingServer)
			{
				controller.StopServer();
				BeginInvoke(new MethodInvoker(delegate
				{
					status.Text = "Сервер отключен";
				}));
			}
		}

		private void Server_FormClosing(object sender, FormClosingEventArgs e)//Controller
		{
			controller.StopServer();
		}
	}
}
