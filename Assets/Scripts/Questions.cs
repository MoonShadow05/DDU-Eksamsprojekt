using UnityEngine;
using System.Linq;
using System.Collections;


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
            popupPanel.SetActive(false);
            feedbackPanel.SetActive(true);

            var feedbackTextUI = feedbackPanel.GetComponentInChildren<TMPro.TMP_Text>();
            if (feedbackTextUI != null && exercises.currentExercise != null)
            {
                feedbackTextUI.text = exercises.currentExercise.FeedbackText;
            }
            else
            {
                Debug.LogWarning("❌ FeedbackTextUI or currentExercise is null.");
            }

            if (WaterManager != null)
            {
                WaterManager.pauseWaterIncrease = true;
                Debug.Log("💧 Water increase paused during feedback.");
            }

            Debug.Log("feedbackPanel is active: " + feedbackPanel.activeSelf);
            Exercises.WrongAnswers += 1;
            Exercises.RightAnswers = 0;

            // ✅ Wait and retry after 3 seconds
            StartCoroutine(FeedbackPanelClose());
        }
    }



    private IEnumerator FeedbackPanelClose()
    {
        yield return new WaitForSeconds(3f);

        feedbackPanel.SetActive(false);
        popupPanel.SetActive(true);

        // ✅ Resume water increase
        if (WaterManager != null)
        {
            WaterManager.pauseWaterIncrease = false;
            Debug.Log("💧 Water increase resumed.");
        }

        exercises.LoadRandomQuestion(this, Exercises.questionDifficulty);
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
 