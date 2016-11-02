using UnityEngine;
using System.Collections;

[System.Serializable]
public class Polygon {

	//non edior stats:
	[HideInInspector]
	public Vector3			direction;

	//Color datas:
	[HideInInspector]
	public EVOLUTION		colorEvolution;
	[HideInInspector]
	public Gradient			colorGradient;
	[HideInInspector]
	public Color			color;

	//speed datas:
	[HideInInspector]
	public Vector2			speedRandoms; //for constant speed, only x is used
	[HideInInspector]
	public AnimationCurve	speedCurve;

	//speed modifier on lifetime ?

	//spawn pattern:
	public SPAWN_PATTERN	spawnPattern;
	public float			spawnPatternSize;
	public int				spawnPatternRepeat;

	//direction modifier:
	public DIRECTION_MODIFIER	directionModifiers;
	public Vector2				directionRandom;
	public Vector2				directionCurveX;
	public Vector2				directionCurveY;

	[HideInInspector]
	public float			scale = 1;
	[HideInInspector]
	public float			timeScale = 1;

	public enum EVOLUTION
	{
		RANDOM_BETWEEN,
		CURVE_ON_LIFETIME,
		CURVE_ON_SPEED,
		CONSTANT
	}

	public enum DIRECTION
	{
		VECTOR,
		PLAYER,
	}

	public enum DIRECTION_MODIFIER
	{
		NONE,			//forward
		SELF_GUIDEN,	//folow player until reaches screen
		RANDOM_BETWEEN,	//random angles
		CURVED,			//folow a curve (IMPORTANT: curve must be in repeat mode !)
	}

	public enum SPAWN_PATTERN
	{
		CIRCLE,
		LINE,

	}
}
