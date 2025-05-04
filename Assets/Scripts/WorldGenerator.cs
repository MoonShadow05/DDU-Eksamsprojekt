using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;
    
    [SerializeField]
    private float _extraWallBreakPercentage;

    /*[SerializeField]
    private float _doorAmountPercentage;*/

    private MazeCell[,] _mazeGrid;

    private int _prefabSize = 10;

    List<List<int>> walls = new List<List<int>>();
    List<List<int>> openings = new List<List<int>>();

    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x,z] = Instantiate(_mazeCellPrefab, new Vector3(x * _prefabSize, 0, z * _prefabSize), Quaternion.identity);

                if (z != 0)
                {
                    walls.Add(new List<int> { x * 2 + 1, z * 2 }); // Back Wall
                }

                if (x != 0)
                {
                    walls.Add(new List<int> { x * 2 , z * 2 + 1 }); // Left Wall
                }
            }
        }

        GenerateMaze(null, _mazeGrid[0, 0]);

        RemoveExtraCellWalls();

        Debug.Log("Walls: " + walls.Count + "    Openings: " + openings.Count);
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x / _prefabSize;
        int z = (int)currentCell.transform.position.z / _prefabSize;

        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];

            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x , z + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        // Goes Right
        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            int x = (int)currentCell.transform.position.x / _prefabSize;
            int z = (int)currentCell.transform.position.z / _prefabSize;

            List<int> wallCoords = new List<int> { x * 2 , z * 2 + 1 };
            walls.RemoveAll(sublist => sublist.SequenceEqual(wallCoords));
            openings.Add(wallCoords);

            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        // Goes Left
        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            int x = (int)previousCell.transform.position.x / _prefabSize;
            int z = (int)previousCell.transform.position.z / _prefabSize;

            List<int> wallCoords = new List<int> { x * 2 , z * 2 + 1 };
            walls.RemoveAll(sublist => sublist.SequenceEqual(wallCoords));
            openings.Add(wallCoords);

            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        // Goes Up
        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            int x = (int)currentCell.transform.position.x / _prefabSize;
            int z = (int)currentCell.transform.position.z / _prefabSize;

            List<int> wallCoords = new List<int> { x * 2 + 1, z * 2 };
            walls.RemoveAll(sublist => sublist.SequenceEqual(wallCoords));
            openings.Add(wallCoords);

            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        // Goes Down
        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            int x = (int)previousCell.transform.position.x / _prefabSize;
            int z = (int)previousCell.transform.position.z / _prefabSize;

            List<int> wallCoords = new List<int> { x * 2 + 1, z * 2 };
            walls.RemoveAll(sublist => sublist.SequenceEqual(wallCoords));
            openings.Add(wallCoords);

            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    
    private void RemoveExtraCellWalls()
    {
        int wallBreakAmount = (int) (walls.Count * _extraWallBreakPercentage / 100);

        for (int i = 0; i < wallBreakAmount; i++)
        {
            int randomIndex = Random.Range(0, walls.Count);

            int wallX = walls[randomIndex][0];
            int wallZ = walls[randomIndex][1];

            openings.Add(walls[randomIndex]);
            walls.RemoveAt(randomIndex);
            if (wallX % 2 == 1)
            {
                int cellX = (wallX - 1) / 2;
                int cellZ = wallZ / 2;

                MazeCell topCell = _mazeGrid[cellX, cellZ];
                MazeCell lowerCell = _mazeGrid[cellX, cellZ - 1];

                topCell.ClearBackWall();
                lowerCell.ClearFrontWall();
            } else
            {
                int cellX = wallX / 2;
                int cellZ = (wallZ - 1) / 2;

                MazeCell rightCell = _mazeGrid[cellX, cellZ];
                MazeCell leftCell = _mazeGrid[cellX - 1, cellZ];

                rightCell.ClearLeftWall();
                leftCell.ClearRightWall();
            }
        }
    }
}
