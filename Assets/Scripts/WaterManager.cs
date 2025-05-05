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

    private int gridWidth;
    private int gridDepth;

    private int prefabSize;

    void Start()
    {
        WorldGenerator worldGeneration = GetComponent<WorldGenerator>();

        gridWidth = worldGeneration._mazeWidth;
        gridDepth = worldGeneration._mazeDepth;

        openingGrid = worldGeneration._openingGrid;
        doorGrid = worldGeneration._doorGrid;

        prefabSize = worldGeneration._prefabSize;

        isWaterGrid = new bool[gridWidth, gridDepth];
        isWaterGrid[0,0] = true;
    }

    private void UpdateWater()
    {
        
    }
}
