﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
	class BulletInfo
	{
		public double speedX, speedY;
		public Point location;
		public string owner;
		public BulletInfo(Point loc)
		{
			this.location = loc;
		}
	}
}
