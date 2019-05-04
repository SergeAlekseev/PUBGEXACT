using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary;
namespace server.Processings
{
	class ShotDown : Processing
	{
		public override void Process(int num)
		{
			Model.ListUsers[num].flagRecharge = false;
			string tmpString = CTransfers.Reading(Model.ListNs[num]);
			if (!Model.ListUsers[num].flagShoting && !Model.ListUsers[num].flagWaitShoting && Model.workingGame)
			{
				Model.ListUsers[num].flagShoting = true;
				Model.ListUsers[num].Shoting = new Thread(new ParameterizedThreadStart(ShotUser));
				Model.ListUsers[num].Shoting.Start(Model.ListUsers[num]);
			}
		}

		public void ShotUser(object ui)//Controller
		{
			UserInfo userInfo = (UserInfo)ui;
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
						lock (Model.ListBullet)
						{
							Model.ListBullet.Add(bi);
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
							lock (Model.ListBullet)
							{
								Model.ListBullet.Add(bi);
							}
						}
						Thread.Sleep(userInfo.Items[userInfo.thisItem].Time);
					}
				}
			}


		}

		public void Bullet(object tmpObject)
		{
			bool flagBreak = false;
			BulletInfo bulletInfo = (BulletInfo)tmpObject;
			double X = bulletInfo.location.X, Y = bulletInfo.location.Y;
			X += (13.0 / bulletInfo.speed) * bulletInfo.speedX;
			bulletInfo.location.X = (int)X;
			Y += (13.0 / bulletInfo.speed) * bulletInfo.speedY;
			bulletInfo.location.Y = (int)Y;
			for (int i = 0; i < bulletInfo.timeLife; i++)
			{
				X += bulletInfo.speedX;
				bulletInfo.location.X = (int)X;
				Y += bulletInfo.speedY;
				bulletInfo.location.Y = (int)Y;
				for (int j = 0; j < Model.ListUsers.Count; j++)
				{
					if (Model.ListUsers[j] != null && Math.Abs(Model.ListUsers[j].userLocation.X - X) <= 9 && Math.Abs(Model.ListUsers[j].userLocation.Y - Y) <= 9)
					{
						byte[] popad = new byte[1];
						popad[0] = 6;
						Model.ListUsers[j].hp -= bulletInfo.damage;
						flagBreak = true;
						if (Model.ListUsers[j].hp <= 0)
						{
							byte[] flagDie = new byte[1];
							flagDie[0] = 7;
							CTransfers.Writing(bulletInfo.owner, 7, Model.ListNs[j]);

							foreach (GeneralInfo g in Model.ListGInfo)
							{
								if (g.Name == Model.ListUsers[j].Name)
									g.Dies += 1;
								if (g.Name == bulletInfo.owner)
									g.Kills += 1;
							}

							Kill kill = new Kill();
							kill.killer = bulletInfo.owner;
							kill.dead = Model.ListUsers[j].Name;

							for (int k = 0; k < Model.ListUsers.Count; k++)
							{
								if (Model.ListUsers[k] != null)
								{
									if (Model.ListUsers[k].Name == bulletInfo.owner)
										Model.ListUsers[k].kills += 1;
									CTransfers.Writing(kill, 20, Model.ListNs[k]);
								}
							}

						}
						break;
					}
				}
				try
				{
					if (Model.Map.bordersForBullets[bulletInfo.location.X, bulletInfo.location.Y])
					{
						flagBreak = true;
					}
				}
				catch { break; }
				if (flagBreak) break;
				Thread.Sleep(20);
			}
			lock (Model.ListBullet)
			{
				Model.ListBullet.Remove(bulletInfo);
			}
		}
	}
}
