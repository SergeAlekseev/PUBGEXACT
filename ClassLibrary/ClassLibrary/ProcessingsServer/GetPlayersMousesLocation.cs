using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsServer
{
	public class GetPlayersMousesLocation : ProcessingServer
	{
		public Point mouse;
		ModelServer Model;
		public override void Process(ModelServer Model)
		{
			this.Model = Model;
			Model.ListUsers[num].mouseLocation = mouse;
		}
	}
}
