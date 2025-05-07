using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

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
 