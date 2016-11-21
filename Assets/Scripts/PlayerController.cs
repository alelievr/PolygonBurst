using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float					speed = 5f;
	public Vector2					maxSpeed = Vector2.one * 8;
	public Vector2					spawnOffset =  Vector2.one;
	public PolygonSpawnPattern		projectileSpawnPattern;

	// Use this for initialization
	void Start () {
		projectileSpawnPattern.attachedGameObject = gameObject;
		projectileSpawnPattern.maxObjects = -1;
	}
	
	// Update is called once per frame
	void Update () {
		projectileSpawnPattern.InstanciateFramePolygons();
	}

	void FixedUpdate() {
		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		input.x = Mathf.Clamp(input.x, -maxSpeed.x, maxSpeed.x);
		input.y = Mathf.Clamp(input.y, -maxSpeed.y, maxSpeed.y);

		transform.position += (Vector3)input * Time.deltaTime * speed;

		Vector3 mouseDiff = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
		float z = Mathf.Atan2(mouseDiff.x, mouseDiff.y) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, -z);
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		Debug.Log("player hitted !");
		//life--
	}
}
