﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Processings
{
	public class GetPlayersMousesLocation : Processing
	{
		public Point mouse;

		public override void Process()
		{
			Model.ListUsers[num].mouseLocation = mouse;
		}
	}
}
