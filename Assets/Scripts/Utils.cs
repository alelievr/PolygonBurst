using UnityEngine;
using System.Collections;

public static class Utils {

	public static Vector3 Round(Vector3 v, int decimals)
	{
		v.x = (float)System.Math.Round(v.x, decimals);
		v.y = (float)System.Math.Round(v.y, decimals);
		v.z = (float)System.Math.Round(v.z, decimals);
		return v;
	}

}
