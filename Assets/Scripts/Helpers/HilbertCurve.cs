using UnityEngine;
using System.Collections.Generic;

public class HilbertCurve {

	static int	d;

	static void Hilbert(ref HilbertTable table, int x0, int y0, int xi, int xj, int yi, int yj, int n)
	{
		if (n <= 0)
		{
			int X = Mathf.FloorToInt((float)x0 + (float)(xi + yi) / 2f);
			int Y = Mathf.FloorToInt((float)y0 + (float)(xj + yj) / 2f);
			table.table[X, Y] = d++;
			// print '%s %s 0' % (X, Y)
		}
		else
		{
			Hilbert(ref table, x0,               y0,               yi/2, yj/2, xi/2, xj/2, n - 1);
			Hilbert(ref table, x0 + xi/2,        y0 + xj/2,        xi/2, xj/2, yi/2, yj/2, n - 1);
			Hilbert(ref table, x0 + xi/2 + yi/2, y0 + xj/2 + yj/2, xi/2, xj/2, yi/2, yj/2, n - 1);
			Hilbert(ref table, x0 + xi/2 + yi,   y0 + xj/2 + yj,  -yi/2,-yj/2,-xi/2,-xj/2, n - 1);
		}
	}

	public static HilbertTable GenerateHilbert(int size)
	{
		HilbertTable table = new HilbertTable();
		int		s = (int)Mathf.Pow(2, size);
		d = 0;

		table.size = s;
		table.table = new int[s, s];
		Hilbert(ref table, 0, 0, s, 0, 0, s, size);
		return table;
	}

	public static void Print(HilbertTable table)
	{
		for (int y = 0; y < table.size; y++)
		{
			string s = "";
			for (int x = 0; x < table.size; x++)
				s += System.String.Format("{0:00} ", table.table[x, y]);
			Debug.Log(s);
		}
	}

	public static List< Vector2 > GetPath(HilbertTable table)
	{
		List< Vector2 > ret = new List< Vector2 >();

		return ret;
	}

	public class HilbertTable
	{
		public int[,]	table;
		public int		size;
	}

}
