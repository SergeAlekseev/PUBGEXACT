using BotLibrary;
using System.Drawing;


namespace BotForm
{
	class BotImpl : Bot
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		protected override void doBot(BotModel model)
		{
			
			Point botLocation = model.ThisUser.userLocation;
			if (model.ThreadStart && model.Map.ListItems.Count > 0)
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
