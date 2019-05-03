using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	public abstract class Item
	{
		public short Count, MaxCount, Time, TimeReloading;
		public Point Location;

		public void SetLocation(Point loc)
		{
			Location = loc;
		}
		public abstract object Use(UserInfo obj);
 	}
}
