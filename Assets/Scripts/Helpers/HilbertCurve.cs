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

	static List< Vector2 > GetPath(HilbertTable table, int startX, int startY, int size, int currentX, int currentY, List< Vector2 > ret)
	{
		if (currentX < startX || currentY < startY || currentX >= startX + size || currentY >= startY + size)
			return ret;

		ret.Add(new Vector2(currentX, currentY));
		int lookingFor = table.table[currentX, currentY] + 1;
		if (table.table[currentX + 1, currentY] == lookingFor)
			return GetPath(table, startX, startY, size, currentX + 1, currentY, ret);
		if (table.table[currentX - 1, currentY] == lookingFor)
			return GetPath(table, startX, startY, size, currentX - 1, currentY, ret);
		if (table.table[currentX, currentY + 1] == lookingFor)
			return GetPath(table, startX, startY, size, currentX, currentY + 1, ret);
		if (table.table[currentX, currentY - 1] == lookingFor)
			return GetPath(table, startX, startY, size, currentX, currentY - 1, ret);
		return ret;
	}

	public static List< Vector2 > GetPath(HilbertTable table, int checksize, int max)
	{
		List< Vector2 > ret = null;
		List< Vector2 > tmp = null;
		int				r = Random.Range(0, table.size * table.size);

		//find the position of r;
		//get the path of r with max.

		/*int				startX = Random.Range(1, table.size - checksize - 1);
		int				startY = Random.Range(1, table.size - checksize - 1);

		//path checking on hilbert curve
		for (int x = 0; x < checksize; x++)
			for (int y = 0; y < checksize; y++)
				if (x == 0 || y == 0 || x == checksize - 1 || y == checksize - 1) //if on border
				{
					tmp = new List< Vector2 >();
					tmp = GetPath(table, startX, startY, checksize, startX + x, startY + y, tmp);
					if ((ret == null || tmp.Count > ret.Count) && tmp.Count <= max)
						ret = tmp;
				}*/
		Debug.Log("path count: " + ret.Count);

		return ret;
	}

	public class HilbertTable
	{
		public int[,]	table;
		public int		size;
	}
}
