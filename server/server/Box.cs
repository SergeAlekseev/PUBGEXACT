using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
	class Box
	{
		Point location;
		public static int size = 20;
		public Box(int X, int Y)
		{
			location = new Point(X, Y);
		}
		
		public Point Location
		{
			get { return location; }
			set { location = value; }
		}

	}
}
