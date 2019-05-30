using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	[Serializable]
	public class Tree
	{ 
		Point location;
		public static int size = 16;
		public Tree()
		{
			location = new Point();
		}
		public Tree(int X, int Y)
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
