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
		public Point mouseLocation;

		public UserInfo(Point userLocation)
		{
			this.userLocation = userLocation;
		}
	}
}
