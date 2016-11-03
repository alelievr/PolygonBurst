using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Pattern {

	public int					spawnObjectPatternCount = 1;
	public SPAWN_PATTERN		spawnPattern = SPAWN_PATTERN.IN_CIRCLE;
	public SPAWN_DISPOSITION	spawnDisposition = SPAWN_DISPOSITION.EQUAL;
	public float				spawnPatternSize = 1;

	public List< Vector3 >		customPoints = new List< Vector3 >();

	public IEnumerable< SpawnInfos > GetNextSpawnInfo()
	{
		SpawnInfos	sp = new SpawnInfos();
		const float  PI = 3.1415926535897932384626433832795f;
		const float Phi = 1.6180339887498948482045868343656f; // The Golden Ratio
		const float  dA = Phi / PI;

		switch (spawnPattern)
		{
			case SPAWN_PATTERN.IN_CIRCLE:
				if (spawnDisposition == SPAWN_DISPOSITION.EQUAL)
				{
					float Angle = dA;
					float size = Mathf.Sqrt(spawnObjectPatternCount) * dA;
					for(int p = spawnObjectPatternCount + 1; --p != 0; Angle += (Phi - 1) * 2 * PI) {
						float r = ((Mathf.Sqrt(p) * 1 * dA) / size) * spawnPatternSize; //radius
						sp.position.x = r * Mathf.Cos(Angle);
						sp.position.y = r * Mathf.Sin(Angle);
						sp.position.z = 0;
						sp.direction = sp.position.normalized;
						yield return sp;
					}
				}
				if (spawnDisposition == SPAWN_DISPOSITION.RANDOM)
				{
					for (int i = 0; i < spawnObjectPatternCount; i++)
					{
						sp.position = Random.insideUnitCircle * spawnPatternSize;
						sp.direction = sp.position.normalized;
						yield return sp;
					}
				}
				break ;
			case SPAWN_PATTERN.ON_CIRCLE:
				if (spawnDisposition == SPAWN_DISPOSITION.EQUAL)
				{
					float q = (PI * 2) / spawnObjectPatternCount;
					for (int i = 0; i < spawnObjectPatternCount; i++)
					{
						sp.position.x = Mathf.Cos(q * i);
						sp.position.y = Mathf.Sin(q * i);
						sp.position.z = 0;
						sp.position *= spawnPatternSize;
						sp.direction = sp.position.normalized;
						yield return sp;
					}
				}
				if (spawnDisposition == SPAWN_DISPOSITION.RANDOM)
				{
					for (int i = 0; i < spawnObjectPatternCount; i++)
					{
						sp.position = Random.insideUnitCircle.normalized;
						sp.direction = sp.position;
						yield return sp;
					}
				}
				break ;
			case SPAWN_PATTERN.POINTS:
				for (int i = 0; i < spawnObjectPatternCount; i++)
					for (int j = 0; j < customPoints.Count; j++)
					{
						sp.position = customPoints[j];
						sp.direction = sp.position.normalized;
						yield return sp;
					}
				break ;
		}
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
}

public enum SPAWN_DISPOSITION
{
	EQUAL,
	RANDOM,
}