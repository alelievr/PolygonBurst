using UnityEngine;
using System.Collections.Generic;

public class EmitterManager {

	PolygonEmitter			e;
	int						currentSpawnPattern = 0;
	int						currentPatternCount = 0;
	GameObject				emitterObject;
	float					transitionTimeout = 0;
	float					transitionTime = 0;

	public void LoadEmitter (PolygonEmitter emitter) {
		e = emitter;

		emitterObject = GameObject.Instantiate(e.visualObject, e.position, Quaternion.identity) as GameObject;
		emitterObject.transform.localScale *= e.scale;
		Enemy enemy = emitterObject.GetComponent< Enemy >();
		enemy.life = e.life;
		enemy.name = e.name;
		Debug.Log("life: " + e.life);
		e.patterns.ForEach(sp => sp.spawnPattern.attachedGameObject = emitterObject);
	}

	public bool isFinished()
	{
		if (e.life <= 0)
			Debug.Log("emitter " + e.name + " defeated");
		return e.life <= 0;
	}
	
	public void EmitterFrame () {
		if (e.patterns.Count == 0 || Time.realtimeSinceStartup < transitionTimeout + transitionTime)
			return ;
		if (currentSpawnPattern == e.patterns.Count)
		{
			currentSpawnPattern = 0;
			currentPatternCount = 0;
		}
		e.patterns[currentSpawnPattern].spawnPattern.InstanciateFramePolygons();
		if (e.patterns[currentSpawnPattern].spawnPattern.isFinished())
		{
			transitionTimeout = Time.realtimeSinceStartup;
			transitionTime = e.patterns[currentSpawnPattern].delay / 1000;
			if (currentPatternCount == e.patterns[currentSpawnPattern].repeat - 1)
			{
				currentSpawnPattern++;
				currentPatternCount = 0;
			}
			else
				currentPatternCount++;
		}
	}
}
