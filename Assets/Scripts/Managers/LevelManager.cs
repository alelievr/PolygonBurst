using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public PolygonStage		stage;
	//public List< StageTransition > transitions = new List< StageTransition >();

	// int						stageIndex = 0;
	StageManager			stageManager;

	// Use this for initialization
	void Start () {
		
		//Generate map:
		var hilbert = HilbertCurve.GenerateHilbert(6);
		// HilbertCurve.Print(hilbert);
		var path = HilbertCurve.GetPath(hilbert, 10, Random.Range(8, 15), Random.Range(15, 23));
		var map = ProceduralMap.GenerateMap(path, 3);

		//place player and bosses:
		SetupPositions(map);
		
		//fill stageManagers list
		StageManager manager = new StageManager();
		manager.LoadEmitters(stage);
		stageManager = manager;
	}

	void	SetupPositions(ProceduralMap.Map map)
	{
		Globals.player.transform.position = map.rooms[0].position;
		Camera.main.transform.position = map.rooms[0].position;

		int		i = 1;
		foreach (var boss in stage.emitters)
		{
			while (i < map.rooms.Count && map.rooms[i].type != ProceduralMap.ROOM_TYPE.BOSS_ROOM)
				i++;
			boss.position = map.rooms[i++].position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (stage == null || stageManager == null)
			return ;
		// if (stageManagers[stageIndex].isFinished())
		// {
		// 	//TODO: transition
		// 	stageIndex++;
		// }
		// else
		// 	stageManagers[stageIndex].StageFrame();
		if (Input.GetKeyDown(KeyCode.R))
		{
			var hilbert = HilbertCurve.GenerateHilbert(6);
			var path = HilbertCurve.GetPath(hilbert, 10, Random.Range(8, 15), Random.Range(15, 23));

			ProceduralMap.GenerateMap(path, 3);
		}
		stageManager.StageFrame();
	}
}
