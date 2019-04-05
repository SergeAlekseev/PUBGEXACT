using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
	class Map
	{
		Box box = new Box();

		public Box Box
		{
			get { return box; }
			set { box = value; }
		}
	}
}
