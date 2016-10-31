using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PolygonEmitter")]
public class PolygonEmitter : ScriptableObject {

	public float						life;
	public float						spwanAt;
	public GameObject					polygonShape;
	public PolygonEmitter[]				OnDieEmitterSpawn;
	public PolygonEmitter[]				OnSpawnEmitterSpawn;
	//emitter loop over spawn patterns and once finished restart until death.
	public List< PolygonSpawnPattern >	polygonSpawnPattern;

}
