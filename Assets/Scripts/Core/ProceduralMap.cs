//#define PROCEDURAL_DEBUG
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ProceduralMap  {

	static List< Vector2 > GenerateRoomVertices(Vector2 p, ROOM_TYPE type)
	{
		List< Vector2 >	verts = new List< Vector2 >();
		float			roomScale = 1;
		float			noiseScale = 0.05f;
		int				nVertices = 40;
		float			degPadd = (360f / nVertices);
		float			deg = 125;

		float rx = Random.Range(1f, 1.5f);
		float ry = Random.Range(1f, 1.5f);
		for (int i = 0; i < nVertices; i++, deg += degPadd)
		{
			float x = Mathf.Cos(deg * Mathf.Deg2Rad);
			float y = Mathf.Sin(deg * Mathf.Deg2Rad);
			
			Vector2 roomPoint = new Vector2(x * rx, y * ry);
			float noiseModifier = PerlinNoise2D.GenerateNoise(x + p.x, y + p.y, .05f, 2);
			noiseModifier = (noiseModifier - .5f) * 2 * noiseScale;
			verts.Add(p + roomPoint * roomScale + (roomPoint * noiseModifier));
		}
		return verts;
	}

	static List< Vector2 > GenerateCorridorVertices(Vector2 from, Vector2 to)
	{
		List< Vector2 > verts = new List< Vector2 >();

		Vector2 v = (from - to);
		Vector2 right = new Vector2(-v.y, v.x) / Mathf.Sqrt(v.x * v.x + v.y * v.y) * .2f;
		verts.Add(to + right);
		verts.Add(from + right);
		verts.Add(from - right);
		verts.Add(to - right);

		//TODO: generate intermediate vertices
		return verts;
	}

	static Map GenerateMapFromPath(List< Vector2 > path, int bossRoomNumber)
	{
		Map	m = new Map();
		int		i = 0;

		foreach (var point in path)
		{
			Room p = new Room();
			p.type = ROOM_TYPE.BOSS_ROOM;
			p.position = point;
			p.vertices = GenerateRoomVertices(point, p.type);
			m.rooms.Add(p);

			i++;
			if (i == path.Count) //if last room
			{
				//put something special here
			}
			else
			{
				//add a corridor to the next room
				Room c = new Room();
				c.type = ROOM_TYPE.CORRIDOR;
				c.position = path[i] + (point - path[i]) / 2;
				c.vertices = GenerateCorridorVertices(point, path[i]);
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
		Vector2 minVertice = Vector2.one * 1e20f;
		int i = 0;
		int	inc = 1;
		int	intersectID = 0;
		bool order = true;
		bool intersect = false;

		//TODO: check if the vertice 0 of the first room is inside the next room (if yes choose another)
		while (true)
		{
			var room = map.rooms[i];
			if (i == map.rooms.Count - 1)
				inc = -1;
			var nextRoom = (i == 0 && inc == -1) ? null : map.rooms[i + inc];
			// Debug.Log("parkouring poly " + i + " in order: " + order);
			foreach (var edge in room.GetEdges(intersectID, order, (intersect) ? 1 : 0, (nextRoom == null) ? true : false)) //foreach edges of room / corridor
			{
				if (intersect && i != map.rooms.Count - 1 && nextRoom != null)
				{
					Vector2 outPoint;
					Segment oldSegment = new Segment(nearestIntersectionPoint, edge.start);
					if (GetEdgeIntersectionWithRoom(nextRoom, room, oldSegment, out intersectID, out order, out outPoint))
					{
						if (outPoint.x < minVertice.x)
						{
							minVertice.x = outPoint.x;
							map.minVerticeIndex = mergedVertices.Count;
						}
						mergedVertices.Add(outPoint);
						break ;
					}
				}
				// Debug.Log("added vertice from poly " + i + " vertIndex: " + (j - 1));
				if (edge.start.x < minVertice.x)
				{
					minVertice.x = edge.start.x;
					map.minVerticeIndex = mergedVertices.Count;
				}
				mergedVertices.Add(edge.start);
				if (nextRoom == null) //if last room do not check intersections
					continue ;
				intersect = false;
				intersect = GetEdgeIntersectionWithRoom(nextRoom, room, edge, out intersectID, out order, out nearestIntersectionPoint);
				if (intersect)
				{
					// Debug.Log("breaked at poly " + i + ", intersect with " + (i + inc) + " at vert " + intersectID);
					if (nearestIntersectionPoint.x < minVertice.x)
					{
						minVertice.x = nearestIntersectionPoint.x;
						map.minVerticeIndex = mergedVertices.Count;
					}					
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

	public static Map GenerateMap(List< Vector2 > path, int bossNumber, float mapScale = 1)
	{
		GameObject		mapObject = new GameObject("map");
		Mesh			mesh = new Mesh();
		List< Vector3 >	vertices;
		List< Vector2 >	vertices2D;
		mapScale *= 8;

		//randomize path:
		path = path.Select(e => e * 5f + new Vector2(Random.value, Random.value) * 2).ToList();

		//generate map rooms and corridors
		Map map = GenerateMapFromPath(path, bossNumber);

		//merge all generated polygons:
		vertices2D = GeneratePolygonVertices(map);

		Triangulator	tr = new Triangulator(vertices2D.ToArray());
		vertices = vertices2D.ConvertAll< Vector3 >(e => { return (Vector3)e; } );

		mesh.SetVertices(vertices);
		mesh.triangles = tr.Triangulate();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mapObject.tag = "Map";
		mapObject.transform.localScale = Vector3.one * mapScale;
		mapObject.AddComponent< MeshFilter >().mesh = mesh;
		mapObject.AddComponent< MeshRenderer >().material = Globals.spriteLitMaterial;
		#if PROCEDURAL_DEBUG
		mapObject.AddComponent< MapDebug >().sharedMap = map;
		#endif

		map.rooms.ForEach(e => e.position *= mapScale);

		EdgeCollider2D edgeCollidder = mapObject.AddComponent< EdgeCollider2D >();
		edgeCollidder.points = vertices2D.ToArray();

		//add kinimatic rigidbody2d to map for bullet collisions:
		Rigidbody2D rbody = mapObject.AddComponent< Rigidbody2D >();
		rbody.isKinematic = true;
		rbody.freezeRotation = true;
		rbody.constraints = RigidbodyConstraints2D.FreezeAll;

		return map;
	}

	public class Map
	{
		public List< Room >		rooms = new List< Room >();
		public List< Vector2 >	finalVertices;
		public List< Segment >	debugSegments = new List< Segment >();
		public int				minVerticeIndex = -1;
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
