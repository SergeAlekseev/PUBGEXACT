using System;
using System.Drawing;
using System.Windows.Forms;
using ClassLibrary;
using System.Diagnostics;
using System.IO;

namespace server
{
	public partial class Server : Form
	{

		public delegate void StartServerD();
		public event StartServerD StartServerEvent;

		public delegate bool StartGameD();
		public event StartGameD StartGameEvent;

		public delegate void StopServerD();
		public event StopServerD StopServerEvent;

		ModelServer model = new ModelServer();
		ControllerServer controller;

		ToolStripMenuItem fileItem;
		public ListBox listBox = new ListBox();

		public static String LOG_FILE_NAME = "logs.log";
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

			controller.StopServerEvent += writeStatus;
			controller.StartServerEvent += writeStatus;
			controller.StartGameEvent += writeStatus;

			Debug.Listeners.Add(new TextWriterTraceListener(LOG_FILE_NAME, "log"));
			Debug.AutoFlush = true;
			Debug.WriteLine("Server start");
			Debug.Indent();

			Trace.Indent();
			Trace.WriteLine("============================================================================");
			Trace.WriteLine("Match start");			
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
			localListBox.Items.AddRange(listBox.Items);

			listBoxForm.Controls.Add(localListBox);

			listBoxForm.Show();
		}

		private void start_Click(object sender, EventArgs e)
		{
			StartServerEvent();
			Debug.WriteLine("The game is open to connect");
		}

		private void stop_Click(object sender, EventArgs e)
		{
			StopServerEvent();
			Debug.WriteLine("Game stop");
		}
		private void Server_FormClosing(object sender, FormClosingEventArgs e)
		{
			StopServerEvent();
			Debug.WriteLine("Server close");
		}

		private void startGame_Click(object sender, EventArgs e)
		{
			if (StartGameEvent()) 
			{

				listBox1.DataSource = model.ListUsers;
				listBox1.DisplayMember = "Name";
				Debug.WriteLine("Game start");
			}
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
				Debug.WriteLine(err.Message + "| Ошибка в отправке статуса работы форме сервера");
			}
		}

		private void maps_Click(object sender, EventArgs e)
		{
			ControllersS.cMap.LoadMap();
		}

		private void Server_Load(object sender, EventArgs e)
		{

		}

		private void clickUserFromBoxToView(object sender, EventArgs e)
		{
			Client form = new Client(model, (UserInfo)listBox1.SelectedItem);
			form.Show();
		}
	}
}
