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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace client
{
	public partial class Client : Form
	{
		public delegate bool ConnectD(string ip);
		public event ConnectD ConnectEvent;

		public delegate void DisconnectD();
		public event DisconnectD DisconnectEvent;

		public delegate void ActionD();
		public event ActionD ActionEvent;

		public delegate void MouseLocatinD(Point MouseLocatin);
		public event MouseLocatinD MouseLocatinEvent;

		public delegate void ShotD(byte type);
		public event ShotD ShotEvent;

		public delegate double RotatateD();
		public event RotatateD RotatateEvent;

		static Model model = new Model();
		Controller controller = new Controller(model);

		Graphics pictureBox;
		BufferedGraphicsContext bufferedGraphicsContext;
		BufferedGraphics bufferedGraphics;
		bool Start = false;
		bool MouseDown = false;

		System.Drawing.TextureBrush tb;


		public Client(string Name, string Pass, string ip)
		{
			InitializeComponent();

			model.Images[0] = client.Properties.Resources.Bush.GetThumbnailImage(20, 20, null, IntPtr.Zero);
			model.Images[1] = client.Properties.Resources.grass.GetThumbnailImage(23, 23, null, IntPtr.Zero);

			timerPaint.Interval = 10;
			timerPaint.Start();
			timerMouseLocation.Interval = 40;
			timerMouseLocation.Start();

			pictureBox = PlayingField.CreateGraphics();
			bufferedGraphicsContext = new BufferedGraphicsContext();
			bufferedGraphics = bufferedGraphicsContext.Allocate(pictureBox, new Rectangle(0, 0, PlayingField.Width, PlayingField.Height));
			bufferedGraphics.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			model.ThisUser = new UserInfo(new Point(300, 300));
			controller.JoinUser(Name, Pass);

			InfoName.Text = model.GInfo.Name;

			ActionEvent += controller.PressKey;
			ConnectEvent += controller.Connect;
			DisconnectEvent += controller.Disconnect;
			ShotEvent += controller.Shot;
			MouseLocatinEvent += controller.WriteMouseLocation;
			RotatateEvent += controller.mouseMove;

			controller.CloseFormEvent += Client_FormClosing;
			controller.CloseEvent += AllClose;
			controller.ErrorConnect += ErrorConnect;

			ConnectEvent(ip); //Подключается к тому же, к чему была подключена форма меню
			Start = true;
		}

		public void ErrorConnect()
		{
			MessageBox.Show("Неверно введен IP");
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e) //Обработчик нажатия на кнопку 
		{
			switch (e.KeyCode)
			{
				case Keys.W: model.Action.actionThishUser = Action.action.moveUp; ActionEvent(); break;
				case Keys.S: model.Action.actionThishUser = Action.action.moveDown; ActionEvent(); break;
				case Keys.D: model.Action.actionThishUser = Action.action.moveRight; ActionEvent(); break;
				case Keys.A: model.Action.actionThishUser = Action.action.noveLeft; ActionEvent(); break;
				case Keys.ShiftKey: case Keys.Shift: model.Action.actionThishUser = Action.action.shiftDown; ActionEvent(); break;
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
				case Keys.ShiftKey: case Keys.Shift: model.Action.actionThishUser = Action.action.shiftUp; ActionEvent(); break;
			}
		}

		private void Client_Paint(object sender, PaintEventArgs e)
		{
			if (model.ListUsers != null && Start)
			{
				if (!model.Die)
				{
					if (model.Images[1] != null)
					{
						TextureBrush grass = new TextureBrush(model.Images[1]); 
						grass.TranslateTransform(-model.ThisUser.userLocation.X, -model.ThisUser.userLocation.Y);
						bufferedGraphics.Graphics.FillRectangle(grass, model.Map.MapBorders.X + model.Map.MapBorders.Width / 2 - model.ThisUser.userLocation.X, model.Map.MapBorders.Y + model.Map.MapBorders.Height / 2 - model.ThisUser.userLocation.Y, model.Map.MapBorders.Width + 3, model.Map.MapBorders.Height + 3);
					}

					foreach (UserInfo user in model.ListUsers)
					{
						if (user != null && user.userLocation != model.ThisUser.userLocation)
						{
							bufferedGraphics.Graphics.FillEllipse(Brushes.Red, user.userLocation.X + model.Map.MapBorders.Width / 2 - 3 - model.ThisUser.userLocation.X, user.userLocation.Y + model.Map.MapBorders.Height / 2 - 3 - model.ThisUser.userLocation.Y, 6, 6);
						}
					}
					if (model.ListUsers.Count > 0)
					{
						/*user*/
						//bufferedGraphics.Graphics.FillRectangle(Brushes.Blue, model.Map.MapBorders.Width / 2 - 3, model.Map.MapBorders.Height / 2 - 3, 6, 6);

						//userBufferedGraphics.Graphics.TranslateTransform(300, 300);
						//userBufferedGraphics.Graphics.RotateTransform((float)model.Rotate);
						//userBufferedGraphics.Graphics.TranslateTransform(-300, -300);
						Bitmap bitmap = new Bitmap(model.Images[0]);
						
						//RotateFlipType rotateFlipType = new RotateFlipType();
						bitmap = RotateImage(bitmap, (int)model.Rotate);

						//Graphics g = Graphics.FromImage(bitmap);
						//g.RotateTransform((float)model.Rotate);
					
						//tb = new System.Drawing.TextureBrush(bitmap,System.Drawing.Drawing2D.WrapMode.Clamp);     //Создаём кисть из изображения.        		



						//tb.TranslateTransform(300, 300);
						//tb.RotateTransform((float)model.Rotate); // поворачиваем объект графики
						bufferedGraphics.Graphics.DrawImage(bitmap, 288, 288,23,23);
						//tb.Dispose();


						bufferedGraphics.Graphics.DrawString(model.ThisUser.hp + "", new Font("Times New Roman", 12, FontStyle.Bold), Brushes.Red, 560, 2);
						if (model.ThisUser.flagZone) bufferedGraphics.Graphics.DrawLine(Pens.Black, model.ThisUser.userLocation.X + model.Map.MapBorders.Width / 2 - model.ThisUser.userLocation.X, model.ThisUser.userLocation.Y + model.Map.MapBorders.Height / 2 - model.ThisUser.userLocation.Y, model.Map.NextZone.ZoneCenterCoordinates.X + model.Map.MapBorders.Width / 2 - model.ThisUser.userLocation.X, model.Map.NextZone.ZoneCenterCoordinates.Y + model.Map.MapBorders.Height / 2 - model.ThisUser.userLocation.Y);
					}
					foreach (BulletInfo bullet in model.ListBullet)
					{
						bufferedGraphics.Graphics.FillEllipse(Brushes.Black, bullet.location.X + model.Map.MapBorders.Width / 2 - 1 - model.ThisUser.userLocation.X, bullet.location.Y + model.Map.MapBorders.Height / 2 - 1 - model.ThisUser.userLocation.Y, 2, 2);
					}
					bufferedGraphics.Graphics.DrawString(model.Ping + "", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.Green, 2, 2);
					bufferedGraphics.Graphics.DrawString(model.ThisUser.userLocation.X + ":" + model.ThisUser.userLocation.Y + "", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.Green, 20, 2);

					foreach (Bush bush in model.Map.ListBush)
					{
						bufferedGraphics.Graphics.DrawImage(model.Images[0], bush.Location.X + model.Map.MapBorders.Width / 2 - 10 - model.ThisUser.userLocation.X, bush.Location.Y + model.Map.MapBorders.Height / 2 - 10 - model.ThisUser.userLocation.Y, 20, 20);
					}

					bufferedGraphics.Graphics.DrawRectangle(Pens.Red, model.Map.MapBorders.X + model.Map.MapBorders.Width / 2 - model.ThisUser.userLocation.X, model.Map.MapBorders.Y + model.Map.MapBorders.Height / 2 - model.ThisUser.userLocation.Y, model.Map.MapBorders.Width + 3, model.Map.MapBorders.Height + 3);
					bufferedGraphics.Graphics.DrawEllipse(Pens.Green, model.Map.NextZone.ZoneCenterCoordinates.X + model.Map.MapBorders.Width / 2 - model.Map.NextZone.ZoneRadius - model.ThisUser.userLocation.X, model.Map.NextZone.ZoneCenterCoordinates.Y + model.Map.MapBorders.Height / 2 - model.Map.NextZone.ZoneRadius - model.ThisUser.userLocation.Y, (float)model.Map.NextZone.ZoneRadius * 2, (float)model.Map.NextZone.ZoneRadius * 2);
					bufferedGraphics.Graphics.DrawEllipse(Pens.Red, model.Map.PrevZone.ZoneCenterCoordinates.X + model.Map.MapBorders.Width / 2 - model.Map.PrevZone.ZoneRadius - model.ThisUser.userLocation.X, model.Map.PrevZone.ZoneCenterCoordinates.Y + model.Map.MapBorders.Height / 2 - model.Map.PrevZone.ZoneRadius - model.ThisUser.userLocation.Y, (float)model.Map.PrevZone.ZoneRadius * 2, (float)model.Map.PrevZone.ZoneRadius * 2);


				}
				else
				{
					bufferedGraphics.Graphics.FillRectangle(Brushes.Black, 0, 0, model.Map.MapBorders.Width, model.Map.MapBorders.Height);
					bufferedGraphics.Graphics.DrawString("You die!", new Font("Times New Roman", 50, FontStyle.Bold), Brushes.Red, 150, 275);

				}
				bufferedGraphics.Render(pictureBox);
				bufferedGraphics.Graphics.Clear(DefaultBackColor);
			}

		}

		private Bitmap RotateImage(Bitmap Image, int angle)
		{
			var pi2 = Math.PI / 2;//ПИ на два
			int oldWidth = Image.Width;
			int oldHeigth = Image.Height;
			var theta = angle * Math.PI / 180.0;//перевод в радианы
			var locked_theta = theta;
			if (locked_theta < 0.0) locked_theta += 2 * Math.PI;
			double newWidth, newHeigth;
			int nWidth, nHeigth;
			double adjacentTop, oppositeTop;
			double adjacentBottom, oppositeBottom;
			#region Вычисление новой ширины и высоты
			double SINUS = Math.Abs(Math.Sin(locked_theta));
			double COSINUS = Math.Abs(Math.Cos(locked_theta));
			if ((locked_theta >= 0.0 && locked_theta < pi2) || (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2)))
			{// Угол (>= 0 и < Pi/2) или (>= ПИ и < ПИ + Пи/2)
				adjacentTop = COSINUS * oldWidth;
				oppositeTop = SINUS * oldWidth;
				adjacentBottom = COSINUS * oldHeigth;
				oppositeBottom = SINUS * oldHeigth;
			}
			else
			{
				adjacentTop = SINUS * oldHeigth;
				oppositeTop = COSINUS * oldHeigth;
				adjacentBottom = SINUS * oldWidth;
				oppositeBottom = COSINUS * oldWidth;
			}
			newWidth = adjacentTop + oppositeBottom;
			newHeigth = adjacentBottom + oppositeTop;
			nWidth = (int)(Math.Ceiling(newWidth));//округление до целых вверх
			nHeigth = (int)(Math.Ceiling(newHeigth));
			#endregion Вычисление новой ширины и высоты
			Bitmap rotatedBmp = new Bitmap(nWidth, nHeigth);
			Graphics g = Graphics.FromImage(rotatedBmp);
			Point[] points = new Point[3];//массив из 3 структур типа Point, которые определяют параллелограмм
										  //Три точки обозначают: верхний левый, верхний правый, нижний левый углы параллелограмма.
										  //Четвёртая точка экстраполируется из первых трёх
										  //точка отсчёта (0,0) - левый верхний угол
			if (locked_theta >= 0.0 && locked_theta < pi2) //90
			{
				points[0] = new Point((int)oppositeBottom, 0);
				points[1] = new Point(nWidth, (int)(oppositeTop));
				points[2] = new Point(0, (int)(adjacentBottom));
			}
			else if (locked_theta >= pi2 && locked_theta < Math.PI) //90-180
			{
				points[0] = new Point(nWidth, (int)(oppositeTop));
				points[1] = new Point((int)(adjacentTop), nHeigth);
				points[2] = new Point((int)(oppositeBottom), 0);
			}
			else if (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2))//180-270
			{
				points[0] = new Point((int)(adjacentTop), nHeigth);
				points[1] = new Point(0, (int)(adjacentBottom));
				points[2] = new Point(nWidth, (int)(oppositeTop));
			}
			else
			{
				points[0] = new Point(0, (int)(adjacentBottom));
				points[1] = new Point((int)(oppositeBottom), 0);
				points[2] = new Point((int)(adjacentTop), nHeigth);
			}
			g.DrawImage(Image, points);
			//g.DrawRectangle(new Pen(Color.Red, 0.1f), new Rectangle(0, 0, nWidth / 2, nHeigth / 2));
			//g.DrawRectangle(new Pen(Color.Red, 0.1f), new Rectangle(nWidth / 2, nHeigth / 2, nWidth / 2 - 1, nHeigth / 2 - 1));
			g.Dispose();
			Image.Dispose();
			return rotatedBmp;
		}
		private void timerPaint_Tick(object sender, EventArgs e)
		{
			Invalidate();
		}

		private void Client_FormClosing(object sender, FormClosingEventArgs e) //Вызвать событие в Controller
		{
			DisconnectEvent();
		}

		public void AllClose()
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
				Point cursorPoint = PlayingField.PointToClient(Cursor.Position);
				cursorPoint.X = cursorPoint.X - 300 + model.ThisUser.userLocation.X;
				cursorPoint.Y = cursorPoint.Y - 300 + model.ThisUser.userLocation.Y;
				MouseLocatinEvent(cursorPoint);
			}

		}

		private void Client_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void PlayingField_MouseMove(object sender, MouseEventArgs e)
		{
			model.MouseCoord = e.Location;
			string st = "" + RotatateEvent();
			label1.Text = st;
		}

		private void PlayingField_Click(object sender, EventArgs e)
		{

		}
	}
}
