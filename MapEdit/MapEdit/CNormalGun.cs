using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEdit
{
	class CNormalGun : Items
	{
		Model model = Controllers.cEditor.model;

		public override void Add(Point mouseLocation)
		{
			NormalGun gun = new NormalGun();
			gun.Location = mouseLocation;
			gun.IdItem = model.Map.ListItems.Count;

			model.Map.ListItems.Add(gun);
		}
	}
}
