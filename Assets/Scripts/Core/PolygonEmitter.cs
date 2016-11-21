using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PolygonEmitter")]
public class PolygonEmitter : ScriptableObject {

	public new string					name;
	public float						life;
	public float						spwanAt;
	//emitter loop over spawn patterns and once finished restart until death.
	public List< PolygonPatternTransition >	patterns = new List< PolygonPatternTransition >();
	
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
