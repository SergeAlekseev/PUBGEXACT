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
using Timer = System.Timers.Timer;

using Newtonsoft.Json;
using Action = ClassLibrary.Action;
namespace server
{
	public partial class Client : Form
	{


		ModelServer model;
		UserInfo thisUser;
		Image[] images = new Image[20];
		Graphics pictureBox;
		BufferedGraphicsContext bufferedGraphicsContext;
		BufferedGraphics bufferedGraphics;


		System.Drawing.TextureBrush tb;


		public Client(ModelServer model, UserInfo user)
		{
			InitializeComponent();

			this.model = model;
			this.thisUser = user;

			images[0] = server.Properties.Resources.Bush.GetThumbnailImage(23, 23, null, IntPtr.Zero);
			images[1] = server.Properties.Resources.grass.GetThumbnailImage(23, 23, null, IntPtr.Zero);
			images[2] = server.Properties.Resources.clear.GetThumbnailImage(25, 25, null, IntPtr.Zero);
			images[3] = server.Properties.Resources.box.GetThumbnailImage(23, 23, null, IntPtr.Zero);
			images[4] = server.Properties.Resources.strelka.GetThumbnailImage(16, 16, null, IntPtr.Zero);
			images[5] = server.Properties.Resources.clear100.GetThumbnailImage(100, 100, null, IntPtr.Zero);
			images[6] = server.Properties.Resources.thisPlayer.GetThumbnailImage(17, 17, null, IntPtr.Zero);
			images[7] = server.Properties.Resources.notThisPlayer.GetThumbnailImage(17, 17, null, IntPtr.Zero);
			images[8] = server.Properties.Resources.marker.GetThumbnailImage(23, 12, null, IntPtr.Zero);
			images[9] = server.Properties.Resources.marker2.GetThumbnailImage(23, 12, null, IntPtr.Zero);
			images[10] = server.Properties.Resources.tree2.GetThumbnailImage(32, 32, null, IntPtr.Zero);
			images[11] = server.Properties.Resources.Grenade.GetThumbnailImage(23, 23, null, IntPtr.Zero);
			images[12] = server.Properties.Resources.Pistol.GetThumbnailImage(23, 23, null, IntPtr.Zero);

			Timer defaultTimer = new Timer(10);
			defaultTimer.Elapsed += /*async*/(sender, e) => /*await Task.Run(() =>*/ Client_Paint()/*)*/;
			defaultTimer.Start();

			pictureBox = PlayingField.CreateGraphics();
			bufferedGraphicsContext = new BufferedGraphicsContext();
			bufferedGraphics = bufferedGraphicsContext.Allocate(pictureBox, new Rectangle(0, 0, PlayingField.Width, PlayingField.Height));
			bufferedGraphics.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
		}

