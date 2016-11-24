using UnityEngine;
using System.Collections;

public class Segment {

	public Vector2	start;
	public Vector2	end;

	public Segment(Vector2 s, Vector2 e)
	{
		start = s;
		end = e;
	}
	static bool ccw(Vector2 A, Vector2 B, Vector2 C)
	{
    	return (C.y-A.y) * (B.x-A.x) > (B.y-A.y) * (C.x-A.x);
	}


	public static bool Intersects(Vector2 A, Vector2 B, Vector2 C, Vector2 D, out Vector2 intersection)
	{
		intersection = Vector2.zero;
    	bool intersect = ccw(A,C,D) != ccw(B,C,D) && ccw(A,B,C) != ccw(A,B,D);

		if (intersect)
		{
			// Get A,B,C of first line - points : ps1 to pe1
			float A1 = B.y-A.y;
			float B1 = A.x-B.x;
			float C1 = A1*A.x+B1*A.y;
			
			// Get A,B,C of second line - points : ps2 to pe2
			float A2 = D.y-C.y;
			float B2 = C.x-D.x;
			float C2 = A2*C.x+B2*C.y;
			 
			// Get delta and check if the lines are parallel
			float delta = A1*B2 - A2*B1;
			 
			// now return the Vector2 intersection point
			intersection = new Vector2(
			    (B2*C1 - B1*C2)/delta,
			    (A1*C2 - A2*C1)/delta
			);
			//calcul of the intersection point:
		}
		
		return intersect;
	}

	public bool Intersects(Segment s2, out Vector2 intersection)
	{
		return Intersects(start, end, s2.start, s2.end, out intersection);
	}

}
