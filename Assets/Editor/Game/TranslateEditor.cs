using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Translate))]
public class TranslateEditor : Editor
{
/*    public override void OnInspectorGUI()
    {
        LevelScript myTarget = (LevelScript)target;
        
        myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
        EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
    }*/
	public void OnSceneGUI()
	{
		Translate t = target as Translate;

		Handles.color = new Color(0, 1, 0);
		if (!Application.isPlaying)
			t.point1 = t.transform.position;
		t.point1 = Handles.FreeMoveHandle(t.point1, Quaternion.identity, .2f, Vector3.zero, Handles.DotCap);
		t.point2 = Handles.FreeMoveHandle(t.point2, Quaternion.identity, .2f, Vector3.zero, Handles.DotCap);
		if (!Application.isPlaying)
			t.transform.position = t.point1;
	}
}
