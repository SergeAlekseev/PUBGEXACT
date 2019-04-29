using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEdit
{
	class Box :Items
	{
		Model model = Controllers.cEditor.model;

		Point location;
		public static int size = 20;
		public Box()
		{
			location = new Point();
		}
		public Box(int X,int Y)
		{
			location = new Point(X,Y);
		}
		public Point Location
		{
			get { return location; }
			set { location = value; }
		}

		public override void Add(Point mouseLocation)
		{
			model.Map.ListBox.Add(new Box(mouseLocation.X, mouseLocation.Y));
		}
	}
}
