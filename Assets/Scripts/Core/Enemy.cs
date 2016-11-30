using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {

	[HideInInspector]
	public float		life = 2000;

	[HideInInspector]
	public float		lifePercent {
		get {
			return life / maxLife;
		}
	}

	float				maxLife;

	// Use this for initialization
	void Start () {
		maxLife = life;
	}
	
	// Update is called once per frame
	void Update () {
		if (life <= 0)
		{
			enabled = false;
			Destroy(transform.gameObject);
		}
	}

	void OnTriggerStay2D(Collider2D c)
	{
		if (c.tag == PlayerController.playerBulletTag || c.tag == PlayerController.playerTag)
		{
			Debug.Log("boss health: " + life);
			life -= 10;
			Globals.currentBoss = this;
		}
	}
}
