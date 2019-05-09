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
using ClassLibrary;

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

		ModelServer model = new ModelServer();
		ControllerServer controller;

		ToolStripMenuItem fileItem;
		public ListBox listBox = new ListBox();

		public Server()
		{
			InitializeComponent();
			#region CreatedFormListBox

			fileItem = new ToolStripMenuItem("Файл");

			ToolStripMenuItem newItem = new ToolStripMenuItem("Список ошибок");
			newItem.Click += menuItem_Click;
			fileItem.DropDownItems.Add(newItem);


			menuStrip1.Items.Add(fileItem);
			#endregion

			controller = new ControllerServer(model);

			StopServerEvent += controller.StopServer;
			StartServerEvent += controller.start;
			StartGameEvent += controller.StartGame;

			CTransfers.ErrorEvent += AddError;

			controller.ErrorEvent += AddError;
			controller.StopServerEvent += writeStatus;
			controller.StartServerEvent += writeStatus;
			controller.StartGameEvent += writeStatus;
		}

		private void menuItem_Click(object sender, EventArgs e)
		{
			Form listBoxForm = new Form();
			listBoxForm.Size = new Size(300, 200);
			listBoxForm.Location = new Point(this.Size.Height / 2, this.Size.Width / 2);

			ListBox localListBox = new ListBox();
			localListBox.Size = new Size(300, 200);
			localListBox.Location = new Point(0, 0);
			localListBox.Dock = DockStyle.Fill;
			

			listBoxForm.Controls.Add(localListBox);

			listBoxForm.Show();
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
			try
			{
				BeginInvoke(new MethodInvoker(delegate
				{
					status.Text = text;
				}));
			}
			catch (Exception err)
			{
				AddError(err.Message + "| Ошибка в отправке статуса работы форме сервера");
			}
		}

		public void AddError(string Error)
		{
			listBox.Items.Add(Error);
		}

		private void maps_Click(object sender, EventArgs e)
		{
			controller.LoadMap();
		}

		private void Server_Load(object sender, EventArgs e)
		{

		}
	}
}
