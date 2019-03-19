using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
namespace client
{
	public partial class Client : Form
	{
		List<UserInfo> listUsers;
		Action actionThishUser;
		TcpClient client;// 25.46.244.0
		NetworkStream nStream;
		Thread threadReading;
		Graphics pictureBox;
		bool threadStart = false;
		public Client()
		{
			InitializeComponent();
			timer1.Interval = 1;
			timer1.Start();
			pictureBox = PlayingField.CreateGraphics();

			Connect();//Соединение с сервером
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e) //Обработчик нажатия на кнопку 
		{

		}

		private void Form1_KeyUp(object sender, KeyEventArgs e) //Обработчик  отпускания кнопки
		{

		}

		private void Reading()
		{
			int count = 0;
			byte[] countRead = new byte[2];
			threadStart = true;
			while (threadStart)//Поставить условие, если флаг дисконет равен истине
			{
				count = 0;		
				while (count != 2)
					count += nStream.Read(countRead, count, countRead.Count() - count);
					
				count = 0;

				short count2 = BitConverter.ToInt16(countRead, 0);
				
				byte[] readBytes = new byte[count2];
				

				while (count != count2)
					count += nStream.Read(readBytes, count, readBytes.Count() - count);

				string tmpString = System.Text.Encoding.UTF8.GetString(readBytes);
				listUsers = JsonConvert.DeserializeObject<List<UserInfo>>(tmpString);
			}
		}
		private void Form1_KeyPress(object sender, KeyPressEventArgs e)// ПРОДУМАТЬ,ШОБЕ МОЖНО БЫЛО ЗАЖИМАТЬ РАЗНЫЕ КЛАВИШИ
		{
			switch (e.KeyChar)
			{
				case 'W': case 'w': case 'Ц': case 'ц': actionThishUser = new Action(Action.action.moveUp); PressKey(actionThishUser); break;
				case 'S': case 's': case 'Ы': case 'ы': actionThishUser = new Action(Action.action.moveDown); PressKey(actionThishUser); break;
				case 'D': case 'd': case 'В': case 'в': actionThishUser = new Action(Action.action.moveRight); PressKey(actionThishUser); break;
				case 'A': case 'a': case 'Ф': case 'ф': actionThishUser = new Action(Action.action.noveLeft); PressKey(actionThishUser); break;
				
			}
		}

		private void PressKey(Action actionThishUser)
		{
			string serialized = JsonConvert.SerializeObject(actionThishUser);
			byte[] massByts = Encoding.UTF8.GetBytes(serialized);
			byte[] countRead = BitConverter.GetBytes((short)massByts.Count());

			nStream.Write(countRead, 0, 2);//Отпраляет кол-во байт, которое сервер должен будет читать
			nStream.Write(massByts, 0, massByts.Count());
		}

		private void Client_Paint(object sender, PaintEventArgs e)

		{
			if (listUsers != null)
			{
				pictureBox.Clear(DefaultBackColor);
				foreach (UserInfo user in listUsers)
				{   
					if(user!=null)
					pictureBox.FillEllipse(Brushes.Red, user.userLocation.X - 1, user.userLocation.Y - 1, 4, 4);
				}
			}
		}

		public void Disconnect()
		{
			threadStart = false;
			threadReading.Join();
			nStream.Close();
			client.Close();
		}

		public void Connect()
		{			
			client = new TcpClient("25.46.244.0", 1337);
			nStream = client.GetStream();

			threadReading = new Thread(Reading);
			threadReading.Start();
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			Invalidate();
		}

		private void Client_FormClosing(object sender, FormClosingEventArgs e)
		{
			Disconnect();
		}
	}
}
