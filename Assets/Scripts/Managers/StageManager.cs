using UnityEngine;
using System.Collections.Generic;

public class StageManager {

	List< EmitterManager >	emitters = new List< EmitterManager >();
	List< float >			transitionDelay;
	int						emitterIndex = 0;

	public void LoadEmitters(PolygonStage stage)
	{
		int i = 0;
		//load emitters and emitterManagers, dont forget to handle emitter repeat and delay ! (stored in PolygonStage)
		transitionDelay = stage.transitionDelay;
		foreach (var emitter in stage.emitters)
		{
			emitter.life = 3000 + i++ * 500;
			EmitterManager em = new EmitterManager();
			em.LoadEmitter(emitter);
			emitters.Add(em);
		}
	}

	public bool isFinished()
	{
		return emitterIndex == emitters.Count;
	}

	public void	StageFrame()
	{
		if (emitters.Count == 0 || emitterIndex >= emitters.Count)
			return ;
		if (emitters[emitterIndex].isFinished())
		{
			//TODO: implement transition (delay)
			emitterIndex++;
		}
		else
			emitters[emitterIndex].EmitterFrame();
	}

}
