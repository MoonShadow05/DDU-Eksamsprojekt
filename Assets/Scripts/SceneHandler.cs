using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public bool won;
    public void gameEnd(bool hasWon){
        won = hasWon;
        SceneManager.LoadScene("SlutPage");
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
