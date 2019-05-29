using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	public class GetCountGamesInfo : ProcessingClient
	{
		public int count;
		public override void Process(ModelClient model)
		{
			model.CountGamers = count;
		}
	}
}
