using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
