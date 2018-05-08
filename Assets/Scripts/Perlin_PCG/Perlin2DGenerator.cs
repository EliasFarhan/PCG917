using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Perlin2DGenerator : MonoBehaviour
{
	const int maxValue = 1000;
	[SerializeField]
	Tile topTile;
	[SerializeField]
	Tile baseTile;

	Tilemap tilemap;
	int[] heightArrays = new int[maxValue];
	float yValueLayer1 = 0.0f;
	float yValueLayer2 = 0.0f;
	Camera mainCamera;
	
	// Use this for initialization
	void Start ()
	{
		yValueLayer1 = Random.value;

		yValueLayer2 = Random.value;
		mainCamera = Camera.main;
		tilemap = GetComponent<Tilemap>();
		for (int i = 0; i < maxValue; i++)
		{
			float xCoord = (float)i / maxValue;
			float sampleLayer1 = Mathf.PerlinNoise(xCoord*100, yValueLayer1);
			float sampleLayer2 = Mathf.PerlinNoise(xCoord*1000, yValueLayer2);

			heightArrays[i] = (int)System.Math.Round(sampleLayer1 * 4 + sampleLayer2 * 2);
			for (int j = 0; j < heightArrays[i]; j++)
			{
				if (j == heightArrays[i] - 1)
				{
					tilemap.SetTile(new Vector3Int(i - maxValue / 2, -6 + j, 0), topTile);
				}
				else
				{
					tilemap.SetTile(new Vector3Int(i - maxValue / 2, -6 + j, 0), baseTile);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
