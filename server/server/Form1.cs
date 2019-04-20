using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
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

		public delegate void StartGameD();
		public event StartGameD StartGameEvent;

		public delegate void StopServerD();
		public event StopServerD StopServerEvent;

		static Model model = new Model();
		Controller controller = new Controller(model);

		public Server()
		{
			InitializeComponent();

			StopServerEvent += controller.StopServer;
			StartServerEvent += controller.start;
			StartGameEvent += controller.StartGame;

			controller.StopServerEvent += writeStatus;
			controller.StartServerEvent += writeStatus; 
			controller.StartGameEvent += writeStatus; 

		}

		private void start_Click(object sender, EventArgs e)
		{
			StartServerEvent();
		}

		private void stop_Click(object sender, EventArgs e)
		{
			StopServerEvent();
		}
		private void Server_FormClosing(object sender, FormClosingEventArgs e)
		{
			StopServerEvent();
		}

		private void startGame_Click(object sender, EventArgs e)
		{
			StartGameEvent();
		}

		private void writeStatus(string text)
		{
			BeginInvoke(new MethodInvoker(delegate
			{
				status.Text = text;
			}));
		}
	}
}
