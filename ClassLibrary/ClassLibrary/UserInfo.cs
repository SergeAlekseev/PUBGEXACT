using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ClassLibrary
{

	public class UserInfo
	{
		public short hp = 100, armor = 0;
		public short userNumber;
		public Point userLocation;
		public bool flagShoting;
		public bool flagWaitShoting;
		public bool flagZone;
		public bool flagRecharge;
		public Point mouseLocation;
		public double Rotate;
		public int kills = 0;
		public Item[] Items = new Item[7];
		public byte thisItem = 1;
		public string Name;
		public bool PrivateWorkingThread;
		public short PistolBullets=999, GunBullets=999, ShotgunBullets=999;
		public Thread Shoting;

		public UserInfo(Point userLocation)
		{
			this.userLocation = userLocation;
		}
	}
}
