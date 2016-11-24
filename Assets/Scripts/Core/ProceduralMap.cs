#define PROCEDURAL_DEBUG
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ProceduralMap  {

	static void AddPoint(List< Vector2 > verts, List< Vector2 > uvs, List< int > tris, Vector2 p, int i)
	{
		p *= 2;
		verts.Add(p + Vector2.zero);
		verts.Add(p + Vector2.right);
		verts.Add(p + Vector2.one);
		verts.Add(p + Vector2.up);

		uvs.Add(Vector2.zero);
		uvs.Add(Vector2.right);
		uvs.Add(Vector2.one);
		uvs.Add(Vector2.up);

		tris.Add(i + 0);
		tris.Add(i + 3);
		tris.Add(i + 2);

		tris.Add(i + 2);
		tris.Add(i + 1);
		tris.Add(i + 0);
	}

	static List< Vector2 > GeneratePointVertices(Vector2 p)
	{
		List< Vector2 > verts = new List< Vector2 >();

		Vector2 d = Vector2.one / 2;
		verts.Add(p + Vector2.zero - d);
		verts.Add(p + Vector2.right - d);
		verts.Add(p + Vector2.one - d);
		verts.Add(p + Vector2.up - d);
		return verts;
	}

	static List< Vector2 > GenerateCorridorVertices(Vector2 from, Vector2 to)
	{
		List< Vector2 > verts = new List< Vector2 >();

		Vector2 v = (from - to);
		Vector2 right = new Vector2(-v.y, v.x) / Mathf.Sqrt(v.x * v.x + v.y * v.y) * .2f;
		Vector2 left = new Vector2(-v.y, v.x) / Mathf.Sqrt(v.x * v.x + v.y * v.y) * -.2f;
		Vector2 forward = (from - to).normalized;
		verts.Add(to + right);
		verts.Add(from + right);
		verts.Add(from - right);
		verts.Add(to - right);
		return verts;
	}

	static Map GenerateMapFromPath(List< Vector2 > path, int bossRoomNumber)
	{
		Map	m = new Map();
		int		i = 0;

		foreach (var point in path)
		{
			Room p = new Room();
			p.type = ROOM_TYPE.BASIC_ROOM;
			p.position = point;
			p.vertices = GeneratePointVertices(point); //room shape generation here
			m.rooms.Add(p);

			i++;
			if (i == path.Count) //if last room
			{
				//put something special here
			}
			else
			{
				//add a corridor to the next room
				Debug.Log("added corridor !");
				Room c = new Room();
				c.type = ROOM_TYPE.CORRIDOR;
				c.position = path[i] + (point - path[i]) / 2;
				c.vertices = GenerateCorridorVertices(point, path[i]);//set corridor shape
				m.rooms.Add(c);
			}
		}
		return m;
	}

	static bool GetEdgeIntersectionWithRoom(Room nextRoom, Room room, Segment edge, out int intersectID, out bool order, out Vector2 nearestIntersectionPoint)
	{
		bool	intersect = false;
		int		k;
		float	dst = 1e20f;

		order = true;
		nearestIntersectionPoint = Vector2.zero;
		k = 0;
		intersectID = 0;
		foreach (var edge2 in nextRoom.GetEdges()) //foreach edges of next room
		{
			Vector2 outPoint;
			if (edge.Intersects(edge2, out outPoint))
			{
				float tmpDst = Vector2.Distance(edge.start, outPoint);
				if (tmpDst < dst)
				{
					dst = tmpDst;
					nearestIntersectionPoint = outPoint;
					order = (Utils.PolygonContainsPoint(room.vertices, edge.start)) ? true : false;
					if (room.type == ROOM_TYPE.CORRIDOR)
					{
						order = !order;
						k++;
					}
					intersectID = k;
				}
				intersect = true;
				//determine the iteration order for the next polygon
			}
			k++;
		}	
		if (!intersect)
			intersectID = 0;
		return intersect;
	}

	static List< Vector2 > GeneratePolygonVertices(Map map)
	{
		// bool			firstRoom = true;
		List< Vector2 >	mergedVertices = new List< Vector2 >();
		Vector2 nearestIntersectionPoint = Vector2.zero;
		int i = 0;
		int	j = 0;
		int	inc = 1;
		int	intersectID = 0;
		bool order = true;
		bool intersect = false;

		while (true)
		{
			var room = map.rooms[i];
			if (i == map.rooms.Count - 1)
				inc = -1;
			var nextRoom = (i == 0 && inc == -1) ? null : map.rooms[i + inc];
			j = 0;
			Debug.Log("parkouring poly " + i + " in order: " + order);
			foreach (var edge in room.GetEdges(intersectID, order, (intersect) ? 1 : 0, (nextRoom == null) ? true : false)) //foreach edges of room / corridor
			{
				if (intersect && nextRoom != null)
				{
					Vector2 outPoint;
					Segment oldSegment = new Segment(nearestIntersectionPoint, edge.start);
					if (GetEdgeIntersectionWithRoom(nextRoom, room, oldSegment, out intersectID, out order, out outPoint))
					{
						//FIXME
						mergedVertices.Add(outPoint);
						break ;
					}
				}
				Debug.Log("added vertice from poly " + i + " vertIndex: " + (j - 1));
				mergedVertices.Add(edge.start);
				if (nextRoom == null) //if last room do not check intersections
					continue ;
				intersect = false;
				intersect = GetEdgeIntersectionWithRoom(nextRoom, room, edge, out intersectID, out order, out nearestIntersectionPoint);
				if (intersect)
				{
					Debug.Log("breaked at poly " + i + ", intersect with " + (i + inc) + " at vert " + intersectID);
					mergedVertices.Add(nearestIntersectionPoint);
					break ;
				}
			}
			i += inc;
		if (i == -1)
			break ;
		}
		map.finalVertices = mergedVertices;
		return mergedVertices;
	}

	public static void GenerateMap(List< Vector2 > path, int bossNumber)
	{
		GameObject		mapObject = new GameObject("map");
		Mesh			mesh = new Mesh();
		List< Vector3 >	vertices;
		List< Vector2 >	vertices2D;
		List< Vector2 >	uvs = new List< Vector2 >();
		List< int >		triangles = new List< int >();

		//randomize path:
		path = path.Select(e => e * 2f + new Vector2(Random.value, Random.value)).ToList();

		/*int				i = 0;
		foreach (var point in path)
		{
			//vertices2D.Add(point);
			AddPoint(vertices2D, uvs, triangles, point, i);
			i += 4;
		}*/

		//generate map rooms and corridors
		Map map = GenerateMapFromPath(path, bossNumber);

		//merge all generated polygons:
		vertices2D = GeneratePolygonVertices(map);

		// vertices2D.Add(Vector2.zero);
		// vertices2D.Add(Vector2.up * 10);
		// vertices2D.Add(Vector2.one * 10);
		// vertices2D.Add(Vector2.right * 10);
		// vertices2D.Add(Vector2.right * 8);
		// vertices2D.Add(Vector2.one * 8);
		// vertices2D.Add(Vector2.up * 8 + Vector2.right * 2);
		// vertices2D.Add(Vector2.right * 2);

		Triangulator	tr = new Triangulator(vertices2D.ToArray());
		vertices = vertices2D.ConvertAll< Vector3 >(e => { return (Vector3)e; } );

		mesh.SetVertices(vertices);
		// mesh.SetUVs(0, uvs);
		mesh.triangles = tr.Triangulate();
		// mesh.SetTriangles(triangles, 0);
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mapObject.AddComponent< MeshFilter >().mesh = mesh;
		mapObject.AddComponent< MeshRenderer >().material = new Material(Shader.Find("Sprites/Diffuse"));
		#if PROCEDURAL_DEBUG
		mapObject.AddComponent< MapDebug >().sharedMap = map;
		#endif

		//TODO: add polygonCollider with mesh datas
	}

	public class Map
	{
		public List< Room > rooms = new List< Room >();
		public List< Vector2 > finalVertices;
		public List< Segment > debugSegments = new List< Segment >();
	}

	public class Room
	{
		public Vector2			position;
		public ROOM_TYPE		type;
		public List< Vector2 >	vertices;

		public IEnumerable< Segment > GetEdges(int beginOffset = 0, bool order = true, int additionalIterations = 0, bool stopAtZero = false)
		{
			int i, j, j1;
			if (order)
			{
				for (i = beginOffset; i < vertices.Count + beginOffset + additionalIterations; i++)
				{
					j = i % vertices.Count;
					j1 = (i + 1) % vertices.Count;
					if (stopAtZero && j == 0)
						break ;
					yield return new Segment(vertices[j], vertices[j1]);
				}
			}
			else
			{
				for (i = vertices.Count - beginOffset; i > -beginOffset - additionalIterations; i--)
				{
					j = (i % vertices.Count);
					j1 =(i - 1) % vertices.Count;
					j = (j < 0) ? j + vertices.Count : j;
					j1 = (j1 < 0) ? j1 + vertices.Count : j1;
					if (stopAtZero && j == 0)
						break ;
					yield return new Segment(vertices[j], vertices[j1]);
				}
			}
		}
	}

	public enum ROOM_TYPE
	{
		BOSS_ROOM,
		CORRIDOR,
		BASIC_ROOM,
		HIDDEN_ROOM
	}

}
