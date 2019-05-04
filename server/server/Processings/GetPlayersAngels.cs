using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Processings
{
	public class GetPlayersAngels : Processing
	{
		double angels;
		int num;

		public GetPlayersAngels(int num,double angels)
		{
			this.num = num;
			this.angels = angels;
		}
			
		public override void Process()
		{
			Model.ListUsers[num].Rotate = angels;
		}
	}
}
