using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.Collections;
using System;
using UnityEngine.Rendering;
using UnityEngine.ProBuilder.Shapes;
using System.IO.Compression;
using UnityEngine.UIElements;

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
    [SerializeField] private QuestionPopupTrigger questions;

    [SerializeField] private float waterStartHeight;
    [SerializeField] private float waterIncreaseRate;
    private float totalWaterAmount = 0;
    private float[,] Height;

    private int waterUpdateAmount = 0;

    private GameObject[,] WaterPlaced;
    
    List<List<int>> openRoomCoords = new List<List<int>>();

    List<GameObject> movingDoors = new List<GameObject>();

    int oldRoomAmount;

    private int movedDoors = 0;

    public float doorSpeed;

    public float doorTargetHeight;




    void Start(){
        GameObject sm = GameObject.FindGameObjectWithTag("SoundManager");
        if (sm != null) {
            sm.GetComponent<SoundManager>().PlaySound(SoundManager.SoundEffects.StartSound);
        }
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
        oldRoomAmount = openRoomCoords.Count;
    }
    
    void Update(){
        foreach (GameObject Door in movingDoors) {
            Door.transform.position += new Vector3(0,doorSpeed*Time.deltaTime,0);
            int i = (int) Door.transform.position.x * 2 / prefabSize + 1;
            int j = (int) Door.transform.position.z * 2 / prefabSize + 1;
            openingGrid[i,j] = new GameObject();
            WaterSpread();
            if (Door.transform.position.y >= doorTargetHeight) {
                Door.transform.position = new Vector3(Door.transform.position.x,doorTargetHeight,Door.transform.position.z);
                movedDoors++;
            }
        }

        for (int i = 0; i < movedDoors; i++) {
            movingDoors.RemoveAt(0);
            movedDoors--;
        }


        UpdateWater();
    }

    public void OpenDoor(bool DoorShouldOpen, GameObject Door){
        if (DoorShouldOpen == true && Door != null) {
            movingDoors.Add(Door);
            
            GameObject sm = GameObject.FindGameObjectWithTag("SoundManager");
            if (sm != null)
            {
                sm.GetComponent<SoundManager>().PlaySound(SoundManager.SoundEffects.DoorSound);
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
        Height[i, j] = waterStartHeight;
        Vector3 waterPosition = new Vector3(i*prefabSize, Height[i,j], j*prefabSize);
        GameObject waterSegment = Instantiate(waterPrefab, waterPosition, quaternion.identity);
        WaterPlaced[i, j] = waterSegment;
        List<int> roomCoords = new List<int> { i, j };
        openRoomCoords.Add(roomCoords);
    }

    private void UpdateWater()
    {
        // Totatle vandmængde stiger med dette
        int roomAmount = openRoomCoords.Count;
        float waterIncrease = Mathf.Pow(waterIncreaseRate,waterUpdateAmount) * Time.deltaTime;
        //Mathf.Pow(baseNumber, exponent)
        totalWaterAmount += waterIncrease; // Den totale vandmængde stigning

        for (int roomNumb = 0; roomNumb < roomAmount; roomNumb++){ // Vandhøjden bliver opdateret for alle rum med vand
            // Koordinater til rum
            int i = openRoomCoords[roomNumb][0];
            int j = openRoomCoords[roomNumb][1];

            // Nuværende højde, og højden den vil opnå
            float currentRoomWaterHeight = Height[i, j];
            float waterHeightTarget = totalWaterAmount / roomAmount + waterStartHeight;

            if (currentRoomWaterHeight < waterHeightTarget) // Øger vandet, hvis det ligger under 
            {
                float waterRiseSpeed = doorSpeed * Time.deltaTime;
                Height[i, j] = currentRoomWaterHeight + waterRiseSpeed;

                if (Height[i, j] > waterHeightTarget)
                {
                    Height[i, j] = waterHeightTarget;
                }
            } else if (currentRoomWaterHeight > waterHeightTarget) // Falder hvis vandet er højere end det skal
            {
                if (oldRoomAmount < roomAmount)
                {
                    int deltaRoomAmount = roomAmount - oldRoomAmount;
                    float waterFallSpeeed = ((doorSpeed * deltaRoomAmount) / oldRoomAmount) * Time.deltaTime;
                    Height[i, j] = currentRoomWaterHeight - waterFallSpeeed ;

                    if (Height[i, j] < waterHeightTarget)
                    {
                        Height[i, j] = waterHeightTarget;
                        oldRoomAmount = roomAmount;
                    }
                } else // Hvis vandet er af en anden grund for højt, vil det falde alligevel.
                {
                    float waterFallSpeeed = doorSpeed * Time.deltaTime;
                    if (Height[i, j] < waterHeightTarget)
                    {
                        Height[i, j] = waterHeightTarget;
                        oldRoomAmount = roomAmount;
                    }
                }
            }

            // Placer højere op
            Vector3 NewPos = WaterPlaced[i, j].transform.position;
            NewPos.y = Height[i, j];
            WaterPlaced[i, j].transform.position = NewPos;
        }

        waterUpdateAmount++;

    }

}