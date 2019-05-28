using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Net;
using System.Threading;

namespace Client.Tests
{
	[TestClass]
	public class ClientControllerTests
	{
		bool actual;
		#region ConnectTests
		[TestMethod]
		public void TestConnect_127001()//Подключение к существующему серверу
		{
			//arrange 
			ModelClient model = new ModelClient();
			ControllerClient controler = new ControllerClient(model);
			bool expected = true;

			Thread threadClient = new Thread(new ThreadStart(serverStart));
			threadClient.Start();

			//act	
			bool actual = controler.Connect("127.0.0.1");

			//assert 
			Assert.AreEqual(expected, actual);
		}
		[TestMethod]
		public void TestConnect_192168125()//Подключение к несуществующему адресу
		{
			//arrange 
			ModelClient model = new ModelClient();
			ControllerClient controler = new ControllerClient(model);
			bool expected = false;

			Thread threadClient = new Thread(new ThreadStart(serverStart));
			threadClient.Start();

			//act	

			//assert 
			Assert.ThrowsException<NullReferenceException>(() => controler.Connect("192.168.12.5"));
		}

		[TestMethod]
		public void TestConnect_null()//Подключение к нулевому адресу
		{
			//arrange 
			ModelClient model = new ModelClient();
			ControllerClient controler = new ControllerClient(model);
			bool expected = false;

			Thread threadClient = new Thread(new ThreadStart(serverStart));
			threadClient.Start();

			//act	

			//assert 
			Assert.ThrowsException<NullReferenceException>(() => controler.Connect(null));
		}
		#endregion

		#region setNameTests
		[TestMethod]
		public void setNameTest_TestName()//На вход метода поступает любое строковое значение
		{
			ModelClient model = new ModelClient();
			ControllerClient controler = new ControllerClient(model);
			controler.threadStart = true;
			Thread threadClient = new Thread(new ThreadStart(serverStart));
			threadClient.Start();
			model.NStream = clientStart();

			bool expected = true;

			bool actual = controler.setName("TestName");

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void setNameTest_null()//На вход метода поступает нулевое значение
		{
			ModelClient model = new ModelClient();
			ControllerClient controler = new ControllerClient(model);
			controler.threadStart = true;
			Thread threadClient = new Thread(new ThreadStart(serverStart));
			threadClient.Start();
			model.NStream = clientStart();

			bool expected = true;

			bool actual = controler.setName(null);

			Assert.AreEqual(expected, actual);
		}
		#endregion

		public void serverStart()//Замена сервера
		{
			TcpListener server = new TcpListener(IPAddress.Any, 1337);
			server.Start();

			TcpClient client = server.AcceptTcpClient();
			NetworkStream NS = client.GetStream();
		}

		public NetworkStream clientStart()
		{
			TcpClient client = new TcpClient();
			client.Connect("127.0.0.1", 1337);
			NetworkStream streamClient = client.GetStream();
			return streamClient;
		}
	}
}
