using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PolygonSpawnPattern")]
public class PolygonSpawnPattern : ScriptableObject {

	//Spawn property
	[TooltipAttribute("in ms")]
	public float					spawnDelayBetweenWaves = 20; //in ms
	public float					spawnDelayInsideWaves = 5; //in ms
	public int						maxObjects = -1;
	public float					rotation;
	public List < GameObject >		spawnableObjects = new List< GameObject >();

	//spawn pattern:
	public Pattern					pattern = new Pattern();
	public int						spawnWavePerCycle = 1;
	public int						spawnWaveNumber = -1;

	[Space]
	//Polygon property
	public Polygon					poly;

	[Space]
	//Other:
	public float					lifeTimeScale = 1;

	//attached gameobject to spawn polygons at his position and rotaion
	public GameObject				attachedGameObject;

	public static GameObject		parent;

	//a lot of datas for spawn patterns (complex and simple)

	int		spawnedObjectsCount;
	int		spawnedObjectInWaveCount;
	int		spawnedWaves;
	float	lastSpawnedObject;
	float	emitterAngle;

	void OnEnable()
	{
		spawnedWaves = 0;
		spawnedObjectsCount = 0;
		spawnedObjectInWaveCount = 0;
		lastSpawnedObject = 0;
		emitterAngle = 0;
	}

	public void	InstanciateFramePolygons(string tag = null, Transform gParent = null)
	{
		Quaternion emitterRotation = Quaternion.Euler(0, 0, emitterAngle);
		emitterAngle += rotation;
		if (parent == null)
			parent = new GameObject("poly parent");
		if (spawnedObjectsCount >= maxObjects && maxObjects != -1)
			return ;
		float timing;
		if (spawnedObjectInWaveCount == spawnWavePerCycle)
			timing = spawnDelayBetweenWaves;
		else
			timing = spawnDelayInsideWaves;
		if (Time.realtimeSinceStartup - lastSpawnedObject < timing / 1000)
			return ;
		if (spawnedObjectInWaveCount == spawnWavePerCycle)
			spawnedObjectInWaveCount = 0;
		//called each frame to spawn polygons if needed
		//iterator over all to-spawn polygons of the frame and returned it once spawned
		foreach (var sp in pattern.GetNextSpawnInfo())
		{
			if (attachedGameObject == null || !attachedGameObject.activeSelf)
				break ;
			Quaternion parentRotation = attachedGameObject.transform.rotation;
			Vector3 direction = parentRotation * emitterRotation * sp.direction;
			Vector3 position = emitterRotation * (parentRotation * sp.position);
			position += attachedGameObject.transform.position;
			GameObject go = GameObject.Instantiate(
				spawnableObjects[0],
				position,
				Quaternion.FromToRotation(Vector3.up, direction)
			) as GameObject;
			if (tag != null)
			go.tag = tag;
			if (gParent != null)
				go.transform.parent = gParent;
			else
				go.transform.parent = parent.transform;
			PolygonBehaviour p = go.GetComponent< PolygonBehaviour >();
			if (p == null)
				p = go.AddComponent< PolygonBehaviour >();
			//setup position, direction and rotation for polygon, other params will be set by polygon script
			p.UpdateParams(direction, poly, attachedGameObject.tag);
			spawnedObjectsCount++;
			lastSpawnedObject = Time.realtimeSinceStartup;
		}
		spawnedObjectInWaveCount++;
		spawnedWaves++;
	}

	public void OnDestroy()
	{
		if (parent != null)
			GameObject.DestroyImmediate(parent);
	}

	public bool isFinished()
	{
		//if (spawnWaveNumber == -1)
			return true;
		//return spawnedWaves == spawnWaveNumber;
	}
}
