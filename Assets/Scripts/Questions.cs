using UnityEngine;
using System.Linq;


public class QuestionPopupTrigger : MonoBehaviour
{
    [SerializeField] private WaterManager WaterManager;

    [Header("References")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Exercises exercises;
    [SerializeField] private MonoBehaviour cameraScript;
    [SerializeField] private Collider doorBlockerCollider;

    private Collider triggerCollider;


    private bool DoorShouldOpen = false;
    private GameObject DoorPosition;

    private void Update(){

    }
    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();

        // Assign WaterManager if not set
        if (WaterManager == null)
            WaterManager = FindFirstObjectByType<WaterManager>();

        // Assign Popup Panel (you can also use a tag or name here)
       if (popupPanel == null)
        {
            var hud = GameObject.Find("HUD");
            if (hud != null)
            {
                popupPanel = hud.GetComponentsInChildren<Transform>(true)
                                .FirstOrDefault(t => t.name == "PopupMenu")?.gameObject;

                Debug.Log($"Popup Panel Found: {popupPanel != null}");

            }
        }



        // Assign Exercises script
        if (exercises == null)
            exercises = FindFirstObjectByType<Exercises>();

        // Assign Camera Script (assumes it's on main camera or tagged object)
        if (cameraScript == null)
        {
            var camObj = Camera.main != null ? Camera.main.gameObject : GameObject.FindWithTag("MainCamera");
            if (camObj != null)
                cameraScript = camObj.GetComponent<MonoBehaviour>(); // Replace with specific type if you know it
        }

        // Assign Door Blocker Collider (e.g. find by tag or name if needed)
        if (doorBlockerCollider == null)
            doorBlockerCollider = GameObject.Find("DoorColider")?.GetComponent<Collider>();

        if (popupPanel != null)
            popupPanel.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        popupPanel.SetActive(true);
        exercises.LoadRandomQuestion(this);

        if (cameraScript != null) cameraScript.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Entered question zone");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        popupPanel.SetActive(false);

        if (cameraScript != null) cameraScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Exited question zone");
    }

    public void CheckAnswer(string selectedAnswer)
    {
        if (selectedAnswer == exercises.correctAnswer)
        {
            Debug.Log("✅ Correct!");
            CompleteQuestion(); 

        }
        else
        {
            Debug.Log("❌ Wrong answer.");
        }
    }

    private void CompleteQuestion()
    {
        popupPanel.SetActive(false);
        triggerCollider.enabled = false;

        if (doorBlockerCollider != null)
            doorBlockerCollider.enabled = false;

        if (cameraScript != null)
            cameraScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        DoorShouldOpen = true;
        DoorPosition = transform.parent.gameObject;
        WaterManager.OpenDoor(DoorShouldOpen, DoorPosition);


        Debug.LogError($"✅ Question complete. Door unlocked.");
    }
}
 