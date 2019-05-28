using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using System;

namespace Server.Tests
{
	[TestClass]
	public class CTransferTests
	{
		string actual;

		[TestMethod]
		public void Reading_nStream_string() // На вход поступает строка
		{
			//arrange 
			Thread myThread = new Thread(new ThreadStart(serverStart));
			myThread.Start();

			clientStart_Test1();
			string expected = "Test_1_2_3";
			myThread.Join();
			//act	

			//assert 
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Reading_nStream_null() // На вход поступает строка
		{
			//arrange 
			Thread myThread = new Thread(new ThreadStart(serverStart));
			myThread.Start();
			//act	

			//assert 
			Assert.ThrowsException<NullReferenceException>(() =>  clientStart_Test2());
		}

		#region starts
		public void clientStart_Test2()
		{
			TcpClient client = new TcpClient();
			client.Connect("127.0.0.1", 1337);
			NetworkStream streamClient = client.GetStream();

			CTransfers.Writing(null, streamClient);
		}

		public void clientStart_Test1()
		{
			TcpClient client = new TcpClient();
			client.Connect("127.0.0.1", 1337);
			NetworkStream streamClient = client.GetStream();

			CTransfers.Writing("Test_1_2_3", streamClient);
		}

		public void serverStart()
		{
			TcpListener server = new TcpListener(IPAddress.Any, 1337);
			server.Start();

			TcpClient client = server.AcceptTcpClient();
			NetworkStream NS = client.GetStream();

			string TmpText = CTransfers.Reading(NS);

			actual = JsonConvert.DeserializeObject<string>(TmpText);

		}
		#endregion
	}
}
