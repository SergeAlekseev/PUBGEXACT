using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
	class Bush
	{
		Point location;

		public Bush(int X, int Y)
		{
			location = new Point(X,Y);
		}

		public Point Location
		{
			get { return location; }
			set { location = value; }
		}
	}
}
