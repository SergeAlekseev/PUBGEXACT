using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Processing
{
	public class GetCountGamesInfo : Processing
	{
		public int count;
		public override void Process(Controller controller)
		{
			controller.model.CountGamers = count;
		}
	}
}
