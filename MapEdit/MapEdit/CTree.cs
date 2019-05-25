using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
namespace MapEdit
{
	class CTree:Items
	{
		Model model = Controllers.cEditor.model;

		public override void Add(Point mouseLocation)
		{
			Tree tree = new Tree(mouseLocation.X, mouseLocation.Y);
			model.Map.ListTrees.Add(tree);

			for (int k = tree.Location.X - 3; k < tree.Location.X + 11; k++)
			{
				for (int j = tree.Location.Y - 3; j < tree.Location.Y + 11; j++)
				{
					model.Map.bordersForBullets[k, j] = true;
				}
			}
			for (int k = tree.Location.X - 3; k < tree.Location.X + 13; k++)
			{
				for (int j = tree.Location.Y - 3; j < tree.Location.Y + 13; j++)
				{
					model.Map.bordersForUsers[k, j] = true;
				}
			}
		}
	}
}
