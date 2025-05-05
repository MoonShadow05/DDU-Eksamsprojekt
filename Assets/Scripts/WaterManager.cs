using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class WaterManager : MonoBehaviour
{
    [SerializeField]
    private GameObject waterPrefab;

    private bool[,] isWaterGrid;
    private GameObject[,] openingGrid;
    private GameObject[,] doorGrid;

    private List<List<int>> unCheckedCoords = new List<List<int>>();

    private int gridWidth;
    private int gridDepth;

    private int prefabSize;

    void Start()
    {
        WorldGenerator worldGeneration = GetComponent<WorldGenerator>();

        //gridWidth = worldGeneration._mazeWidth;
        //gridDepth = worldGeneration._mazeDepth;

        //openingGrid = worldGeneration._openingGrid;
        //doorGrid = worldGeneration._doorGrid;

        //prefabSize = worldGeneration._prefabSize;

        //isWaterGrid = new bool[gridWidth, gridDepth];
        isWaterGrid = new bool[10, 10];

        List<int> coords = new List<int>{ 0, 0 };
        unCheckedCoords.Add(coords);
        isWaterGrid[coords[0], coords[1]] = true;

        UpdateWater();
    }

    private void UpdateWater()
    {
        for(int i = 0; i < unCheckedCoords.Count; i++)
        {
            int x = unCheckedCoords[i][0];
            int z = unCheckedCoords[i][1];

            Debug.Log(isWaterGrid[x, z]);
            //Debug.Log(isWaterGrid[-1, -1]);
        }
    }
}
