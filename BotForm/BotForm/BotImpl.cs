using System;
using System.Drawing;
using BotLibrary;
using ClassLibrary;
using Action = ClassLibrary.Action;

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
			Point botLocation = model.ThisUser.userLocation;

			if (model.threadStart && model.Map.ListItems.Count > 0)
			{
				Point itemLocation = model.Map.ListItems[0].Location;
				moveToPoint(botLocation, itemLocation, false);

				tryTakeItem(itemLocation);
			}
			else
			{
				Point center = model.Map.NextZone.ZoneCenterCoordinates;
				moveToPoint(botLocation, center, false);
			}
		}
	}
}
