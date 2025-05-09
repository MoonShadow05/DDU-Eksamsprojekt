using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.VFX;

public class SlutPageHandler : MonoBehaviour
{
    [SerializeField] private GameObject Lost;
    [SerializeField] private GameObject Won;
    [SerializeField] private GameObject LostDoor;
    [SerializeField] private SceneHandler sceneHandler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake(){

        var Visual = GameObject.Find("Visual");
        if(Visual != null){
            if (Lost == null)
            {
                var LostObj = Visual.transform.Find("Lost");
                if (LostObj != null){
                    Lost = LostObj.GetComponent<GameObject>();
                }
            }

            // Find equation text
            if (Won == null){
                var WonObj = Visual.transform.Find("Won");
                if (WonObj != null){
                    Won = WonObj.GetComponent<GameObject>();
                }
            }
            if (LostDoor == null)
            {
                var LostDoorObj = Visual.transform.Find("LostDoor");
                if (LostDoorObj != null){
                    LostDoor = LostDoorObj.GetComponent<GameObject>();
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(sceneHandler.won == true){
            Won.SetActive(true);
            Lost.SetActive(false);
            LostDoor.SetActive(false);
        }
        else if(sceneHandler.won == false){
            Won.SetActive(false);
            Lost.SetActive(true);
            LostDoor.SetActive(true);
        }
    }
}
