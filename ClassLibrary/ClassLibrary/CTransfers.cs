using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ClassLibrary
{
	public static class CTransfers
	{
		public delegate void ErrorD(string Error);
		public static event ErrorD ErrorEvent;

		public static JsonSerializerSettings jss = new JsonSerializerSettings
		{
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All,
			Formatting = Newtonsoft.Json.Formatting.Indented,
		};

		static public string Reading(NetworkStream nStream)
		{
			byte[] countRead = new byte[4];
			int countReadingBytes = 0;
			do
				countReadingBytes += nStream.Read(countRead, countReadingBytes, countRead.Count() - countReadingBytes);
			while (countReadingBytes != 4 && countReadingBytes > 0);

			if (countReadingBytes == 0)
				throw new System.IO.IOException();

			countReadingBytes = 0;

			int lengthBytesRaed = BitConverter.ToInt32(countRead, 0);

			byte[] readBytes = new byte[lengthBytesRaed];


			while (countReadingBytes != lengthBytesRaed)
				countReadingBytes += nStream.Read(readBytes, countReadingBytes, readBytes.Count() - countReadingBytes);

			string tmpString = System.Text.Encoding.UTF8.GetString(readBytes);

			return tmpString;
		}

		static public void Writing(object obj, NetworkStream nStream)
		{
			string serialized = "";
			try
			{
				lock (obj)
				{
					serialized = JsonConvert.SerializeObject(obj, jss);
				}


				byte[] massByts = Encoding.UTF8.GetBytes(serialized);
				byte[] countRead = BitConverter.GetBytes(massByts.Count());


				lock (nStream)
				{
					try
					{
						nStream.Write(countRead, 0, 4);//Отпраляет кол-во байт, которое сервер должен будет читать
						nStream.Write(massByts, 0, massByts.Count());
					}
					catch (Exception err) { ErrorEvent(err.Message + " |Ошибка в CTransfers, методе Writing"); }
				}
			}
			catch (Exception err) { ErrorEvent(err.Message + " |Ошибка в CTransfers, методе Writing"); }
		}

		static public void WritingInMenu(object obj, byte numComand, NetworkStream nStream)
		{
			string serialized = "";
			lock (obj)
			{
				serialized = JsonConvert.SerializeObject(obj, jss);
			}
			byte[] massByts = Encoding.UTF8.GetBytes(serialized);
			byte[] countRead = BitConverter.GetBytes(massByts.Count());
			byte[] typeComand = new byte[1];
			typeComand[0] = numComand;


			lock (nStream)
			{

				nStream.Write(typeComand, 0, 1);//Отпраляет тип команды
				nStream.Write(countRead, 0, 4);//Отпраляет кол-во байт, которое сервер должен будет читать
				nStream.Write(massByts, 0, massByts.Count());

			}

		}
	}
}
