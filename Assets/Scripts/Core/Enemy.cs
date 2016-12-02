using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {

	[HideInInspector]
	public float			life = 2000;

	public static string	enemyTag = "Enemy";
	public static string	enemyBulletTag = "EnemyBullets";

	[HideInInspector]
	public float		lifePercent {
		get {
			return life / maxLife;
		}
	}

	float				maxLife;

	// Use this for initialization
	void Start () {
		gameObject.tag = enemyTag;
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

	void OnTriggerEnter2D(Collider2D c)
	{
		if (c.tag == PlayerController.playerBulletTag || c.tag == PlayerController.playerTag)
		{
			life -= 20;
			Globals.currentBoss = this;
		}
	}
}
