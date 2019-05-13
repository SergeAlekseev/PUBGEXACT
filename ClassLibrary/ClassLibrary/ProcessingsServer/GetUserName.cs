using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsServer
{
	public class GetUserName : ProcessingServer
	{ 
		public string name;
		ModelServer Model;
		public override void Process(ModelServer Model)
		{
			this.Model = Model;
			try
			{
				Model.ListUsers[num].Name = name;
			}
			catch { throw new Exception(); }
		}
	}
}
