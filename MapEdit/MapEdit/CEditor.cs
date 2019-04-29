using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEdit
{
	class CEditor
	{
		public Model model = new Model();

		public void Motion(Point mouseLocation)
		{
			if (mouseLocation.X >= 580 && mouseLocation.Y < 600)
			{
			//	model.MouseCoordSecond = new Point(model.MouseCoordSecond.X+2, model.MouseCoordSecond.Y);
				model.ThisUserLocation = new Point(model.ThisUserLocation.X+2, model.ThisUserLocation.Y);
			}
			if(mouseLocation.X < 600 && mouseLocation.Y >= 580)
			{
			//	model.MouseCoordSecond = new Point(model.MouseCoordSecond.X, model.MouseCoordSecond.Y+2);
				model.ThisUserLocation = new Point(model.ThisUserLocation.X, model.ThisUserLocation.Y+2);
			}
			if (mouseLocation.X <= 20 && mouseLocation.Y < 600)
			{
			//	model.MouseCoordSecond = new Point(model.MouseCoordSecond.X-2, model.MouseCoordSecond.Y);
				model.ThisUserLocation = new Point(model.ThisUserLocation.X-2, model.ThisUserLocation.Y);
			}
			if (mouseLocation.X < 600 && mouseLocation.Y <=20)
			{
			//	model.MouseCoordSecond = new Point(model.MouseCoordSecond.X, model.MouseCoordSecond.Y - 2);
				model.ThisUserLocation = new Point(model.ThisUserLocation.X, model.ThisUserLocation.Y - 2);
			}
			model.MouseCoordSecond = new Point(model.ThisUserLocation.X + model.MouseCoordFirst.X, model.ThisUserLocation.Y + model.MouseCoordFirst.Y);
		}
	}
}
