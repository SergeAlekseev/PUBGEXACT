using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
	class Box
	{
		Point location;
		int size;
		public Box()
		{
			location = new Point();
			size = 10;
		}
		//Мб добавить фичу, что внутри боксов будет что-то лежать
		public Point Location
		{
			get { return location; }
			set { location = value; }
		}

		public int Size
		{
			get { return size; }
			set { size = value; }
		}
	}
}
