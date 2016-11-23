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

		verts.Add(p + Vector2.zero);
		verts.Add(p + Vector2.right);
		verts.Add(p + Vector2.one);
		verts.Add(p + Vector2.up);
		return verts;
	}

	static List< Vector2 > GenerateCorridorVertices(Vector2 from, Vector2 to)
	{
		List< Vector2 > verts = new List< Vector2 >();

		Vector2 forward = (from - to).normalized;
		var right = new Vector2(0, forward.y);
		var left = -right;
		verts.Add(to + right * .2f);
		verts.Add(from + right * .2f);
		verts.Add(from - right * .2f);
		verts.Add(to - right * .2f);
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

	static List< Vector2 > GeneratePolygonVertices(Map map)
	{
		// bool			firstRoom = true;
		List< Vector2 >	mergedVertices = new List< Vector2 >();
		int i = 0;

		while (true)
		{
			if (i == map.rooms.Count)
				break ;
			var room = map.rooms[i];
			var nextRoom = (i == map.rooms.Count - 1) ? null : map.rooms[i + 1];
			foreach (var edge in room.GetEdges()) //foreach edges of room / corridor
			{
				mergedVertices.Add(edge.start);
				if (nextRoom == null) //if last room continue
					continue ;
				bool intersect = false;
				foreach (var edge2 in nextRoom.GetEdges()) //foreach edges of next room
				{
					Vector2 outPoint;
					if (edge.Intersects(edge2, out outPoint))
					{
						mergedVertices.Add(outPoint);
						intersect = true;
						break ;
					}
				}
				if (intersect)
					break ;
			}
			i++;
		}
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
	}

	public class Room
	{
		public Vector2			position;
		public ROOM_TYPE		type;
		public List< Vector2 >	vertices;

		public IEnumerable< Segment > GetEdges()
		{
			int i;
			for (i = 0; i < vertices.Count - 1; i++)
				yield return new Segment(vertices[i], vertices[i + 1]);
			yield return new Segment(vertices[vertices.Count - 1], vertices[0]);
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
