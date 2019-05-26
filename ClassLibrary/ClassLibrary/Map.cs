using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	[Serializable]
	public class Map
	{
		 List<Box> listBox = new List<Box>();
		 List<Bush> listBush = new List<Bush>();
		 List<Tree> listTrees = new List<Tree>();

		public Rectangle mapBorders = new Rectangle(0, 0, 6000, 6000); //Переделать, чтобы размер зоны изменялся под загружаемую карту___________!!!!
		List<Item> items = new List<Item>();
		Zone nextZone = new Zone();
		Zone prevZone = new Zone();
		public bool[,] bordersForUsers;
		public bool[,] bordersForBullets;

		public Map()
		{
			bordersForUsers = new bool[mapBorders.Width, mapBorders.Height];
			bordersForBullets = new bool[mapBorders.Width, mapBorders.Height];

		}

		public List<Item> ListItems
		{
			get { return items; }
			set { items = value; }
		}

		public List<Tree> ListTrees
		{
			get { return listTrees; }
			set { listTrees = value; }
		}

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
			set
			{
				mapBorders = value;
			}
		}

		public Zone NextZone
		{
			get { return nextZone; }
			set { nextZone = value; }
		}

		public Zone PrevZone
		{
			get { return prevZone; }
			set { prevZone = value; }
		}

		public void Remove()
		{
			listBox = new List<Box>();
			listBush = new List<Bush>();
			listTrees = new List<Tree>();
			nextZone = new Zone();
			prevZone = new Zone();
			bordersForUsers = new bool[mapBorders.Width, mapBorders.Height];
			bordersForBullets = new bool[mapBorders.Width, mapBorders.Height];
		}
	}
}
