using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

[CustomEditor(typeof(PolygonStage))]
public class PolygonStageEditor : Editor {

	PolygonStage			stage = null;
	private ReorderableList	emitterList;

	void OnEnable()
	{
		stage = (PolygonStage)target;
		emitterList = new ReorderableList(
			serializedObject, 
			serializedObject.FindProperty("emitters"),
			true,
			true,
			true,
			true
		);

		emitterList.drawElementCallback =
			(Rect rect, int index, bool isActive, bool isFocused) => 
			{
				//get the PolygonEmitter element:
				var element = emitterList.serializedProperty.GetArrayElementAtIndex(index);
				PolygonEmitterObject peo = element.objectReferenceValue as System.Object as PolygonEmitterObject;

				rect.y += 2;
				EditorGUI.PropertyField(
					new Rect(rect.x, rect.y, 300, EditorGUIUtility.singleLineHeight),
					element,
					GUIContent.none
				);
				
				//add missing delay for emitters
				while (stage.transitionDelay.Count < stage.emitters.Count)
					stage.transitionDelay.Add(0);
				
				if (peo != null && peo.emitter != null && stage.transitionDelay.Count > index)
				{
					stage.transitionDelay[index] = EditorGUI.FloatField(
						new Rect(rect.x + 320, rect.y, 60, EditorGUIUtility.singleLineHeight),
						stage.transitionDelay[index]
					);
				}
			};
		emitterList.onSelectCallback = (ReorderableList l) => {  
			var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index).objectReferenceValue as System.Object as PolygonEmitterObject;
			if (prefab)
				EditorGUIUtility.PingObject(prefab);
		};
		emitterList.onRemoveCallback = (ReorderableList l) => {  
			if (l.serializedProperty.GetArrayElementAtIndex(l.index).objectReferenceValue as System.Object as PolygonEmitterObject != null)
				ReorderableList.defaultBehaviours.DoRemoveButton(l);
			ReorderableList.defaultBehaviours.DoRemoveButton(l);
		};
		//new the uninitialized vas
	}

	public override void OnInspectorGUI()
	{
		//Write custom editor for polygon emitter
		stage.name = EditorGUILayout.TextField("name", stage.name);
		serializedObject.Update();
        emitterList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

		if (GUI.changed) 
        {
            EditorUtility.SetDirty(stage);
        }
	}
}
