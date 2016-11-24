using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapDebug))]
public class MapDebugEditor : Editor {

	MapDebug		md;

	void OnEnable()
	{
		md = (MapDebug)target;
	}

	void OnSceneGUI()
	{
		GUIStyle blueText = new GUIStyle();
		GUIStyle cyanText = new GUIStyle();
		blueText.normal.textColor = Color.blue;
		cyanText.normal.textColor = Color.cyan;
		if (md.enabled)
		{
			int i = 0;
			foreach (var room in md.sharedMap.rooms)
			{
				//display map edges
				int j = 0;
				foreach (var edge in room.GetEdges())
				{
					Handles.Label(edge.start - Vector2.one * .02f, j.ToString(), blueText);
					Debug.DrawLine(edge.start, edge.end, Color.blue);
					j++;
				}

				//display map room center:
				//change color with type:
				if (room.type == ProceduralMap.ROOM_TYPE.CORRIDOR)
					Handles.color = Color.green;
				else
					Handles.color = Color.red;
				Handles.DotCap(0, room.position, Quaternion.identity, 0.04f);
				Handles.Label(room.position, i.ToString());
				i++;
			}

			//draw final polygon vertices:
			for (i = 0; i < md.sharedMap.finalVertices.Count - 1; i++)
			{
				Handles.Label(md.sharedMap.finalVertices[i] + Vector2.one * 0.02f, i.ToString(), cyanText);
				Debug.DrawLine(md.sharedMap.finalVertices[i], md.sharedMap.finalVertices[i + 1], Color.cyan);
			}
			Debug.DrawLine(md.sharedMap.finalVertices[md.sharedMap.finalVertices.Count - 1], md.sharedMap.finalVertices[0], Color.cyan);

			i = 0;
			foreach (var seg in md.sharedMap.debugSegments)
			{
				Debug.DrawLine(seg.start, seg.end, Color.yellow);
				Handles.color = Color.yellow;
				Handles.Label(seg.start + new Vector2(-1, 1) * .02f, i.ToString());
				Handles.ArrowCap(0, seg.start, Quaternion.FromToRotation(Vector3.back, (seg.start - seg.end).normalized), 0.2f);
				i++;
			}
		}
	}
}
