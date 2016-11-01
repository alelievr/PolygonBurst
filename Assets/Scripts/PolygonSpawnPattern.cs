using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PolygonSpawnPattern")]
public class PolygonSpawnPattern : ScriptableObject {

	//Spawn property
	[TooltipAttribute("in ms")]
	public float					spawnDelay = 20; //in ms
	public int						maxObjects = 20;
	public GameObject[]				spawnableObjects;
	public Vector3					position;

	[Space]
	//Polygon property
	public Vector3					direction;
	public AnimationCurve			speedOverLifeTime;
	public Gradient					colorOverLifeTime;
	public float					scale = 1;

	[Space]
	//Other:
	public float					lifeTimeScale = 1;

	//a lot of datas for spawn patterns (complex and simple)

	int		spawnedObjectsCount;
	float	lastSpawnedObject;

	void OnEnable()
	{
		spawnedObjectsCount = 0;
		lastSpawnedObject = 0;
	}

	public void	InstanciateFramePolygons()
	{
		if (spawnedObjectsCount >= maxObjects && maxObjects != -1)
			return ;
		if (Time.realtimeSinceStartup - lastSpawnedObject < spawnDelay / 1000)
			return ;
		//called each frame to spawn polygons if needed
		//iterator over all to-spawn polygons of the frame and returned it once spawned
		if (spawnableObjects.Length != 0)
		{
			GameObject go = GameObject.Instantiate(spawnableObjects[0], position, Quaternion.Euler(direction)) as GameObject;
			go.transform.localScale = Vector3.one * scale;
			Polygon p = go.GetComponent< Polygon >();
			p.speedOverLifetime = speedOverLifeTime;
			p.colorOverLifetime = colorOverLifeTime;
			p.scale = scale;
			p.timeScale = lifeTimeScale;
			p.direction = direction.normalized;
			spawnedObjectsCount++;
			lastSpawnedObject = Time.realtimeSinceStartup;
		}
	}

	public bool isFinished()
	{
		//check if wave is finished to spawn;
		return false;
	}

	enum DIRECTION {
		DIRECTIONAL,
		SPHERICAL,
	}

	enum SHAPE
	{
		SQUARE,
		CIRCLE,
	}

}
