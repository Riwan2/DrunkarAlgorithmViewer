using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Drunkar_Walk_Viewer
{
	class util
	{
		static Random RandomGen = new Random();

		//Seed
		public static void setRandomSedd(int pSeed)
		{
			RandomGen = new Random(pSeed);
		}

		public static int getInt(int pMin, int pMax)
		{
			return RandomGen.Next(pMin, pMax + 1);
		}

		public static double getDouble()
		{
			return RandomGen.NextDouble();
		}

		public static float getAngle(Vector2 a, Vector2 b)
		{
			float xDiff = a.X - b.X;
			float yDiff = a.Y - b.Y;
			return (float)Math.Atan2(yDiff, xDiff);
		}

		public static int getDistance(Vector2 a, Vector2 b)
		{
			int d = (int)((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
			return (int)(Math.Sqrt(d));
		}

		public static int getHeuristic(Point a, Point b)
		{
			//Manhattan distance on a square grid
			return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
		}
	}
}
