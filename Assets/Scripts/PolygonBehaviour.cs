using UnityEngine;
using System.Collections;

public class PolygonBehaviour : MonoBehaviour {

	public Polygon			poly;

	new SpriteRenderer		renderer;
	float					lifetime;

	void OnEnable()
	{
		renderer = GetComponent< SpriteRenderer >();
		lifetime = 0;
	}

	void OnBecameInvisible()
	{
		enabled = false;
		GameObject.Destroy(gameObject);
	}

	void Start()
	{
		// transform.localScale = Vector3.one * scale;
		Update();
	}

	void Update()
	{
		//transform.position += direction * speedOverLifetime.Evaluate(lifetime);
		//renderer.color = colorOverLifetime.Evaluate(lifetime);
		//lifetime += 0.07f * timeScale;
	}
}