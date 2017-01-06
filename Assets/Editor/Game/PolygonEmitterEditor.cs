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
		GUIStyle 	redText = new GUIStyle();
		Collider2D	c;
		redText.normal.textColor = Color.red;

		//emitter settings
		emitter.name = EditorGUILayout.TextField("name", emitter.name);
		emitter.life = EditorGUILayout.IntField("life points", (int)emitter.life);
		emitter.spwanAt = EditorGUILayout.FloatField("spawn at", emitter.spwanAt);
		emitter.visualObject = (GameObject)EditorGUILayout.ObjectField("visual", emitter.visualObject, typeof(GameObject), false);
		emitter.alwaysAwoken = EditorGUILayout.Toggle("always awoken", emitter.alwaysAwoken);
		if (!emitter.alwaysAwoken)
			emitter.awokenRange = EditorGUILayout.FloatField("awoken range", emitter.awokenRange);
		if (emitter.visualObject != null)
		{
			if (emitter.visualObject.GetComponent< Enemy >() == null)
				EditorGUILayout.LabelField("visual object require the Enemy script as component !", redText);
			if ((c = emitter.visualObject.GetComponent< Collider2D >()) == null || c.isTrigger == false)
				EditorGUILayout.LabelField("visual object require a collider2D in trigger mode !", redText);
		}
		emitter.scale = EditorGUILayout.FloatField("object scale", emitter.scale);

		EditorGUILayout.Space();

		//first spawn pattern:
		emitter.first = (PolygonSpawnPattern)EditorGUILayout.ObjectField("on spawn pattern", emitter.first, typeof(PolygonSpawnPattern), false);

		//last spawn pattern:
		emitter.last = (PolygonSpawnPattern)EditorGUILayout.ObjectField("on death pattern", emitter.last, typeof(PolygonSpawnPattern), false);

		//default pattern list
		serializedObject.Update();
        patternList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

		if (GUI.changed) 
            EditorUtility.SetDirty(emitter);
	}
}
