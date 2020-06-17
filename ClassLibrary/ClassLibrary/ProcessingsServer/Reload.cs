using System.Threading;
using System.Diagnostics;
using System.Data;
using System;

namespace ClassLibrary.ProcessingsServer
{
	public class Reload : ProcessingServer
	{
		ModelServer Model;
		public override void Process(ModelServer model)
		{
			this.Model = model;
			try
			{
				if (model.ListUsers[num].Items[model.ListUsers[num].thisItem] is Weapon && model.ListUsers[num].Items[model.ListUsers[num].thisItem]!=null)
				{
					model.ListUsers[num].flagRecharge = true;
					model.ListShoting[num].Abort();
					model.ListUsers[num].flagShoting = false;
					Thread t = new Thread(() =>
					{
						int time = 0;
						while (model.ListUsers[num] != null && model.ListUsers[num].flagRecharge)
						{
							time++;
							Thread.Sleep(100);

							if (model.ListUsers[num] != null && model.ListUsers[num].Items[model.ListUsers[num].thisItem].Name != null && time >= model.ListUsers[num].Items[model.ListUsers[num].thisItem].TimeReloading)
							{
								switch ((model.ListUsers[num].Items[model.ListUsers[num].thisItem] as Weapon).TypeBullets)
								{
									case Weapon.typeBullets.Gun:
										{
											model.ListUsers[num].GunBullets += model.ListUsers[num].Items[model.ListUsers[num].thisItem].Count;
											model.ListUsers[num].Items[model.ListUsers[num].thisItem].Count = model.ListUsers[num].Items[model.ListUsers[num].thisItem].MaxCount;
											model.ListUsers[num].GunBullets -= model.ListUsers[num].Items[model.ListUsers[num].thisItem].MaxCount;
											if (model.ListUsers[num].GunBullets < 0)
											{
												model.ListUsers[num].Items[model.ListUsers[num].thisItem].Count += model.ListUsers[num].GunBullets;
												model.ListUsers[num].GunBullets = 0;
											}
											break;
										}
									case Weapon.typeBullets.Pistol:
										{
											model.ListUsers[num].PistolBullets += model.ListUsers[num].Items[model.ListUsers[num].thisItem].Count;
											model.ListUsers[num].Items[model.ListUsers[num].thisItem].Count = model.ListUsers[num].Items[model.ListUsers[num].thisItem].MaxCount;
											model.ListUsers[num].PistolBullets -= model.ListUsers[num].Items[model.ListUsers[num].thisItem].MaxCount;
											if (model.ListUsers[num].PistolBullets < 0)
											{
												model.ListUsers[num].Items[model.ListUsers[num].thisItem].Count += model.ListUsers[num].PistolBullets;
												model.ListUsers[num].PistolBullets = 0;
											}
											break;
										}
									case Weapon.typeBullets.Shotgun:
										{
											model.ListUsers[num].ShotgunBullets += model.ListUsers[num].Items[model.ListUsers[num].thisItem].Count;
											model.ListUsers[num].Items[model.ListUsers[num].thisItem].Count = model.ListUsers[num].Items[model.ListUsers[num].thisItem].MaxCount;
											model.ListUsers[num].ShotgunBullets -= model.ListUsers[num].Items[model.ListUsers[num].thisItem].MaxCount;
											if (model.ListUsers[num].ShotgunBullets < 0)
											{
												model.ListUsers[num].Items[model.ListUsers[num].thisItem].Count += model.ListUsers[num].ShotgunBullets;
												model.ListUsers[num].ShotgunBullets = 0;
											}
											break;
										}
								}

								model.ListUsers[num].flagRecharge = false;
							}
						}
					});
					t.Start();
				}
			}
			catch(NullReferenceException NRE)
			{
				Debug.WriteLine("Экстренное завершение перезарядки");
			}
			catch { };
		}
	}
}
