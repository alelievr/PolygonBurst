using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PolygonEmitterObject))]
public class PolygonEmitterObjectEditor : Editor {

	public PolygonEmitterObject emitter = null;

	void OnEnable()
	{
		emitter = (PolygonEmitterObject)target;
		//new the uninitialized vas
		if (emitter.emitter == null)
			emitter.emitter = new PolygonEmitter();
	}

	public override void OnInspectorGUI()
	{
		//Write custom editor for polygon emitter
		base.OnInspectorGUI();
		if (GUI.changed) 
        {
            EditorUtility.SetDirty(emitter);
        }
	}
}
