using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
	class Map
	{
		//Пока не знаю,что здесь будет лежать	
		List<Box> listBox = new List<Box>();
		List<Bush> listBush = new List<Bush>();
		Rectangle mapBorders = new Rectangle(0, 0, 600, 600);
		Zone zone = new Zone();

		public List<Box> ListBox
		{
			get { return listBox; }
			set { listBox = value; }
		}

		public List<Bush> ListBush
		{
			get { return listBush; }
			set { listBush = value; }
		}

		public Rectangle MapBorders
		{
			get { return mapBorders; }
			set { mapBorders = value; }
		}

		public Zone Zone
		{
			get { return zone; }
			set { zone = value; }
		}
	}
}
