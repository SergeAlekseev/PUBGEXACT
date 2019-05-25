using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
namespace MapEdit
{
	class CBox : Items
	{
		Model model = Controllers.cEditor.model;

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
			for (int k = box.Location.X - 13; k < box.Location.X + 13; k++)
			{
				for (int j = box.Location.Y - 13; j < box.Location.Y + 13; j++)
				{
						model.Map.bordersForUsers[k, j] = true;
				}
			}
		}
	}
}
