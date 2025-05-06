using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.Collections;

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
    void Start()
    {
        gridWidth = worldGeneration._mazeWidth;
        gridDepth = worldGeneration._mazeDepth;

        openingGrid = worldGeneration._openingGrid;
        doorGrid = worldGeneration._doorGrid;

        prefabSize = worldGeneration._prefabSize;

        WaterGrid = new bool[gridWidth, gridDepth];

        WaterGrid[0, 0] = true;
        SpawnWater(0,0);
        UpdateWater();
    }

    // Funtionen tjekker for hver felt i gridet om der er plads
    private void UpdateWater()
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
                                }
                            }
                        }
                        // tjek om øvre dør er åben (i-1, j)
                        if(i > 0){
                            if(WaterGrid[i - 1, j]){ 
                                int x = (i) * 2;
                                int z = j * 2 + 1;
                                
                                if (openingGrid[x,z] != null){
                                    SpawnWater(i, j);
                                    WaterSpread = true;
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
                                }
                            }
                        }
                            
                    }
                }
            }
        }
        
         
        /* for(int i = 0; i < unCheckedCoords.Count; i++)
        {
            int x = unCheckedCoords[i][0];
            int z = unCheckedCoords[i][1];

            Debug.Log(isWaterGrid[x, z]);
            //Debug.Log(isWaterGrid[-1, -1]);
        } */
    }

    private void SpawnWater(int i, int j){
        WaterGrid[i, j] = true;
        Instantiate(waterPrefab, new Vector3(i * prefabSize, 0, j * prefabSize), quaternion.identity);
    }
}
