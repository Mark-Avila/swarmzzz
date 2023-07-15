using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseObjects;
    [SerializeField] private TMPro.TextMeshProUGUI headerText;
    private bool isPaused = false;

    private void Start()
    {
        pauseObjects.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        headerText.SetText("Paused");
        isPaused = true;
        Time.timeScale = 0f;
        pauseObjects.SetActive(true);
        AudioManager.Instance.MuteAllClips();
    }

    public void ResumeGame()
    {
        headerText.SetText("");
        isPaused = false;
        Time.timeScale = 1f;
        pauseObjects.SetActive(false);
        AudioManager.Instance.UnmuteAllClips();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
