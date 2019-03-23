using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
		BufferedGraphicsContext bufferedGraphicsContext;
		BufferedGraphics bufferedGraphics;
		Stopwatch PingWatch = new Stopwatch();
		int Ping;
		bool threadStart = false;
		public Client()
		{
			InitializeComponent();
			timerPaint.Interval = 20;
			timerPaint.Start();
			timerPing.Interval = 2000;
			timerPing.Start();
			pictureBox = PlayingField.CreateGraphics();
			bufferedGraphicsContext = new BufferedGraphicsContext();
			bufferedGraphics = bufferedGraphicsContext.Allocate(pictureBox, new Rectangle(0,0, PlayingField.Width, PlayingField.Height));
			bufferedGraphics.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			
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
			while (threadStart)//Поставить условие, если флаг дисконет равен истине
			{
				count = 0;		
				while (count != 2)
					count += nStream.Read(countRead, count, countRead.Count() - count);
					
				count = 0;

				short count2 = BitConverter.ToInt16(countRead, 0);
				if (count2 != -1)
				{
					byte[] readBytes = new byte[count2];


					while (count != count2)
						count += nStream.Read(readBytes, count, readBytes.Count() - count);

					string tmpString = System.Text.Encoding.UTF8.GetString(readBytes);
					listUsers = JsonConvert.DeserializeObject<List<UserInfo>>(tmpString);
				}
				else //ping
				{
					PingWatch.Stop();
					Ping = (int)PingWatch.ElapsedMilliseconds;
				}
				
			}
		}
		private void Form1_KeyPress(object sender, KeyPressEventArgs e)// ПРОДУМАТЬ,ШОБЕ МОЖНО БЫЛО ЗАЖИМАТЬ РАЗНЫЕ КЛАВИШИ
		{
			if(threadStart)
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
				foreach (UserInfo user in listUsers)
				{
					if (user != null)
						bufferedGraphics.Graphics.FillEllipse(Brushes.Red, user.userLocation.X - 2, user.userLocation.Y - 2, 4, 4);
				}
				bufferedGraphics.Graphics.DrawString(Ping + "", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.Green, 2, 2);

				bufferedGraphics.Render(pictureBox);
				bufferedGraphics.Graphics.Clear(DefaultBackColor);
			}
			
		}

		public void Disconnect()
		{

			if (threadStart)
			{
				
				threadReading.Abort();
				nStream.Close();
				client.Close();
				threadStart = false;
			}
		}

		public void Connect()
		{
			try
			{
				client = new TcpClient("25.46.244.0", 1337);

				nStream = client.GetStream();

				threadReading = new Thread(Reading);
				threadReading.Start();
				threadStart = true;
			}
			catch (System.Net.Sockets.SocketException err) //не удалось подключится по заданным параметрам
			{
				Application.Exit();
				Close();
				
			}

		}
		private void timerPaint_Tick(object sender, EventArgs e)
		{
			Invalidate();
		}

		private void timerPing_Tick(object sender, EventArgs e)
		{
			if (threadStart)
			{
				if (PingWatch.ElapsedMilliseconds > 2000)
				{
					PingWatch.Stop();
					OnFormClosing(null);
				}
				else
				{
					PingWatch = new Stopwatch();
					byte[] Ping = BitConverter.GetBytes((short)-1);
					nStream.Write(Ping, 0, 2);
					PingWatch.Start();
				}
			}

		}

		private void Client_FormClosing(object sender, FormClosingEventArgs e)
		{
			Disconnect();
		}

		private void PlayingField_Click(object sender, EventArgs e)
		{
			if (!threadStart)
			{
				Connect();
				threadStart = true;
			}//Соединение с сервером
		}

		
	}
}
