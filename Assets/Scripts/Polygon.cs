using UnityEngine;
using System.Collections;

public class Polygon : MonoBehaviour {

	[HideInInspector]
	public Vector3	direction;
	[HideInInspector]
	public float	speed;

	void OnBecameInvisible()
	{
		GameObject.Destroy(gameObject);
	}

}
