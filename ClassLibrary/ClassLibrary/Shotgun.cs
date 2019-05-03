using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	public class Shotgun : Weapon
	{
		List<BulletInfo> Bullets;

		BulletInfo bullet;
		public override object Use(UserInfo obj)
		{
			Bullets = new List<BulletInfo>();
			this.UserInfo = obj;
			if (this.Count > 0 && this.UserInfo.userLocation != this.UserInfo.mouseLocation)
			{
				double interval = Math.Sqrt(Math.Pow(this.UserInfo.mouseLocation.X - this.UserInfo.userLocation.X, 2)
									+ Math.Pow(this.UserInfo.mouseLocation.Y - this.UserInfo.userLocation.Y, 2));
				Random random = new Random();
				Point SpreadPoint;
				double k = interval / this.Speed;  //для точности хорошо б менять интервал, но для точности
				for (int i = 0; i < 7; i++)
				{
					SpreadPoint = new Point(random.Next(this.UserInfo.mouseLocation.X - (int)interval / this.Spread, this.UserInfo.mouseLocation.X + (int)interval / this.Spread),
												random.Next(this.UserInfo.mouseLocation.Y - (int)interval / this.Spread, this.UserInfo.mouseLocation.Y + (int)interval / this.Spread));
					bullet = new BulletInfo(this.UserInfo.userLocation);
					bullet.speedX = (SpreadPoint.X - this.UserInfo.userLocation.X) / k;
					bullet.speedY = (SpreadPoint.Y - this.UserInfo.userLocation.Y) / k;
					bullet.owner = this.UserInfo.Name;
					bullet.damage = this.Damage;
					bullet.timeLife = this.TimeLife;
					bullet.speed = this.Speed;
					Bullets.Add(bullet);
				}
				this.Count--;
			}
			
			return Bullets;
		}
		
	}
}
