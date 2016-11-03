using UnityEngine;

public class PolygonSpawnPatternBehaviour : MonoBehaviour {

	public PolygonSpawnPattern		spawnPattern;

	[HideInInspector]
	public bool						editorMode = false;
	[HideInInspector]
	public Transform				editorParent = null;
	[HideInInspector]
	public bool						editorPlay = false;

	// Use this for initialization
	void Start () {
		spawnPattern.attachedGameObject = gameObject;
	}
	
	// Update is called once per frame
	[ExecuteInEditMode]
	void Update () {
		Debug.Log("here");
		if (editorMode && editorPlay)
			spawnPattern.InstanciateFramePolygons(editorParent);
		else
			spawnPattern.InstanciateFramePolygons();
	}
}
