using UnityEngine;
using System.Collections.Generic;

public class EmitterManager {

	PolygonEmitter			e;
	int						currentSpawnPattern = 0;
	int						currentPatternCount = 0;
	GameObject				emitterObject;

	public void LoadEmitter (PolygonEmitter emitter) {
		e = emitter;

		emitterObject = GameObject.Instantiate(e.visualObject, e.position, Quaternion.identity) as GameObject;
		emitterObject.transform.localScale *= e.scale;
		e.patterns.ForEach(sp => sp.spawnPattern.attachedGameObject = emitterObject);
	}

	public bool isFinished()
	{
		if (e.life <= 0)
			Debug.Log("emitter " + e.name + " defeated");
		return e.life <= 0;
	}
	
	public void EmitterFrame () {
		if (e.patterns.Count == 0)
			return ;
		if (currentSpawnPattern == e.patterns.Count)
		{
			currentSpawnPattern = 0;
			currentPatternCount = 0;
		}
		if (e.patterns[currentSpawnPattern].spawnPattern.isFinished())
		{
			if (currentPatternCount == e.patterns[currentSpawnPattern].repeat - 1)
			{
				currentSpawnPattern++;
				currentPatternCount = 0;
			}
			else
				currentPatternCount++;
		}
		else
			e.patterns[currentSpawnPattern].spawnPattern.InstanciateFramePolygons();
	}
}
