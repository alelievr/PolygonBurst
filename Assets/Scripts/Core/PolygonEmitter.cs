using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PolygonEmitter")]
public class PolygonEmitter : ScriptableObject {

	public new string					name;
	public float						life;
	public float						spwanAt;
	public GameObject					visualObject;
	public float						scale = 1;
	//emitter loop over spawn patterns and once finished restart until death.
	public List< PolygonPatternTransition >	patterns = new List< PolygonPatternTransition >();

	public Vector3						position;
	public Quaternion					rotation;
	
	[System.Serializable]
	public class PolygonPatternTransition
	{
		public PolygonSpawnPattern		spawnPattern;
		public int						repeat;
		public float					delay;

		public PolygonPatternTransition()
		{
			repeat = 1;
		}
	};
}
