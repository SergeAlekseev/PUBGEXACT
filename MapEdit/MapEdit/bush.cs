using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEdit
{
	class Bush:Items
	{
		Model model = Controllers.cEditor.model;
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

		public override void Add(Point mouseLocation)
		{
			model.Map.ListBush.Add(new Bush(mouseLocation.X,mouseLocation.Y));
		}
	}
}
