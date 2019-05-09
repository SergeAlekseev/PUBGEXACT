using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	public class Weapon:Item
	{
		public string Name;
		public short Speed, TimeLife, Spread;
		public UserInfo UserInfo;
		public short Damage;
		public typeBullets TypeBullets;
		public enum typeBullets {Pistol, Shotgun, Gun};

		public override object Use(UserInfo obj)
		{
			return null;
		}
		
	}
}
