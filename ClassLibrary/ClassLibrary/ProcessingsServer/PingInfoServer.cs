using ClassLibrary.ProcessingsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsServer
{
	public class PingInfoServer : ProcessingServer
	{
		ModelServer Model;

		public override void Process(ModelServer Model)
		{
			this.Model = Model;
			byte[] ping = new byte[1];
			ping[0] = 2;
			PingInfoClient pic = new PingInfoClient();
			CTransfers.Writing(pic, Model.ListNs[this.num]);
		}
	}


}
