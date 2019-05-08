using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	public class Vector
	{
		public double X { get; private set; }

		public double Y { get; private set; }

		public double Length { get; private set; }


		private Vector(double x, double y)
		{
			X = x;
			Y = y;

			CalculateLength();
		}


		// вычисляет вектор по двум точкам
		public static Vector FromPoints(Point start, Point end)
		{
			return new Vector(start.X - end.X, start.Y - end.Y);
		}

		// вычисляет угол между веторами
		public static double CalculateAngleBetween(Vector vector1, Vector vector2)
		{
			if (vector1.Length == 0 || vector2.Length == 0)
			{
				return 0;
			}
			var scalarMultiplier = vector1 * vector2;
			var cos = scalarMultiplier / (vector1.Length * vector2.Length);
			var angle = Math.Acos(cos);

			return angle;
		}

		// вычисляет скалярное произведение векторов
		public static double operator *(Vector a, Vector b)
		{
			return a.X * b.X + a.Y * b.Y;
		}


		// вычисляет длину вектора
		private void CalculateLength()
		{
			var xPow2 = X * X;
			var yPow2 = Y * Y;
			var sum = xPow2 + yPow2;
			var sqrt = Math.Sqrt(sum);

			Length = sqrt;
		}
	}
}
