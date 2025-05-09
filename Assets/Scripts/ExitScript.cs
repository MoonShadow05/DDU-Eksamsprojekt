using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class ExitScript : MonoBehaviour
{
    [SerializeField] private GameObject exitCollider;
    [SerializeField] private WaterManager WaterManager;
    [SerializeField] private GameObject player;
    [SerializeField] private SceneHandler sceneHandler;

    private bool atExit = false;
    public bool hasWon = false;

    private void Start()
    {
        if (WaterManager == null)
            WaterManager = FindFirstObjectByType<WaterManager>();
        if (player == null)
            player = GameObject.Find("Spiller");
        if (sceneHandler == null)
            sceneHandler = FindFirstObjectByType<SceneHandler>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WaterManager.FoundExit();
            atExit = true;
        }
    }
    private void Update()
    {
        if (atExit == true)
        {
            if (player.transform.position.y >= 17)
            {
                hasWon = true;
                sceneHandler.gameEnd(hasWon);
                Time.timeScale = 0f;
                Debug.Log("Has won");
            }
        }
    }
}
