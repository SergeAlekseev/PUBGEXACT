using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Processings
{
	public class PingInfo : Processing
	{
		int num;
		public PingInfo(int num)
		{
			this.num = num;
		}
		public override void Process()
		{
			byte[] ping = new byte[1];
			ping[0] = 2;
			lock (Model.ListNs[num])
			{
				Model.ListNs[num].Write(ping, 0, 1);
			}
		}
	}
}
