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
		public delegate void ConnectD();
		public event ConnectD ConnectEvent;

		public delegate void DisconnectD();
		public event DisconnectD DisconnectEvent;

		public delegate void ActionD();
		public event ActionD ActionEvent;

		public delegate void MouseLocatinD(Point MouseLocatin);
		public event MouseLocatinD MouseLocatinEvent;

		public delegate void ShotD(byte type);
		public event ShotD ShotEvent;

		static Model model = new Model();
		Controller controller = new Controller(model);

		Graphics pictureBox;
		BufferedGraphicsContext bufferedGraphicsContext;
		BufferedGraphics bufferedGraphics;

		bool Start = false;
		bool MouseDown = false;


		public Client()
		{
			InitializeComponent();
			timerPaint.Interval = 20;
			timerPaint.Start();
			timerMouseLocation.Interval = 40;
			timerMouseLocation.Start();

			pictureBox = PlayingField.CreateGraphics();
			bufferedGraphicsContext = new BufferedGraphicsContext();
			bufferedGraphics = bufferedGraphicsContext.Allocate(pictureBox, new Rectangle(0, 0, PlayingField.Width, PlayingField.Height));
			bufferedGraphics.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			model.ThisUser = new UserInfo(new Point(300, 300));

			ActionEvent += controller.PressKey;
			ConnectEvent += controller.Connect;
			DisconnectEvent += controller.Disconnect;
			ShotEvent += controller.Shot;
			MouseLocatinEvent += controller.WriteMouseLocation;

			controller.CloseFormEvent += Client_FormClosing;
			controller.CloseEvent += ErrorOfConnect;
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e) //Обработчик нажатия на кнопку 
		{
			switch (e.KeyCode)
			{
				case Keys.W: model.Action.actionThishUser = Action.action.moveUp; ActionEvent(); break;
				case Keys.S: model.Action.actionThishUser = Action.action.moveDown; ActionEvent(); break;
				case Keys.D: model.Action.actionThishUser = Action.action.moveRight; ActionEvent(); break;
				case Keys.A: model.Action.actionThishUser = Action.action.noveLeft; ActionEvent(); break;
			}
		}

		private void Form1_KeyUp(object sender, KeyEventArgs e) //Обработчик  отпускания кнопки
		{
			switch (e.KeyCode)
			{
				case Keys.W: model.Action.actionThishUser = Action.action.stopUp; ActionEvent(); break;
				case Keys.S: model.Action.actionThishUser = Action.action.stopDown; ActionEvent(); break;
				case Keys.D: model.Action.actionThishUser = Action.action.stopRight; ActionEvent(); break;
				case Keys.A: model.Action.actionThishUser = Action.action.stopLeft; ActionEvent(); break;
			}
		}


		private void Form1_KeyPress(object sender, KeyPressEventArgs e)// ПРОДУМАТЬ,ШОБЕ МОЖНО БЫЛО ЗАЖИМАТЬ РАЗНЫЕ КЛАВИШИ 
		{

			//switch (e.KeyChar)
			//{
			//	case 'W': case 'w': case 'Ц': case 'ц': model.Action.actionThishUser = Action.action.moveUp; ActionEvent(); break;
			//	case 'S': case 's': case 'Ы': case 'ы': model.Action.actionThishUser = Action.action.moveDown; ActionEvent(); break;
			//	case 'D': case 'd': case 'В': case 'в': model.Action.actionThishUser = Action.action.moveRight; ActionEvent(); break;
			//	case 'A': case 'a': case 'Ф': case 'ф': model.Action.actionThishUser = Action.action.noveLeft; ActionEvent(); break;

			//}
		}


		private void Client_Paint(object sender, PaintEventArgs e)

		{

			if (model.ListUsers != null&&Start)
			{
				foreach (UserInfo user in model.ListUsers)
				{
					if (user != null)
					{
						bufferedGraphics.Graphics.FillEllipse(Brushes.Red, user.userLocation.X - 3, user.userLocation.Y - 3, 6, 6);
					}
				}
				if(model.ListUsers.Count>0)
					bufferedGraphics.Graphics.FillEllipse(Brushes.Blue, model.ListUsers[model.ThisUser.userNumber].userLocation.X - 3, model.ListUsers[model.ThisUser.userNumber].userLocation.Y - 3, 6, 6);

				foreach (BulletInfo bullet in model.ListBullet)
				{
					bufferedGraphics.Graphics.FillEllipse(Brushes.Black, bullet.location.X-1, bullet.location.Y-1 , 2, 2);
				}

				bufferedGraphics.Graphics.DrawString(model.Ping + "", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.Green, 2, 2);

				bufferedGraphics.Render(pictureBox);
				bufferedGraphics.Graphics.Clear(DefaultBackColor);
			}

		}


		private void timerPaint_Tick(object sender, EventArgs e)
		{
			Invalidate();
		}

		private void Client_FormClosing(object sender, FormClosingEventArgs e) //Вызвать событие в Controller
		{
			DisconnectEvent();
		}

		private void PlayingField_Click(object sender, EventArgs e) //Вызвать событие в Controller
		{
			if (!Start)
			{
				ConnectEvent();
				Start = true;
			}

		}

		public void ErrorOfConnect()
		{
			BeginInvoke(new MethodInvoker(delegate
			{
				timerPaint.Stop();
				Start = false;
				Application.Exit();
				Close();
			}));
		}

		private void PlayingField_MouseDown(object sender, MouseEventArgs e)
		{
			if (!MouseDown)
			{
				ShotEvent(3);
				MouseDown = true;
			}
		}

		private void PlayingField_MouseUp(object sender, MouseEventArgs e)
		{
			if (MouseDown)
			{
				ShotEvent(4);
				MouseDown = false;
			}
		}

		private void timerMouseLocation_Tick(object sender, EventArgs e)
		{
			if (Start)
			{
				MouseLocatinEvent(PlayingField.PointToClient(Cursor.Position));
			}
			
		}
	}
}
