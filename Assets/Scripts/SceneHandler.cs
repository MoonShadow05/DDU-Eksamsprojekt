using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public bool hasWon;

    public void gameEnd(bool won){
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        if(won){
            SceneManager.LoadScene("WonPage");
        }
        else{
            SceneManager.LoadScene("LostPage");
        }

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

}
