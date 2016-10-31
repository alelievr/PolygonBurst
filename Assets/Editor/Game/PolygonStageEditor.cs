using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PolygonStage))]
public class PolygonStageEditor : Editor {

	PolygonStage stage = null;

	public override void OnInspectorGUI()
	{
		//Write custom editor for polygon emitter
		if (GUI.changed) 
        {
            EditorUtility.SetDirty(stage);
        }
	}
}
