using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(PolygonSpawnPattern))]
public class PolygonSpawnPatternEditor : Editor {

	public PolygonSpawnPattern	spawnPattern = null;

	ReorderableList				spawableObjectsList;

	void OnEnable()
	{
		SceneView.onSceneGUIDelegate += OnSceneGUI;

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

		spawnPattern.spawnDelayBetweenWaves = EditorGUILayout.FloatField("spawn delay between waves", spawnPattern.spawnDelayBetweenWaves);
		spawnPattern.spawnDelayInsideWaves = EditorGUILayout.FloatField("spawn delay inside waves", spawnPattern.spawnDelayInsideWaves);
		spawnPattern.maxObjects = EditorGUILayout.IntField("max objects", spawnPattern.maxObjects);
		spawnPattern.rotation = EditorGUILayout.FloatField("rotation", spawnPattern.rotation);

		EditorGUILayout.Space();
	}

	void DrawPatternSettings()
	{
		EditorGUILayout.LabelField("Spawn pattern settings", EditorStyles.boldLabel);

		var pa = spawnPattern.pattern;
		pa.spawnPattern = (SPAWN_PATTERN)EditorGUILayout.EnumPopup("spawn pattern", pa.spawnPattern);
		if (pa.spawnPattern == SPAWN_PATTERN.POINTS)
		{
			EditorGUI.indentLevel++;
			int i = 0;
			foreach (var p in pa.customPoints)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Vector3Field("", p);
				if (GUILayout.Button("X", GUILayout.Width(50)))
				{
					pa.customPoints.RemoveAt(i);
					break ;
				}
				EditorGUILayout.EndHorizontal();
				i++;
			}
			//display points on the editor:
			EditorGUI.indentLevel--;
			if (GUILayout.Button("add point", GUILayout.Width(100)))
				pa.customPoints.Add(Vector3.up);
		}
		pa.spawnDisposition = (SPAWN_DISPOSITION)EditorGUILayout.EnumPopup("spawn disposition", pa.spawnDisposition);
		pa.spawnObjectPatternCount = EditorGUILayout.IntField("spawned object per waves", pa.spawnObjectPatternCount);
		pa.spawnPatternSize = EditorGUILayout.FloatField("spawn pattern size", pa.spawnPatternSize);
		spawnPattern.spawnWavePerCycle= EditorGUILayout.IntField("waves per cycle", spawnPattern.spawnWavePerCycle);

