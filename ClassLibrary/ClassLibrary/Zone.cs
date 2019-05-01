using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibary
{
	[Serializable]
	public class Zone
	{
		double timeTocompression;
		int zoneRadius;
		Point zoneCenterCoordinates;

		public double TimeTocompression
		{
			get { return timeTocompression; }
			set { timeTocompression = value; }
		}

		public int ZoneRadius
		{
			get { return zoneRadius; }
			set { zoneRadius = value; }
		}

		public int ZoneCenterCoordinateX
		{
			set { zoneCenterCoordinates.X = value; }
		}

		public int ZoneCenterCoordinateY
		{
			set { zoneCenterCoordinates.Y = value; }
		}
		public Point ZoneCenterCoordinates
		{
			get { return zoneCenterCoordinates; }
			set { zoneCenterCoordinates = value; }
		}

		public void NewCenterZone(Rectangle mapBorders,Point prevCenter,int prevRadius) //Костыльный метод. 
		{
			int minX, minY, maxX, maxY;
			minX = prevCenter.X - prevRadius + zoneRadius;
			minY = prevCenter.Y - prevRadius + zoneRadius;
			maxX = prevCenter.X + prevRadius - zoneRadius;
			maxY = prevCenter.Y + prevRadius - zoneRadius;
			if (minX < 0) minX = 0;
			if (minY < 0) minY = 0;
			if (maxY > mapBorders.Width) maxY = mapBorders.Width;
			if (maxX > mapBorders.Height) maxX = mapBorders.Height;
			do
			{
				Random n = new Random();
				zoneCenterCoordinates.X = n.Next(minX, maxX);
				zoneCenterCoordinates.Y = n.Next(minY, maxY);
			}
			while (Math.Sqrt(Math.Pow(prevCenter.X-zoneCenterCoordinates.X,2)+Math.Pow(prevCenter.Y - zoneCenterCoordinates.Y, 2))> prevRadius - zoneRadius);
		}

		public void startCenterZone(Rectangle rectangle)
		{
			Random n = new Random();
			zoneCenterCoordinates.X = n.Next(rectangle.Width);
			zoneCenterCoordinates.Y = n.Next(rectangle.Height);
		}
	}
}
