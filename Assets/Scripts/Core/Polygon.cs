using UnityEngine;
using System.Collections;

[System.Serializable]
public class Polygon {

	//non edior stats:
	public Vector3			direction;

	//Color datas:
	public EVOLUTION		colorEvolution = EVOLUTION.CONSTANT;
	public Gradient			colorGradient;
	public Color			color1 = Color.white;
	public Color			color2 = Color.white;
	public bool				colorLoop = false;

	//speed datas:
	public EVOLUTION		speedEvolution = EVOLUTION.CONSTANT;
	public Vector2			speedRandoms; //for constant speed, only x is used
	public AnimationCurve	speedCurve = new AnimationCurve();
	public float			speedMultiplier = 1;

	//direction modifier:
	public int					directionModifiers = 1;
	public float				directionMaxAngularVelocity;
	public Vector2				directionRandom;
	public AnimationCurve		directionCurve = new AnimationCurve();
	public string				directionTargetName;

	//scale datas:
	public EVOLUTION		scaleEvolution = EVOLUTION.CONSTANT;
	public Vector2			scale = Vector2.one; //for constant, only x is used
	public AnimationCurve	scaleCurve = new AnimationCurve();

	//z position evolution:
	public EVOLUTION		zPositionEvolution;
	public Vector2			zPosition;
	public AnimationCurve	zPositionCurve;

	public float			timeScale = 1;
	public bool				dontDestroyOnInvisible = false;
	public float			lifeTime = -1;
	public bool				invincible = false;
}

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
	SELF_GUIDEN,	//folow player until reaches screen
	RANDOM_BETWEEN,	//random angles
	CURVED,			//folow a curve (IMPORTANT: curve must be in repeat mode !)
}