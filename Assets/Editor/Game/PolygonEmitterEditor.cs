using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PolygonEmitter))]
public class PolygonEmitterEditor : Editor {

	public PolygonEmitter emitter = null;

	public override void OnInspectorGUI()
	{
		//Write custom editor for polygon emitter
		if (GUI.changed) 
        {
            EditorUtility.SetDirty(emitter);
        }
	}
}
