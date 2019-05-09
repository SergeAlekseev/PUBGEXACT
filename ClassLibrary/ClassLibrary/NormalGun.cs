using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	public class NormalGun:LinearWeapons
	{
		public NormalGun()
		{
			this.Name = "Normal Gun";
			this.Damage = 20;
			this.Count = 30;
			this.MaxCount = 30;
			this.Speed = 6;
			this.Time = 200;
			this.TimeLife = 150;
			this.TimeReloading = 20;
			this.Spread = 20;
			this.TypeBullets = typeBullets.Gun;
		}
	}
}
