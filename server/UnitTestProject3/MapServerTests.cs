using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary;

namespace Server.Tests
{
	[TestClass]
	public class MapServerTests
	{
		[TestMethod]
		public void RemoveTest() //Тест очистки карты
		{
			//arrange 
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);
			bool expected = true;

			//act
			bool actual = model.Map.Remove();

			//assert 
			Assert.AreEqual(expected, actual);
		}
	}
}
