using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
	class UserInfo
	{
		public ushort hp = 100;
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
