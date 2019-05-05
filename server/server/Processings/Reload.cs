using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary;
namespace server.Processings
{
	public class Reload : Processing
	{
		
		public override void Process()
		{
			
			if (Model.ListUsers[num].Items[Model.ListUsers[num].thisItem] is Weapon)
			{
				Model.ListUsers[num].flagRecharge = true;
				Model.ListShoting[num].Abort();
				Model.ListUsers[num].flagShoting = false;
				Thread t = new Thread(() =>
				{
					int time = 0;
					while (Model.ListUsers[num].flagRecharge)
					{
						time++;
						Thread.Sleep(100);
						if (time >= Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].TimeReloading)
						{
							switch ((Model.ListUsers[num].Items[Model.ListUsers[num].thisItem] as Weapon).TypeBullets)
							{
								case Weapon.typeBullets.Gun:
									{
										Model.ListUsers[num].GunBullets += Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count;
										Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count = Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].MaxCount;
										Model.ListUsers[num].GunBullets -= Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].MaxCount;
										if (Model.ListUsers[num].GunBullets < 0)
										{
											Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count += Model.ListUsers[num].GunBullets;
											Model.ListUsers[num].GunBullets = 0;
										}
										break;
									}
								case Weapon.typeBullets.Pistol:
									{
										Model.ListUsers[num].PistolBullets += Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count;
										Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count = Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].MaxCount;
										Model.ListUsers[num].PistolBullets -= Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].MaxCount;
										if (Model.ListUsers[num].PistolBullets < 0)
										{
											Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count += Model.ListUsers[num].PistolBullets;
											Model.ListUsers[num].PistolBullets = 0;
										}
										break;
									}
								case Weapon.typeBullets.Shotgun:
									{
										Model.ListUsers[num].ShotgunBullets += Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count;
										Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count = Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].MaxCount;
										Model.ListUsers[num].ShotgunBullets -= Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].MaxCount;
										if (Model.ListUsers[num].ShotgunBullets < 0)
										{
											Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Count += Model.ListUsers[num].ShotgunBullets;
											Model.ListUsers[num].ShotgunBullets = 0;
										}
										break;
									}
							}

							Model.ListUsers[num].flagRecharge = false;
						}
					}


				});
				t.Start();
			}
		}
	}
}
