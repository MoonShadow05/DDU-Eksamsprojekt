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

    //private int _wallAmount;

    List<List<int>> walls = new List<List<int>>();

    void Start()
    {
        //_wallAmount = ((_mazeDepth - 1) * _mazeWidth) + ((_mazeWidth - 1) * _mazeDepth);

        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x,z] = Instantiate(_mazeCellPrefab, new Vector3(x * _prefabSize, 0, z * _prefabSize), Quaternion.identity);

                if (z != 0)
                {
                    walls.Add(new List<int> { x * 2 + 1, z * 2 - 2 }); // Back Wall
                }

                if (x != 0)
                {
                    walls.Add(new List<int> { x * 2 - 2, z * 2 + 1 }); // Left Wall
                }
            }
        }

        GenerateMaze(null, _mazeGrid[0, 0]);

        List<int> list = new List<int> { 1 , 0 };
        //walls.RemoveAll(sublist => sublist.SequenceEqual(list));


        for (int i  = 0; i < walls.Count; i++)
        {
            Debug.Log("Coords: " + walls[i][0] + " ; " + walls[i][1]);
        }
        //int _wallDeleteAmount = (int) (_wallAmount * _extraWallBreakPercentage / 100);
        //RemoveExtraCellWalls(_wallDeleteAmount);
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
                //_wallAmount--;
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
            var wallCoords = new List<int> { x * 2 - 2, z * 2 + 1 };
            //walls.Remove(wallCoords);

            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        // Goes Left
        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            int x = (int)previousCell.transform.position.x / _prefabSize;
            int z = (int)previousCell.transform.position.z / _prefabSize;
            var wallCoords = new List<int> { x * 2 - 2, z * 2 + 1 };
            walls.Remove(wallCoords);

            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        // Goes Up
        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            int x = (int)currentCell.transform.position.x / _prefabSize;
            int z = (int)currentCell.transform.position.z / _prefabSize;
            walls.Remove(new List<int> { 2 + 1, z * 2 - 2 });

            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        // Goes Down
        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            int x = (int)previousCell.transform.position.x / _prefabSize;
            int z = (int)previousCell.transform.position.z / _prefabSize;
            walls.Remove(new List<int> { 2 + 1, z * 2 - 2 });

            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    
    /*private void RemoveExtraCellWalls(int _wallDeleteAmount)
    {
        while (_wallDeleteAmount != 0)
        {
            int x = Random.Range(0, _mazeWidth);
            int z = Random.Range(0, _mazeDepth);

            MazeCell cell = _mazeGrid[x, z];

            var potentialCells = GetWallToBreak(cell, x, z);

            if (!potentialCells.Any())
            {
                continue;
            }

            MazeCell specificNeighboorCell = potentialCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();

            ClearWalls(cell, specificNeighboorCell);
            _wallDeleteAmount--;
            _wallAmount--;
            Debug.Log(_wallAmount);
        }
    }

    private IEnumerable<MazeCell> GetWallToBreak(MazeCell cell, int x, int z)
    {
        if (cell.LeftWallStatus() == true && x != 0)
        {
            yield return _mazeGrid[x - 1, z];
        }

        if (cell.RightWallStatus() == true && x != _mazeWidth - 1)
        {
            yield return _mazeGrid[x + 1, z];
        }

        if (cell.FrontWallStatus() == true && z != _mazeDepth - 1)
        {
            yield return _mazeGrid[x, z + 1];
        }

        if (cell.BackWallStatus() == true && z != 0)
        {
            yield return _mazeGrid[x, z - 1];
        }
    }*/
}
