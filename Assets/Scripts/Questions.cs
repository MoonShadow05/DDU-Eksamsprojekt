using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class QuestionPopupTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Exercises exercises;
    [SerializeField] private MonoBehaviour cameraScript;
    [SerializeField] private Collider doorBlockerCollider;

    private Collider triggerCollider;

    [SerializeField] private float doorOpenHeight = 25f;
    [SerializeField] private float doorOpenSpeed = 5f;

    private bool isOpening = false;
    private Vector3 doorTargetPosition;

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

        Vector3 DoorPosition = transform.parent.position;
        OpenDoor(DoorPosition);
        
        doorTargetPosition = DoorPosition + Vector3.up * doorOpenHeight;
        isOpening = true;

        Debug.LogError($"✅ Question complete. Door unlocked.");
    }

    public void OpenDoor(Vector3 DoorPosition){
        bool SlideDoor = true;
        float StartPosition = DoorPosition.y;

        while(SlideDoor){
            DoorPosition.y +=2*Time.deltaTime;
            if(DoorPosition.y > StartPosition+35){
                SlideDoor = false;
            }
        }
    }
}
 