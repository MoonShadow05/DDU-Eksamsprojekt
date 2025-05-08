using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    // Call this from the Start button
    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("Main"); // Loads the scene named "Main"
    }

    // Call this from the Quit button
    public void OnQuitButtonPressed()
    {
        Debug.Log("Quit button pressed. Exiting game...");
        Application.Quit();

        // Note: This won't quit the game in the Unity editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
