using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace server
{
	class UserInfo
	{		
		public short hp = 100;
		public short userNumber;
		public Point userLocation;
		public bool flagShoting;
		public bool flagWaitShoting;
		public bool flagZone;
		public Point mouseLocation;
		public double Rotate;
		public int kills = 0;

		public string Name;

		public UserInfo(Point userLocation)
		{
			this.userLocation = userLocation;
		}
	}
}
