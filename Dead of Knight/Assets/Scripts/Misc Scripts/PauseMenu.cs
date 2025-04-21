using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;
    public GameObject pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // 0 is for left mouse button
        {
            Debug.Log("Mouse click detected at position at " + Input.mousePosition);
        }
   

        if (Input.GetButtonDown("Cancel"))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Debug.Log("Pausing the game.");
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        isPaused = true;
        Debug.Log("Game paused.");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPaused = false;
    }

    // public void OnPauseMenuButtonClick()
    // {
    //     Debug.Log("Pause clicked");
    //     SceneManager.LoadScene(0);
    //     Debug.Log("Scene load request made");
    // }
}