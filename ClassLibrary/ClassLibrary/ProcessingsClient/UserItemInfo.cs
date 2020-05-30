
using System.Diagnostics;

namespace ClassLibrary.ProcessingsClient
{
	class UserItemInfo : ProcessingClient
	{
		public Item[] Items;

		public override void Process(ModelClient model)
		{
			model.ThisUser.Items = Items;
		}
	}
}
