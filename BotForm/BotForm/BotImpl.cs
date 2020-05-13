
using BotLibrary;
using ClassLibrary;
using System.Drawing;


namespace BotForm
{
	class BotImpl : Bot
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		protected override void doBot(ModelClient model)
		{
			if (model.serverStart)
			{
				Point center = model.Map.NextZone.ZoneCenterCoordinates;
				Point botLocation = model.ThisUser.userLocation;
				Point userLocation = model.ListUsers[0].userLocation;
				//if (model.ListUsers.Count != 0)
				//userLocation
				//else userLocation = new Point(0, 0);
				moveToPoint(botLocation, center, false);

				captureTarget(userLocation);
			}
		}
	}
}
