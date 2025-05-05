using UnityEngine;

public class QuestionPopupTrigger : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private MonoBehaviour cameraControlScript; // e.g., your mouse/camera script

    private void Start()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && popupPanel != null)
        {
            popupPanel.SetActive(true);
            if (cameraControlScript != null)
                cameraControlScript.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log("Popup shown and camera locked.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && popupPanel != null)
        {
            popupPanel.SetActive(false);
            if (cameraControlScript != null)
                cameraControlScript.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Debug.Log("Popup hidden and camera unlocked.");
        }
    }


    public void CompleteQuestion()
    {
        if (cameraControlScript != null)
            cameraControlScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        popupPanel.SetActive(false);
        Debug.LogError($"Question '{popupPanel.name}' completed!");
    }

}
