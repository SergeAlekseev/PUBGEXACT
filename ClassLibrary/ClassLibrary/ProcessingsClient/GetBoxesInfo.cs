using System.Collections.Generic;
using System.Diagnostics;

namespace ClassLibrary.ProcessingsClient
{
	public class GetBoxesInfo : ProcessingClient
	{
		public List<Box> listBox;
		public override void Process(ModelClient model)
		{
			model.Map.ListBox = listBox;
		}
	}
}
