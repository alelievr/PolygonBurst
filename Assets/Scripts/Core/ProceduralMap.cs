using UnityEngine;
using System.Collections.Generic;

public class ProceduralMap  {

	static void AddPoint(List< Vector3 > verts, List< Vector2 > uvs, List< int > tris, Vector2 p, int i)
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

	public static void GenerateMap(List< Vector2 > path)
	{
		GameObject		map = new GameObject("map");
		Mesh			mesh = new Mesh();
		List< Vector3 >	vertices = new List< Vector3 >();
		List< Vector2 >	uvs = new List< Vector2 >();
		List< int >		triangles = new List< int >();

		int				i = 0;
		foreach (var point in path)
		{
			AddPoint(vertices, uvs, triangles, point, i);
			i += 4;
		}
		mesh.SetVertices(vertices);
		mesh.SetUVs(0, uvs);
		mesh.SetTriangles(triangles, 0);
		mesh.RecalculateBounds();
		mesh.Optimize();
		map.AddComponent< MeshFilter >().mesh = mesh;
		map.AddComponent< MeshRenderer >().material = new Material(Shader.Find("Sprites/Diffuse"));
	}

}
