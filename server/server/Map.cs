using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
	class Map
	{
		//Пока не знаю,что здесь будет лежать
		Box box = new Box();

		public Box Box
		{
			get { return box; }
			set { box = value; }
		}
	}
}
