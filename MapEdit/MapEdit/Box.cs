using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEdit
{
	[Serializable]
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
			Box box = new Box(mouseLocation.X, mouseLocation.Y);
			model.Map.ListBox.Add(box);

			for (int k = box.Location.X - 10; k < box.Location.X + 10; k++)
			{
				for (int j = box.Location.Y - 10; j < box.Location.Y + 10; j++)
				{
					model.Map.bordersForBullets[k, j] = true;
				}
			}
			for (int k = box.Location.X - 10 - 3; k < box.Location.X + 10 + 3; k++)
			{
				for (int j = box.Location.Y - 10 - 3; j < box.Location.Y + 10 + 3; j++)
				{
					model.Map.bordersForUsers[k, j] = true;
				}
			}
		}
	}
}
