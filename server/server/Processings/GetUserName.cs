using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Processings
{
	public class GetUserName : Processing
	{
		int num;
		string name;
		public GetUserName(int num,string name)
		{
			this.num = num;
			this.name = name;
		}

		public override void Process()
		{
			Model.ListUsers[num].Name = name;
		}
	}
}
