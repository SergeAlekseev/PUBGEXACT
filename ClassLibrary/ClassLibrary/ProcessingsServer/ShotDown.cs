using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.ProcessingsClient;

namespace ClassLibrary.ProcessingsServer
{
	public class ShotDown : ProcessingServer
	{
		UserInfo userInfo;
		ModelServer model;
		public override void Process(ModelServer Model)
		{
			this.model = Model;

			Model.ListUsers[num].flagRecharge = false;
			if (!Model.ListUsers[num].flagShoting && !Model.ListUsers[num].flagWaitShoting && Model.workingGame)
			{
				Model.ListUsers[num].flagShoting = true;
				Model.ListShoting[num] = new Thread(new ParameterizedThreadStart(ShotUser));
				Model.ListShoting[num].Start(Model.ListUsers[num]);
			}
		}

		public void ShotUser(object ui)//Controller
		{
			userInfo = (UserInfo)ui;
			Object obj = null;
			while (userInfo.flagShoting)
			{
				obj = (userInfo.Items[userInfo.thisItem] as Item).Use(userInfo);
				if (obj != null)
				{
					if (obj is BulletInfo)
					{
						BulletInfo bi = (BulletInfo)obj;
						Thread thread = new Thread(new ParameterizedThreadStart(Bullet));
						thread.Start(bi);
						lock (model.ListBullet)
						{
							model.ListBullet.Add(bi);
						}
						Thread.Sleep(userInfo.Items[userInfo.thisItem].Time);
					}
					else if (obj is List<BulletInfo>)
					{
						List<BulletInfo> bis = (List<BulletInfo>)obj;
						foreach (BulletInfo bi in bis)
						{
							Thread thread = new Thread(new ParameterizedThreadStart(Bullet));
							thread.Start(bi);
							lock (model.ListBullet)
							{
								model.ListBullet.Add(bi);
							}
						}
						Thread.Sleep(userInfo.Items[userInfo.thisItem].Time);
					}
					else if (obj is GrenadeInfo)
					{
						GrenadeInfo gi = (GrenadeInfo)obj;
						Thread thread = new Thread(new ParameterizedThreadStart(Grenade));
						thread.Start(gi);
						lock (model.ListGrenade)
						{
							model.ListGrenade.Add(gi);
						}
						break;
					}
				}
			}


		}

		public void Bullet(object tmpObject)
		{
			bool flagBreak = false;
			BulletInfo bulletInfo = (BulletInfo)tmpObject;
			double X = bulletInfo.location.X, Y = bulletInfo.location.Y;
			X += (15.0 / bulletInfo.speed) * bulletInfo.speedX;
			bulletInfo.location.X = (int)X;
			Y += (15.0 / bulletInfo.speed) * bulletInfo.speedY;
			bulletInfo.location.Y = (int)Y;
			for (int i = 0; i < bulletInfo.timeLife; i++)
			{
				X += bulletInfo.speedX;
				bulletInfo.location.X = (int)X;
				Y += bulletInfo.speedY;
				bulletInfo.location.Y = (int)Y;
				for (int j = 0; j < model.ListUsers.Count; j++)
				{
					if (model.ListUsers[j] != null && Math.Abs(model.ListUsers[j].userLocation.X - X) <= 9 && Math.Abs(model.ListUsers[j].userLocation.Y - Y) <= 9 && model.ListUsers[j].hp > 0)
					{

						byte[] popad = new byte[1];
						popad[0] = 6;
						model.ListUsers[j].hp -= bulletInfo.damage;
						flagBreak = true;
						if (model.ListUsers[j].hp <= 0)
						{

							SingalForDroping Signal = new SingalForDroping();
							CTransfers.Writing(Signal, model.ListNs[j]);
							Thread.Sleep(500);//Чтобы вещи успевали дропнуться до удаления игрока

							foreach (GeneralInfo g in model.ListGInfo)
							{
								if (g.Name == model.ListUsers[j].Name)
									g.Dies += 1;
								if (g.Name == bulletInfo.owner)
									g.Kills += 1;
							}

							GetKillsInfo kill = new GetKillsInfo();
							kill.kill.killer = bulletInfo.owner;
							kill.kill.dead = model.ListUsers[j].Name;

							for (int k = 0; k < model.ListUsers.Count; k++)
							{
								if (model.ListUsers[k] != null)
								{
									if (model.ListUsers[k].Name == bulletInfo.owner)
										model.ListUsers[k].kills += 1;
									CTransfers.Writing(kill, model.ListNs[k]);
								}
							}

							PlayerDeath pd = new PlayerDeath();
							pd.Killer = bulletInfo.owner;
							CTransfers.Writing(pd, model.ListNs[j]);

						}
						break;
					}
				}
				try
				{
					if (model.Map.bordersForBullets[bulletInfo.location.X, bulletInfo.location.Y])
					{
						flagBreak = true;
					}
				}
				catch { break; }
				if (flagBreak) break;
				Thread.Sleep(20);
			}
			lock (model.ListBullet)
			{
				model.ListBullet.TryTake(out bulletInfo);
			}
		}


