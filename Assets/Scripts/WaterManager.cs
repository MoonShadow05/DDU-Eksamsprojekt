using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

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

    //[SerializeField] private float preferredHeight;
    //[SerializeField] private float prefferedHeightIncrease;
    [SerializeField] private float waterStartHeight;
    [SerializeField] private float startWaterAmount;
    [SerializeField] private float waterFlowRate;
    private float totalWaterAmount;
    private float[,] Height;

    private GameObject[,] WaterPlaced;
    
    List<List<int>> openRoomCoords = new List<List<int>>();

    List<GameObject> movingDoors = new List<GameObject>();
    //List<GameObject> movedDoors = new List<GameObject>();
    private int movedDoors = 0;

    public float doorSpeed;

    public float doorTargetHeight;


    private void Awake()
    {
        if (questions == null)
        {
            questions = FindFirstObjectByType<QuestionPopupTrigger>();
        }
    }

    void Start(){
        totalWaterAmount = startWaterAmount;
        waterFlowRate = waterFlowRate / 100;
        doorSpeed = doorSpeed / 100;
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
        List<int> roomCoords = new List<int> { i, j };
        openRoomCoords.Add(roomCoords);
    }

    private void UpdateWater()
    {
        // Totatle vandmængde stiger med dette
        int roomAmount = openRoomCoords.Count;
        totalWaterAmount += waterFlowRate * Time.deltaTime * roomAmount;

        for (int roomNumb = 0; roomNumb < roomAmount; roomNumb++){
            // Koordinater til rum
            int i = openRoomCoords[roomNumb][0];
            int j = openRoomCoords[roomNumb][1];

            Height[i, j] = waterStartHeight + totalWaterAmount / roomAmount;

            // Placer højere op
            Vector3 NewPos = WaterPlaced[i, j].transform.position;
            NewPos.y = Height[i, j];
            WaterPlaced[i, j].transform.position = NewPos;
        }


        /*
        for (int roomNumb = 0; roomNumb < openRoomCoords.Count; roomNumb++)
        {
            int i = openRoomCoords[roomNumb][0];
            int j = openRoomCoords[roomNumb][1];

            preferredHeight += (prefferedHeightIncrease / 100 * Time.deltaTime);
            float currentHeight = Height[i, j];

            if (preferredHeight > currentHeight)
            {
                Height[i, j] = currentHeight + (waterFlowRate * Time.deltaTime);
                if (Height[i, j] > preferredHeight)
                {
                    Height[i, j] = preferredHeight;
                }
            } else if (preferredHeight < currentHeight) {
                Height[i, j] = currentHeight - (waterFlowRate * Time.deltaTime);
                if (Height[i, j] < preferredHeight)
                {
                    Height[i, j] = preferredHeight;
                }
            }

            Vector3 NewPos = WaterPlaced[i, j].transform.position;
            NewPos.y = Height[i, j];
            WaterPlaced[i, j].transform.position = NewPos;
        }*/

    }

}
