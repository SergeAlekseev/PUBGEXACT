using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using Action = ClassLibrary.Action;

namespace BotLibrary
{
	public class BotModel
	{
		List<UserInfo> listUsers = new List<UserInfo>();
		List<BulletInfo> listBullet = new List<BulletInfo>();
		List<GrenadeInfo> listGrenade = new List<GrenadeInfo>();
		Action action = new Action();
		int countGamers = 0;
		UserInfo thisUser = new UserInfo(new Point(300, 300));
		ClassLibrary.Map map = new ClassLibrary.Map(); 
		short lifes;
		GeneralInfo gInfo = new GeneralInfo();
		string killer = null;
		Kill[] arrayKills = new Kill[3];
		private bool threadStart = false;

		

		/// <summary>
		/// Создаст ограниченную базу данныз для игрока из полной
		/// </summary>
		/// <param name="client">Полная база данных</param>
		public BotModel(ModelClient client) 
		{
		
		}

		public void setCountGamers(int countGamers) 
		{
			this.countGamers = countGamers;
		}

		public void setKiller(string killer) 
		{
			this.killer = killer;
		}

		public void setLifes(short lifes)
		{
			this.lifes = lifes;
		}

		public void setListBullet(List<BulletInfo> listBullet)
		{
			this.listBullet.Clear();
			foreach (BulletInfo bullet in listBullet) {
				if (isInView(thisUser.userLocation, bullet.location)) {
					this.listBullet.Add(bullet);
				}
			}
		}

		public void setListGrenade(List<GrenadeInfo> listGrenade)
		{
			this.listGrenade.Clear();
			foreach (GrenadeInfo grenade in listGrenade)
			{
				if (isInView(thisUser.userLocation, grenade.location))
				{
					this.listGrenade.Add(grenade);
				}
			}
		}

		public void setThisUser(UserInfo thisUser)
		{
			this.thisUser = thisUser;
		}

		public void setListUsers(List<UserInfo> listUsers)
		{
			this.listUsers.Clear();
			foreach (UserInfo user in listUsers)
			{
				if(user != null)
				if (isInView(thisUser.userLocation, user.userLocation))
				{
					this.listUsers.Add(user);
				}
			}
		}

		public void setAction(Action action)
		{
			this.action = action;
		}

		public void setMap(ClassLibrary.Map map)
		{
			this.map.ListBox = map.ListBox;
			this.map.ListBush = map.ListBush;
			this.map.ListTrees = map.ListTrees;
			this.map.NextZone = map.NextZone;
			this.map.PrevZone = map.PrevZone;
			this.map.MapBorders = map.MapBorders;
			this.map.bordersForBullets = map.bordersForBullets;
			this.map.bordersForUsers = map.bordersForUsers;

			this.map.ListItems.Clear();
			foreach (Item item in map.ListItems)
			{
				if (item != null)
					if (isInView(thisUser.userLocation, item.Location))
					{
						this.map.ListItems.Add(item);
					}
			}
		}

		public void setGInfo(GeneralInfo gInfo)
		{
			this.gInfo = gInfo;
		}

		public void setArrayKills(Kill[] arrayKills)
		{
			this.arrayKills = arrayKills;
		}

		public void setThreadStart(bool threadStart)
		{
			this.threadStart = threadStart;
		}




		private bool isInView(Point user, Point obj) 
		{
			if (Math.Abs(user.X - obj.X) < 301 && Math.Abs(user.Y - obj.Y) < 301) 
			{
				return true;
			}
			return false;
		}


		public int CountGamers { get { return countGamers; }  }
		public string Killer { get { return killer; }  }
		public short Lifes { get { return lifes; }  }
		public List<BulletInfo> ListBullet { get { return listBullet; }  }
		public List<GrenadeInfo> ListGrenade { get { return listGrenade; }  }
		public UserInfo ThisUser { get { return thisUser; }  }
		public List<UserInfo> ListUsers { get { return listUsers; } }
		public Action Action { get { return action; }  }
		public Map Map { get { return map; }  }
		public GeneralInfo GInfo { get { return gInfo; }  }
		public Kill[] ArrayKills { get { return arrayKills; }  }
		public bool ThreadStart { get => threadStart;  }
	}
}
