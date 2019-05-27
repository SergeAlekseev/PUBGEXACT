using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary;
using System.Drawing;

namespace Server.Tests
{
	[TestClass]
	public class ZoneTests
	{
		[TestMethod]
		public void NewCenterZone_600x600_1515_10_trueReturned() //Создание нового центра зоны при существующей границе карты,
																 //Предыдущем центре и ридусе
		{
			//arrange 
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);
			Rectangle rect = new Rectangle(0, 0, 600, 600);
			Point center = new Point(15, 15);
			int radius = 10;

			bool expected = true;

			//act
			bool actual = model.Map.NextZone.NewCenterZone(rect, center, radius);

			//assert 
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void NewCenterZone_600x600_610610_10_falseReturned()//Создание нового центра зоны при существующей границе карты,
																//Предыдущем центре и ридусе, но центр выходит за пределы границы карты.
		{
			//arrange 
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);
			Rectangle rect = new Rectangle(0, 0, 600, 600);
			Point center = new Point(610, 610);
			int radius = 10;

			bool expected = false;

			//act
			bool actual = model.Map.NextZone.NewCenterZone(rect, center, radius);

			//assert 
			Assert.AreEqual(expected, actual);
		}
	}
}
