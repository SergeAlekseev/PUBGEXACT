using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	public class NormalShotgun:Shotgun
	{
		public NormalShotgun()
		{
			this.Name = "ShotGun";
			this.Damage = 15;
			this.Count = 8;
			this.MaxCount = 8;
			this.Speed = 5;
			this.Time = 500;
			this.TimeLife = 60;
			this.TimeReloading = 30;
			this.Spread = 5;
			this.TypeBullets = typeBullets.Shotgun;
		}
	}
}
