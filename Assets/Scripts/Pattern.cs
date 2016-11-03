using UnityEngine;
using System.Collections.Generic;

public class Pattern {

	public int				nPos;
	public SPAWN_PATTERN	spawnPattern;

	public Pattern(int n, SPAWN_PATTERN pattern)
	{
		nPos = n;
		spawnPattern = pattern;
	}

	public IEnumerable< SpawnInfos > GetNextSpawnInfo()
	{
		SpawnInfos	sp = new SpawnInfos();

		switch (spawnPattern)
		{
			
		}

		yield return sp;
	}

	public struct SpawnInfos
	{
		public Vector3 position;
		public Vector3 direction;
	}

}

public enum SPAWN_PATTERN
{
	ON_CIRCLE,
	IN_CIRCLE,
	LINE,
	POINTS,		//draw with handles
	CURVE,
}