using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Processings
{
	class PingInfo : Processing
	{
		public override void Process(int numberUser)
		{
			byte[] ping = new byte[1];
			ping[0] = 2;
			lock (Model.ListNs[numberUser])
			{
				Model.ListNs[numberUser].Write(ping, 0, 1);
			}
		}
	}
}
