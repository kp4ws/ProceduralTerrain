using UnityEngine;

public class Noise : MonoBehaviour
{
	private static int offsetValue = 100000;

	public static float[,] GenerateNoiseMap(int mapWidth, int mapDepth, Vector3 mapOffset, string seed, float scale, int octaves, float lacunarity, float persistence)
	{
		float[,] noiseMap = new float[mapWidth, mapDepth];

		System.Random randomSeed = new System.Random(seed.GetHashCode());
		Vector2[] seedOffsets = new Vector2[octaves];
		
		for (int i = 0; i < octaves; i++)
		{
			seedOffsets[i] = new Vector2(randomSeed.Next(-offsetValue, offsetValue),randomSeed.Next(-offsetValue, offsetValue));
		}


		float maxHeight = float.MinValue;
		float minHeight = float.MaxValue;

		for (int z = 0; z < mapDepth; z++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				float frequency = 1;
				float amplitude = 1;
				float noiseSample = 0;

				float xCoord = (x + mapOffset.x) / mapWidth * scale;
				float zCoord = (z + mapOffset.z)/ mapDepth * scale;

				for (int i = 0; i < octaves; i++)
				{
					noiseSample += amplitude * Mathf.PerlinNoise(xCoord * frequency + seedOffsets[i].x, zCoord * frequency + seedOffsets[i].y);
					frequency *= lacunarity;
					amplitude *= persistence;
				}
				

				if(noiseSample > maxHeight)
				{
					maxHeight = noiseSample;
				}
				else if(noiseSample < minHeight)
				{
					minHeight = noiseSample;
				}

				noiseMap[x, z] = noiseSample;
			}
		}

		//Normalize noise in range 0 to 1
		for (int z = 0; z < mapDepth; z++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				noiseMap[x, z] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[x, z]);
			}
		}

		return noiseMap;
	}
}
