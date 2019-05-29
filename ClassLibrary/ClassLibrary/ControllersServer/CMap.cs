using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassLibrary.ControllersServer
{
	public class CMap
	{
		ModelServer model;

		public CMap(ModelServer model)
		{
			this.model = model;
		}
		public void RandomBushs()//Карта
		{
			Random random = new Random();
			for (int i = 0; i < model.Map.MapBorders.Height * model.Map.MapBorders.Width / 10000; i++)
			{
				model.Map.ListBush.Add(new Bush(random.Next(model.Map.MapBorders.Width), random.Next(model.Map.MapBorders.Height)));
			}
		}

		public void RandomBox()//Карта
		{
			bool flag = true;
			Random random = new Random();
			for (int i = 0; i < model.Map.MapBorders.Height * model.Map.MapBorders.Width / 50000;)
			{
				Box box = new Box(random.Next(13, model.Map.MapBorders.Width - 13), random.Next(13, model.Map.MapBorders.Height - 13));
				foreach (Box b in model.Map.ListBox)//Проверка, заспавнился ли ящик в ящике
				{
					if (Math.Abs(b.Location.X - box.Location.X) < Box.size && Math.Abs(b.Location.Y - box.Location.Y) < Box.size)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					model.Map.ListBox.Add(box);
					for (int k = box.Location.X - 10; k < box.Location.X + 10; k++)
					{
						for (int j = box.Location.Y - 10; j < box.Location.Y + 10; j++)
						{
							model.Map.bordersForBullets[k, j] = true;
						}
					}
					for (int k = box.Location.X - 13; k < box.Location.X + 13; k++)
					{
						for (int j = box.Location.Y - 13; j < box.Location.Y + 13; j++)
						{
							model.Map.bordersForUsers[k, j] = true;
						}
					}
					i++;
				}
				flag = true;
			}
		}
		public bool RandomTree()//Карта
		{
			try
			{
				bool flag = true;
				Random random = new Random();
				for (int i = 0; i < model.Map.MapBorders.Height * model.Map.MapBorders.Width / 68000;)
				{
					Tree tree = new Tree(random.Next(13, model.Map.MapBorders.Width - 13), random.Next(13, model.Map.MapBorders.Height - 13));
					foreach (Box b in model.Map.ListBox)//Проверка, заспавнилось ли дерево в ящике
					{
						if (Math.Abs(b.Location.X - tree.Location.X) < Box.size && Math.Abs(b.Location.Y - tree.Location.Y) < Box.size)
						{
							flag = false;
							break;
						}
					}

					if (flag)
					{
						model.Map.ListTrees.Add(tree);
						for (int k = tree.Location.X - 3; k < tree.Location.X + 11; k++)
						{
							for (int j = tree.Location.Y - 3; j < tree.Location.Y + 11; j++)
							{
								model.Map.bordersForBullets[k, j] = true;
							}
						}
						for (int k = tree.Location.X - 3; k < tree.Location.X + 13; k++)
						{
							for (int j = tree.Location.Y - 3; j < tree.Location.Y + 13; j++)
							{
								model.Map.bordersForUsers[k, j] = true;
							}
						}
						i++;
					}
					flag = true;
				}
				return true;
			}
			catch { return false; }
		}

		public void createdZone()//карта
		{
			model.Map.NextZone.startCenterZone(model.Map.MapBorders); //Создаст зону внутри игровой области
			model.Map.NextZone.TimeTocompression = 60;
			model.Map.NextZone.ZoneRadius = (int)model.Map.MapBorders.Height / 2;

			model.Map.PrevZone.ZoneCenterCoordinates = new Point(model.Map.MapBorders.Width / 2, model.Map.MapBorders.Height / 2);//Создаст зону внутри игровой области
			model.Map.PrevZone.ZoneRadius = (int)model.Map.MapBorders.Height / 4 * 3;
		}

		public bool GenerateItems()//карта
		{
			try
			{
				Random n = new Random();
				List<Item> ListItems = new List<Item>();
				int Count = model.Map.MapBorders.Height * model.Map.MapBorders.Width / 450000;

				for (int i = 0; i < Count; i++)
				{
					NormalShotgun gun = new NormalShotgun();
					Thread.Sleep(7);
					gun.Location = new Point(n.Next(0, model.Map.MapBorders.Width), n.Next(0, model.Map.MapBorders.Height));
					gun.IdItem = ListItems.Count;
					ListItems.Add(gun);
				}

				for (int i = 0; i < Count; i++)
				{
					NormalGun gun = new NormalGun();
					Thread.Sleep(8);
					gun.Location = new Point(n.Next(0, model.Map.MapBorders.Width), n.Next(0, model.Map.MapBorders.Height));
					gun.IdItem = ListItems.Count;
					ListItems.Add(gun);
				}

				for (int i = 0; i < Count; i++)
				{
					Grenade grenade = new Grenade();
					Thread.Sleep(8);
					grenade.Location = new Point(n.Next(0, model.Map.MapBorders.Width), n.Next(0, model.Map.MapBorders.Height));
					grenade.IdItem = ListItems.Count;
					ListItems.Add(grenade);
				}

				for (int i = 0; i < Count; i++)
				{
					NormalPistol pistol = new NormalPistol();
					Thread.Sleep(8);
					pistol.Location = new Point(n.Next(0, model.Map.MapBorders.Width), n.Next(0, model.Map.MapBorders.Height));
					pistol.IdItem = ListItems.Count;
					ListItems.Add(pistol);
				}

				model.Map.ListItems = ListItems;
				return true;
			}
			catch { return false; }
		}

		public void LoadMap()//карта
		{
			BinaryFormatter formatter = new BinaryFormatter();
			OpenFileDialog sn = new OpenFileDialog();
			sn.DefaultExt = ".dat";
			sn.Filter = "Text files(*.dat)|*.dat|All files(*.*)|*.*";
			sn.InitialDirectory = @"C:\Users\Василий\Desktop\Exaxt\PUBGEXACT\MapEdit\MapEdit\Maps";
			DialogResult res = sn.ShowDialog();
			if (res == DialogResult.Cancel)
				return;
			if (res == DialogResult.OK)
			{
				string NameFile = sn.FileName;
				try
				{
					using (FileStream fs = new FileStream(NameFile, FileMode.Open))
					{
						Map m = (Map)formatter.Deserialize(fs);
						model.Map = m;
					}
				}
				catch (Exception err)
				{
					//ErrorEvent(err.Message + " |Ошибка в ControllerServer, методе LoadMap");
				}
			}
			Random random = new Random();
			for (int i = 0; i < model.ListUsers.Count; i++)
			{
				if (model.ListUsers[i] != null)
				{
					ControllersS.cPlay.SendingInformationAboutObjects(i);//Отправляется инфа обо всех объектах новоый карты

					do
						model.ListUsers[i] = new UserInfo(new Point(random.Next(2, model.Map.MapBorders.Width - 2), random.Next(2, model.Map.MapBorders.Height - 2)));
					while (model.Map.bordersForUsers[model.ListUsers[i].userLocation.X, model.ListUsers[i].userLocation.Y]);
				}
			}
		}
	}


}
