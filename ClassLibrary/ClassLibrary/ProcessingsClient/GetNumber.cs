
using System.Diagnostics;

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
