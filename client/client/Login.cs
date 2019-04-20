using Newtonsoft.Json;
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
		TcpClient client;
		NetworkStream nStream1, nStream2;

		public Login()
		{
			InitializeComponent();
		}

		private void Join_Click(object sender, EventArgs e)
		{
			try
			{
				TcpClient client = new TcpClient(ip.Text, 3337);
				if (correctData())
				{
					nStream1 = client.GetStream();
					MyMenu form = new MyMenu(tName.Text, Pass.Text, ip.Text);
					form.Show();
					this.Hide();
				}
				else
				{
					MessageBox.Show("Неверный логин или пароль");
				}
			}
			catch
			{
				MessageBox.Show("Невозможно подключиться к заданному IP");
			}
		}

		private bool correctData()
		{
			client = new TcpClient(ip.Text, 2337);
			nStream2 = client.GetStream();
			GeneralInfo gi = new GeneralInfo();
			gi.Name = tName.Text;
			gi.Password = Pass.Text;
			Writing(gi, 10);
			if (ReadingStream(gi)) return true;
			else return false; 
		}

		private bool ReadingStream(GeneralInfo GInfo)
		{
			bool flag = false;
			while (!flag)
			{
				byte[] typeCommand = new byte[1];
				try
				{
					nStream2.Read(typeCommand, 0, 1);
				}
				catch
				{
					break;
				}
				if (typeCommand[0] == 10|| typeCommand[0] == 12)
				{
					string tmpString = Reading(nStream2);
					GInfo = JsonConvert.DeserializeObject<GeneralInfo>(tmpString);
					flag = true;
					return true;
				}
				else if (typeCommand[0] == 11)
				{
					flag = true;
					return false;
				}
				
			}
			return false;
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

		public string Reading(NetworkStream nStream)
		{
			byte[] countRead = new byte[4];
			int countReadingBytes = 0;
			while (countReadingBytes != 4)
				countReadingBytes += nStream.Read(countRead, countReadingBytes, countRead.Count() - countReadingBytes);

			countReadingBytes = 0;

			int lengthBytesRaed = BitConverter.ToInt32(countRead, 0);

			byte[] readBytes = new byte[lengthBytesRaed];


			while (countReadingBytes != lengthBytesRaed)
				countReadingBytes += nStream.Read(readBytes, countReadingBytes, readBytes.Count() - countReadingBytes);

			string tmpString = System.Text.Encoding.UTF8.GetString(readBytes);

			return tmpString;
		}

		private void Writing(object obj, byte numComand)
		{
			string serialized = JsonConvert.SerializeObject(obj);
			byte[] massByts = Encoding.UTF8.GetBytes(serialized);
			byte[] countRead = BitConverter.GetBytes(massByts.Count());
			byte[] typeComand = new byte[1];
			typeComand[0] = numComand;

			nStream2.Write(typeComand, 0, 1);//Отпраляет тип команды
			nStream2.Write(countRead, 0, 4);//Отпраляет кол-во байт, которое сервер должен будет читать
			nStream2.Write(massByts, 0, massByts.Count());
		}

		private void Ip_Click(object sender, EventArgs e)
		{

		}
	}
}
