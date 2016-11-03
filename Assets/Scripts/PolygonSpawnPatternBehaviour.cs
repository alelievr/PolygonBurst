using UnityEngine;

public class PolygonSpawnPatternBehaviour : MonoBehaviour {

	public PolygonSpawnPattern		spawnPattern;

	// Use this for initialization
	void Start () {
		spawnPattern.attachedGameObject = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		spawnPattern.InstanciateFramePolygons();
	}
}
