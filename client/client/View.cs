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
using ClassLibrary;

using Newtonsoft.Json;
using Action = ClassLibrary.Action;
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

		public delegate void ShotUpD();
		public event ShotUpD ShotUpEvent;

		public delegate void ShotDownD();
		public event ShotDownD ShotDownEvent;

		public delegate double RotatateD();
		public event RotatateD RotatateEvent;

		public delegate void RechargeD();
		public event RechargeD RechargeEvent;

		public delegate bool СhangeItemD(byte num);
		public event СhangeItemD СhangeItemEvent;

		public delegate void MouseClickD();
		public event MouseClickD MouseClickEvent;

		static ModelClient model = new ModelClient();
		ControllerClient controller = new ControllerClient(model);

		Graphics pictureBox;
		BufferedGraphicsContext bufferedGraphicsContext;
		BufferedGraphics bufferedGraphics;
		bool Start = false;
		bool MouseDown = false;


		System.Drawing.TextureBrush tb;


		public Client(string Name, string Pass, string ip)
		{
			InitializeComponent();

			model.Images[0] = client.Properties.Resources.Bush.GetThumbnailImage(23, 23, null, IntPtr.Zero);
			model.Images[1] = client.Properties.Resources.grass.GetThumbnailImage(23, 23, null, IntPtr.Zero);
			model.Images[2] = client.Properties.Resources.clear.GetThumbnailImage(25, 25, null, IntPtr.Zero);
			model.Images[3] = client.Properties.Resources.box.GetThumbnailImage(23, 23, null, IntPtr.Zero);
			model.Images[4] = client.Properties.Resources.strelka.GetThumbnailImage(16, 16, null, IntPtr.Zero);
			model.Images[5] = client.Properties.Resources.clear100.GetThumbnailImage(100, 100, null, IntPtr.Zero);
			model.Images[6] = client.Properties.Resources.thisPlayer.GetThumbnailImage(17, 17, null, IntPtr.Zero);
			model.Images[7] = client.Properties.Resources.notThisPlayer.GetThumbnailImage(17, 17, null, IntPtr.Zero);
			model.Images[8] = client.Properties.Resources.marker.GetThumbnailImage(23, 12, null, IntPtr.Zero);
			model.Images[9] = client.Properties.Resources.marker2.GetThumbnailImage(23, 12, null, IntPtr.Zero);
			model.Images[10] = client.Properties.Resources.tree2.GetThumbnailImage(32, 32, null, IntPtr.Zero);
			model.Images[11] = client.Properties.Resources.Grenade.GetThumbnailImage(23, 23, null, IntPtr.Zero);
			model.Images[12] = client.Properties.Resources.Pistol.GetThumbnailImage(23, 23, null, IntPtr.Zero);

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
			model.ThisUser.Name = model.GInfo.Name; //Предаю имя игрока для будущей проверки

			ActionEvent += controller.PressKey;
			ConnectEvent += controller.Connect;
			DisconnectEvent += controller.Disconnect;

			ShotUpEvent += controller.ShotUp;
			ShotDownEvent += controller.ShotDown;

			MouseLocatinEvent += controller.WriteMouseLocation;
			RotatateEvent += controller.mouseMove;
			СhangeItemEvent += controller.ChangeItem;
			RechargeEvent += controller.Recharge;

			controller.CloseFormEvent += Client_FormClosing;
			controller.CloseEvent += AllClose;

			controller.ErrorConnect += ErrorConnect;

			ConnectEvent(ip); //Подключается к тому же, к чему была подключена форма меню
			Start = true;


			System.Windows.Forms.Timer timerMove = new System.Windows.Forms.Timer();
			timerMove.Interval = 100;
			timerMove.Start();
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
				case Keys.A: model.Action.actionThishUser = Action.action.moveLeft; ActionEvent(); break;
				case Keys.ShiftKey: case Keys.Shift: model.Action.actionThishUser = Action.action.shiftDown; ActionEvent(); break;

				case Keys.D1: СhangeItemEvent(1); break;
				case Keys.D2: СhangeItemEvent(2); break;
				case Keys.D3: СhangeItemEvent(3); break;
				case Keys.D4: СhangeItemEvent(4); break;
				case Keys.D5: СhangeItemEvent(5); break;
				case Keys.D6: СhangeItemEvent(6); break;

				case Keys.R: RechargeEvent(); break;
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
				if (!model.Die && !model.Win)
				{
					#region DrawingGrass
					if (model.Images[1] != null)
					{
						TextureBrush grass = new TextureBrush(model.Images[1]);
						grass.TranslateTransform(-model.ThisUser.userLocation.X, -model.ThisUser.userLocation.Y);
						bufferedGraphics.Graphics.FillRectangle(grass, model.Map.MapBorders.X - 300 + PlayingField.Width / 2 - model.ThisUser.userLocation.X, model.Map.MapBorders.Y - 300 + PlayingField.Height / 2 - model.ThisUser.userLocation.Y, model.Map.MapBorders.Width + 603, model.Map.MapBorders.Height + 603);
					}
					#endregion

					#region DrawingBoxes
					foreach (Box box in model.Map.ListBox)
					{
						bufferedGraphics.Graphics.DrawImage(model.Images[3], box.Location.X + PlayingField.Width / 2 - 10 - model.ThisUser.userLocation.X, box.Location.Y + PlayingField.Height / 2 - 10 - model.ThisUser.userLocation.Y, 20, 20);
					}
					#endregion

					#region drawingOtherUsers
					foreach (UserInfo user in model.ListUsers)
					{
						if (user != null && user.userLocation != model.ThisUser.userLocation)
						{
							Bitmap background = new Bitmap(model.Images[2]);

							Graphics g = Graphics.FromImage(background);
							g.TranslateTransform(13, 13);
							g.RotateTransform((float)user.Rotate);
							g.TranslateTransform(-13, -13);
							g.DrawImage(model.Images[7], 5, 5);
							bufferedGraphics.Graphics.DrawImage(background, user.userLocation.X + PlayingField.Width / 2 - 13 - model.ThisUser.userLocation.X, user.userLocation.Y + PlayingField.Height / 2 - 13 - model.ThisUser.userLocation.Y);
						}
					}
					#endregion

					#region DrawingThisUser
					if (model.ListUsers.Count > 0)
					{

						Bitmap background = new Bitmap(model.Images[2]);

						Graphics g = Graphics.FromImage(background);
						g.TranslateTransform(13, 13);
						g.RotateTransform((float)model.ThisUser.Rotate);
						g.TranslateTransform(-13, -13);
						g.DrawImage(model.Images[6], 5, 5);

						bufferedGraphics.Graphics.DrawImage(background, 288, 288);
						///////////
						if (model.ThisUser.flagZone)
						{
							Bitmap background1 = new Bitmap(model.Images[5]);

							Graphics g1 = Graphics.FromImage(background1);
							g1.TranslateTransform(50, 50);

							g1.RotateTransform((float)model.AngelToZone + 90);

							g1.TranslateTransform(-50, -50);
							g1.DrawImage(model.Images[4], 0, 42, 16, 16);

							bufferedGraphics.Graphics.DrawImage(background1, 250, 250, 100, 100);
						}
						//////////

						//		if (model.ThisUser.flagZone) bufferedGraphics.Graphics.DrawLine(Pens.Black, model.ThisUser.userLocation.X + PlayingField.Width / 2 - model.ThisUser.userLocation.X, model.ThisUser.userLocation.Y + PlayingField.Height / 2 - model.ThisUser.userLocation.Y, model.Map.NextZone.ZoneCenterCoordinates.X + PlayingField.Width / 2 - model.ThisUser.userLocation.X, model.Map.NextZone.ZoneCenterCoordinates.Y + PlayingField.Height / 2 - model.ThisUser.userLocation.Y);
					}
					#endregion

					#region DraingBushes
					foreach (Bush bush in model.Map.ListBush)
					{
						bufferedGraphics.Graphics.DrawImage(model.Images[0], bush.Location.X + PlayingField.Width / 2 - 10 - model.ThisUser.userLocation.X, bush.Location.Y + PlayingField.Height / 2 - 10 - model.ThisUser.userLocation.Y, 20, 20);
					}
					#endregion

					#region DrawingTrees
					foreach (Tree tree in model.Map.ListTrees)
					{
						bufferedGraphics.Graphics.DrawImage(model.Images[10], tree.Location.X + PlayingField.Width / 2 - 10 - model.ThisUser.userLocation.X, tree.Location.Y + PlayingField.Height / 2 - 10 - model.ThisUser.userLocation.Y, 32, 32);
					}
					#endregion

					#region Drawing_Borders_andZone
					bufferedGraphics.Graphics.DrawRectangle(Pens.Red, model.Map.MapBorders.X + PlayingField.Width / 2 - model.ThisUser.userLocation.X, model.Map.MapBorders.Y + PlayingField.Height / 2 - model.ThisUser.userLocation.Y, model.Map.MapBorders.Width + 3, model.Map.MapBorders.Height + 3);
					bufferedGraphics.Graphics.DrawEllipse(Pens.Green, model.Map.NextZone.ZoneCenterCoordinates.X + PlayingField.Width / 2 - model.Map.NextZone.ZoneRadius - model.ThisUser.userLocation.X, model.Map.NextZone.ZoneCenterCoordinates.Y + PlayingField.Height / 2 - model.Map.NextZone.ZoneRadius - model.ThisUser.userLocation.Y, (float)model.Map.NextZone.ZoneRadius * 2, (float)model.Map.NextZone.ZoneRadius * 2);
					bufferedGraphics.Graphics.DrawEllipse(Pens.Red, model.Map.PrevZone.ZoneCenterCoordinates.X + PlayingField.Width / 2 - model.Map.PrevZone.ZoneRadius - model.ThisUser.userLocation.X, model.Map.PrevZone.ZoneCenterCoordinates.Y + PlayingField.Height / 2 - model.Map.PrevZone.ZoneRadius - model.ThisUser.userLocation.Y, (float)model.Map.PrevZone.ZoneRadius * 2, (float)model.Map.PrevZone.ZoneRadius * 2);

					#endregion

					#region DrawingBullets
					foreach (BulletInfo bullet in model.ListBullet)
					{
						bufferedGraphics.Graphics.FillEllipse(Brushes.Yellow, bullet.location.X + PlayingField.Width / 2 - 1 - model.ThisUser.userLocation.X, bullet.location.Y + PlayingField.Height / 2 - 1 - model.ThisUser.userLocation.Y, 2, 2);
					}
					#endregion

					#region DrawingGrenades
					foreach (GrenadeInfo grenade in model.ListGrenade)
					{
						if (grenade.timeBoo > 50)
							bufferedGraphics.Graphics.FillEllipse(Brushes.Black, grenade.location.X + PlayingField.Width / 2 - 3 - model.ThisUser.userLocation.X, grenade.location.Y + PlayingField.Height / 2 - 3 - model.ThisUser.userLocation.Y, 7, 7);
						else if (grenade.timeBoo > 0)
							bufferedGraphics.Graphics.FillEllipse(Brushes.DarkRed, grenade.location.X + PlayingField.Width / 2 - 3 - model.ThisUser.userLocation.X, grenade.location.Y + PlayingField.Height / 2 - 3 - model.ThisUser.userLocation.Y, 7, 7);
						else
							bufferedGraphics.Graphics.FillEllipse(Brushes.OrangeRed, grenade.location.X + PlayingField.Width / 2 - 50 - model.ThisUser.userLocation.X, grenade.location.Y + PlayingField.Height / 2 - 50 - model.ThisUser.userLocation.Y, 100, 100);

					}
					#endregion

					#region DrawingItems
					foreach (Item item in model.Map.ListItems)
					{
						if ((item is NormalGun)) bufferedGraphics.Graphics.DrawImage(model.Images[8], item.Location.X + PlayingField.Width / 2 - 10 - model.ThisUser.userLocation.X, item.Location.Y + PlayingField.Height / 2 - 10 - model.ThisUser.userLocation.Y, 23, 12);
						if ((item is NormalShotgun)) bufferedGraphics.Graphics.DrawImage(model.Images[9], item.Location.X + PlayingField.Width / 2 - 10 - model.ThisUser.userLocation.X, item.Location.Y + PlayingField.Height / 2 - 10 - model.ThisUser.userLocation.Y, 23, 12);
						if ((item is Grenade)) bufferedGraphics.Graphics.DrawImage(model.Images[11], item.Location.X + PlayingField.Width / 2 - 10 - model.ThisUser.userLocation.X, item.Location.Y + PlayingField.Height / 2 - 10 - model.ThisUser.userLocation.Y, 23, 23);
						if ((item is NormalPistol)) bufferedGraphics.Graphics.DrawImage(model.Images[12], item.Location.X + PlayingField.Width / 2 - 10 - model.ThisUser.userLocation.X, item.Location.Y + PlayingField.Height / 2 - 10 - model.ThisUser.userLocation.Y, 23, 23);
					}
					#endregion

					#region UserInteface
					bufferedGraphics.Graphics.DrawString("Hp " + model.ThisUser.hp + "", new Font("Times New Roman", 14, FontStyle.Bold), Brushes.Red, 385, 520);

					if (model.ArrayKills[2] != null)
						bufferedGraphics.Graphics.DrawString(model.ArrayKills[0].dead + " killed " + model.ArrayKills[0].killer + "\n" +
															 model.ArrayKills[1].dead + " killed " + model.ArrayKills[1].killer + "\n" +
															 model.ArrayKills[2].dead + " killed " + model.ArrayKills[2].killer, new Font("Times New Roman", 10, FontStyle.Bold), Brushes.White, 2, 22);
					else if (model.ArrayKills[1] != null)
						bufferedGraphics.Graphics.DrawString(model.ArrayKills[0].dead + " killed " + model.ArrayKills[0].killer + "\n" +
															 model.ArrayKills[1].dead + " killed " + model.ArrayKills[1].killer + "\n", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.White, 2, 22);
					else if (model.ArrayKills[0] != null)
						bufferedGraphics.Graphics.DrawString(model.ArrayKills[0].dead + " killed " + model.ArrayKills[0].killer + "\n", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.White, 2, 22);

					bufferedGraphics.Graphics.DrawString(model.ThisUser.userLocation.X + ":" + model.ThisUser.userLocation.Y + "", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.White, 535, 2);

					bufferedGraphics.Graphics.DrawString(model.ThisUser.kills + " kills", new Font("Times New Roman", 13, FontStyle.Bold), Brushes.PaleVioletRed, 350, 2);

					bufferedGraphics.Graphics.DrawString(model.CountGamers + " Gamers", new Font("Times New Roman", 13, FontStyle.Bold), Brushes.White, 180, 2);

					if (model.ThisUser.Items[model.ThisUser.thisItem] != null)
					{
						bufferedGraphics.Graphics.DrawString(model.ThisUser.Items[model.ThisUser.thisItem].Name + " : " + model.ThisUser.Items[model.ThisUser.thisItem].Count + "", new Font("Times New Roman", 14, FontStyle.Bold), Brushes.White, 150, 520);
					}

					bufferedGraphics.Graphics.DrawEllipse(Pens.White, 275, -25, 50, 50);
					bufferedGraphics.Graphics.DrawRectangle(Pens.White, 170, -25, 260, 50);
					bufferedGraphics.Graphics.FillRectangle(Brushes.LightBlue, 150 + (model.ThisUser.thisItem - 1) * 50, 550, 50, 50);
					bufferedGraphics.Graphics.DrawRectangle(Pens.White, 150, 550, 300, 50);

					for (int i = 200; i < 500; i += 50)
					{
						bufferedGraphics.Graphics.DrawLine(Pens.White, i, 550, i, 600);
						if ((model.ThisUser.Items[(i - 150) / 50] is NormalGun)) bufferedGraphics.Graphics.DrawImage(model.Images[8], i-48 , 563 , 46, 24);
						if ((model.ThisUser.Items[(i - 150) / 50] is NormalShotgun)) bufferedGraphics.Graphics.DrawImage(model.Images[9], i - 48, 563, 46, 24);
						if ((model.ThisUser.Items[(i - 150) / 50] is Grenade)) bufferedGraphics.Graphics.DrawImage(model.Images[11], i - 48, 552, 46, 46);
						if ((model.ThisUser.Items[(i - 150) / 50] is NormalPistol)) bufferedGraphics.Graphics.DrawImage(model.Images[12], i - 48, 552, 46, 46);
					}

					


					bufferedGraphics.Graphics.DrawString(model.Map.NextZone.TimeTocompression + "", new Font("Times New Roman", 13, FontStyle.Bold), Brushes.LightBlue, 290, 2);
					#endregion

				}
				else if (model.Die)
				{
					#region Death

					bufferedGraphics.Graphics.FillRectangle(Brushes.Black, 0, 0, PlayingField.Width, PlayingField.Height);
					bufferedGraphics.Graphics.DrawString("You lose!", new Font("Times New Roman", 50, FontStyle.Bold), Brushes.Red, 150, 275);
					bufferedGraphics.Graphics.DrawString("You killed " + model.Killer, new Font("Times New Roman", 25, FontStyle.Bold), Brushes.Red, 150, 340);
					bufferedGraphics.Graphics.DrawString(model.ThisUser.kills + " kills", new Font("Times New Roman", 25, FontStyle.Bold), Brushes.Red, 150, 375);
					#endregion
				}
				else
				{
					#region Win
					bufferedGraphics.Graphics.FillRectangle(Brushes.White, 0, 0, PlayingField.Width, PlayingField.Height);
					bufferedGraphics.Graphics.DrawString("You win!", new Font("Times New Roman", 50, FontStyle.Bold), Brushes.Green, 150, 275);
					bufferedGraphics.Graphics.DrawString(model.ThisUser.kills + " kills", new Font("Times New Roman", 25, FontStyle.Bold), Brushes.Green, 150, 340);

					#endregion
				}
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
				ShotDownEvent();
				MouseDown = true;
			}
		}

		private void PlayingField_MouseUp(object sender, MouseEventArgs e)
		{
			if (MouseDown)
			{
				ShotUpEvent();
				MouseDown = false;
			}
		}

		private void timerMouseLocation_Tick(object sender, EventArgs e)
		{
			if (Start)
			{
				Point cursorPoint = PlayingField.PointToClient(Cursor.Position);
				model.MouseCoord = cursorPoint;
				cursorPoint.X = cursorPoint.X - 300 + model.ThisUser.userLocation.X;
				cursorPoint.Y = cursorPoint.Y - 300 + model.ThisUser.userLocation.Y;
				MouseLocatinEvent(cursorPoint);
				string st = "" + RotatateEvent();
				label1.Text = st;
			}

		}

		private void Client_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void Client_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ((e.KeyChar == 'q' || e.KeyChar == 'й') || (e.KeyChar == 'Q' || e.KeyChar == 'Й'))
			{
				MouseClickEvent = controller.ItemDroping;
				MouseClickEvent();
				//Событие в котором метод, что добавляет предмет в список предметов карты из инвентаря и удаляет его оттуда
			}
			else if (e.KeyChar == 'f' || e.KeyChar == 'а' || (e.KeyChar == 'F' || e.KeyChar == 'А'))
			{
				MouseClickEvent = controller.Mouse_Click;
				MouseClickEvent();
			}

		}
	}
}
