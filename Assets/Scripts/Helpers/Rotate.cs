using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public float		rotationSpeed = 30f;

	public void Update()
	{
		transform.Rotate(0, 0, Time.deltaTime * rotationSpeed);
	}

}