﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	public class GetMapBordersInfo : ProcessingClient
	{
		public Rectangle rectangle;
		public override void Process(ControllerClient controller)
		{
			controller.model.Map.MapBorders = rectangle;
		}
	}
}
