using UnityEngine;
using System.Collections.Generic;

[System.SerializableAttribute]
public class PolygonEmitter {

	public string						name;
	public float						life;
	public float						spwanAt;
	//emitter loop over spawn patterns and once finished restart until death.
	public List< PolygonSpawnPattern >	polygonSpawnPattern = new List< PolygonSpawnPattern >();
	public List< int >					repeatBeforeTransition = new List< int >();
	public List< float >				transitionDelay = new List< float >();

}
