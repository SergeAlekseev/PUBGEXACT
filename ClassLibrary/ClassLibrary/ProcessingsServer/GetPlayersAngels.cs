using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsServer
{
	public class GetPlayersAngels : ProcessingServer
	{
		public double angels;
		ModelServer Model;
		public override void Process(ModelServer Model)
		{
			this.Model = Model;
			Model.ListUsers[num].Rotate = angels;
		
		}
	}
}
