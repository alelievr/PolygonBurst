﻿using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PolygonStage")]
public class PolygonStage : ScriptableObject {

	public new string					name;
	public List< PolygonEmitter >		emitters = new List< PolygonEmitter >();
	//TOOD: maybe StageTransitionHere? -> renamed in Transition
	public List< float >				transitionDelay = new List< float >();

}