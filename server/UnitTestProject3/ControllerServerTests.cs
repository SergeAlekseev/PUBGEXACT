using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary;
using System.Collections.Generic;

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
			GeneralInfo actual = controller.PlayerCheck(listG, userG);

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
			GeneralInfo actual = controller.PlayerCheck(listG, userG);
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
			GeneralInfo actual = controller.PlayerCheck(listG, userG);

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
			bool actual = controller.CheckData(listG, userG);

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
			bool actual = controller.CheckData(listG, userG);
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
			bool actual = controller.CheckData(listG, userG);

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
			bool actual = controller.CheckData(listG, userG);

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
			bool actual = controller.PlayerSave(listG);

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
			bool actual = controller.PlayerSave(listG);

			//assert
			Assert.AreEqual(expected, actual);
		}
		#endregion
	}
}
