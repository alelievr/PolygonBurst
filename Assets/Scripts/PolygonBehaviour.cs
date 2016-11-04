using UnityEngine;
using System.Collections;

public class PolygonBehaviour : MonoBehaviour {

	public Polygon			poly;

	float					speed;
	Color					color;
	Vector3					direction;
	Vector3					initialDirection;

	new SpriteRenderer		renderer;
	float					lifetime;
	float					lastTickUpdated;
	float					randomTickTime = .2f;
	float					spawnedTime;
	Vector3					wantedDirection;

	float					maxSpeed = -1e10f;
	float					minSpeed = 1e10f;

	void OnEnable()
	{
		lastTickUpdated = 0;
		spawnedTime = Time.realtimeSinceStartup;
		renderer = GetComponent< SpriteRenderer >();
		lifetime = 0;
	}

	void OnBecameInvisible()
	{
		if (!poly.dontDestroyOnInvisible)
		{
			enabled = false;
			GameObject.Destroy(gameObject);
		}
	}

	void		FindSpeedBounds()
	{
		if (poly.speedEvolution == EVOLUTION.CURVE_ON_LIFETIME
			|| poly.speedEvolution == EVOLUTION.CURVE_ON_SPEED)
		{
			foreach (var k in poly.speedCurve.keys)
			{
				minSpeed = Mathf.Min(minSpeed, k.value);
				maxSpeed = Mathf.Max(maxSpeed, k.value);
			}
		}
		else
		{
			minSpeed = speed;
			maxSpeed = speed;
		}
	}

	public void UpdateParams(Vector3 direction, Polygon p)
	{
		poly = p;
		this.direction = direction;
		initialDirection = direction;
		transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
		if (p.scaleEvolution == EVOLUTION.CONSTANT)
			transform.localScale = p.scale.x * Vector3.one;
		else if (p.scaleEvolution == EVOLUTION.RANDOM_BETWEEN)
			transform.localScale = Vector3.one * Random.Range(p.scale.x, p.scale.y);
		if (p.zPositionEvolution == EVOLUTION.CONSTANT)
			transform.position = new Vector3(transform.position.x, transform.position.y, p.zPosition.x);
		else if (p.zPositionEvolution == EVOLUTION.RANDOM_BETWEEN)
			transform.position = new Vector3(transform.position.x, transform.position.y, Random.Range(p.zPosition.x, p.zPosition.y));
		if (p.speedEvolution == EVOLUTION.CONSTANT)
			speed = p.speedRandoms.x * p.speedMultiplier;
		else if (p.speedEvolution == EVOLUTION.RANDOM_BETWEEN)
			speed = Random.Range(p.speedRandoms.x, p.speedRandoms.y) * p.speedMultiplier;
		FindSpeedBounds();
	}

	void Start()
	{
		// transform.localScale = Vector3.one * scale;
		Update();
	}

	void Update()
	{
		if (!enabled)
			return ;
		//WARNING: DO NOT CHANGE poly ATTRIBUTES VALUES !

		if (direction != Vector3.zero)
			transform.position += direction * speed;

		if (poly.directionModifiers != 0)
		{
			if ((poly.directionModifiers & (1 << (int)DIRECTION_MODIFIER.CURVED)) != 0)
			{
				float angle = poly.directionCurve.Evaluate(lifetime) * .1f;
				wantedDirection = Quaternion.Euler(0, 0, angle) * wantedDirection;
			}
			if ((poly.directionModifiers & (1 << (int)DIRECTION_MODIFIER.RANDOM_BETWEEN)) != 0)
			{
				if (Time.realtimeSinceStartup - lastTickUpdated >= randomTickTime)
					wantedDirection = Quaternion.Euler(0, 0, Random.Range(poly.directionRandom.x, poly.directionRandom.y)) * poly.direction;
			}

			//update direction with maxAngularVelocity and wantedDirection
			float directionAngle = Vector3.Angle(direction, wantedDirection);
			float clampedAngle = Mathf.Clamp(directionAngle, -poly.directionMaxAngularVelocity, poly.directionMaxAngularVelocity);
			direction = Quaternion.Euler(0, 0, clampedAngle) * direction;
		}

		float speedRate = (speed - minSpeed) / (minSpeed - maxSpeed);
		Debug.Log("speedRate: " + speedRate);

		//speed evolution:
		if (poly.speedEvolution == EVOLUTION.CURVE_ON_LIFETIME)
			speed = poly.speedCurve.Evaluate(lifetime) * poly.speedMultiplier;
		if (poly.speedEvolution == EVOLUTION.CURVE_ON_SPEED)
			speed = poly.speedCurve.Evaluate(speedRate) * poly.speedMultiplier;

		//color evolution:
		if (poly.colorEvolution == EVOLUTION.CURVE_ON_LIFETIME)
		{
			float c = (poly.colorLoop) ? Mathf.Repeat(lifetime, 1f) : lifetime;
			renderer.color = poly.colorGradient.Evaluate(c);
		}
		if (poly.colorEvolution == EVOLUTION.CURVE_ON_SPEED)
			renderer.color = poly.colorGradient.Evaluate(speedRate);

		//scale evolution:
		if (poly.scaleEvolution == EVOLUTION.CURVE_ON_LIFETIME)
			transform.localScale = Vector3.one * poly.scaleCurve.Evaluate(lifetime);
		if (poly.scaleEvolution == EVOLUTION.CURVE_ON_SPEED)
			transform.localScale = Vector3.one * poly.scaleCurve.Evaluate(speedRate);

		//z position evolution:
		if (poly.zPositionEvolution == EVOLUTION.CURVE_ON_LIFETIME)
			transform.position = new Vector3(transform.position.x, transform.position.y, poly.zPositionCurve.Evaluate(lifetime));
		if (poly.zPositionEvolution == EVOLUTION.CURVE_ON_SPEED)
			transform.position = new Vector3(transform.position.x, transform.position.y, poly.zPositionCurve.Evaluate(speedRate));
			
		lifetime += 0.05f * poly.timeScale;
		lastTickUpdated = Time.realtimeSinceStartup;
		if (poly.lifeTime != -1 && Time.realtimeSinceStartup - spawnedTime > poly.lifeTime)
		{
			enabled = false;
			Destroy(gameObject);
		}
	}
}