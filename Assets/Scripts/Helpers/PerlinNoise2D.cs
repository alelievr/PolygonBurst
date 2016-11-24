using UnityEngine;
using System.Collections;

public class PerlinNoise2D : MonoBehaviour
{
    static bool         init = false;
    static int          initedOcatves;
    static Vector2[]    octaveOffsets;

    static void Init(int octaves)
    {
  		System.Random prng = new System.Random (Random.Range(-100000, 100000));
        octaveOffsets = new Vector2[octaves];
		for (int i = 0; i < octaves; i++) {
			float offsetX = prng.Next (-100000, 100000) + 230;
			float offsetY = prng.Next (-100000, 100000) + 964;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);
		}
        init = true;
        initedOcatves = octaves;
    }

    static public float GenerateNoise(float x,
        float y,
        float scale = 1,
        int octaves = 2,
        float frequency = 2,
        float lacunarity = 1,
        float amplitude = 1,
        float persistance = 1,
        int seed = -1)
    {
        float noiseHeight = 0;
        if (!init || initedOcatves != octaves)
            Init(octaves);

        for (int i = 0; i < octaves; i++) {
            float sampleX = (x) / scale * frequency + octaveOffsets[i].x;
            float sampleY = (y) / scale * frequency + octaveOffsets[i].y;

            float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
            noiseHeight += perlinValue * amplitude;

            amplitude *= persistance;
            frequency *= lacunarity;
        }

        return noiseHeight;
    }
}
