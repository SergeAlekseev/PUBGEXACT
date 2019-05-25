using System;
using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEdit
{
	class CNormalShotGun : Items
	{
		Model model = Controllers.cEditor.model;

		public override void Add(Point mouseLocation)
		{
			NormalShotgun gun = new NormalShotgun();
			gun.Location = mouseLocation;
			gun.IdItem = model.Map.ListItems.Count;

			model.Map.ListItems.Add(gun);
		}
	}
}
