using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary;
using System.Collections.Generic;
using System.Drawing;

namespace Server.Tests
{
	[TestClass]
	public class ControllerServerTests
	{
		#region Test_PlayerCheck
		[TestMethod]
		public void PlayerCheckTest_null_null() //Если на вход поступает пустой список пользователей и пустая иформация об игроке
		{
			//arrange 
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);

			List<GeneralInfo> listG = null;
			GeneralInfo userG = null;
			GeneralInfo expected = null;

			//act	
			GeneralInfo actual = ControllersS.cInfoUsers.PlayerCheck(listG, userG);

			//assert 
			Assert.AreEqual(expected, actual);
		}


		[TestMethod]
		public void PlayerCheckTest_2_null() //Если на вход поступает список из двух пользователей и пустая иформация об игроке
		{
			//arrange 
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);

			List<GeneralInfo> listG = new List<GeneralInfo>();
			GeneralInfo gen = new GeneralInfo();
			gen.Name = "Test_1";
			gen.Password = "1";
			listG.Add(gen);

			gen = new GeneralInfo();
			gen.Name = "Test_2";
			gen.Password = "1";
			listG.Add(gen);

			GeneralInfo userG = null;
			GeneralInfo expected = null;
			//act
			GeneralInfo actual = ControllersS.cInfoUsers.PlayerCheck(listG, userG);
			//assert 
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void PlayerCheckTest_2_1() //Если на вход поступает список из двух пользователей и иформация об игроке
		{
			//arrange 
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);

			List<GeneralInfo> listG = new List<GeneralInfo>();
			GeneralInfo gen = new GeneralInfo();
			gen.Name = "Test_1";
			gen.Password = "1";
			listG.Add(gen);

			gen = new GeneralInfo();
			gen.Name = "Test_2";
			gen.Password = "1";
			listG.Add(gen);

			GeneralInfo userG = new GeneralInfo();
			userG.Name = "Test_1";
			userG.Password = "1";

			GeneralInfo expected = listG[0];

			//act	
			GeneralInfo actual = ControllersS.cInfoUsers.PlayerCheck(listG, userG);

