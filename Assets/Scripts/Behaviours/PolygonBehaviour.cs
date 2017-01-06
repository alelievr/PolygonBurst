using UnityEngine;
using System.Collections;

public class PolygonBehaviour : MonoBehaviour {

	const float				scaleMultiplier = 1f;
	const float				fps = 60;

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
	Quaternion				wantedDirection;

	Transform				selfGuidenTarget;

	float					maxSpeed = -1e10f;
	float					minSpeed = 1e10f;

	string					emitterTag;

	Vector3					baseScale;

	void OnEnable()
	{
		lastTickUpdated = 0;
		spawnedTime = Time.realtimeSinceStartup;
		renderer = GetComponent< SpriteRenderer >();
		lifetime = 0;
	}

	void DestroySelf()
	{
		if (!poly.invincible)
		{
			enabled = false;
			Destroy(gameObject);
		}
	}

	void OnBecameInvisible()
	{
		if (!poly.dontDestroyOnInvisible)
			DestroySelf();
	}

	void		OnTriggerEnter2D(Collider2D c)
	{
		if (emitterTag == PlayerController.playerTag && c.tag != PlayerController.playerBulletTag && c.tag != PlayerController.playerTag)
			DestroySelf();
		if (emitterTag == Enemy.enemyTag && c.tag == Enemy.enemyBulletTag && c.tag != Enemy.enemyTag)
			DestroySelf();
		if (c.tag == "Map")
			DestroySelf();
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
		else if (poly.speedEvolution == EVOLUTION.RANDOM_BETWEEN)
		{
			minSpeed = poly.speedRandoms.x;
			minSpeed = poly.speedRandoms.y;
		}
		else
		{
			minSpeed = speed;
			maxSpeed = speed;
		}
	}

	public void UpdateParams(Vector3 direction, Polygon p, string eTag)
	{
		emitterTag = eTag;
		poly = p;
		this.direction = direction;
		baseScale = transform.localScale;
		transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
		wantedDirection = transform.rotation;
		initialDirection = direction;
		//scale:
		if (p.scaleEvolution == EVOLUTION.CONSTANT)
			transform.localScale = p.scale.x * baseScale * scaleMultiplier;
		else if (p.scaleEvolution == EVOLUTION.RANDOM_BETWEEN)
			transform.localScale = baseScale * Random.Range(p.scale.x, p.scale.y) * scaleMultiplier;
		//z:
		if (p.zPositionEvolution == EVOLUTION.CONSTANT)
			transform.position = new Vector3(transform.position.x, transform.position.y, p.zPosition.x);
		else if (p.zPositionEvolution == EVOLUTION.RANDOM_BETWEEN)
			transform.position = new Vector3(transform.position.x, transform.position.y, Random.Range(p.zPosition.x, p.zPosition.y));
		//speed:
		if (p.speedEvolution == EVOLUTION.CONSTANT)
			speed = p.speedRandoms.x * p.speedMultiplier;
		else if (p.speedEvolution == EVOLUTION.RANDOM_BETWEEN)
			speed = Random.Range(p.speedRandoms.x, p.speedRandoms.y) * p.speedMultiplier;
		FindSpeedBounds();

		//color:
		if (poly.colorEvolution == EVOLUTION.CONSTANT)
			renderer.color = poly.color1;
		
		//self guided target
		var t = GameObject.Find(poly.directionTargetName);
		if (t != null)
			selfGuidenTarget = t.transform;
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

		//time rate with skipped farmes
		float tr = Time.deltaTime * fps;
		if (direction != Vector3.zero)
			transform.position += direction * speed * tr;

		if (poly.directionModifiers != 0)
		{
			if ((poly.directionModifiers & (1 << (int)DIRECTION_MODIFIER.RANDOM_BETWEEN)) != 0)
			{
				if (Time.realtimeSinceStartup - lastTickUpdated >= randomTickTime)
				{
					float angle = Random.Range(poly.directionRandom.x, poly.directionRandom.y);
					wantedDirection = Quaternion.FromToRotation(Vector3.up, Quaternion.Euler(0, 0, angle) * initialDirection);
					lastTickUpdated = Time.realtimeSinceStartup;
				}
			}
			if ((poly.directionModifiers & (1 << (int)DIRECTION_MODIFIER.SELF_GUIDEN)) != 0)
			{
				if (selfGuidenTarget != null)
				{
					Vector3 relativePos = selfGuidenTarget.position - transform.position;
				    float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
					wantedDirection = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
					//interpolate between player direction and current diration
					wantedDirection = Quaternion.Slerp(transform.rotation, wantedDirection, .07f);
				}
				else
					wantedDirection = Quaternion.FromToRotation(Vector3.up, direction);
			}
			if ((poly.directionModifiers & (1 << (int)DIRECTION_MODIFIER.CURVED)) != 0)
				wantedDirection *= Quaternion.Euler(0, 0, poly.directionCurve.Evaluate(lifetime));

			//update direction with maxAngularVelocity and wantedDirection
			/*float directionAngle = Vector3.Angle(direction, wantedDirection);

			float clampedAngle = Mathf.Clamp(directionAngle, -poly.directionMaxAngularVelocity, poly.directionMaxAngularVelocity);
			Debug.Log("clamped angle: " + clampedAngle);
			Debug.Log("polygon direction: " + direction);
			direction = Quaternion.AngleAxis(clampedAngle, Vector3.up) * Vector3.up;
			transform.rotation = Quaternion.Euler(direction);*/
			//Vector3 velocity = Vector3.zero;
			//direction = Vector3.SmoothDamp(direction, wantedDirection, ref velocity, .3f, 1);
			// Debug.Log("wantedDirection: " + wantedDirection);
			transform.rotation = Quaternion.Lerp(transform.rotation, wantedDirection, 1f);
			direction = transform.up;
		}

		float speedRate = ((speed * (1 / (poly.speedMultiplier * 10))) - minSpeed) / (maxSpeed - minSpeed);

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
			transform.localScale = baseScale * poly.scaleCurve.Evaluate(lifetime) * scaleMultiplier;
		if (poly.scaleEvolution == EVOLUTION.CURVE_ON_SPEED)
			transform.localScale = baseScale * poly.scaleCurve.Evaluate(speedRate) * scaleMultiplier;

		//z position evolution:
		if (poly.zPositionEvolution == EVOLUTION.CURVE_ON_LIFETIME)
			transform.position = new Vector3(transform.position.x, transform.position.y, poly.zPositionCurve.Evaluate(lifetime));
		if (poly.zPositionEvolution == EVOLUTION.CURVE_ON_SPEED)
			transform.position = new Vector3(transform.position.x, transform.position.y, poly.zPositionCurve.Evaluate(speedRate));
			
		lifetime += 0.01f * poly.timeScale * tr;
		if (poly.lifeTime != -1 && Time.realtimeSinceStartup - spawnedTime > poly.lifeTime)
		{
			enabled = false;
			Destroy(gameObject);
		}
	}
}