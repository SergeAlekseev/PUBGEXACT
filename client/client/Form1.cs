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
		TcpClient client = new TcpClient("25.46.244.0", 1337);
		NetworkStream nStream;
		Thread threadReading;
		Graphics pictureBox;
		public Client()
		{
			InitializeComponent();
			timer1.Interval = 30;
			pictureBox = PlayingField.CreateGraphics();
			nStream = client.GetStream();

			threadReading = new Thread(Reading);
			threadReading.Start();	
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
			while(true)//Поставить условие, если флаг дисконет равен истине
			{
				count = 0;
				while (count != 2)
					count += nStream.Read(countRead, count, countRead.Count() - count);

				count = 0;

				short count2 = BitConverter.ToInt16(countRead, 0);
				count2 = IPAddress.NetworkToHostOrder(count2);
				byte[] readBytes = new byte[count2];
				string tmpString = System.Text.Encoding.UTF8.GetString(readBytes);
				listUsers = JsonConvert.DeserializeObject<List<UserInfo>>(tmpString);

				while (count != count2)
					count += nStream.Read(readBytes, count, readBytes.Count() - count);
			}
		}
		private void Form1_KeyPress(object sender, KeyPressEventArgs e)// ПРОДУМАТЬ,ШОБЕ МОЖНО БЫЛО ЗАЖИМАТЬ РАЗНЫЕ КЛАВИШИ
		{
			switch (e.KeyChar)
			{
				case 'W': actionThishUser.act = Action.action.moveUp; break;
				case 'S': actionThishUser.act = Action.action.moveDown; break;
				case 'D': actionThishUser.act = Action.action.moveRight; break;
				case 'A': actionThishUser.act = Action.action.noveLeft; break;
				default:
					{

						string serialized = JsonConvert.SerializeObject(actionThishUser);
						byte[] massByts = Encoding.UTF8.GetBytes(serialized);
						byte[] countRead = BitConverter.GetBytes((short)massByts.Count());	

						nStream.Write(countRead, 0, 2);//Отпраляет кол-во байт, которое сервер должен будет читать
						nStream.Write(massByts,0, massByts.Count());
					}
					break;
			}
		}

		private void Client_Paint(object sender, PaintEventArgs e)
		{
			foreach (UserInfo user in listUsers)
			{
				pictureBox.FillEllipse(Brushes.Red, user.userLocation.X-1, user.userLocation.Y-1, 2,2);
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			Invalidate();
		}
	}
}
