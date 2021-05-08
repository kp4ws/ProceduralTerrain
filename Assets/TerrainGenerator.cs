using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
	[SerializeField] LevelChunk chunk;
	[SerializeField] private int mapWidthInChunks;
	[SerializeField] private int mapLengthInChunks;

    void Update()
    {
		for (int z = 0; z < mapLengthInChunks; z++)
		{
			for (int x = 0; x < mapWidthInChunks; x++)
			{

			}
		}
    }
}
