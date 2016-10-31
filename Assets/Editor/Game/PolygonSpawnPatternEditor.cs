using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PolygonSpawnPattern))]
public class PolygonSpawnPatternEditor : Editor {

	public PolygonSpawnPattern spawnPattern = null;

	public override void OnInspectorGUI()
	{
		//Write custom editor for polygon emitter
		if (GUI.changed) 
        {
            EditorUtility.SetDirty(spawnPattern);
        }
	}
}
