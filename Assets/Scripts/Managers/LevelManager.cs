using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public PolygonStage		stage;
	//public List< StageTransition > transitions = new List< StageTransition >();

	// int						stageIndex = 0;
	StageManager			stageManager;

	// Use this for initialization
	void Start () {
		//fill stageManagers list
		StageManager manager = new StageManager();
		manager.LoadEmitters(stage);
		stageManager = manager;
		
		//Generate map:
		var hilbert = HilbertCurve.GenerateHilbert(3);
		HilbertCurve.Print(hilbert);
		var path = HilbertCurve.GetPath(hilbert);

		//TODO: generate map with path
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
		stageManager.StageFrame();
	}
	
}
