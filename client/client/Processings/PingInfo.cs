using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Processing
{
	public class PingInfo : Processing
	{
		public Stopwatch PingWatch;

		public override void Process(Controller controller)
		{
			PingWatch.Stop();
			controller.model.Ping = (int)PingWatch.ElapsedMilliseconds;
		}
	}
}
