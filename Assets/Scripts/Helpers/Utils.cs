using UnityEngine;
using System.Collections.Generic;

public static class Utils {

	public static Vector3 Round(Vector3 v, int decimals)
	{
		v.x = (float)System.Math.Round(v.x, decimals);
		v.y = (float)System.Math.Round(v.y, decimals);
		v.z = (float)System.Math.Round(v.z, decimals);
		return v;
	}

	public static float ClampAngle(float angle, float from, float to)
	{
		if(angle > 180)
			angle = 360 - angle;
		angle = Mathf.Clamp(angle, from, to);
		if(angle < 0)
			angle = 360 + angle;
		return angle;
	}

	public static bool PolygonContainsPoint(List< Vector2 > poly, Vector2 point)
	{
		Bounds polyBounds = new Bounds();

		polyBounds.min = Vector2.one * 1000000;
		polyBounds.max = -Vector2.one * 1000000;
		poly.ForEach(v => {
			polyBounds.min = Vector3.Min(polyBounds.min, v);
			polyBounds.max = Vector3.Min(polyBounds.min, v);
		});
		return polyBounds.Contains(point);
	}
}
