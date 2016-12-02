using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour {

	public static PlayerController		player;

	Vector3		basePosition;

	void OnTriggerEnter2D(Collider2D c)
	{
		if (c.tag != PlayerController.playerTag && c.tag != PlayerController.playerBulletTag && c.tag != "Map")
			player.life -= 50;
	}
}
