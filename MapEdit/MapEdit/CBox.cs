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
					try
					{
						model.Map.bordersForBullets[k, j] = true;
					}
					catch { }
				}
			}
			for (int k = box.Location.X - 10 - 3; k < box.Location.X + 10 + 3; k++)
			{
				for (int j = box.Location.Y - 10 - 3; j < box.Location.Y + 10 + 3; j++)
				{
					try
					{
						model.Map.bordersForUsers[k, j] = true;
					}
					catch { }
				}
			}
		}
	}
}
