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
		public string name;

		public override void Process()
		{
			try
			{
				Model.ListUsers[num].Name = name;
			}
			catch { throw new Exception(); }
		}
	}
}
