using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.UI;

public class QuestionPopupTrigger : MonoBehaviour
{
    [SerializeField] private WaterManager WaterManager;

    [Header("References")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Exercises exercises;
    [SerializeField] private MonoBehaviour cameraScript;
    [SerializeField] private Collider doorBlockerCollider;
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private Button CloseFeedbackButton;


    private Collider triggerCollider;

    private bool DoorShouldOpen = false;
    private GameObject DoorPosition;

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

                feedbackPanel = hud.GetComponentsInChildren<Transform>(true)
                                .FirstOrDefault(t => t.name == "FeedbackMenu")?.gameObject;
                Debug.Log($"Feedback Panel Found: {feedbackPanel != null}");
                if (CloseFeedbackButton == null)
                {
                    var fbuttonObj = hud.transform.Find("FeedbackMenu/CloseFeedback");
                    if (fbuttonObj != null)
                        CloseFeedbackButton = fbuttonObj.GetComponent<Button>();
                }
            }
            else
            {
                Debug.LogError("❌ HUD not found. Please assign the PopupPanel and FeedbackPanel in the inspector.");
            }
        }

        if (CloseFeedbackButton != null)
        {
            var buttonComponent = CloseFeedbackButton.GetComponent<UnityEngine.UI.Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(CloseFeedbackPanel);
            }
            else
            {
                Debug.LogError("❌ CloseFeedbackButton does not have a Button component.");
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
                cameraScript = Camera.main.transform.parent.GetComponent<PlayerLook>();
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

            Exercises.WrongAnswers += 1;
            Exercises.RightAnswers = 0;
            CloseFeedbackButton.onClick.AddListener(() =>
            {
                FeedbackPanelClose();
            });
        }
    }

    private void FeedbackPanelClose()
    {

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

    // New method to close feedback panel
    private void CloseFeedbackPanel()
    {
        feedbackPanel.SetActive(false);  // Close the feedback panel
        popupPanel.SetActive(true);      // Open the popup panel for the next question

        // Resume water increase if needed
        if (WaterManager != null)
        {
            WaterManager.pauseWaterIncrease = false;
            Debug.Log("💧 Water increase resumed.");
        }

        Exercises.WrongAnswers = 0;
        Exercises.RightAnswers = 0;
        exercises.LoadRandomQuestion(this, Exercises.questionDifficulty);  // Load next question
    }
}
