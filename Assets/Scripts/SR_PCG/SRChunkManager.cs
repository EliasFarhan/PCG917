using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MazeGeneratorType
{
	DFS,
	DFS_Total,
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
				//_envChunks[i,j].BinarySpacePartitioning();
			}
		}
		MazeGenerator();
	}

	//From 0,0 to sizeX-1, sizeY-1
	void MazeGenerator()
	{
		bool[,] visitedChunk = new bool[_sizeX,_sizeY];
		Vector2Int[,] previousPos = new Vector2Int[_sizeX,_sizeY];
		Vector2Int currentPos = Vector2Int.zero;
		Vector2Int targetPos = new Vector2Int(_sizeX-1,_sizeY-1);
		List<Vector2Int> nextPositions = new List<Vector2Int>();
		List<Vector2Int> mainPathPoints = new List<Vector2Int>();
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
						if (currentPos == Vector2Int.zero)
						{
							break;
						}
						else
						{
							currentPos = previousPos[currentPos.x, currentPos.y];
						}
					}
				}
			}
			break;
			case MazeGeneratorType.DFS_Total:
			{
				visitedChunk[currentPos.x, currentPos.y] = true;
				while (true)
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
					Debug.Log("Current Position: " + currentPos);
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
						if (currentPos == Vector2Int.zero)
						{
							break;
						}
						else
						{
							currentPos = previousPos[currentPos.x, currentPos.y];
						}

					}
				}
			}
				break;
			case MazeGeneratorType.BFS:
				{
					visitedChunk[currentPos.x, currentPos.y] = true;
					while (currentPos != targetPos)
					{
						//Check neighbors
						List<Vector2Int> tmpNeighbors = new List<Vector2Int>();
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

								if (nextPositions.Contains(neighborPos))
								{
									continue;
								}

								tmpNeighbors.Add(neighborPos);

							}
						}
						//Random neighbor choose heuristic
						while (tmpNeighbors.Count > 0)
						{
							Vector2Int nextPos = tmpNeighbors.Random();
							nextPositions.Add(nextPos);
							tmpNeighbors.Remove(nextPos);
						}
						Debug.Log("Current Position: " + currentPos);
						if (nextPositions.Count != 0)
						{
							var newCurrentPos = nextPositions.First();
							previousPos[newCurrentPos.x, newCurrentPos.y] = currentPos;
							currentPos = newCurrentPos;
							visitedChunk[currentPos.x, currentPos.y] = true;
							nextPositions.Remove(currentPos);
						}
						else
						{
							break;
						}
					}
				}
				break;
		}

		currentPos = targetPos;
		mainPathPoints.Add(targetPos);
		while (currentPos != Vector2Int.zero)
		{
			var parentPos = previousPos[currentPos.x, currentPos.y];
			mainPathPoints.Add(parentPos);
			currentPos = parentPos;
			Debug.Log("PATH Current Position: " + currentPos);
		}
		mainPathPoints.Reverse();
		Debug.Log("PATH: ");
		for (int i = 1; i < mainPathPoints.Count; i++)
		{
			CreateDoor(mainPathPoints[i - 1], mainPathPoints[i]);
			
		}

		for (int x = 0; x < _sizeX; x++)
		{
			for (int y = 0; y < _sizeY; y++)
			{
				Vector2Int chunkPos = new Vector2Int(x,y);
				if (mainPathPoints.Contains(chunkPos))
				{
					continue;
				}
				else
				{
					CreateDoor(chunkPos, previousPos[x,y]);
				}
			}
		}
	}

	void CreateDoor(Vector2Int chunkPos1, Vector2Int chunkPos2)
	{
		Vector2Int deltaPos = chunkPos2 - chunkPos1;

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
		else
		{
			return;
		}

		_envChunks[chunkPos1.x, chunkPos1.y].AddDoor(door1);
		_envChunks[chunkPos2.x, chunkPos2.y].AddDoor(door2);
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}
