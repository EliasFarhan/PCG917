using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SRChunk : MonoBehaviour
{
	private SRChunkManager _chunkManager;
	public const float TileSize = 0.5f;
	public const int SizeX = 20;
	public const int SizeY = 18;
	private Tilemap _chunkTilemap;
	[SerializeField]
	private Tile _wallTile;
	List<SRDoor> _doors = new List<SRDoor>();
	public Vector2Int Pos { get; set; }

	public SRChunkManager ChunkManager
	{
		get
		{
			return _chunkManager;
		}

		set
		{
			_chunkManager = value;
		}
	}

	void Awake()
	{
		_chunkTilemap = GetComponent<Tilemap>();
	}

	//Doors are 3 tiles long
	public void AddDoor(SRDoor newDoor)
	{
		if (newDoor.Vertical)
		{
			for (int j = -1; j <= 1; j++)
			{
				_chunkTilemap.SetTile(
					new Vector3Int(newDoor.Pos.x - SizeX / 2, newDoor.Pos.y+j - SizeY / 2, 0), 
					null);
			}
		}
		else
		{
			for (int i = -1; i <= 1; i++)
			{
				_chunkTilemap.SetTile(
					new Vector3Int(newDoor.Pos.x + i - SizeX / 2, newDoor.Pos.y - SizeY / 2, 0), 
					null);
			}
		}
		_doors.Add(newDoor);
	}

	public void BinarySpacePartitioning()
	{
		/*int newLine = Random.Range(3, 14);
		int newColumn = Random.Range(3, 16);

		for (int i = 0; i < SizeX; i++)
		{
			for (int j = 0; j < SizeY; j++)
			{
				if (i == newColumn || j == newLine)
				{
					_chunkTilemap.SetTile(new Vector3Int(i - SizeX / 2, j - SizeY / 2, 0), _wallTile);
				}
			}
		}*/
	}
}
