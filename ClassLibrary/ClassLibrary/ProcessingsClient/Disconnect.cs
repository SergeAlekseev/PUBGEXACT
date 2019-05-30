using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	public class Disconnect : ProcessingClient
	{
		public override void Process(ModelClient model)
		{
			if (model.threadStart)
			{
				model.NStream.Close();
				model.serverStart = false;
				model.exit();
			}
		}
	}
}
