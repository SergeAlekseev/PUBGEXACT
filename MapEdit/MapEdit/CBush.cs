using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibary;
namespace MapEdit
{
	class CBush : Items
	{
		Model model = Controllers.cEditor.model;

		public override void Add(Point mouseLocation)
		{
			model.Map.ListBush.Add(new Bush(mouseLocation.X, mouseLocation.Y));
		}
	}
}
