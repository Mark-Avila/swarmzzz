using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private void Start()
    {
        HideGameOverScreen();
    }

    // Call this method to show the game over screen
    public void ShowGameOverScreen()
    {
        gameObject.SetActive(true);
    }

    // Call this method to hide the game over screen
    public void HideGameOverScreen()
    {
        gameObject.SetActive(false);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
