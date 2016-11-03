using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PolygonSpawnPattern")]
public class PolygonSpawnPattern : ScriptableObject {

	//Spawn property
	[TooltipAttribute("in ms")]
	public float					spawnDelayBetweenWaves = 20; //in ms
	public float					spawnDelayInsideWaves = 5; //in ms
	public int						maxObjects = -1;
	public List < GameObject >		spawnableObjects = new List< GameObject >();

	//spawn pattern:
	public SPAWN_PATTERN			spawnPattern = SPAWN_PATTERN.IN_CIRCLE;
	public SPAWN_DISPOSITION		spawnDisposition = SPAWN_DISPOSITION.EQUAL;
	public int						spawnObjectsPerWave = 1;
	public int						spawnWaveNumber = -1;
	public float					spawnPatternSize = 1;
	public List< Vector3 >			spawnPoints = new List< Vector3 >();

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
	float	lastSpawnedObject;

	void OnEnable()
	{
		spawnedObjectsCount = 0;
		lastSpawnedObject = 0;
	}

	public void	InstanciateFramePolygons(Transform gParent = null)
	{
		if (parent == null)
			parent = new GameObject("poly parent");
		if (spawnedObjectsCount >= maxObjects && maxObjects != -1)
			return ;
		if (Time.realtimeSinceStartup - lastSpawnedObject < spawnDelayInsideWaves / 1000)
			return ;
		//called each frame to spawn polygons if needed
		//iterator over all to-spawn polygons of the frame and returned it once spawned
		for (int i = 0; i < spawnObjectsPerWave; i++)
		{
			//spawn pattern algo to generate position and direction:
			Vector3 direction = Vector3.up;
			Vector3 position = Vector3.zero;
			switch (spawnPattern)
			{
				default:
				case SPAWN_PATTERN.ON_CIRCLE:
					if (spawnDisposition == SPAWN_DISPOSITION.EQUAL)
					{
						//circle dispoition with objectNumberOnPattern;
					}
					if (spawnDisposition == SPAWN_DISPOSITION.RANDOM)
					{
						position = Random.insideUnitCircle.normalized;
						direction = position;
						position += attachedGameObject.transform.position;
					}
					break ;
				case SPAWN_PATTERN.IN_CIRCLE:
					if (spawnDisposition == SPAWN_DISPOSITION.EQUAL)
					{
						//circle dispoition with objectNumberOnPattern;
					}	
					if (spawnDisposition == SPAWN_DISPOSITION.RANDOM)
					{
						position = Random.insideUnitCircle;
						direction = position.normalized;
						position += attachedGameObject.transform.position;
					}
					break ;
				case SPAWN_PATTERN.LINE:
					break ;

				case SPAWN_PATTERN.POINTS:
					break ;
			}
			GameObject go = GameObject.Instantiate(
				spawnableObjects[0],
				position,
				Quaternion.FromToRotation(Vector3.up, direction)
			) as GameObject;
			if (gParent != null)
				go.transform.parent = gParent;
			else
				go.transform.parent = parent.transform;
			PolygonBehaviour p = go.AddComponent< PolygonBehaviour >();
			//setup position, direction and rotation for polygon, other params will be set by polygon script
			p.UpdateParams(direction, poly);
			spawnedObjectsCount++;
			lastSpawnedObject = Time.realtimeSinceStartup;
		}
	}

	public void OnDestroy()
	{
		if (parent != null)
			GameObject.DestroyImmediate(parent);
	}

	public bool isFinished()
	{
		//check if wave is finished to spawn;
		return false;
	}

	public enum SPAWN_DISPOSITION
	{
		EQUAL,
		RANDOM,
	}
}
