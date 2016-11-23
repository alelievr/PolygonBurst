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
		if (md.enabled)
		{
			foreach (var room in md.sharedMap.rooms)
			{
				//display map edges
				foreach (var edge in room.GetEdges())
				{
					Debug.DrawLine(edge.start, edge.end, Color.blue);
				}

				//display map room center:
				//change color with type:
				if (room.type == ProceduralMap.ROOM_TYPE.CORRIDOR)
					Handles.color = Color.green;
				else
					Handles.color = Color.red;
				Handles.DotCap(0, room.position, Quaternion.identity, 0.04f);
			}
		}
	}

}
