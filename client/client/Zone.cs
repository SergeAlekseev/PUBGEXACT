using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
	class Zone
	{
		double timeTocompression;
		double zoneRadius;
		Point zoneCenterCoordinates;

		public double TimeTocompression
		{
			get { return timeTocompression; }
			set { timeTocompression = value; }
		}

		public double ZoneRadius
		{
			get { return zoneRadius; }
			set { zoneRadius = value; }
		}

		public Point ZoneCenterCoordinates
		{
			get { return zoneCenterCoordinates; }
			set { zoneCenterCoordinates = value; }
		}

		public void NewCenterZone(Point center, double radius) //Костыльный метод. Убрать цикл!!
		{
			do
			{
				Random n = new Random();
				zoneCenterCoordinates.X = n.Next(0, 600);
				zoneCenterCoordinates.Y = n.Next(0, 600);
			}
			while (Math.Pow(radius, 2) <= Math.Sqrt(Math.Pow(radius, 2) + 2 * center.X * zoneCenterCoordinates.X + (zoneCenterCoordinates.Y - center.Y) * (zoneCenterCoordinates.Y - center.Y)));
		}

		public void startCenterZone(Rectangle rectangle)
		{

			Random n = new Random();
			zoneCenterCoordinates.X = n.Next(rectangle.X, rectangle.Height);
			zoneCenterCoordinates.Y = n.Next(rectangle.Y, rectangle.Width);

		}
	}
}
