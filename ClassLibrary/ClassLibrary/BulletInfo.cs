using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	[Serializable]
	public class BulletInfo
	{
		public double speedX, speedY;
		public Point location;
		public string owner;
		public short damage,speed;
		public int timeLife;
		public BulletInfo(Point loc)
		{
			this.location = loc;
		}
	}
}
