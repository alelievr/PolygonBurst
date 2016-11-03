using UnityEngine;
using System.Collections;

[System.Serializable]
public class Polygon {

	//non edior stats:
	public Vector3			direction;

	//Color datas:
	public EVOLUTION		colorEvolution = EVOLUTION.CONSTANT;
	public Gradient			colorGradient;
	public Color			color1;
	public Color			color2;

	//speed datas:
	public EVOLUTION		speedEvolution = EVOLUTION.CONSTANT;
	public Vector2			speedRandoms; //for constant speed, only x is used
	public AnimationCurve	speedCurve = new AnimationCurve();

	//direction modifier:
	public int					directionModifiers = 1;
	public Vector2				directionRandom;
	public AnimationCurve		directionCurveX = new AnimationCurve();
	public AnimationCurve		directionCurveY = new AnimationCurve();
	public string				directionTargetName;

	public EVOLUTION		scaleEvolution = EVOLUTION.CONSTANT;
	public Vector2			scale = Vector2.one; //for constant, only x is used
	public AnimationCurve	scaleCurve = new AnimationCurve();

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
		SELF_GUIDEN,	//folow player until reaches screen
		RANDOM_BETWEEN,	//random angles
		CURVED,			//folow a curve (IMPORTANT: curve must be in repeat mode !)
	}
}
