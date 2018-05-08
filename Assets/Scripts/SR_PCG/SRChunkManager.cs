using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeGeneratorType
{
	DFS,
	BFS,
	Kruskal
}

public class SRChunkManager : MonoBehaviour
{
	[SerializeField]
	private SRChunk _chunkPrefab;
	private SRChunk[,] _envChunks;
	[SerializeField]
	private int _sizeX = 3;
	[SerializeField]
	private int _sizeY = 3;
	[SerializeField]
	private MazeGeneratorType _mazeGeneratorType = MazeGeneratorType.DFS;
	// Use this for initialization
	void Start()

	{
		_envChunks = new SRChunk[_sizeX,_sizeY];
		for (int i = 0; i < _sizeX; i++)
		{
			for (int j = 0; j < _sizeY; j++)
			{
				_envChunks[i,j] = Instantiate(_chunkPrefab);
				_envChunks[i,j].transform.position = SRChunk.TileSize * new Vector3(SRChunk.SizeX*i, SRChunk.SizeY*j,0.0f);
				_envChunks[i,j].transform.parent = transform;
				_envChunks[i,j].ChunkManager = this;
				_envChunks[i,j].Pos = new Vector2Int(i,j);
				_envChunks[i,j].BinarySpacePartitioning();
			}
		}
	}

	//From 0,0 to sizeX-1, sizeY-1
	void MazeGenerator()
	{
		bool[,] visitedChunk = new bool[_sizeX,_sizeY];
		Vector2Int[,] previousPos = new Vector2Int[_sizeX,_sizeY];
		Vector2Int currentPos = Vector2Int.zero;
		Vector2Int targetPos = new Vector2Int(_sizeX-1,_sizeY-1);
		List<Vector2Int> nextPositions = new List<Vector2Int>();
		List<Vector2Int> path = new List<Vector2Int>();
		Debug.Log("Current Position: " + currentPos);

		switch (_mazeGeneratorType)
		{
			case MazeGeneratorType.DFS:
			{
				visitedChunk[currentPos.x, currentPos.y] = true;
				while (currentPos != targetPos)
				{
					//Check neighbors
					for (int dx = -1; dx <= 1; dx++)
					{
						for (int dy = -1; dy <= 1; dy++)
						{
							if (dx == dy || dx == -dy)
							{
								continue;
							}
							Vector2Int neighborPos = currentPos + new Vector2Int(dx, dy);
							if (neighborPos.x < 0 || 
							    neighborPos.y < 0 || 
							    neighborPos.x == _sizeX || 
							    neighborPos.y == _sizeY)
							{
								continue;
							}

							if (visitedChunk[neighborPos.x, neighborPos.y])
							{
								continue;
							}

							nextPositions.Add(neighborPos);
							
						}
					}
					Debug.Log("Current Position: "+currentPos);
					if (nextPositions.Count != 0)
					{
						//Random heuristic with neighbors of current pos
						var newCurrentPos = nextPositions.Random();
						previousPos[newCurrentPos.x, newCurrentPos.y] = currentPos;
						currentPos = newCurrentPos;
						visitedChunk[currentPos.x, currentPos.y] = true;
						nextPositions.Clear();
					}
					else
					{
						currentPos = previousPos[currentPos.x, currentPos.y];
					}
				}
			}
			break;
		}

		currentPos = targetPos;
		path.Add(targetPos);
		while (currentPos != Vector2Int.zero)
		{
			var parentPos = previousPos[currentPos.x, currentPos.y];
			path.Add(parentPos);
			currentPos = parentPos;
			Debug.Log("PATH Current Position: " + currentPos);
		}
		path.Reverse();
		Debug.Log("PATH: ");
		for (int i = 1; i < path.Count; i++)
		{
			Vector2Int deltaPos = path[i] - path[i - 1];

			SRDoor door1 = ScriptableObject.CreateInstance<SRDoor>();
			SRDoor door2 = ScriptableObject.CreateInstance<SRDoor>();

			if (deltaPos == Vector2Int.down)
			{
				door1.Pos.x = Random.Range(2, SRChunk.SizeX - 2);
				door1.Pos.y = 0;
				door2.Pos.x = door1.Pos.x;
				door2.Pos.y = SRChunk.SizeY - 1;
				door1.Vertical = false;
				door2.Vertical = false;
			}
			else if (deltaPos == Vector2Int.up)
			{
				door2.Pos.x = Random.Range(2, SRChunk.SizeX - 2);
				door2.Pos.y = 0;
				door1.Pos.x = door2.Pos.x;
				door1.Pos.y = SRChunk.SizeY - 1;

				door1.Vertical = false;

				door2.Vertical = false;
			}
			else if (deltaPos == Vector2Int.right)
			{
				door2.Pos.x = 0;
				door2.Pos.y = Random.Range(2, SRChunk.SizeY - 2);
				door1.Pos.x = SRChunk.SizeX - 1;
				door1.Pos.y = door2.Pos.y;

				door1.Vertical = true;

				door2.Vertical = true;
			}
			else if (deltaPos == Vector2Int.left)
			{
				door1.Pos.x = 0;
				door1.Pos.y = Random.Range(2, SRChunk.SizeY - 2);
				door2.Pos.x = SRChunk.SizeX - 1;
				door2.Pos.y = door1.Pos.y;

				door1.Vertical = true;

				door2.Vertical = true;
			}

			_envChunks[path[i-1].x, path[i-1].y].AddDoor(door1);
			_envChunks[path[i].x, path[i].y].AddDoor(door2);

			Debug.Log("FROM: "+path[i-1]+" TO: "+path[i]);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (Time.frameCount == 100)
		{
			Debug.Log("PROUT");
			Debug.Break();
			MazeGenerator();
		}
	}
}
