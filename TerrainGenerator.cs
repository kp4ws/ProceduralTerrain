using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{
	[SerializeField] private bool debugMode;
	[SerializeField] private Material terrainMaterial;
	[SerializeField] private int mapWidth = 150;
	[SerializeField] private int mapDepth = 150;
	[SerializeField] private Vector3 mapOffset;
	[SerializeField] private string seed;
	[SerializeField] private float scale = 2f;
	[SerializeField] private int octaves = 8;
	[SerializeField] private float lacunarity = 2;
	[SerializeField] private float persistence = 0.5f;
	[SerializeField] private float heightMultiplier = 0;
	[SerializeField] AnimationCurve heightCurve;
	[SerializeField] private TerrainType[] terrainTypes;

	private Vector3[] vertices;
	private int[] triangles;
	private Color[] colors;

	private Mesh mesh;
	private MeshRenderer meshRenderer;

	private void Awake()
	{
		mesh = GetComponent<MeshFilter>().mesh;
		meshRenderer = GetComponent<MeshRenderer>();
	}

	private void Start()
	{
		meshRenderer.material = terrainMaterial;
		CreateMesh();
		UpdateMesh();
	}

	private void Update()
	{
		if(debugMode)
		{
			CreateMesh();
			UpdateMesh();
		}
	}

	private void CreateMesh()
	{
		vertices = new Vector3[mapWidth * mapDepth];
		triangles = new int[(mapWidth - 1) * (mapDepth - 1) * 6];
		colors = new Color[vertices.Length];

		float[,] heightMap = Noise.GenerateNoiseMap(mapWidth, mapDepth, mapOffset, seed, scale, octaves, lacunarity, persistence);

		//Assign vertices
		for (int z = 0; z < mapDepth; z++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				float height = heightMap[x, z];
				int vertexIndex = z * mapWidth + x;
				vertices[vertexIndex] = new Vector3(x, heightCurve.Evaluate(height) * heightMultiplier, z);

				TerrainType t = GetTerrain(height);
				colors[vertexIndex] = t.color;
			}
		}


		int triangleIndex = 0;
		for (int z = 0; z < mapDepth - 1; z++)
		{
			for (int x = 0; x < mapWidth - 1; x++)
			{
				int vertexIndex = z * mapWidth + x;

				triangles[triangleIndex + 0] = vertexIndex;
				triangles[triangleIndex + 1] = triangles[triangleIndex + 4] = vertexIndex + mapWidth;
				triangles[triangleIndex + 2] = triangles[triangleIndex + 3] = vertexIndex + 1;
				triangles[triangleIndex + 5] = vertexIndex + mapWidth + 1;

				triangleIndex += 6;
			}
		}
	}

	private void UpdateMesh()
	{
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.colors = colors;
		mesh.RecalculateNormals();
	}

	private TerrainType GetTerrain(float height)
	{
		foreach (TerrainType t in terrainTypes)
		{
			if (height < t.height)
			{
				return t;
			}
		}
		return terrainTypes[terrainTypes.Length - 1];
	}

	[System.Serializable]
	public class TerrainType
	{
		public string name;
		public float height;
		public Color color;
	}
}
