﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	public class GetCountWinsInfo : ProcessingClient
	{
		public override void Process(ControllerClient controller)
		{
			controller.model.Win = true;
			controller.nStream.Close();
			controller.serverStart = false;
			controller.client.Close();
			controller.timerPing.Stop();
			controller.threadReading.Abort();
		}
	}
}
