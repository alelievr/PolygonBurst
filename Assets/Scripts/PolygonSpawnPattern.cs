using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PolygonSpawnPattern")]
public class PolygonSpawnPattern : ScriptableObject {

	//Spawn property
	[TooltipAttribute("in ms")]
	public float					spawnDelay = 20; //in ms
	public int						maxObjects = -1;
	public List < GameObject >		spawnableObjects = new List< GameObject >();

	//spawn pattern:
	public SPAWN_PATTERN			spawnPattern;
	public SPAWN_DISPOSITION		spawnDisposition;
	public int						objectNumberOnPattern;
	public float					spawnPatternSize;
	public List< Vector3 >			spawnPoints = new List< Vector3 >();

	[Space]
	//Polygon property
	public Polygon					poly;

	[Space]
	//Other:
	public float					lifeTimeScale = 1;

	//attached gameobject to spawn polygons at his position and rotaion
	public GameObject				attachedGameObject;


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
		if (spawnableObjects.Count != 0)
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
			PolygonBehaviour p = go.AddComponent< PolygonBehaviour >();
			//setup position, direction and rotation for polygon, other params will be set by polygon script
			p.UpdateParams(direction, poly);
			spawnedObjectsCount++;
			lastSpawnedObject = Time.realtimeSinceStartup;
		}
	}

	public bool isFinished()
	{
		//check if wave is finished to spawn;
		return false;
	}

	public enum SPAWN_PATTERN
	{
		ON_CIRCLE,
		IN_CIRCLE,
		LINE,
		POINTS,		//draw with handles
	}

	public enum SPAWN_DISPOSITION
	{
		EQUAL,
		RANDOM,
	}
}
