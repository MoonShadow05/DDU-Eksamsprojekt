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
    [SerializeField] private GameObject feedbackPanel;

    private Collider triggerCollider;


    private bool DoorShouldOpen = false;
    private GameObject DoorPosition;

   /*  private void Update(){

    } */
    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();

        // Assign WaterManager if not set
        if (WaterManager == null)
            WaterManager = FindFirstObjectByType<WaterManager>();

        // Assign Popup Panel (you can also use a tag or name here)
       if (popupPanel == null || feedbackPanel == null)
        {
            var hud = GameObject.Find("HUD");
            if (hud != null)
            {
                popupPanel = hud.GetComponentsInChildren<Transform>(true)
                                .FirstOrDefault(t => t.name == "PopupMenu")?.gameObject;
                                 /* Debug.Log($"Popup Panel Found: {popupPanel != null}"); */

                feedbackPanel = hud.GetComponentsInChildren<Transform>(true)
                                .FirstOrDefault(t => t.name == "FeedbackMenu")?.gameObject;
                                Debug.Log($"Feedback Panel Found: {feedbackPanel != null}");
            }
        }



        // Assign Exercises script
        if (exercises == null)
            exercises = FindFirstObjectByType<Exercises>();

        // Assign Camera Script (assumes it's on main camera or tagged object)
        if (cameraScript == null)
        {
            
            if (Camera.main != null && Camera.main.transform.parent != null)
            {
                /* Debug.Log("✅ PlayerLook script found on CameraHolder."); */
                cameraScript = Camera.main.transform.parent.GetComponent<PlayerLook>();
            } else
            {
                /* Debug.LogError("❌ PlayerLook script not found on CameraHolder."); */
            }
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
        exercises.LoadRandomQuestion(this, Exercises.questionDifficulty);

        if (cameraScript != null)
        {

            if (cameraScript is PlayerLook lookScript)
            lookScript.SetFrozen(true);
        }

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        /* Debug.Log("Entered question zone"); */
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        popupPanel.SetActive(false);

         if (cameraScript != null)
        {
            if (cameraScript is PlayerLook lookScript)
                lookScript.SetFrozen(false);
        }

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

       /*  Debug.Log("Exited question zone"); */
    }

    public void CheckAnswer(string selectedAnswer)
    {
        if (selectedAnswer == exercises.correctAnswer)
        {
            feedbackPanel.SetActive(false);
            Exercises.WrongAnswers = 0;
            Exercises.RightAnswers += 1;
            CompleteQuestion(); 
        }
        else
        {
            feedbackPanel.SetActive(true);
            popupPanel.SetActive(false);
            feedbackPanel.GetComponentInChildren<TMPro.TMP_Text>().text = exercises.FeedbackText.text;
            Debug.Log("feedbackPanel is active: " + feedbackPanel.activeSelf);
            Exercises.WrongAnswers += 1;
            Exercises.RightAnswers = 0;
            
        }
    }

    private void CompleteQuestion()
    {
        popupPanel.SetActive(false);
        triggerCollider.enabled = false;

        if (cameraScript != null)
            if (cameraScript is PlayerLook lookScript)
                lookScript.SetFrozen(false);

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        DoorShouldOpen = true;
        DoorPosition = transform.parent.gameObject;
        WaterManager.OpenDoor(DoorShouldOpen, DoorPosition);

    }
}
 