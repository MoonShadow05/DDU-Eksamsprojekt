using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    private Collider triggerCollider;

    public void Awake()
    {
        triggerCollider = GetComponent<Collider>();
    }

    // Call this from the Start button
    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("Main"); // Loads the scene named "Main"
    }

    public void OnRestartButtonPressed(){
        SceneManager.LoadScene("Main"); // Loads the scene named "Main"
    }
    // Call this from the Quit button
    public void OnQuitButtonPressed()
    {
        Debug.Log("Quit button pressed. Exiting game...");
        Application.Quit();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("Gået ind i slutning");

    }

}
