using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibary
{
	[Serializable]
	public class Bush
	{
		Point location;

		public Bush()
		{
			location = new Point();
		}
		public Bush(int X,int Y)
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
