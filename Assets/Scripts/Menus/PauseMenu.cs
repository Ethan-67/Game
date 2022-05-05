using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

/* PAUSE MENU 
 * Handles displaying the pause meny on screen and all button listeners for each button on pause menu.
*/
public class PauseMenu : MonoBehaviour
{
    // whole pause menu object 
    public GameObject pauseMenu; 

    // buttons 
    private Button resumeButton; 
    private Button exitButton;

    PlayerLook playerLook;

    // pause state 
    private bool isPaused = false; 

    // initialise attributes 
    private void Start()
    {
        pauseMenu = GameObject.Find("PauseMenu");
        playerLook = GameObject.Find("Player").GetComponent<PlayerLook>();

        resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
        exitButton = GameObject.Find("ExitButton").GetComponent<Button>();

        resumeButton.onClick.AddListener(ResumeButtonListener);
        exitButton.onClick.AddListener(ExitButtonListener);

        pauseMenu.SetActive(false);
    }

    // handles pause, flips pause state and calls methods needed for this 
    public void HandlePause()
    {
        isPaused = !isPaused;
        // if paused, pause game and set mouse to unlocked 
        if (isPaused)
        {
            PauseGame();
            playerLook.SetActive(false);
            playerLook.SetMouseLockedState(false); 
        }
        else /* otherwise unpause game set mouse to locked in centre of screen */
        {
            UnPauseGame();
            playerLook.SetActive(true);
            playerLook.SetMouseLockedState(true);
        }
    }

    // stop time and set pause menu to active 
    private void PauseGame()
    {
        Debug.Log("Pause");
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; 
    }

    // unfreeze time and set pause menu to inactive 
    private void UnPauseGame()
    {
        Debug.Log("UnPause");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; 
    }

    // resume button listener resume game
    public void ResumeButtonListener()
    {
        Debug.Log("Resume Pressed"); 
        HandlePause(); 
    }

    // exit button listener return to main menu
    public void ExitButtonListener()
    {
        Debug.Log("Exit Pressed");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    
    public void HandleClick()
    {
        if (!isPaused)
            return;
    }
}
