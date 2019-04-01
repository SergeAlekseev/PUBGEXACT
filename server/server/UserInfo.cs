﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace server
{
	class UserInfo
	{
		public int userNumber;
		public Point userLocation;
		public bool flagShoting;
		public bool flagWaitShoting;
		public Point mouseLocation;

		public UserInfo(Point userLocation)
		{
			this.userLocation = userLocation;
		}
	}
}
