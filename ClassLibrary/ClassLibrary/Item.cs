using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	public  class Item
	{
		public int IdItem;
		public string Name;
		public short Count, MaxCount, Time, TimeReloading;
		public Point Location;
		public void SetLocation(Point loc)
		{
			Location = loc;
		}
		public virtual object Use(UserInfo obj) { return null; }
 	}
}
