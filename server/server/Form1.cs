﻿using System;
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

		public Server()
		{

			InitializeComponent();

			controller = new ControllerServer(model);

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
			try
			{
				BeginInvoke(new MethodInvoker(delegate
				{
					status.Text = text;
				}));
			}
			catch
			{

			}
		}

		private void maps_Click(object sender, EventArgs e)
		{
			controller.LoadMap();
		}
	}
}
