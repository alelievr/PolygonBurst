using UnityEngine;
using System.Collections;

public class PolygonEmitterBehaviour : MonoBehaviour {

	public PolygonEmitter		emitter;

	EmitterManager				manager;

	// Use this for initialization
	void Start () {
		manager = new EmitterManager();
		emitter.position = transform.position;
		manager.LoadEmitter(emitter);
	}
	
	// Update is called once per frame
	void Update () {
		manager.EmitterFrame();
		if (manager.isFinished())
		{
			enabled = false;
			GameObject.Destroy(gameObject);
		}
	}
}
