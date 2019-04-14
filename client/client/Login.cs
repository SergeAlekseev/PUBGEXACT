using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
	public partial class Login : Form
	{
		public Login()
		{
			InitializeComponent();
		}

		private void Join_Click(object sender, EventArgs e)
		{
			try
			{
				TcpClient client = new TcpClient(ip.Text, 3337);
				NetworkStream nStream = client.GetStream();
				MyMenu form = new MyMenu(tName.Text, Pass.Text, ip.Text);
				form.Show();
				this.Hide();
			}
			catch
			{
				MessageBox.Show("Невозможно подключиться к заданному IP");
			}
		}

		private void SelectionOfLetters(object sender, EventArgs e)
		{
			string pattern = @"([0-9]{1,3}[\.]){3}[0-9]{1,3}";
			Regex regex = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
			ToolStripTextBox R = sender as ToolStripTextBox;
			if (!regex.IsMatch(R.Text))
			{ ip.Text = "25.53.91.50"; }
		}

		private void KeyPress(object sender, KeyPressEventArgs e)
		{
			char l = e.KeyChar;
			if ((l < '0' || l > '9') && l != '\b' && l != '.')
			{
				e.Handled = true;
			}
		}
	}
}
