using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public List< PolygonStage > stages = new List< PolygonStage >();
	public List< StageTransition > transitions = new List< StageTransition >();

	int						stageIndex = 0;
	List< StageManager >	stageManagers = new List< StageManager >();

	// Use this for initialization
	void Start () {
		//fill stageManagers list
		foreach (var stage in stages)
		{
			StageManager manager = new StageManager();
			manager.LoadEmitters(stage);
			stageManagers.Add(manager);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (stages.Count == 0)
			return ;
		if (stageIndex >= stageManagers.Count)
		{
			Debug.Log("no more stages !");
			return ;
		}
		if (stageManagers[stageIndex].isFinished())
		{
			//TODO: transition
			stageIndex++;
		}
		else
			stageManagers[stageIndex].StageFrame();
	}
	
}
