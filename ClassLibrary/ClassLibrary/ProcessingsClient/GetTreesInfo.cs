using System.Collections.Generic;
using System.Diagnostics;

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
