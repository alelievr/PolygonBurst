using UnityEngine;
using System.Collections.Generic;

public class EmitterManager {

	PolygonEmitter			e;
	int						patternPointer;
	int						currentSpawnPattern = 0;
	int						currentPatternCount = 0;
	GameObject				emitterObject;
	float					transitionTimeout = 0;
	float					transitionTime = 0;
	Enemy					enemy;
	bool 					first = true;

	public void LoadEmitter (PolygonEmitter emitter) {
		e = emitter;

		emitterObject = GameObject.Instantiate(e.visualObject, e.position, Quaternion.identity) as GameObject;
		emitterObject.transform.localScale *= e.scale;
		emitterObject.GetComponent< SpriteRenderer >().sortingOrder = 2;
		enemy = emitterObject.GetComponent< Enemy >();
		enemy.life = e.life;
		enemy.name = e.name;
		e.patterns.ForEach(sp => sp.spawnPattern.attachedGameObject = emitterObject);
		if (e.first != null)
			e.first.attachedGameObject = emitterObject;
		if (e.last != null)
		e.last.attachedGameObject = emitterObject;
	}

	public bool isFinished()
	{
		if (enemy.life <= 0)
		{
			Debug.Log("emitter " + enemy.name + " defeated");
			if (e.last != null)
				e.last.InstanciateFramePolygons(Enemy.enemyBulletTag);
			Globals.PlayExplosion();
		}
		return enemy.life <= 0;
	}
	
	public void EmitterFrame () {
		if (e.life <= 0 || emitterObject == null)
			return ;
		//if emitter is not in range
		if (!e.alwaysAwoken && Globals.player != null && Vector2.Distance(emitterObject.transform.position, Globals.player.transform.position) > e.awokenRange)
			return ;
		if (first && e.first != null)
		{
			e.first.InstanciateFramePolygons(Enemy.enemyBulletTag);
			first = false;
		}
		if (e.patterns.Count == 0 || Time.realtimeSinceStartup < transitionTimeout + transitionTime)
			return ;
		if (currentSpawnPattern == e.patterns.Count)
		{
			currentSpawnPattern = 0;
			currentPatternCount = 0;
		}
		e.patterns[currentSpawnPattern].spawnPattern.InstanciateFramePolygons(Enemy.enemyBulletTag);
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
