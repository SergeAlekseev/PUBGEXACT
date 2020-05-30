using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ClassLibrary.ProcessingsClient
{
	public class AngelToZone : ProcessingClient
	{
		public List<UserInfo> listUserInfo;

		public override void Process(ModelClient model)
		{

			model.ListUsers = listUserInfo;
			model.ThisUser = model.ListUsers[model.number];

			model.AngelToZone = defineAngleZone(model.Map.NextZone.ZoneCenterCoordinates,model.ThisUser.userLocation);
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
