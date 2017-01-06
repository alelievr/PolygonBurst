using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PolygonEmitter")]
public class PolygonEmitter : ScriptableObject {

	public new string					name;
	public float						life = 5000;
	public float						spwanAt;
	public GameObject					visualObject;
	public float						scale = 1;
	public float						awokenRange = 80;
	public bool							alwaysAwoken = false;
	//emitter loop over spawn patterns and once finished restart until death.
	//default pattern:
	public List< PolygonPatternTransition >	patterns = new List< PolygonPatternTransition >();
	//pattern enable if boss health is less than patternSwitchLifePercent1
	public List< PolygonPatternTransition >	patterns2 = new List< PolygonPatternTransition >();
	public float							patternSwitchLifePercent1;
	//pattern enable if boss health is less than patternSwitchLifePercent2
	public List< PolygonPatternTransition >	patterns3 = new List< PolygonPatternTransition >();
	public float							patternSwitchLifePercent2;

	//pattern executed when boss dies
	public PolygonSpawnPattern				last;
	//pattern executed when boss spawn
	public PolygonSpawnPattern				first;

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
