using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Processing
{
	public class GetBoxesInfo : Processing
	{
		public List<Box> listBox;
		public override void Process(Controller controller)
		{
			controller.model.Map.ListBox = listBox;
		}
	}
}
