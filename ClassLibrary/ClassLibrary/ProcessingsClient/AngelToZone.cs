using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	public class AngelToZone : ProcessingClient
	{
		public List<UserInfo> listUserInfo;

		public override void Process(ControllerClient controller)
		{

			controller.model.ListUsers = listUserInfo;
			controller.model.ThisUser = controller.model.ListUsers[controller.model.number];

			controller.model.AngelToZone = defineAngleZone(controller.model.Map.NextZone.ZoneCenterCoordinates, controller.model.ThisUser.userLocation);
		}

		//Уберём при рефакторинге контроллера
		public double defineAngleZone(Point onePoint, Point twoPoint)
		{
			Point start1 = new Point { X = onePoint.X, Y = onePoint.Y };
			Point end1 = new Point { X = twoPoint.X, Y = twoPoint.Y };
			Vector vector1 = Vector.FromPoints(start1, end1);

			Point start2 = new Point { X = onePoint.X, Y = onePoint.Y };
			Point end2 = new Point { X = onePoint.X, Y = onePoint.Y + 200 };
			Vector vector2 = Vector.FromPoints(start2, end2);

			double angleRad = Vector.CalculateAngleBetween(vector1, vector2);

			double angleDegree = angleRad / Math.PI * 180;
			if (onePoint.X < twoPoint.X) angleDegree = 360 - angleDegree;

			return angleDegree;
		}
	}
}
