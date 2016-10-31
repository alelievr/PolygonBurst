using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PolygonSpawnPattern")]
public class PolygonSpawnPattern : ScriptableObject {

	//a lot of datas for spawn patterns (complex and simple)

	public GameObject[]				spawnableObjects;

	public IEnumerable< Polygon >	InstanciateFramePolygons()
	{
		//called each frame to spawn polygons if needed
		//iterator over all to-spawn polygons of the frame and returned it once spawned
		if (spawnableObjects.Length != 0)
			yield return (GameObject.Instantiate(spawnableObjects[0], Vector3.zero, Quaternion.identity) as GameObject).GetComponent< Polygon >();
	}

	public bool isFinished()
	{
		//check if wave is finished to spawn;
		return false;
	}

}
