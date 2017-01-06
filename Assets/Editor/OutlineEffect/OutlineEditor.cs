using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomEditor(typeof(Outline))]
public class OutlineEditor : Editor
{
	// draw lines between a chosen game object
	// and a selection of added game objects
	ReorderableList	outlineVerticesList;
	int				selectedID;
	Dictionary< int, int >	dotCapIds = new Dictionary< int, int >();
	int				currentIndex;

	void OnEnable()
	{
		selectedID = -1;

		outlineVerticesList = new ReorderableList(
			serializedObject,
			serializedObject.FindProperty("outlineVertices"),
			true,
			true,
			true,
			true
		);

		outlineVerticesList.drawElementCallback =
			(Rect rect, int index, bool isActive, bool isFocused) =>
			{
				var element = outlineVerticesList.serializedProperty.GetArrayElementAtIndex(index);
				
				EditorGUI.LabelField(
					new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight),
					"v" + index);
				EditorGUI.PropertyField(
					new Rect(rect.x + 50, rect.y, 300, EditorGUIUtility.singleLineHeight),
					element.FindPropertyRelative("position"),
					GUIContent.none
				);
			};
	}

	void CustomDotCap(int id, Vector3 pos, Quaternion rot, float size)
	{
		if (GUIUtility.hotControl == id)
			selectedID = id;
		dotCapIds[currentIndex] = id;
		Handles.DotCap(id, pos, rot, size);
	}

	void DrawEditorLine(Vector3 center, int firstIndex, int secondIndex)
	{
		Outline t = target as Outline;

		Vector3 p1 = t.outlineVertices[firstIndex].position;
		Vector3 p2 = t.outlineVertices[secondIndex].position;
		if (t.outlineBezier && t.outlineVertices.Count > 2)
		{
			Handles.Label(center + t.outlineVertices[firstIndex].position + Vector3.up * .03f, "v " + firstIndex);
			//Vector3 p3 = (firstIndex - 1 >= 0) ? t.outlineVertices[firstIndex - 1].position : t.outlineVertices[t.outlineVertices.Count - 1].position;
			//Vector3 p4 = (secondIndex + 1 < t.outlineVertices.Count) ? t.outlineVertices[secondIndex + 1].position : t.outlineVertices[0].position;
			// p3 = p1 + (p1 - p3);
			// p4 = p2 + (p2 - p4);
			Vector3 p3 = t.outlineVertices[firstIndex].t2;
			Vector3 p4 = t.outlineVertices[secondIndex].t1;
			Handles.DrawBezier(center + p1, center + p2, center + p1 + p3, center + p2 + p4, Color.green, null, 1);
		}
		else
			Handles.DrawLine(center + p1, center + p2);
	}

	bool DrawTangentEditor(int i)
	{
		Outline	t = target as Outline;
		var		v = t.outlineVertices[i];
		var		p = v.position;
		var		center = t.transform.position;
		bool	changed = false;

		Handles.color = Color.magenta;
		//tangent point line:
		Handles.DrawLine(center + p, center + p + v.t1);
		Handles.DrawLine(center + p, center + p + v.t2);
		//tangent points:
		float size = HandleUtility.GetHandleSize(center + p + v.t1) * 0.04f;
		currentIndex = i * 2;
		Vector3	tt = t.outlineVertices[i].t1;
		t.outlineVertices[i].t1 = Handles.FreeMoveHandle(center + p + v.t1, Quaternion.identity, size, Vector3.zero, CustomDotCap) - center - p;
		if (tt != t.outlineVertices[i].t1)
			changed = true;
		size = HandleUtility.GetHandleSize(center + p + v.t2) * 0.04f;
		currentIndex++;
		tt = t.outlineVertices[i].t2;
		t.outlineVertices[i].t2 = Handles.FreeMoveHandle(center + p + v.t2, Quaternion.identity, size, Vector3.zero, CustomDotCap) - center - p;
		if (tt != t.outlineVertices[i].t2)
			changed = true;
		Handles.color = Color.green;
		return changed;
	}
	
	void OnSceneGUI()
	{
		// get the chosen game object
		Outline t = target as Outline;

		if( t == null )
			return;

		if (!t.autoOutline)
		{
			// grab the center of the parent
			Vector3 center = t.transform.position;
			int		selectedIndex = -1;
			bool	changed = false;

			if (GUIUtility.hotControl != 0 && GUIUtility.hotControl != selectedID)
				selectedID = -1;
			
			//draw each editable vertices
			for (int i = 0; i < t.outlineVertices.Count; i++)
			{
				currentIndex = i;
				float size = HandleUtility.GetHandleSize(t.outlineVertices[i].position) * 0.04f;
				Handles.color = Color.green;
				if (i != t.outlineVertices.Count - 1)
					DrawEditorLine(center, i, i + 1);
				Vector3		p = t.outlineVertices[i].position;
				t.outlineVertices[i].position = Handles.FreeMoveHandle(center + t.outlineVertices[i].position, Quaternion.identity, size, Vector3.zero, CustomDotCap) - center;
				if (p != t.outlineVertices[i].position)
					changed = true;
				int dID = -2;
				dotCapIds.TryGetValue(i, out dID);
				if (selectedID == dID)
					selectedIndex = i;
				if (t.outlineBezier)
					if (DrawTangentEditor(i))
						changed = true;
			}

			//last line if enabled
			if (t.lastLinkedToFirst && t.outlineVertices.Count > 2)
				DrawEditorLine(center, t.outlineVertices.Count - 1, 0);
			
			//keyboard event to create / delete seleted node
			if (Event.current.type == EventType.KeyDown)
			{
				if (Event.current.keyCode == KeyCode.P)
				{
					Vector3 point = (selectedID == -1) ? Vector3.zero : t.outlineVertices[selectedIndex].position;
					Vector3 t1 = (selectedID == -1) ? Vector3.zero : t.outlineVertices[selectedIndex].t1;
					Vector3 t2 = (selectedID == -1) ? Vector3.zero : t.outlineVertices[selectedIndex].t2;
					t.outlineVertices.Insert(selectedIndex, new Outline.OutlineVertice(point, t1, t2));
					serializedObject.ApplyModifiedProperties();
					SceneView.RepaintAll();
				}
				if (Event.current.keyCode == KeyCode.M)
				{
					t.outlineVertices.RemoveAt(selectedIndex);
					serializedObject.ApplyModifiedProperties();
					dotCapIds.Remove(selectedIndex);
					SceneView.RepaintAll();
				}
				serializedObject.ApplyModifiedProperties();
			}
			if (changed && !t.autoOutline && (EditorApplication.isPlaying || EditorApplication.isPaused))
				t.CreateLinerendererPoints();
		}
	}

	public override void OnInspectorGUI()
	{
		Outline t = target as Outline;
		
		t.fullSpriteGlow = EditorGUILayout.Toggle("Full glow mode", t.fullSpriteGlow);

		EditorGUILayout.Space();
		t.color = EditorGUILayout.ColorField("Outline color", t.color);
		t.lineThickness = EditorGUILayout.Slider("Line thickness", (int)t.lineThickness, 0, 50);
		t.lineIntensity = EditorGUILayout.Slider("Line Intensity", t.lineIntensity, 0, 1);
		t.alphaCutoff = EditorGUILayout.Slider("Alpha cutoff", t.alphaCutoff, 0, 1);
		t.pixelSnap = EditorGUILayout.Toggle("Pixel snap", t.pixelSnap);

		EditorGUILayout.Space();
		t.flipY = EditorGUILayout.Toggle("flipY", t.flipY);
		t.hideSprite = EditorGUILayout.Toggle("Hide sprite", t.hideSprite);
		t.allowOutlineOverlap = EditorGUILayout.Toggle("Allow outline overlap", t.allowOutlineOverlap);
		t.autoColor = EditorGUILayout.Toggle("Auto color outline", t.autoColor);
		
		EditorGUILayout.Space();
		t.autoOutline = EditorGUILayout.Toggle("Auto outline", t.autoOutline);
		if (!t.autoOutline)
		{
			t.lastLinkedToFirst = EditorGUILayout.Toggle("Link first and last points", t.lastLinkedToFirst);
			t.outlineBezier = EditorGUILayout.Toggle("Bezier lines", t.outlineBezier);
			t.outlineStep = EditorGUILayout.Slider("Outline step", t.outlineStep, .005f, 1f);

			serializedObject.Update();
			outlineVerticesList.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}

		if (GUI.changed)
		{
			SceneView.RepaintAll();
			if (EditorApplication.isPlaying || EditorApplication.isPaused)
				if (!t.autoOutline)
					t.CreateLinerendererPoints();
		}
	}
}