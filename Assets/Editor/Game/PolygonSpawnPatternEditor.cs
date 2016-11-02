using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PolygonSpawnPattern))]
public class PolygonSpawnPatternEditor : Editor {

	public PolygonSpawnPattern	spawnPattern = null;

	void OnEnable()
	{
		spawnPattern = (PolygonSpawnPattern)target;
		if (spawnPattern.poly == null)
			spawnPattern.poly = new Polygon();
		//new the uninitialized vas
	}

	public override void OnInspectorGUI()
	{
		//Write custom editor for polygon emitter
		base.OnInspectorGUI();

		EditorGUILayout.LabelField("Emitter settings", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Object settings", EditorStyles.boldLabel);
		
		var poly = spawnPattern.poly;
		//poly.speedOverLifetime = EditorGUILayout.CurveField("speed over time", poly.speedOverLifetime);

		if (GUI.changed) 
        {
            EditorUtility.SetDirty(spawnPattern);
        }
	}
}
