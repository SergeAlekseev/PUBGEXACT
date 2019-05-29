using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	class GetTreesInfo:ProcessingClient
	{
		public List<Tree> listTree;
		public override void Process(ModelClient model)
		{
			model.Map.ListTrees = listTree;
		}
	}
}
