using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary
{
	public class Grenade :Item
	{
		public GrenadeInfo Grena = new GrenadeInfo();
		public short Speed, TimeLife;
		public short Damage;

		public Grenade()
		{
			this.Name = "Grenade";
			this.Damage = 50;
			this.Count = 6;
			this.MaxCount = 6;
			this.Speed = 1;
			this.Time = 5000;
			this.TimeLife = 250;
		}
		public override object Use(UserInfo obj)
		{
			if (this.Count > 0 && obj.userLocation != obj.mouseLocation)
			{
				this.Count--;
				Grena = new GrenadeInfo();
				Grena.location = obj.userLocation;
				Grena.owner = obj.Name;
				Grena.damage = Damage;
			}
			else { Grena = null; }
			return Grena;
		}
	}
}
