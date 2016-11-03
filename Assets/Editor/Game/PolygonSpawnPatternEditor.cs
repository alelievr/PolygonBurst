using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(PolygonSpawnPattern))]
public class PolygonSpawnPatternEditor : Editor {

	public PolygonSpawnPattern	spawnPattern = null;

	ReorderableList				spawableObjectsList;

	GameObject					parent;
	PolygonSpawnPatternBehaviour	pspb;

	void OnEnable()
	{
		parent = new GameObject("editor-" + this.GetHashCode() % 100);
		pspb = parent.AddComponent< PolygonSpawnPatternBehaviour >();
		pspb.spawnPattern = spawnPattern;
		pspb.editorParent = parent.transform;
		pspb.editorMode = true;

		parent.transform.position = Vector3.zero;
		parent.transform.rotation = Quaternion.identity;

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

		EditorGUILayout.Space();
	}

	void DrawPatternSettings()
	{
		EditorGUILayout.LabelField("Spawn pattern settings", EditorStyles.boldLabel);

		spawnPattern.spawnPattern = (SPAWN_PATTERN)EditorGUILayout.EnumPopup("spawn pattern", spawnPattern.spawnPattern);
		if (spawnPattern.spawnPattern == SPAWN_PATTERN.POINTS)
		{
			EditorGUI.indentLevel++;
			for (int i = 0; i < spawnPattern.spawnPoints.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Vector3Field("", spawnPattern.spawnPoints[i]);
				if (GUILayout.Button("X", GUILayout.Width(50)))
					spawnPattern.spawnPoints.RemoveAt(i);
				EditorGUILayout.EndHorizontal();
			}
			//display points on the editor:
			EditorGUI.indentLevel--;
			if (GUILayout.Button("add point", GUILayout.Width(100)))
				spawnPattern.spawnPoints.Add(Vector3.zero);
		}
		spawnPattern.spawnDisposition = (PolygonSpawnPattern.SPAWN_DISPOSITION)EditorGUILayout.EnumPopup("spawn disposition", spawnPattern.spawnDisposition);
		spawnPattern.spawnObjectsPerWave = EditorGUILayout.IntField("spawned object per waves", spawnPattern.spawnObjectsPerWave);
		spawnPattern.spawnWaveNumber = EditorGUILayout.IntField("max number of waves", spawnPattern.spawnWaveNumber);
		spawnPattern.spawnPatternSize = EditorGUILayout.FloatField("spawn pattern size", spawnPattern.spawnPatternSize);

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
		SceneView.RepaintAll();
	}

	public void OnSceneGUI(SceneView sv)
	{
		if (parent == null)
			return ;
		Vector3 pos = parent.transform.position;
		Quaternion rot = parent.transform.rotation;
		Handles.color = new Color(0, 1, 0, .3f);
		Pattern p = new Pattern(spawnPattern.spawnObjectsPerWave, spawnPattern.spawnPattern);
		switch (spawnPattern.spawnPattern)
		{
			case SPAWN_PATTERN.IN_CIRCLE:
				//Handles.FreeMoveHandle(pos, rot, spawnPattern.spawnPatternSize, Vector3.zero, Handles.CircleCap);
				Handles.DrawSolidDisc(pos, Vector3.back, spawnPattern.spawnPatternSize);
				spawnPattern.spawnPatternSize = Handles.RadiusHandle(rot, pos, spawnPattern.spawnPatternSize, false);

				//display with arraws direction taken by polygons
				foreach (var pi in p.GetNextSpawnInfo())
				{
					float eSize = HandleUtility.GetHandleSize(pos) * 0.05f;
					Handles.ArrowCap(0, pos + pi.position, Quaternion.FromToRotation(Vector3.up, pi.direction), eSize);
				}
				break ;
			case SPAWN_PATTERN.ON_CIRCLE:
				spawnPattern.spawnPatternSize = Handles.RadiusHandle(rot, pos, spawnPattern.spawnPatternSize, false);
				break ;
			case SPAWN_PATTERN.LINE:
				break ;
			case SPAWN_PATTERN.POINTS:
			Handles.color = Color.green;
				float size = HandleUtility.GetHandleSize(pos) * 0.05f;
				for (int i = 0; i < spawnPattern.spawnPoints.Count; i++)
				{
					spawnPattern.spawnPoints[i] = Handles.FreeMoveHandle(pos + spawnPattern.spawnPoints[i], rot, size, Vector3.zero, Handles.DotCap) - pos;
					//snapping
					spawnPattern.spawnPoints[i] = Utils.Round(spawnPattern.spawnPoints[i], 1);
				}
				break ;
			case SPAWN_PATTERN.CURVE:
				break ;
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
		GameObject.DestroyImmediate(parent);
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
	}
}