		public void Grenade(object tmpObject)
		{
			bool flagfly = false, stop = false;
			GrenadeInfo grena = (GrenadeInfo)tmpObject;
			double X = grena.location.X, Y = grena.location.Y;
			for (int i = 0; i < 250; i++)
			{
				grena.timeBoo--;
				if (grena.flagFly&&!flagfly&&!stop)
				{
					grena.flagFly = false;
					grena.location = new Point(userInfo.userLocation.X, userInfo.userLocation.Y);
					double interval = Math.Sqrt(Math.Pow(userInfo.mouseLocation.X - userInfo.userLocation.X, 2)
									+ Math.Pow(userInfo.mouseLocation.Y - userInfo.userLocation.Y, 2));
					double k = interval / 3;
					grena.speedX = (userInfo.mouseLocation.X - userInfo.userLocation.X) / k;
					grena.speedY = (userInfo.mouseLocation.Y - userInfo.userLocation.Y) / k;
					X = grena.location.X;
					Y = grena.location.Y;
					flagfly = true;
				}
				if (flagfly)
				{
					X += grena.speedX;
					grena.location.X = (int)X;
					Y += grena.speedY;
					grena.location.Y = (int)Y;
					if (grena.timeLife > 0)
						grena.timeLife--;
					else
					{
						flagfly = false;
						stop = true;
					}
				}
				else if(!stop) { grena.location = userInfo.userLocation; }
				Thread.Sleep(20);
			}
			for (int j = 0; j < model.ListUsers.Count; j++)
			{
				if (model.ListUsers[j] != null && Math.Abs(model.ListUsers[j].userLocation.X - X) <= 59 && Math.Abs(model.ListUsers[j].userLocation.Y - Y) <= 59 && model.ListUsers[j].hp > 0)
				{

					byte[] popad = new byte[1];
					popad[0] = 6;
					model.ListUsers[j].hp -= grena.damage;
					if (model.ListUsers[j].hp <= 0)
					{

						SingalForDroping Signal = new SingalForDroping();
						CTransfers.Writing(Signal, model.ListNs[j]);
						Thread.Sleep(500);//Чтобы вещи успевали дропнуться до удаления игрока

						foreach (GeneralInfo g in model.ListGInfo)
						{
							if (g.Name == model.ListUsers[j].Name)
								g.Dies += 1;
							if (g.Name == grena.owner)
								g.Kills += 1;
						}

						GetKillsInfo kill = new GetKillsInfo();
						kill.kill.killer = grena.owner;
						kill.kill.dead = model.ListUsers[j].Name;

						for (int k = 0; k < model.ListUsers.Count; k++)
						{
							if (model.ListUsers[k] != null)
							{
								if (model.ListUsers[k].Name == grena.owner)
									model.ListUsers[k].kills += 1;
								CTransfers.Writing(kill, model.ListNs[k]);
							}
						}

						PlayerDeath pd = new PlayerDeath();
						pd.Killer = grena.owner;
						CTransfers.Writing(pd, model.ListNs[j]);

					}
				}
			}
			Thread.Sleep(700);
			model.ListGrenade.TryTake(out grena);
		}
	}
}