		private void Client_Paint()
		{
			try
			{
				if (model.ListUsers != null && model.workingGame)
				{
					if (thisUser.hp > 0 && model.workingGame)
					{
						#region DrawingGrass
						if (images[1] != null)
						{
							TextureBrush grass = new TextureBrush(images[1]);
							grass.TranslateTransform(-thisUser.userLocation.X, -thisUser.userLocation.Y);
							bufferedGraphics.Graphics.FillRectangle(grass, model.Map.MapBorders.X - 300 + PlayingField.Width / 2 - thisUser.userLocation.X, model.Map.MapBorders.Y - 300 + PlayingField.Height / 2 - thisUser.userLocation.Y, model.Map.MapBorders.Width + 603, model.Map.MapBorders.Height + 603);
						}
						#endregion

						#region DrawingBoxes
						foreach (Box box in model.Map.ListBox)
						{
							bufferedGraphics.Graphics.DrawImage(images[3], box.Location.X + PlayingField.Width / 2 - 10 - thisUser.userLocation.X, box.Location.Y + PlayingField.Height / 2 - 10 - thisUser.userLocation.Y, 20, 20);
						}
						#endregion

						#region drawingOtherUsers
						foreach (UserInfo user in model.ListUsers)
						{
							if (user != null && user.userLocation != thisUser.userLocation)
							{
								Bitmap background = new Bitmap(images[2]);

								Graphics g = Graphics.FromImage(background);
								g.TranslateTransform(13, 13);
								g.RotateTransform((float)user.Rotate);
								g.TranslateTransform(-13, -13);
								g.DrawImage(images[7], 5, 5);
								bufferedGraphics.Graphics.DrawImage(background, user.userLocation.X + PlayingField.Width / 2 - 13 - thisUser.userLocation.X, user.userLocation.Y + PlayingField.Height / 2 - 13 - thisUser.userLocation.Y);
							}
						}
						#endregion

						#region DrawingThisUser
						if (model.ListUsers.Count > 0)
						{

							Bitmap background = new Bitmap(images[2]);

							Graphics g = Graphics.FromImage(background);
							g.TranslateTransform(13, 13);
							g.RotateTransform((float)thisUser.Rotate);
							g.TranslateTransform(-13, -13);
							g.DrawImage(images[6], 5, 5);

							bufferedGraphics.Graphics.DrawImage(background, 288, 288);
							///////////
							/*if (thisUser.flagZone)
							{
								Bitmap background1 = new Bitmap(images[5]);

								Graphics g1 = Graphics.FromImage(background1);
								g1.TranslateTransform(50, 50);

								g1.RotateTransform((float)thisUser.Rotate + 90);

								g1.TranslateTransform(-50, -50);
								g1.DrawImage(images[4], 0, 42, 16, 16);

								bufferedGraphics.Graphics.DrawImage(background1, 250, 250, 100, 100);
							}*/
							//////////

							//		if (thisUser.flagZone) bufferedGraphics.Graphics.DrawLine(Pens.Black, thisUser.userLocation.X + PlayingField.Width / 2 - thisUser.userLocation.X, thisUser.userLocation.Y + PlayingField.Height / 2 - thisUser.userLocation.Y, model.Map.NextZone.ZoneCenterCoordinates.X + PlayingField.Width / 2 - thisUser.userLocation.X, model.Map.NextZone.ZoneCenterCoordinates.Y + PlayingField.Height / 2 - thisUser.userLocation.Y);
						}
						#endregion

						#region DraingBushes
						foreach (Bush bush in model.Map.ListBush)
						{
							bufferedGraphics.Graphics.DrawImage(images[0], bush.Location.X + PlayingField.Width / 2 - 10 - thisUser.userLocation.X, bush.Location.Y + PlayingField.Height / 2 - 10 - thisUser.userLocation.Y, 20, 20);
						}
						#endregion

						#region DrawingTrees
						foreach (Tree tree in model.Map.ListTrees)
						{
							bufferedGraphics.Graphics.DrawImage(images[10], tree.Location.X + PlayingField.Width / 2 - 10 - thisUser.userLocation.X, tree.Location.Y + PlayingField.Height / 2 - 10 - thisUser.userLocation.Y, 32, 32);
						}
						#endregion

						#region Drawing_Borders_andZone
						bufferedGraphics.Graphics.DrawRectangle(Pens.Red, model.Map.MapBorders.X + PlayingField.Width / 2 - thisUser.userLocation.X, model.Map.MapBorders.Y + PlayingField.Height / 2 - thisUser.userLocation.Y, model.Map.MapBorders.Width + 3, model.Map.MapBorders.Height + 3);
						bufferedGraphics.Graphics.DrawEllipse(Pens.Green, model.Map.NextZone.ZoneCenterCoordinates.X + PlayingField.Width / 2 - model.Map.NextZone.ZoneRadius - thisUser.userLocation.X, model.Map.NextZone.ZoneCenterCoordinates.Y + PlayingField.Height / 2 - model.Map.NextZone.ZoneRadius - thisUser.userLocation.Y, (float)model.Map.NextZone.ZoneRadius * 2, (float)model.Map.NextZone.ZoneRadius * 2);
						bufferedGraphics.Graphics.DrawEllipse(Pens.Red, model.Map.PrevZone.ZoneCenterCoordinates.X + PlayingField.Width / 2 - model.Map.PrevZone.ZoneRadius - thisUser.userLocation.X, model.Map.PrevZone.ZoneCenterCoordinates.Y + PlayingField.Height / 2 - model.Map.PrevZone.ZoneRadius - thisUser.userLocation.Y, (float)model.Map.PrevZone.ZoneRadius * 2, (float)model.Map.PrevZone.ZoneRadius * 2);

						#endregion

						#region DrawingBullets
						foreach (BulletInfo bullet in model.ListBullet)
						{
							bufferedGraphics.Graphics.FillEllipse(Brushes.Yellow, bullet.location.X + PlayingField.Width / 2 - 1 - thisUser.userLocation.X, bullet.location.Y + PlayingField.Height / 2 - 1 - thisUser.userLocation.Y, 2, 2);
						}
						#endregion

						#region DrawingGrenades
						foreach (GrenadeInfo grenade in model.ListGrenade)
						{
							if (grenade.timeBoo > 50)
								bufferedGraphics.Graphics.FillEllipse(Brushes.Black, grenade.location.X + PlayingField.Width / 2 - 3 - thisUser.userLocation.X, grenade.location.Y + PlayingField.Height / 2 - 3 - thisUser.userLocation.Y, 7, 7);
							else if (grenade.timeBoo > 0)
								bufferedGraphics.Graphics.FillEllipse(Brushes.DarkRed, grenade.location.X + PlayingField.Width / 2 - 3 - thisUser.userLocation.X, grenade.location.Y + PlayingField.Height / 2 - 3 - thisUser.userLocation.Y, 7, 7);
							else
								bufferedGraphics.Graphics.FillEllipse(Brushes.OrangeRed, grenade.location.X + PlayingField.Width / 2 - 50 - thisUser.userLocation.X, grenade.location.Y + PlayingField.Height / 2 - 50 - thisUser.userLocation.Y, 100, 100);

						}
						#endregion

						#region DrawingItems
						foreach (Item item in model.Map.ListItems)
						{
							if ((item is NormalGun)) bufferedGraphics.Graphics.DrawImage(images[8], item.Location.X + PlayingField.Width / 2 - 10 - thisUser.userLocation.X, item.Location.Y + PlayingField.Height / 2 - 10 - thisUser.userLocation.Y, 23, 12);
							if ((item is NormalShotgun)) bufferedGraphics.Graphics.DrawImage(images[9], item.Location.X + PlayingField.Width / 2 - 10 - thisUser.userLocation.X, item.Location.Y + PlayingField.Height / 2 - 10 - thisUser.userLocation.Y, 23, 12);
							if ((item is Grenade)) bufferedGraphics.Graphics.DrawImage(images[11], item.Location.X + PlayingField.Width / 2 - 10 - thisUser.userLocation.X, item.Location.Y + PlayingField.Height / 2 - 10 - thisUser.userLocation.Y, 23, 23);
							if ((item is NormalPistol)) bufferedGraphics.Graphics.DrawImage(images[12], item.Location.X + PlayingField.Width / 2 - 10 - thisUser.userLocation.X, item.Location.Y + PlayingField.Height / 2 - 10 - thisUser.userLocation.Y, 23, 23);
						}
						#endregion

						#region UserInteface
						bufferedGraphics.Graphics.DrawString("Hp " + thisUser.hp + "", new Font("Times New Roman", 14, FontStyle.Bold), Brushes.Red, 385, 520);

						/*if (model.ArrayKills[2] != null)
							bufferedGraphics.Graphics.DrawString(model.ArrayKills[0].dead + " killed " + model.ArrayKills[0].killer + "\n" +
																 model.ArrayKills[1].dead + " killed " + model.ArrayKills[1].killer + "\n" +
																 model.ArrayKills[2].dead + " killed " + model.ArrayKills[2].killer, new Font("Times New Roman", 10, FontStyle.Bold), Brushes.White, 2, 22);
						else if (model.ArrayKills[1] != null)
							bufferedGraphics.Graphics.DrawString(model.ArrayKills[0].dead + " killed " + model.ArrayKills[0].killer + "\n" +
																 model.ArrayKills[1].dead + " killed " + model.ArrayKills[1].killer + "\n", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.White, 2, 22);
						else if (model.ArrayKills[0] != null)
							bufferedGraphics.Graphics.DrawString(model.ArrayKills[0].dead + " killed " + model.ArrayKills[0].killer + "\n", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.White, 2, 22);
	*/
						bufferedGraphics.Graphics.DrawString(thisUser.userLocation.X + ":" + thisUser.userLocation.Y + "", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.White, 535, 2);

						bufferedGraphics.Graphics.DrawString(thisUser.kills + " kills", new Font("Times New Roman", 13, FontStyle.Bold), Brushes.PaleVioletRed, 350, 2);

						bufferedGraphics.Graphics.DrawString(model.CountGamers + " Gamers", new Font("Times New Roman", 13, FontStyle.Bold), Brushes.White, 180, 2);

						if (thisUser.Items[thisUser.thisItem] != null)
						{
							bufferedGraphics.Graphics.DrawString(thisUser.Items[thisUser.thisItem].Name + " : " + thisUser.Items[thisUser.thisItem].Count + "", new Font("Times New Roman", 14, FontStyle.Bold), Brushes.White, 150, 520);
						}

						bufferedGraphics.Graphics.DrawEllipse(Pens.White, 275, -25, 50, 50);
						bufferedGraphics.Graphics.DrawRectangle(Pens.White, 170, -25, 260, 50);
						bufferedGraphics.Graphics.FillRectangle(Brushes.LightBlue, 150 + (thisUser.thisItem - 1) * 50, 550, 50, 50);
						bufferedGraphics.Graphics.DrawRectangle(Pens.White, 150, 550, 300, 50);

						for (int i = 200; i < 500; i += 50)
						{
							bufferedGraphics.Graphics.DrawLine(Pens.White, i, 550, i, 600);
							if ((thisUser.Items[(i - 150) / 50] is NormalGun)) bufferedGraphics.Graphics.DrawImage(images[8], i - 48, 563, 46, 24);
							if ((thisUser.Items[(i - 150) / 50] is NormalShotgun)) bufferedGraphics.Graphics.DrawImage(images[9], i - 48, 563, 46, 24);
							if ((thisUser.Items[(i - 150) / 50] is Grenade)) bufferedGraphics.Graphics.DrawImage(images[11], i - 48, 552, 46, 46);
							if ((thisUser.Items[(i - 150) / 50] is NormalPistol)) bufferedGraphics.Graphics.DrawImage(images[12], i - 48, 552, 46, 46);
						}




						bufferedGraphics.Graphics.DrawString(model.Map.NextZone.TimeTocompression + "", new Font("Times New Roman", 13, FontStyle.Bold), Brushes.LightBlue, 290, 2);
						#endregion

					}
					else if (thisUser.hp <= 0)
					{
						#region Death

						bufferedGraphics.Graphics.FillRectangle(Brushes.Black, 0, 0, PlayingField.Width, PlayingField.Height);
						bufferedGraphics.Graphics.DrawString("You lose!", new Font("Times New Roman", 50, FontStyle.Bold), Brushes.Red, 150, 275);
						/*bufferedGraphics.Graphics.DrawString("You killed " + model.Killer, new Font("Times New Roman", 25, FontStyle.Bold), Brushes.Red, 150, 340);*/
						bufferedGraphics.Graphics.DrawString(thisUser.kills + " kills", new Font("Times New Roman", 25, FontStyle.Bold), Brushes.Red, 150, 375);
						#endregion
					}
					else
					{
						#region Win
						bufferedGraphics.Graphics.FillRectangle(Brushes.White, 0, 0, PlayingField.Width, PlayingField.Height);
						bufferedGraphics.Graphics.DrawString("You win!", new Font("Times New Roman", 50, FontStyle.Bold), Brushes.Green, 150, 275);
						bufferedGraphics.Graphics.DrawString(thisUser.kills + " kills", new Font("Times New Roman", 25, FontStyle.Bold), Brushes.Green, 150, 340);

						#endregion
					}
					bufferedGraphics.Render(pictureBox);
					bufferedGraphics.Graphics.Clear(DefaultBackColor);
				}
			}
			catch { }; //FIXME
		}

		private void PlayingField_Click(object sender, EventArgs e)
		{

		}
	}
}