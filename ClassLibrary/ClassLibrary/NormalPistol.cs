using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	[Serializable]
	public class NormalPistol: LinearWeapons
	{
		public NormalPistol()
		{
			this.Name = "Normal pistol";
			this.Damage = 15;
			this.Count = 12;
			this.MaxCount = 12;
			this.Speed = 5;
			this.Time = 400;
			this.TimeLife = 150;
			this.TimeReloading = 15;
			this.Spread = 30;
			this.TypeBullets = typeBullets.Pistol;
		}
	}
}
