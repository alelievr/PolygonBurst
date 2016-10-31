using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PolygonSpawnPattern))]
public class PolygonSpawnPatternEditor : Editor {

	public PolygonSpawnPattern spawnPattern = null;

	void OnEnable()
	{
		spawnPattern = (PolygonSpawnPattern)target;
		//new the uninitialized vas
	}

	public override void OnInspectorGUI()
	{
		//Write custom editor for polygon emitter
		base.OnInspectorGUI();
		if (GUI.changed) 
        {
            EditorUtility.SetDirty(spawnPattern);
        }
	}
}
