using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	[Serializable]
	public class GrenadeInfo
	{
		public double speedX, speedY;
		public Point location;
		public string owner;
		public short damage, speed,timeBoo=250;
		public short timeLife=50;
		public bool flagFly;
	}
}
