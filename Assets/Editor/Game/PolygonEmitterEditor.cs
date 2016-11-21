using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(PolygonEmitter))]
public class PolygonEmitterEditor : Editor {

	public PolygonEmitter emitter = null;

	ReorderableList		patternList;

	void OnEnable()
	{
		emitter = (PolygonEmitter)target;
		//new the uninitialized vas
		// if (emitter == null)
			// emitter = new PolygonEmitter();

		patternList = new ReorderableList(
			serializedObject,
			serializedObject.FindProperty("patterns"),
			true,
			true,
			true,
			true
		);
		patternList.drawElementCallback = 
			(Rect rect, int index, bool isActive, bool isFocused) => 
			{
				var element = patternList.serializedProperty.GetArrayElementAtIndex(index);

				rect.y += 2;
				EditorGUI.PropertyField(
					new Rect(rect.x, rect.y, 250, EditorGUIUtility.singleLineHeight),
					element.FindPropertyRelative("spawnPattern"),
					GUIContent.none
				);
				
				rect.x += 250 + 30;
				EditorGUI.PropertyField(
					new Rect(rect.x, rect.y, 70, EditorGUIUtility.singleLineHeight),
					element.FindPropertyRelative("repeat"),
					GUIContent.none
				);
				rect.x += 70 + 30;
				EditorGUI.PropertyField(
					new Rect(rect.x, rect.y, 70, EditorGUIUtility.singleLineHeight),
					element.FindPropertyRelative("delay"),
					GUIContent.none
				);
				rect.x += 70;
			};
		patternList.drawHeaderCallback = 
			(Rect rect) => 
			{
				EditorGUI.LabelField(
					new Rect(rect.x, rect.y, 250, EditorGUIUtility.singleLineHeight),
					"Attack patterns"
				);
				rect.x += 250 + 30;
				EditorGUI.LabelField(
					new Rect(rect.x + 10, rect.y, 250, EditorGUIUtility.singleLineHeight),
					"repeat"
				);
				rect.x += 70 + 30;
				EditorGUI.LabelField(
					new Rect(rect.x + 10, rect.y, 250, EditorGUIUtility.singleLineHeight),
					"delay"
				);				
			};
	}

	public override void OnInspectorGUI()
	{
		emitter.name = EditorGUILayout.TextField("name", emitter.name);
		emitter.life = EditorGUILayout.IntField("life points", (int)emitter.life);
		emitter.spwanAt = EditorGUILayout.FloatField("spawn at", emitter.spwanAt);
		emitter.visualObject = (GameObject)EditorGUILayout.ObjectField("visual", emitter.visualObject, typeof(GameObject), false);
		emitter.scale = EditorGUILayout.FloatField("object scale", emitter.scale);

		EditorGUILayout.Space();

		serializedObject.Update();
        patternList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

		if (GUI.changed) 
        {
            EditorUtility.SetDirty(emitter);
        }
	}
}