			//assert 
			Assert.AreEqual(expected, actual);
		}
		#endregion

		#region Test_CheckData
		[TestMethod]
		public void CheckDataTest_null_null()//Если на вход поступает пустой список пользователей и пустая иформация об игроке
		{
			//arrange 
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);
			List<GeneralInfo> listG = null;
			GeneralInfo userG = null;
			bool expected = false;

			//act	
			bool actual = ControllersS.cInfoUsers.CheckData(listG, userG);

			//assert 
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void CheckDataTest_2_null() //Если на вход поступает список из двух пользователей и пустая иформация об игроке
		{
			//arrange 
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);

			List<GeneralInfo> listG = new List<GeneralInfo>();
			GeneralInfo gen = new GeneralInfo();
			gen.Name = "Test_1";
			gen.Password = "1";
			listG.Add(gen);

			gen = new GeneralInfo();
			gen.Name = "Test_2";
			gen.Password = "1";
			listG.Add(gen);

			GeneralInfo userG = null;
			bool expected = false;
			//act
			bool actual = ControllersS.cInfoUsers.CheckData(listG, userG);
			//assert 
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void CheckDataTest_2_1() //Если на вход поступает список из двух пользователей и иформация об игроке
		{
			//arrange 
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);

			List<GeneralInfo> listG = new List<GeneralInfo>();
			GeneralInfo gen = new GeneralInfo();
			gen.Name = "Test_1";
			gen.Password = "1";
			listG.Add(gen);

			gen = new GeneralInfo();
			gen.Name = "Test_2";
			gen.Password = "1";
			listG.Add(gen);

			GeneralInfo userG = new GeneralInfo();
			userG.Name = "Test_1";
			userG.Password = "1";

			bool expected = true;

			//act	
			bool actual = ControllersS.cInfoUsers.CheckData(listG, userG);

			//assert 
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void CheckDataTest_2_1_errorPassword() //Если на вход поступает список из двух пользователей и иформация об игроке, но пароль не верен
		{
			//arrange 
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);

			List<GeneralInfo> listG = new List<GeneralInfo>();
			GeneralInfo gen = new GeneralInfo();
			gen.Name = "Test_1";
			gen.Password = "1";
			listG.Add(gen);

			gen = new GeneralInfo();
			gen.Name = "Test_2";
			gen.Password = "1";
			listG.Add(gen);

			GeneralInfo userG = new GeneralInfo();
			userG.Name = "Test_1";
			userG.Password = "12";

			bool expected = false;

			//act	
			bool actual = ControllersS.cInfoUsers.CheckData(listG, userG);

			//assert 
			Assert.AreEqual(expected, actual);
		}
		#endregion

		#region Test_PlayerSave
		[TestMethod]
		public void PlayerSaveTest_null() //В метод поступает пустое значение
		{
			//arrange 
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);

			List<GeneralInfo> listG = null;
			bool expected = false;

			//act	
			bool actual = ControllersS.cInfoUsers.PlayerSave(listG);

			//assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void PlayerSaveTest_1() //В метод поступает пустое значение
		{
			//arrange 
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);

			List<GeneralInfo> listG = new List<GeneralInfo>();
			GeneralInfo gen = new GeneralInfo();
			gen.Name = "Test_1";
			gen.Password = "1";
			listG.Add(gen);

			gen = new GeneralInfo();
			gen.Name = "Test_2";
			gen.Password = "1";
			listG.Add(gen);

			bool expected = true;
			//act	
			bool actual = ControllersS.cInfoUsers.PlayerSave(listG);

			//assert
			Assert.AreEqual(expected, actual);
		}
		#endregion

		#region Test_GenerateItems
		[TestMethod]
		public void GenerateItemsTests_12000x12000() //Генерация предметов на карте при ОГРОМНЫХ размерах
		{
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);
			model.Map.MapBorders = new Rectangle(0, 0, 12000, 12000);
			bool expected = true;

			bool actual = ControllersS.cMap.GenerateItems();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GenerateItemsTests_6000x6000() //Генерация предметов на карте при нормальных размерах
		{
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);
			model.Map.MapBorders = new Rectangle(0, 0, 6000, 6000);
			bool expected = true;

			bool actual = ControllersS.cMap.GenerateItems();

			Assert.AreEqual(expected, actual);
		}

		#endregion

		#region Test_RandomTree
		[TestMethod]
		public void RandomTreeTests_15000x15000() //Генерация предметов на карте при ОГРОМНЫХ размерах
		{
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);
			model.Map.MapBorders = new Rectangle(0, 0, 15000, 15000);
			model.Map.bordersForUsers = new bool[model.Map.mapBorders.Width, model.Map.mapBorders.Height];
			model.Map.bordersForBullets = new bool[model.Map.mapBorders.Width, model.Map.mapBorders.Height];
			bool expected = true;

			bool actual = ControllersS.cMap.RandomTree();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void RandomTreeTests_6000x6000() //Генерация предметов на карте при нормальных размерах
		{
			ModelServer model = new ModelServer();
			ControllerServer controller = new ControllerServer(model);
			model.Map.MapBorders = new Rectangle(0, 0, 6000, 6000);
			model.Map.bordersForUsers = new bool[model.Map.mapBorders.Width, model.Map.mapBorders.Height];
			model.Map.bordersForBullets = new bool[model.Map.mapBorders.Width, model.Map.mapBorders.Height];
			bool expected = true;

			bool actual = ControllersS.cMap.RandomTree();

			Assert.AreEqual(expected, actual);
		}

		#endregion

		#region Test_CalculateAngleBetween

		[TestMethod]
		public void CalculateAngleBetweenTets_001010_00510()//Вектора стоят под прямым углом
		{
			Vector v1 = Vector.FromPoints(new Point(0, 0), new Point(0, 10));
			Vector v2 = Vector.FromPoints(new Point(0, 0), new Point(10, 0));
			double expected = 90;

			double actual = Vector.CalculateAngleBetween(v1, v2);
			actual= actual/Math.PI * 180;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void CalculateAngleBetweenTets_null_null()//Вектора стоят под прямым углом
		{
			double expected = -1;

			double actual = Vector.CalculateAngleBetween(null, null);

			Assert.AreEqual(expected, actual);
		}
		#endregion
	}
}
