using System;
using System.Windows.Forms;

namespace BotForm
{
	public partial class Form1 : Form
	{
		BotImpl bot;
		bool botStart = false;
		public Form1(String ip)
		{
			InitializeComponent();
			serverIp.Text = ip;
			name.Text = "autoBot";
			password.Text = "111";

			bot = new BotImpl();
			startBot(null, null);
		}

		public Form1()
		{
			InitializeComponent();
			bot = new BotImpl();
		}

		private void startBot(object sender, EventArgs e)
		{
			try
			{
				this.label1.Text += "Начали\n"; ;

				if (bot.join(serverIp.Text, name.Text, password.Text))
				{
					this.label1.Text += "Подключився\n";
					botStart = true;
				}
				this.label1.Text += "Закончили\n";

			}
			catch (Exception ex)
			{
				this.label1.Text += "\n" + ex + "\n";
			}
		}

		private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			char l = e.KeyChar;
			if ((l < '0' || l > '9') && l != '\b' && l != '.')
			{
				e.Handled = true;
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (botStart)
				bot.disconect();
		}
	}
}
