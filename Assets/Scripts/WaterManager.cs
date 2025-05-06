using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.Collections;
using System;

public class WaterManager : MonoBehaviour
{
    [SerializeField] private GameObject waterPrefab;

    private bool[,] WaterGrid;
    private GameObject[,] openingGrid;
    private GameObject[,] doorGrid;

    private int gridWidth;
    private int gridDepth;

    private int prefabSize;
    
    [SerializeField] private WorldGenerator worldGeneration;

    [SerializeField] private float waterFlowRate;
    private float[,] Height;

    private GameObject[,] WaterPlaced;
    private float OpenRoomCount = 0;
    
    List<List<int>> openRoomCoords = new List<List<int>>();

    void Start()
    {
        gridWidth = worldGeneration._mazeWidth;
        gridDepth = worldGeneration._mazeDepth;

        openingGrid = worldGeneration._openingGrid;
        doorGrid = worldGeneration._doorGrid;

        prefabSize = worldGeneration._prefabSize;

        WaterGrid = new bool[gridWidth, gridDepth];
        WaterPlaced = new GameObject[gridWidth, gridDepth];
        Height = new float[gridWidth,gridDepth];

        WaterGrid[0, 0] = true;
        Height[0,0] = -8.5f;
        SpawnWater(0,0);
        WaterSpread();
    }
    void Update(){
        for(int i = 0; i < gridWidth; i++){
            for(int j = 0 ; j < gridDepth ; j++){
                if(WaterGrid[i, j] && WaterPlaced[i,j]){
                    Height[i,j] += waterFlowRate/100*Time.deltaTime;
                    Vector3 NewPos = WaterPlaced[i, j].transform.position;
                    NewPos.y = Height[i,j];
                    WaterPlaced[i, j].transform.position = NewPos;
                }
            }
        }
    }

    // Funtionen tjekker for hver felt i gridet om der er plads
    private void WaterSpread()
    {
        bool WaterSpread = true;

        while(WaterSpread){
            
            WaterSpread = false;

            for(int i = 0; i < gridWidth; i++){
                for(int j = 0 ; j < gridDepth ; j++){
                    if(!WaterGrid[i, j]){
                        // tjek om venstre dør er åben (i, j-1)                        
                        if(j > 0){
                            if(WaterGrid[i, j - 1]){
                                int x = i * 2 + 1;
                                int z = j * 2;
                                
                                if (openingGrid[x,z] != null){
                                    SpawnWater(i, j);
                                    WaterSpread = true;
                                    continue;
                                }
                            }
                        }
                        // tjek om højre dør er åben (i, j+1)
                        if(j < gridDepth - 1){
                            if(WaterGrid[i, j + 1]){
                                int x = i * 2 + 1;
                                int z = (j + 1) * 2;
                                
                                if (openingGrid[x,z] != null){
                                    SpawnWater(i, j);
                                    WaterSpread = true;
                                    continue;
                                }
                            }
                        }
                        // tjek om øvre dør er åben (i-1, j)
                        if(i > 0){
                            if(WaterGrid[i - 1, j]){ 
                                int x = i * 2;
                                int z = j * 2 + 1;
                                
                                if (openingGrid[x,z] != null){
                                    SpawnWater(i, j);
                                    WaterSpread = true;
                                    continue;
                                }
                            }
                        }
                        // tjek om nedre dør er åben (i+1, j)
                        if(i < gridWidth - 1){
                            if(WaterGrid[i + 1, j]){
                                int x = (i + 1) * 2;
                                int z = j * 2 + 1;
                                if (openingGrid[x,z] != null){
                                    SpawnWater(i, j);
                                    WaterSpread = true;
                                    continue;
                                }
                            }
                        }    
                    }
                }
            }
        }
    }

    private void SpawnWater(int i, int j){
        WaterGrid[i, j] = true;
        Vector3 waterPosition = new Vector3(i*prefabSize, Height[i,j], j*prefabSize);
        GameObject waterSegment = Instantiate(waterPrefab, waterPosition, quaternion.identity);
        WaterPlaced[i, j] = waterSegment;
        Height[i,j] = -8.5f;
        List<int> coords = [i,j];
        OpenRoomCount +=1;
    }

    private void UpdateWater()
    {
        int i = 
        if (Heigt)
    }

}
