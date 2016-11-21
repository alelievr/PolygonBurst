using UnityEngine;
using System.Collections;

public class FolowingCamera : MonoBehaviour {

	public Transform	target;
	public float		maxSpeed = 2;
	public float		smoothVelocity = 1;

	Vector3				velocity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if (target == null)
			return ;

		transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothVelocity, maxSpeed);
	}
}
