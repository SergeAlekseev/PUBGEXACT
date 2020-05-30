using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	public class GetCountWinsInfo : ProcessingClient
	{
		public override void Process(ModelClient model)
		{
			String name = model.ListUsers[model.number].Name;
			model.threadStart = false;
			model.Win = true;
			model.NStream.Close();
			model.serverStart = false;
			model.exit();
		}
	}
}
