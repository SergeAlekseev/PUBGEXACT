using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	public class GetNumber : ProcessingClient
	{
		public int num;
		public override void Process(ModelClient model)
		{
			model.number = num;
			model.threadStart = true;
			model.setName(model.GInfo.Name);
		}
	}
}
