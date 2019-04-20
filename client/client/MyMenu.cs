using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
	public partial class MyMenu : Form
	{
		Model model = new Model();
		CMyMenu Controller;

		public MyMenu(string Name, string Pass, string ip)
		{
			InitializeComponent();
			Controller = new CMyMenu(model);
			Controller.JoinUser(Name, Pass);
			InfoIP.Text = ip;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				if (Controller.Connect(InfoIP.Text) && !Controller.flagGame)
				{
					TcpClient client = new TcpClient(InfoIP.Text, 3337);
					NetworkStream nStream = client.GetStream();
					Client form = new Client(model.GInfo.Name, model.GInfo.Password, InfoIP.Text);
					form.Show();
					this.Hide();
				}
				else
				{
					MessageBox.Show("Подождите, игра идёт");
				}
			}
			catch
			{
				MessageBox.Show("IP недоступен");
			}
		}

		private void MyMenu_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void MyMenu_Load(object sender, EventArgs e)
		{
			if (Controller.Connect(InfoIP.Text))
			{
				InfoName.Text = model.GInfo.Name;
				InfoKills.Text = "" + model.GInfo.Kills;
				InfoWins.Text = "" + model.GInfo.Wins;
				dies.Text = "" + model.GInfo.Dies;
				if (Controller.flagGame)
				{
					MessageBox.Show("Подождите, игра идёт");
				}
			}
			else
			{
				InfoName.Text = "Неверный пароль";
				bPlay.Enabled = false;
			}

		}
	}
}
