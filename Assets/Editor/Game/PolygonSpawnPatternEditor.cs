using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

[CustomEditor(typeof(PolygonSpawnPattern))]
public class PolygonSpawnPatternEditor : Editor {

	public PolygonSpawnPattern	spawnPattern = null;

	ReorderableList				spawableObjectsList;

	void OnEnable()
	{
		spawnPattern = (PolygonSpawnPattern)target;
		if (spawnPattern.poly == null)
			spawnPattern.poly = new Polygon();
		
		spawableObjectsList = new ReorderableList(
			serializedObject,
			serializedObject.FindProperty("spawnableObjects"),
			true,
			true,
			true,
			true
		);

		spawableObjectsList.drawElementCallback = 
			(Rect rect, int index, bool isActive, bool isFocused) => 
			{
				var element = spawableObjectsList.serializedProperty.GetArrayElementAtIndex(index);

				rect.y += 2;
				EditorGUI.PropertyField(
					new Rect(rect.x, rect.y, 250, EditorGUIUtility.singleLineHeight),
					element,
					GUIContent.none
				);
			};
		spawableObjectsList.drawHeaderCallback = 
			(Rect rect) =>
			{
				EditorGUI.LabelField(
					rect,
					"Spawnable objects (must have a PolygonBehaviour)"
				);
			};
		//new the uninitialized vas
	}

	void DrawEmitterSettings()
	{
		EditorGUILayout.LabelField("Emitter settings", EditorStyles.boldLabel);

		spawnPattern.spawnDelay = EditorGUILayout.FloatField("spawn delay", spawnPattern.spawnDelay);
		spawnPattern.maxObjects = EditorGUILayout.IntField("max objects", spawnPattern.maxObjects);

		EditorGUILayout.Space();
	}

	void DrawPatternSettings()
	{
		EditorGUILayout.LabelField("Spawn pattern settings", EditorStyles.boldLabel);

		spawnPattern.spawnPattern = (PolygonSpawnPattern.SPAWN_PATTERN)EditorGUILayout.EnumPopup("spawn pattern", spawnPattern.spawnPattern);
		spawnPattern.spawnDisposition = (PolygonSpawnPattern.SPAWN_DISPOSITION)EditorGUILayout.EnumPopup("spawn disposition", spawnPattern.spawnDisposition);
		spawnPattern.objectNumberOnPattern = EditorGUILayout.IntField("spawned object per cycle", spawnPattern.objectNumberOnPattern);
		spawnPattern.spawnPatternSize = EditorGUILayout.FloatField("spawn pattern size", spawnPattern.spawnPatternSize);
		if (spawnPattern.spawnPattern == PolygonSpawnPattern.SPAWN_PATTERN.POINTS)
		{
			EditorGUI.indentLevel++;
			foreach (Vector3 point in spawnPattern.spawnPoints)
				EditorGUILayout.Vector3Field("", point);
			//display points on the editor:
			//Handles.FreeMoveHandle();
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.Space();
	}

	void DrawObjectSettings()
	{
		EditorGUILayout.LabelField("Object settings", EditorStyles.boldLabel);

		var p = spawnPattern.poly;
		//color settings:
		p.colorEvolution = (Polygon.EVOLUTION)EditorGUILayout.EnumPopup("color evolution type", p.colorEvolution);
		switch (p.colorEvolution)
		{
			case Polygon.EVOLUTION.CURVE_ON_LIFETIME:
			case Polygon.EVOLUTION.CURVE_ON_SPEED:
				var e = serializedObject.FindProperty("poly").FindPropertyRelative("colorGradient");
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(e, true, null);
				if(EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
				}
				break ;
			case Polygon.EVOLUTION.CONSTANT:
				p.color1 = EditorGUILayout.ColorField("color", p.color1);
				break ;
			case Polygon.EVOLUTION.RANDOM_BETWEEN:
				p.color1 = EditorGUILayout.ColorField("color 1", p.color1);
				p.color2 = EditorGUILayout.ColorField("color 2", p.color2);
				break ;
		}
		EditorGUILayout.Space();

		//speed settings:
		p.speedEvolution = (Polygon.EVOLUTION)EditorGUILayout.EnumPopup("speed evolution type", p.speedEvolution);
		switch (p.speedEvolution)
		{
			case Polygon.EVOLUTION.CURVE_ON_LIFETIME:
			case Polygon.EVOLUTION.CURVE_ON_SPEED:
				p.speedCurve = EditorGUILayout.CurveField("speed evolution", p.speedCurve);
				break ;
			case Polygon.EVOLUTION.CONSTANT:
				p.speedRandoms.x = EditorGUILayout.FloatField("speed", p.speedRandoms.x);
				break ;
			case Polygon.EVOLUTION.RANDOM_BETWEEN:
				p.speedRandoms = EditorGUILayout.Vector2Field("speed between", p.speedRandoms);
				break ;
		}
		EditorGUILayout.Space();

		//direction settings:
		p.directionModifiers = EditorGUILayout.MaskField(
			"direction modifiers",
			p.directionModifiers,
			System.Enum.GetNames(typeof(Polygon.DIRECTION_MODIFIER))
		);
		if ((p.directionModifiers & (1 << (int)Polygon.DIRECTION_MODIFIER.CURVED)) != 0)
		{
			p.directionCurveX = EditorGUILayout.CurveField("x modifier", p.directionCurveX);
			p.directionCurveY = EditorGUILayout.CurveField("y modifier", p.directionCurveY);
		}
		if ((p.directionModifiers & (1 << (int)Polygon.DIRECTION_MODIFIER.SELF_GUIDEN)) != 0)
			p.directionTargetName = EditorGUILayout.TextField("target name when insatncied", p.directionTargetName);
		if ((p.directionModifiers & (1 << (int)Polygon.DIRECTION_MODIFIER.RANDOM_BETWEEN)) != 0)
			p.directionRandom = EditorGUILayout.Vector2Field("direction random angle bounds", p.directionRandom);
		EditorGUILayout.Space();

		p.scaleEvolution = (Polygon.EVOLUTION)EditorGUILayout.EnumPopup("scale evolution type", p.scaleEvolution);
		switch (p.scaleEvolution)
		{
			case Polygon.EVOLUTION.CURVE_ON_LIFETIME:
			case Polygon.EVOLUTION.CURVE_ON_SPEED:
				p.scaleCurve = EditorGUILayout.CurveField("scale curve", p.scaleCurve);
				break ;
			case Polygon.EVOLUTION.CONSTANT:
				p.scale.x = EditorGUILayout.FloatField("scale", p.scale.x);
				break ;
			case Polygon.EVOLUTION.RANDOM_BETWEEN:
				p.scale = EditorGUILayout.Vector2Field("scale between", p.scale);
				break ;
		}
	}

	public override void OnInspectorGUI()
	{
		DrawEmitterSettings();
		
		serializedObject.Update();
		spawableObjectsList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

		EditorGUILayout.Space();

		DrawPatternSettings();
		
		DrawObjectSettings();
		
		if (GUI.changed) 
        {
            EditorUtility.SetDirty(spawnPattern);
        }
	}
}
