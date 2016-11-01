using UnityEngine;
using System.Collections.Generic;

public class EmitterManager {

	PolygonEmitter					e;
	int								currentSpawnPattern = 0;
	int								currentPatternCount = 0;

	public void LoadEmitter (PolygonEmitterObject emitter) {
		e = emitter.emitter;
	}

	public bool isFinished()
	{
		if (e.life < 0)
			Debug.Log("emitter " + e.name + " defeated");
		return e.life < 0;
	}
	
	public void EmitterFrame () {
		if (e.polygonSpawnPattern.Count == 0)
			return ;
		if (currentSpawnPattern == e.polygonSpawnPattern.Count)
		{
			currentSpawnPattern = 0;
			currentPatternCount = 0;
		}
		if (e.polygonSpawnPattern[currentSpawnPattern].isFinished())
		{
			if (currentPatternCount == e.repeatBeforeTransition[currentSpawnPattern] - 1)
			{
				currentSpawnPattern++;
				currentPatternCount = 0;
			}
			else
				currentPatternCount++;
		}
	}
}
