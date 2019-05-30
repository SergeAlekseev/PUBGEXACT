using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	public class GetCountWinsInfo : ProcessingClient
	{
		public override void Process(ModelClient model)
		{
			model.threadStart = false;
			model.Win = true;
			model.NStream.Close();
			model.serverStart = false;
			model.exit();
		}
	}
}