		EditorGUILayout.Space();
	}

	void DrawObjectSettings()
	{
		EditorGUILayout.LabelField("Object settings", EditorStyles.boldLabel);

		var p = spawnPattern.poly;
		//color settings:
		p.colorEvolution = (EVOLUTION)EditorGUILayout.EnumPopup("color evolution type", p.colorEvolution);
		switch (p.colorEvolution)
		{
			case EVOLUTION.CURVE_ON_LIFETIME:
			case EVOLUTION.CURVE_ON_SPEED:
				var e = serializedObject.FindProperty("poly").FindPropertyRelative("colorGradient");
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(e, true, null);
				if(EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
				}
				break ;
			case EVOLUTION.CONSTANT:
				p.color1 = EditorGUILayout.ColorField("color", p.color1);
				break ;
			case EVOLUTION.RANDOM_BETWEEN:
				p.color1 = EditorGUILayout.ColorField("color 1", p.color1);
				p.color2 = EditorGUILayout.ColorField("color 2", p.color2);
				break ;
		}
		EditorGUILayout.Space();

		//speed settings:
		p.speedEvolution = (EVOLUTION)EditorGUILayout.EnumPopup("speed evolution type", p.speedEvolution);
		switch (p.speedEvolution)
		{
			case EVOLUTION.CURVE_ON_LIFETIME:
			case EVOLUTION.CURVE_ON_SPEED:
				p.speedCurve = EditorGUILayout.CurveField("speed evolution", p.speedCurve);
				break ;
			case EVOLUTION.CONSTANT:
				p.speedRandoms.x = EditorGUILayout.FloatField("speed", p.speedRandoms.x);
				break ;
			case EVOLUTION.RANDOM_BETWEEN:
				p.speedRandoms = EditorGUILayout.Vector2Field("speed between", p.speedRandoms);
				break ;
		}
		EditorGUILayout.Space();

		//direction settings:
		p.directionModifiers = EditorGUILayout.MaskField(
			"direction modifiers",
			p.directionModifiers,
			System.Enum.GetNames(typeof(DIRECTION_MODIFIER))
		);
		if ((p.directionModifiers & (1 << (int)DIRECTION_MODIFIER.CURVED)) != 0)
		{
			p.directionCurveX = EditorGUILayout.CurveField("x modifier", p.directionCurveX);
			p.directionCurveY = EditorGUILayout.CurveField("y modifier", p.directionCurveY);
		}
		if ((p.directionModifiers & (1 << (int)DIRECTION_MODIFIER.SELF_GUIDEN)) != 0)
			p.directionTargetName = EditorGUILayout.TextField("target name when insatncied", p.directionTargetName);
		if ((p.directionModifiers & (1 << (int)DIRECTION_MODIFIER.RANDOM_BETWEEN)) != 0)
			p.directionRandom = EditorGUILayout.Vector2Field("direction random angle bounds", p.directionRandom);
		EditorGUILayout.Space();

		p.scaleEvolution = (EVOLUTION)EditorGUILayout.EnumPopup("scale evolution type", p.scaleEvolution);
		switch (p.scaleEvolution)
		{
			case EVOLUTION.CURVE_ON_LIFETIME:
			case EVOLUTION.CURVE_ON_SPEED:
				p.scaleCurve = EditorGUILayout.CurveField("scale curve", p.scaleCurve);
				break ;
			case EVOLUTION.CONSTANT:
				p.scale.x = EditorGUILayout.FloatField("scale", p.scale.x);
				break ;
			case EVOLUTION.RANDOM_BETWEEN:
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
		SceneView.RepaintAll();
	}

	public void OnSceneGUI(SceneView sv)
	{
		Vector3 pos = Vector3.zero;
		Quaternion rot = Quaternion.identity;
		Handles.color = new Color(0, 1, 0, .3f);
		var pa = spawnPattern.pattern;
		switch (pa.spawnPattern)
		{
			case SPAWN_PATTERN.IN_CIRCLE:
				//Handles.FreeMoveHandle(pos, rot, spawnPattern.spawnPatternSize, Vector3.zero, Handles.CircleCap);
				Handles.DrawSolidDisc(pos, Vector3.back, pa.spawnPatternSize);
				pa.spawnPatternSize = Handles.RadiusHandle(rot, pos, pa.spawnPatternSize, false);

				//display with arraws direction taken by polygons

				break ;
			case SPAWN_PATTERN.ON_CIRCLE:
				pa.spawnPatternSize = Handles.RadiusHandle(rot, pos, pa.spawnPatternSize, false);
				break ;
			case SPAWN_PATTERN.LINE:
				break ;
			case SPAWN_PATTERN.POINTS:
			Handles.color = Color.green;
				float size = HandleUtility.GetHandleSize(pos) * 0.05f;
				for (int i = 0; i < pa.customPoints.Count; i++)
				{
					pa.customPoints[i] = Handles.FreeMoveHandle(pos + pa.customPoints[i], rot, size, Vector3.zero, Handles.DotCap) - pos;
					//snapping
					pa.customPoints[i] = Utils.Round(pa.customPoints[i], 1);
				}
				break ;
		}
		foreach (var pi in spawnPattern.pattern.GetNextSpawnInfo())
		{
			float eSize = HandleUtility.GetHandleSize(pos) * 0.5f;
			Handles.color = Color.red;
			Handles.ArrowCap(0, pos + pi.position, Quaternion.FromToRotation(Vector3.forward, pi.direction), eSize);
		}
 /*       Handles.BeginGUI();
		if (GUILayout.Button("Play", GUILayout.Width(120)))
			pspb.editorPlay = true;
		if (GUILayout.Button("Stop", GUILayout.Width(120)))
			pspb.editorPlay = false;
		Handles.EndGUI();*/
	}

	void OnDisable()
	{
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
	}

	~PolygonSpawnPatternEditor()
	{
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
	}
}