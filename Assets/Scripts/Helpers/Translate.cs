using UnityEngine;
using System.Collections;

public class Translate : MonoBehaviour {

	public float		translateSpeed = 1f; //sec
	public Vector2		point1;
	public Vector2		point2;

	Bounds				bounds;

	void Start () {
		transform.position = point1;
		bounds = new Bounds(point2, Vector3.one * 2);
	}
	
	void Update () {
		Vector2 move = (point2 - point1) * Time.deltaTime * translateSpeed;
		transform.position += (Vector3)move;
		if (bounds.Contains(transform.position))
			transform.position = point1;
	}
}
