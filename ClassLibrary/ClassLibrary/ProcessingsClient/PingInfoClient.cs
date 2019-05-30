using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	public class PingInfoClient : ProcessingClient
	{
		

		public override void Process(ModelClient model)
		{
			model.PingWatch.Stop();
			model.Ping = (int)model.PingWatch.ElapsedMilliseconds;
		}
	}
}
