﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
	static class CTransfers
	{
		public static JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

		static public string Reading(NetworkStream nStream)
		{
			byte[] countRead = new byte[4];
			int countReadingBytes = 0;
			while (countReadingBytes != 4)
				countReadingBytes += nStream.Read(countRead, countReadingBytes, countRead.Count() - countReadingBytes);

			countReadingBytes = 0;

			int lengthBytesRaed = BitConverter.ToInt32(countRead, 0);

			byte[] readBytes = new byte[lengthBytesRaed];


			while (countReadingBytes != lengthBytesRaed)
				countReadingBytes += nStream.Read(readBytes, countReadingBytes, readBytes.Count() - countReadingBytes);

			string tmpString = System.Text.Encoding.UTF8.GetString(readBytes);

			return tmpString;
		}

		static public void Writing(object obj, byte numComand, NetworkStream nStream)
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
				try
				{
					nStream.Write(typeComand, 0, 1);//Отпраляет тип команды
					nStream.Write(countRead, 0, 4);//Отпраляет кол-во байт, которое сервер должен будет читать
					nStream.Write(massByts, 0, massByts.Count());
				}
				catch
				{

				}
			}


		}
	}
}
