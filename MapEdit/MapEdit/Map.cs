using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEdit
{
	[Serializable]
	class Map
	{
		List<Box> listBox = new List<Box>();
		List<Bush> listBush = new List<Bush>();
		static public Rectangle mapBorders = new Rectangle(0, 0, 1200, 1200);

		public bool[,] bordersForUsers = new bool[mapBorders.Width,mapBorders.Height];
		public bool[,] bordersForBullets = new bool[mapBorders.Width, mapBorders.Height];

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
	}
}
